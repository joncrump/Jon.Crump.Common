using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data
{
    public abstract class ValueFactoryBuilderBase
    {
        protected readonly IValueFactory ValueFactory;

        protected ValueFactoryBuilderBase(IValueFactory valueFactory)
        {
            ValueFactory = Guard.EnsureIsNotNull("valueFactory", valueFactory);
        }

        protected void AddValueFactory(ValueFactoryModelBase model, 
            string loaderKey, ParameterInfo parameter = null)
        {
            if (model.IsNull())
            {
                return;
            }

            Guard.EnsureIsNotNullOrEmpty("loaderKey", loaderKey);

            var valueExpression = ValueFactory.GetValueFactory(
                loaderKey, parameter);

            valueExpression.Do(() =>
                model.ValueFactories.Add(loaderKey, valueExpression.Compile()));
        }
    }
}
