namespace TightlyCurly.Com.Common.Data
{
    public class Join
    {
        public string LeftTableName { get; set; }
        public string LeftTableAlias { get; set; }
        public string RightTableName { get; set; }
        public string RightTableAlias { get; set; }
        public JoinType JoinType { get; set; }
        public string LeftJoinField { get; set; }
        public string RightJoinField { get; set; }
        public string LeftJoinFieldPrefix { get; set; }
        public string RightJoinFieldPrefix { get; set; }
    }
}
