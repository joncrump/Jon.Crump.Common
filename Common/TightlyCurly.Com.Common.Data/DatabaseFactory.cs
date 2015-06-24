namespace TightlyCurly.Com.Common.Data
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly IResolver _resolver;

        public DatabaseFactory(IResolver resolver)
        {
            _resolver = Guard.EnsureIsNotNull("resolver", resolver);
        }

        public IDatabaseWrapper GetDatabase(string databaseName)
        {
            Guard.EnsureIsNotNullOrEmpty("databaseName", databaseName);

            return _resolver.Resolve<IDatabaseWrapper>(databaseName);
        }
    }
}
