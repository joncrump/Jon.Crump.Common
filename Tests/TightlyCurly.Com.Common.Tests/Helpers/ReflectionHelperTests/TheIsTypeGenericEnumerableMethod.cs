using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Helpers;
using TightlyCurly.Com.Tests.Common.Base;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Tests.Helpers.ReflectionHelperTests
{
    [TestClass]
    public class TheIsTypeGenericEnumerableMethod : MsTestMoqTestBase<ReflectionHelper>
    {
        //[TestMethod]
        //public void WillThrowArgumentNullExceptionIfCandidateTypeIsNull()
        //{
        //    //TestRunner
        //    //    .ExecuteTest(() =>
        //    //    {
        //            Asserter
        //                .AssertExceptionIsThrown<ArgumentNullException>(
        //                    () => ItemUnderTest.IsTypeGenericEnumerable(null, It.IsAny<Type>()))
        //                .AndVerifyHasParameter("candidateType");
        //        //});
        //}

       // [TestMethod]


        /*
         public static bool IsTypeGenericEnumerable(Type candidateType, Type genericType)
        {
            throw new NotImplementedException();
            //if (typeInterface.IsGenericType
            //        && typeInterface.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            //{
            //    var implementType = typeInterface.GetGenericArguments().FirstOrDefault();

            //    if (implementType.IsInterface &&
            //        childType.GetInterfaces().SafeWhere(i => i == implementType).IsNullOrEmpty()
            //        || (implementType == childType))
            //    {
            //        return true;
            //    }
            //}

            //return false;
        }
         */
    }
}
