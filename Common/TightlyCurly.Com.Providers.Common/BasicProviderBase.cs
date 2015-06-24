using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Helpers;

namespace TightlyCurly.Com.Providers.Common
{
    public abstract class BasicProviderBase
    {
        protected readonly IMapper Mapper;

        protected BasicProviderBase(IMapper mapper)
        {
            Mapper = Guard.EnsureIsNotNull("mapper", mapper);
        }
    }
}
