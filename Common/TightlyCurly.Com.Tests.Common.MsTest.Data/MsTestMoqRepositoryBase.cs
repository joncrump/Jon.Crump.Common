using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TightlyCurly.Com.Common.Data;
using TightlyCurly.Com.Common.Data.Repositories;

namespace TightlyCurly.Com.Tests.Common.MsTest.Data
{
    public abstract class MsTestMoqRepositoryBase<TRepository> : MsTestMoqDataItemTestBase<TRepository>
        where TRepository : DatabaseRepositoryBase
    {
        protected Mock<IDatabaseWrapper> MockDatabase { get; set; }

        public override void Setup()
        {
            base.Setup();

            MockDatabase = new Mock<IDatabaseWrapper>();

            MockDatabase
                .Setup(x => x.WithParameter(It.IsAny<DbParameter>()))
                .Returns(MockDatabase.Object);

            SetupGenericWithParameterMocks();

            MockDatabase
                .Setup(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()))
                .Returns(MockDatabase.Object);

            MockDatabase
                .Setup(x => x.CreateCommandText(It.IsAny<string>(), It.IsAny<QueryType>()))
                .Returns(MockDatabase.Object);

            ItemUnderTest.Database = MockDatabase.Object;
        }

        private void SetupGenericWithParameterMocks()
        {
            SetupGenericWithParameterForType<int>();
            SetupGenericWithParameterForType<int?>();
            SetupGenericWithParameterForType<double>();
            SetupGenericWithParameterForType<double?>();
            SetupGenericWithParameterForType<float>();
            SetupGenericWithParameterForType<float?>();
            SetupGenericWithParameterForType<short>();
            SetupGenericWithParameterForType<short?>();
            SetupGenericWithParameterForType<Guid>();
            SetupGenericWithParameterForType<Guid?>();
            SetupGenericWithParameterForType<string>();
        }

        private void SetupGenericWithParameterForType<TValue>()
        {
            MockDatabase
                .Setup(x => x.WithParameter<TValue>(It.IsAny<string>(), It.IsAny<object>(),
                    It.IsAny<ParameterDirection>()))
                .Returns(MockDatabase.Object);
        }
    }
}
