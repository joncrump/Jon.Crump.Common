using System.Collections.Generic;
using System.Data;
using TightlyCurly.Com.Common.Data.Attributes;

namespace TightlyCurly.Com.Common.Data.Tests
{
    [Table("dbo.TestTable")]
    public class TestClass
    {
        public TestClass()
        {
            Values = new List<string>();
        }

        [FieldMetadata("TestClassId", SqlDbType.Int, "@id", true)]
        public int Id { get; set; }

        [FieldMetadata("SomeFoo", SqlDbType.NVarChar, "@fooParameter")]
        public string Foo { get; set; }
        
        
        [FieldMetadata("PioneerSquareBar", SqlDbType.NVarChar, "@itsFridayLetsGoToTheBar")]
        public string Bar { get; set; }

        public IEnumerable<string> Values { get; set; }
    }

    public class DummyClass
    {
        [FieldMetadata("Baz", SqlDbType.BigInt, "@baz")]
        public string Baz { get; set; }
    }

    [Table("dbo.SomeTable")]
    public class DummyClass1
    {
        public string Baz { get; set; }
    }
}
