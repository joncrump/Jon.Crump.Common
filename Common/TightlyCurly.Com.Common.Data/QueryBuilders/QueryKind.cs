namespace TightlyCurly.Com.Common.Data.QueryBuilders
{
    public enum QueryKind
    {
        SelectSingleTable = 1,
        SelectJoinTable,
        Insert,
        Update,
        Delete,
        Count,
        PagedSingle
    }
}
