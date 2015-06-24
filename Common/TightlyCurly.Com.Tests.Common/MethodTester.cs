using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Tests.Common.Base;

namespace TightlyCurly.Com.Tests.Common
{
    public class MethodTester 
    {
        public void TestMethodParameters<TItemUnderTest>(string methodName, 
            IEnumerable<string> parametersToSkip = null)
            where TItemUnderTest : class
        {
            Guard.EnsureIsNotNullOrEmpty("methodName", methodName);

            var methodInfo = typeof(TItemUnderTest).GetMethod(methodName);

            if (methodInfo.IsNull())
            {
                throw new InvalidOperationException("The method {0} was not found."
                    .FormatString(methodName));
            }

            var parameters = methodInfo.GetParameters();

            TestParameters<TItemUnderTest>(parameters, methodInfo);
        }

        private void TestParameters<TItemUnderTest>(IEnumerable<ParameterInfo> parameters, 
            MethodInfo method, IEnumerable<string> parametersToSkip = null)
            where TItemUnderTest : class
        {
            if (parameters.IsNullOrEmpty())
            {
                return;
            }

            var instance = ConstructInstance<TItemUnderTest>();

            foreach (var parameter in parameters)
            {
                var parameter1 = parameter;
                
                if (parametersToSkip
                    .Where(p => p.ToLower() == parameter1.Name.ToLower())
                    .IsNullOrEmpty())
                {
                    continue;
                }


            }
        }

        private TItemUnderTest ConstructInstance<TItemUnderTest>()
            where TItemUnderTest : class
        {
            TItemUnderTest instance;

            var constructors = typeof(TItemUnderTest)
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public);

            var parameterlessConstructor = constructors
                .FirstOrDefault(c => c.GetParameters().IsNullOrEmpty());

            if (parameterlessConstructor.IsNotNull())
            {
                instance = Activator.CreateInstance<TItemUnderTest>();    
            }
            else
            {
                var constructor = constructors.First();
                var parameters = constructor.GetParameters();
                var values = InitializeParameters(parameters);

                instance = (TItemUnderTest)Activator.CreateInstance(typeof(TItemUnderTest), values);
            }

            return instance;
        }
    }
}
