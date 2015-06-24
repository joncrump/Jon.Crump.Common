using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data
{
    // Shamelessly borrowed this code from http://stackoverflow.com/questions/7731905/how-to-convert-an-expression-tree-to-a-partial-sql-query
    public class DatabaseQueryPredicateBuilder : ExpressionVisitor, IPredicateBuilder
    {
        private StringBuilder _queryBuilder;
        private string _orderByClause = String.Empty;
        private int? _skip;
        private int? _take;
        private string _whereClause = String.Empty;
        private readonly List<IDbDataParameter> _parameters;
        private string _currentParameter;
        private string _currentField;
        private Type _declaringType;
        private bool _addParameters = false;
        private string _tableAlias;
        private string _fieldPrefix;

        public int? Skip
        {
            get
            {
                return _skip;
            }
        }

        public int? Take
        {
            get
            {
                return _take;
            }
        }

        public string OrderBy
        {
            get
            {
                return _orderByClause;
            }
        }

        public string WhereClause
        {
            get
            {
                return _whereClause;
            }
        }

        public DatabaseQueryPredicateBuilder()
        {
            _parameters = new List<IDbDataParameter>();
        }

        public QueryContainer BuildContainer(Expression expression, Type declaringType, bool addParameters = true, 
            string tableAlias = null, string fieldPrefix = null)
        {
            Guard.EnsureIsNotNull("expression", expression);
            _declaringType = Guard.EnsureIsNotNull("declaringType", declaringType);
            _addParameters = addParameters;

            _parameters.Clear();
            _currentParameter = null;
            _currentField = null;
            _tableAlias = tableAlias;
            _fieldPrefix = fieldPrefix;

            _queryBuilder = new StringBuilder();

            try
            {
                Visit(expression);
            }
            catch (ArgumentException exception)
            {
                if (!exception.Message.StartsWith(
                        "Expression of type"))
                {
                    throw;
                }
            }

            _whereClause = _queryBuilder.ToString();

            var container = new QueryContainer(_whereClause, _parameters.ToList(), _orderByClause);

            _parameters.Clear();
            _whereClause = null;
            _orderByClause = null;
            _currentField = null;
            _currentParameter = null;
            _tableAlias = null;
            _fieldPrefix = null;

            return container;
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where")
            {
                Visit(m.Arguments[0]);
                var lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                Visit(lambda.Body);
                return m;
            }

            if (m.Method.Name == "Take")
            {
                if (ParseTakeExpression(m))
                {
                    Expression nextExpression = m.Arguments[0];
                    return Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "Skip")
            {
                if (ParseSkipExpression(m))
                {
                    Expression nextExpression = m.Arguments[0];
                    return Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "OrderBy")
            {
                if (ParseOrderByExpression(m, "ASC"))
                {
                    Expression nextExpression = m.Arguments[0];
                    return Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "OrderByDescending")
            {
                if (ParseOrderByExpression(m, "DESC"))
                {
                    Expression nextExpression = m.Arguments[0];
                    return Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "StartsWith")
            {
                var field = ParseStartsWithExpression(m);

                var memberExpression = (MemberExpression) m.Object;

                var property = memberExpression.Member as PropertyInfo;

                var attribute = property.GetCustomAttributes(typeof(FieldMetadataAttribute)).FirstOrDefault() as FieldMetadataAttribute;
                
                _queryBuilder.Append("{0} LIKE ('{1}%')".FormatString(
                    BuildFieldName(attribute.FieldName), field));

                var nextExpression = m.Arguments[0]; 
                return Visit(nextExpression);
            }
            else if (m.Method.Name == "Contains")
            {
                var field = ParseStartsWithExpression(m);

                var memberExpression = (MemberExpression)m.Object;

                var property = memberExpression.Member as PropertyInfo;

                var attribute = property.GetCustomAttributes(typeof(FieldMetadataAttribute)).FirstOrDefault() as FieldMetadataAttribute;

                _queryBuilder.Append("{0} LIKE ('%{1}%')".FormatString(
                    BuildFieldName(attribute.FieldName), field));

                var nextExpression = m.Arguments[0];
                return Visit(nextExpression);
            }
            else if (m.Method.Name == "EndsWith")
            {
                var field = ParseStartsWithExpression(m);

                var memberExpression = (MemberExpression)m.Object;

                var property = memberExpression.Member as PropertyInfo;

                var attribute = property.GetCustomAttributes(typeof(FieldMetadataAttribute)).FirstOrDefault() as FieldMetadataAttribute;

                _queryBuilder.Append("{0} LIKE ('%{1}')".FormatString(
                    BuildFieldName(attribute.FieldName), field));

                var nextExpression = m.Arguments[0];
                return Visit(nextExpression);
            }
            else if (m.Method.Name == "Equals")
            {
                var field = ParseEqualsExpression(m);
                var memberExpression = (MemberExpression)m.Object;
                var property = memberExpression.Member as PropertyInfo;
                var attribute = property.GetCustomAttributes(typeof(FieldMetadataAttribute)).FirstOrDefault() as FieldMetadataAttribute;

                // Handle date type.
                if (attribute.IsNull() 
                    && ((PropertyInfo)((MemberExpression)memberExpression.Expression).Member).PropertyType == typeof(DateTime) 
                    && (property.Name == "Day" || property.Name == "Month" || property.Name == "Year"))
                {
                    var parentMemberExpression = memberExpression.Expression as MemberExpression;

                    var parentProperty = parentMemberExpression.Member as PropertyInfo;

                    attribute = GetMetadataAttribute(parentMemberExpression, parentProperty);

                    _queryBuilder.Append("{0}({1}) = {2}".FormatString(property.Name.ToUpper(), 
                        BuildFieldName(attribute.FieldName),
                        BuildField(field)));
                }
                else
                {
                    var fieldValue = BuildField(field);

                    if (fieldValue.IsNullOrEmpty())
                    {
                        _queryBuilder.AppendFormat("{0} IS NULL".FormatString(
                            BuildFieldName(attribute.FieldName)));
                    }
                    else if (attribute.IsNotNull())
                    {
                        _queryBuilder.Append("{0} = {1}".FormatString(
                            BuildFieldName(attribute.FieldName), BuildField(field)));
                    }
                }

                var nextExpression = m.Arguments[0];
                return Visit(nextExpression);
            }

            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    _queryBuilder.Append(" NOT ");
                    Visit(u.Operand);
                    break;
                case ExpressionType.Convert:
                    Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }

            return u;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression b)
        {
            //_queryBuilder.Append("(");
            Visit(b.Left);

            switch (b.NodeType)
            {
                case ExpressionType.And:
                    _queryBuilder.Append(" AND ");
                    break;

                case ExpressionType.AndAlso:
                    _queryBuilder.Append(" AND ");
                    break;

                case ExpressionType.Or:
                    _queryBuilder.Append(" OR ");
                    break;

                case ExpressionType.OrElse:
                    _queryBuilder.Append(" OR ");
                    break;

                case ExpressionType.Equal:
                    if (IsNullConstant(b.Right))
                    {
                        _queryBuilder.Append(" IS ");
                    }
                    else
                    {
                        _queryBuilder.Append(" = ");
                    }
                    break;

                case ExpressionType.NotEqual:
                    if (IsNullConstant(b.Right))
                    {
                        _queryBuilder.Append(" IS NOT ");
                    }
                    else
                    {
                        _queryBuilder.Append(" <> ");
                    }
                    break;

                case ExpressionType.LessThan:
                    _queryBuilder.Append(" < ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    _queryBuilder.Append(" <= ");
                    break;

                case ExpressionType.GreaterThan:
                    _queryBuilder.Append(" > ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    _queryBuilder.Append(" >= ");
                    break;

                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));

            }

            Visit(b.Right);
            //_queryBuilder.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;

            var parameter = _parameters.Find(p => p.ParameterName == _currentParameter);

            if (parameter.IsNull())
            {
                return c;
            }

            if (q == null && c.Value == null)
            {
                parameter.Value = "NULL";
            }
            else if (q == null)
            {
                parameter.Value = c.Value.IsNull() ? DBNull.Value : c.Value;
            }

            if (_addParameters)
            {
                _queryBuilder.Append(parameter.ParameterName);
            }
            else
            {
                _queryBuilder.Append(parameter.Value);
            }
            return c;
        }

        protected override Expression VisitMember(MemberExpression memberExpression)
        {
            if (memberExpression.IsNull())
            {
                return memberExpression;
            }

            if (memberExpression.Expression.NodeType == ExpressionType.Parameter)
            {
                var property = memberExpression.Member as PropertyInfo;
                var attribute = GetMetadataAttribute(memberExpression, property);

                _currentParameter = attribute.ParameterName;
                var parameter = new SqlParameter
                {
                    ParameterName = attribute.ParameterName,
                    SqlDbType = attribute.DbType,
                };

                _parameters.Add(parameter);

                _currentField = "{0}".FormatString(BuildFieldName(attribute.FieldName));

                _queryBuilder.Append(_currentField);
            }

            if (memberExpression.Expression.NodeType == ExpressionType.MemberAccess)
            {
                var property = memberExpression.Member as PropertyInfo;
                
                var attribute = property.GetCustomAttributes(typeof(FieldMetadataAttribute)).FirstOrDefault() as FieldMetadataAttribute;
                var parameter = attribute.IsNull() 
                    ? _parameters.Find(p => p.ParameterName == _currentParameter) 
                    : _parameters.Find(p => p.ParameterName == attribute.ParameterName);

                var value = GetLocalValue(memberExpression);

                if (!property.Name.Equals("Day") && !property.Name.Equals("Month") && !property.Name.Equals("Year"))
                {
                    if (_addParameters)
                    {
                        parameter.Value = value.IsNull() ? DBNull.Value : value;

                        _queryBuilder.Append(parameter.ParameterName);

                        _currentParameter = parameter.ParameterName;
                    }
                    else
                    {
                        _queryBuilder.Append(value.IsNull() ? "NULL" : value);
                    }
                }
            }

            if (memberExpression.Expression.NodeType == ExpressionType.Call)
            {
                var value = GetLocalValue(memberExpression);

                _queryBuilder.Append(_currentParameter);

                var parameter = _parameters.Find(p => p.ParameterName == _currentParameter);

                parameter.Value = value.IsNull() ? DBNull.Value : value;
            }

            if (memberExpression.Expression.NodeType == ExpressionType.Constant)
            {
                var value = GetLocalValue(memberExpression);

                _queryBuilder.Append(_currentParameter);

                var parameter = _parameters.Find(p => p.ParameterName == _currentParameter);

                parameter.Value = value.IsNull() ? DBNull.Value : value;
            }

            return memberExpression;
        }

        private FieldMetadataAttribute GetMetadataAttribute(MemberExpression memberExpression, PropertyInfo property)
        {
            // Look for the decorated attribute in the property object.
            var attribute =
                property.GetCustomAttributes(typeof (FieldMetadataAttribute)).FirstOrDefault() as FieldMetadataAttribute;

            // We can't find the attribute, so let's go to our backup.  Typically happens when the property's DeclaringType
            // is an interface.
            if (attribute.IsNull())
            {
                var declaredTypeProperty = _declaringType.GetProperties().FirstOrDefault(p => p.Name == property.Name);

                if (declaredTypeProperty.IsNotNull())
                {
                    attribute =
                        declaredTypeProperty.GetCustomAttributes(typeof (FieldMetadataAttribute)).FirstOrDefault() as
                            FieldMetadataAttribute;
                }
            }

            // Give up.  We couldn't find the decorated attribute.
            if (attribute.IsNull())
            {
                throw new InvalidOperationException(
                    string.Format("Cannot build query.  Property has no metadata attributes: {0}", property.Name));
            }
            return attribute;
        }

        protected bool IsNullConstant(Expression exp)
        {
            return (exp.NodeType == ExpressionType.Constant && ((ConstantExpression)exp).Value == null);
        }

        private bool ParseOrderByExpression(MethodCallExpression expression, string order)
        {
            UnaryExpression unary = (UnaryExpression)expression.Arguments[1];
            LambdaExpression lambdaExpression = (LambdaExpression)unary.Operand;

            lambdaExpression = (LambdaExpression)Evaluator.PartialEval(lambdaExpression);

            MemberExpression body = lambdaExpression.Body as MemberExpression;
            if (body != null)
            {
                if (string.IsNullOrEmpty(_orderByClause))
                {
                    _orderByClause = string.Format("{0} {1}", body.Member.Name, order);
                }
                else
                {
                    _orderByClause = string.Format("{0}, {1} {2}", _orderByClause, body.Member.Name, order);
                }

                return true;
            }

            return false;
        }

        private string ParseStartsWithExpression(MethodCallExpression expression)
        {
            var valueExpression = (ConstantExpression) expression.Arguments[0];

            var value = valueExpression.Value.ToString();

            return value;
        }

        private object ParseEqualsExpression(MethodCallExpression expression)
        {
            object value = null;

            if (expression.Arguments[0] is ConstantExpression)
            {
                var valueExpression = (ConstantExpression) expression.Arguments[0];
                value = valueExpression.Value;
            }
            else if (expression.Arguments[0] is System.Linq.Expressions.MemberExpression)
            {
                var valueExpression = (MemberExpression) expression.Arguments[0];

                value = GetLocalValue(valueExpression);
            }

            return value;
        }

        private bool ParseTakeExpression(MethodCallExpression expression)
        {
            ConstantExpression sizeExpression = (ConstantExpression)expression.Arguments[1];

            int size;
            if (int.TryParse(sizeExpression.Value.ToString(), out size))
            {
                _take = size;
                return true;
            }

            return false;
        }

        private bool ParseSkipExpression(MethodCallExpression expression)
        {
            ConstantExpression sizeExpression = (ConstantExpression)expression.Arguments[1];

            int size;
            if (int.TryParse(sizeExpression.Value.ToString(), out size))
            {
                _skip = size;
                return true;
            }

            return false;
        }

        private object GetLocalValue(MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));

            var getterLambda = Expression.Lambda<Func<object>>(objectMember);

            try
            {
                var getter = getterLambda.Compile();

                return getter();
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.IndexOf("referenced from scope", System.StringComparison.Ordinal) >= 0)
                {
                    return null; 
                }

                throw;
            }
        }

        private string BuildFieldName(string fieldName)
        {
            var fieldBuilder = new StringBuilder();

            if (_tableAlias.IsNotNull())
            {
                fieldBuilder.AppendFormat("{0}.", _tableAlias);
            }

            if (_fieldPrefix.IsNotNull())
            {
                fieldBuilder.Append(_fieldPrefix);
            }

            fieldBuilder.Append(fieldName);

            return fieldBuilder.ToString();
        }

        private string BuildField(object field)
        {
            if (field.IsNull())
            {
                return String.Empty;
            }

            var type = field.GetType();

            if (type == typeof (Int32)
                || type == typeof (double)
                || type == typeof (float))
            {
                return field.ToString();
            }

            return "'{0}'".FormatString(field.ToString());
        }
    }
}
