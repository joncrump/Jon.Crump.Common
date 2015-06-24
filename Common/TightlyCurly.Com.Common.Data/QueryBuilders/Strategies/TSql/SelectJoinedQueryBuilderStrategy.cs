using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data.QueryBuilders.Strategies.TSql
{
    public class SelectJoinedQueryBuilderStrategy : QueryBuilderStrategyBase, IQueryBuilderStrategy
    {
        private readonly IPredicateBuilder _predicateBuilder;
        private readonly IQueryBuilderStrategyFactory _queryBuilderStrategyFactory;

        public SelectJoinedQueryBuilderStrategy(IFieldHelper fieldHelper, IPredicateBuilder predicateBuilder, 
            IQueryBuilderStrategyFactory queryBuilderStrategyFactory) 
            : base(fieldHelper)
        {
            _predicateBuilder = Guard.EnsureIsNotNull("predicateBuilder", predicateBuilder);
            _queryBuilderStrategyFactory = Guard.EnsureIsNotNull("queryBuilderStrategyFactory", queryBuilderStrategyFactory);
        }

        public QueryInfo BuildQuery<TValue>(dynamic parameters = null) where TValue : class
        {
            Expression<Func<TValue, bool>> predicate = parameters.Predicate;
            bool canDirtyRead = parameters.CanDirtyRead;
            bool includeParameters = parameters.IncludeParameters;
            IEnumerable<string> desiredFields = parameters.DesiredFields;
            string tableName = parameters.TableName;

            var joinProperties = typeof(TValue)
                .GetProperties()
                .SafeWhere(p => p.GetCustomAttributes(typeof(JoinAttribute), true).IsNotNullOrEmpty())
                .SafeOrderBy(p => p.Name)
                .ToSafeList();

            if (joinProperties.IsNullOrEmpty())
            {
                var strategy = _queryBuilderStrategyFactory.GetBuilderStrategy(QueryKind.SelectSingleTable);

                return strategy.BuildQuery<TValue>(parameters);
            }

            var tableFieldMappings = GetTableObjectMappings<TValue>(desiredFields, tableName, joinProperties);
            var queryBuilder = new StringBuilder();

            queryBuilder.Append("SELECT ");

            BuildJoinedFields(tableFieldMappings, queryBuilder);

            queryBuilder.Append(" ");

            var tableFieldInfo = tableFieldMappings.First();

            BuildFromJoinClause(tableFieldMappings, queryBuilder, canDirtyRead);
            var dbParameters = BuildWhereClause(predicate, includeParameters, queryBuilder, tableFieldInfo.Alias, 
                "{0}_".FormatString(tableFieldInfo.Alias));

            queryBuilder.Append(";");

            return new QueryInfo(queryBuilder.ToString().Trim(), tableFieldMappings, dbParameters);
        }

        private IEnumerable<IDbDataParameter> BuildWhereClause<TValue>(Expression<Func<TValue, bool>> predicate, bool includeParameters, StringBuilder queryBuilder, 
            string tableAlias, string fieldPrefix)
            where TValue : class
        {
            var dbParameters = new List<IDbDataParameter>();

            if (predicate.IsNotNull())
            {
                var container = _predicateBuilder.BuildContainer(predicate, typeof (TValue), includeParameters, tableAlias, fieldPrefix);

                if (includeParameters)
                {
                    dbParameters = container.Parameters.ToSafeList();
                }

                queryBuilder.Append(" ");
                queryBuilder.Append("WHERE {0}".FormatString(container.WhereClause));
            }

            return dbParameters;
        }

        private void BuildFromJoinClause(IEnumerable<TableObjectMapping> tableObjectMappings, StringBuilder queryBuilder, bool canDirtyRead)
        {
            var parent = tableObjectMappings.FirstOrDefault();

            if (parent.IsNull())
            {
                return;
            }

            Guard.EnsureIsValid("parent", p => p.Joins.Where(j => j.JoinType == JoinType.None).IsNullOrEmpty(), parent, "Joins cannot be of join type none.");

            queryBuilder.Append("FROM {0} {1} {2} ".FormatString(parent.TableName, canDirtyRead ? "(NOLOCK)" : String.Empty, parent.Alias));

            for (var index = 0; index < parent.Joins.Count; index++)
            {
                var join = parent.Joins[index];

                queryBuilder.AppendFormat("{0} JOIN {1} {2} ", GetStringFromJoinType(join.JoinType), join.RightTableName,
                    canDirtyRead ? "(NOLOCK)" : String.Empty);

                if (join.RightTableAlias.IsNotNullOrEmpty())
                {
                    queryBuilder.Append("{0} ".FormatString(join.RightTableAlias));
                }

                queryBuilder.AppendFormat("ON {0}.{1}{2} = {3}.{4}{5}",
                    join.LeftTableAlias.IsNullOrEmpty() ? join.LeftTableName : join.LeftTableAlias,
                    join.LeftJoinFieldPrefix.IsNullOrEmpty() ? String.Empty : join.LeftJoinFieldPrefix,
                    join.LeftJoinField,
                    join.RightTableAlias.IsNullOrEmpty() ? join.RightTableName : join.RightTableAlias,
                    join.RightJoinFieldPrefix.IsNullOrEmpty() ? String.Empty : join.RightJoinFieldPrefix,
                    join.RightJoinField);

                if (index < parent.Joins.Count - 1)
                {
                    queryBuilder.Append(" ");
                }
            }
        }

        private string GetStringFromJoinType(JoinType joinType)
        {
            switch (joinType)
            {
                case JoinType.Inner:
                    return "INNER";
                case JoinType.Left:
                    return "LEFT";
                case JoinType.Right:
                    return "RIGHT";
            }

            throw new InvalidOperationException("The join type: {0} is not supported."
                .FormatString(joinType.ToString()));
        }

        private List<TableObjectMapping> GetTableObjectMappings<TValue>(IEnumerable<string> desiredFields, string tableName, List<PropertyInfo> joinProperties)
            where TValue : class
        {
            var initialTable = FieldHelper.BuildFields<TValue>(desiredFields, tableName, alias: "t1");

            var tableObjectMappings = new List<TableObjectMapping>
            {
                initialTable
            };

            var fieldHelperMethod = FieldHelper.GetType().GetMethod("BuildFields");
            var index = 2;

            foreach (var property in joinProperties)
            {
                var attribute = (JoinAttribute) property.GetCustomAttributes(typeof (JoinAttribute), true)
                    .First();
                var type = attribute.JoinedType;
                var genericMethod = fieldHelperMethod.MakeGenericMethod(new[] { type });
                var tableObjectMapping = (TableObjectMapping) genericMethod.Invoke(FieldHelper, new object[]
                {
                    desiredFields, tableName, null, false, "t{0}".FormatString(index), property.Name 
                });

                tableObjectMapping.Prefix = "t{0}".FormatString(index);
                tableObjectMapping.ParentKey = attribute.ParentProperty;
                tableObjectMapping.ChildKey = attribute.ChildProperty;
                tableObjectMapping.JoinType = type;

                index++;

                tableObjectMappings.Add(tableObjectMapping);

                tableObjectMappings[0] = AddJoins(attribute, tableObjectMappings.First(), tableObjectMapping);
            }

            return tableObjectMappings;
        }

        private TableObjectMapping AddJoins(JoinAttribute attribute, TableObjectMapping parent, TableObjectMapping child)
        {
            if (attribute.JoinTable.IsNotNullOrEmpty())
            {
                BuildComplexJoin(attribute, parent, child);
            }
            else // No need to join through Network table.
            {
                BuildSimpleJoin(attribute, parent, child);
            }
           
            return parent;
        }

        private static void BuildComplexJoin(JoinAttribute attribute, TableObjectMapping parent, TableObjectMapping child)
        {
            var parentTableName = parent.TableName;
            var parentAlias = parent.Alias;
            var leftKey = attribute.LeftKey;

            var joinTableName = attribute.JoinTable;
            var joinTableLeftKey = attribute.JoinTableLeftKey;
            var joinTableRightKey = attribute.JoinTableRightKey;
            var joinTableJoinType = attribute.JoinTableJoinType;

            var childTableName = child.TableName;
            var childAlias = child.Alias;
            var rightKey = attribute.RightKey;

            parent.Joins.Add(new Join
            {
                JoinType = joinTableJoinType,
                LeftJoinField = "{0}_{1}".FormatString(parent.Alias, leftKey),
                LeftTableName = parentTableName,
                LeftTableAlias = parentAlias,
                RightJoinField = joinTableLeftKey,
                RightTableName = joinTableName,
            });
            
            parent.Joins.Add(new Join
            {
                JoinType = joinTableJoinType,
                LeftJoinField = joinTableRightKey,
                LeftTableName = joinTableName,
                RightJoinField = "{0}_{1}".FormatString(childAlias, rightKey),
                RightTableAlias = childAlias,
                RightTableName = childTableName
            });
        }

        private static void BuildSimpleJoin(JoinAttribute attribute, TableObjectMapping parent, TableObjectMapping child)
        {
            parent.Do(() => parent.Joins.Add(new Join
            {
                JoinType = attribute.JoinType,
                LeftJoinField = attribute.LeftKey,
                LeftTableName = parent.TableName,
                LeftTableAlias = parent.Alias,
                LeftJoinFieldPrefix = parent.Alias.IsNullOrEmpty() ? String.Empty : "{0}_".FormatString(parent.Alias),
                RightJoinField = attribute.RightKey,
                RightTableName = child.TableName,
                RightTableAlias = child.Alias,
                RightJoinFieldPrefix = child.Alias.IsNullOrEmpty() ? String.Empty : "{0}_".FormatString(child.Alias)
            }));
        }

        private void BuildJoinedFields(List<TableObjectMapping> tableObjectMappings, StringBuilder queryBuilder)
        {
            var tableIndex = 0;

            foreach (var tableFieldInfo in tableObjectMappings.Where(t => t.IsNotNull()))
            {
                queryBuilder.Append(tableFieldInfo.FieldMappings.Values
                    .Select(f => "{0}.{1} AS {2}_{3}".FormatString(f.Prefix, f.FieldName, f.Prefix, f.FieldName))
                    .ToDelimitedString(", "));

                if (tableIndex < tableObjectMappings.Count - 1)
                {
                    queryBuilder.Append(", ");
                }

                tableIndex++;
            }
        }
    }
}