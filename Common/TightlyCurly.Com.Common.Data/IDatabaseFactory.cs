using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TightlyCurly.Com.Common.Data
{
    public interface IDatabaseFactory
    {
        IDatabaseWrapper GetDatabase(string databaseName);
    }
}
