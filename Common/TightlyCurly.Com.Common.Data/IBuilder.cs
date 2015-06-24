namespace TightlyCurly.Com.Common.Data
{
    public interface IBuilder<in TDataSource>
    {
        TItem Build<TItem>(TDataSource dataSource, BuilderOptions builderOptions = null);
    }

    public interface IBuilder<TDataSource, TModel>
    {
        TModel Build<TModel>(TDataSource dataSource);
    }
}
