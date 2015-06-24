using System.Collections.Generic;
using System.Linq.Expressions;

namespace TightlyCurly.Com.Common.Data.Tests.ValueFactoryTests
{
    public class TestableValueFactory : ValueFactory
    {
        public new Dictionary<string, LambdaExpression> Delegates
        {
            get
            {
                return base.Delegates;
            }
            set
            {
                base.Delegates = value;
            }
        }
    }
}
