using System.Collections.Generic;
using System.Linq;
using System.Text;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data.QueryBuilders.Strategies.TSql
{
    public class InsertQueryBuilderStrategy : QueryBuilderStrategyBase, IQueryBuilderStrategy
    {
        public InsertQueryBuilderStrategy(IFieldHelper fieldHelper) : base(fieldHelper)
        {
        }

        public QueryInfo BuildQuery<TValue>(dynamic parameters = null) where TValue : class
        {
            Guard.EnsureIsNotNull("parameters", parameters);

            TValue model = parameters.Model; 
            bool returnNewId = parameters.ReturnNewId; 
            bool ignoreIdentity = parameters.IgnoreIdentity;
            IEnumerable<string> desiredFields = parameters.DesiredFields;
            string tableName = parameters.TableName;

            var fields = FieldHelper.BuildFields(desiredFields, model: model, ignoreIdentity: ignoreIdentity, tableName: tableName);

            var queryBuilder = new StringBuilder();

            queryBuilder.Append("INSERT {0} (".FormatString(fields.TableName));

            var columns = fields.FieldMappings.Values;

            queryBuilder.Append(columns
                .Select(f => "[{0}]".FormatString(f.FieldName))
                .ToDelimitedString(", "));

            queryBuilder.Append(") VALUES (");

            queryBuilder.Append(columns
                .Select(f => "{0}".FormatString(f.ParameterName))
                .ToDelimitedString(", "));

            queryBuilder.Append("); ");

            if (returnNewId)
            {
                queryBuilder.Append("SELECT SCOPE_IDENTITY() AS Id;");
            }

            var dbParameters = FieldHelper.ExtractParameters(fields, true);

            return new QueryInfo(queryBuilder.ToString().Trim(), new[] {fields}, dbParameters);
        }
    }
}
