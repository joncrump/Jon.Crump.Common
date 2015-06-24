namespace TightlyCurly.Com.Common.Data
{
    public class BuilderOptions
    {
        public BuilderOptions() : this(BuildMode.Single, null)
        {
        }

        public BuilderOptions(BuildMode buildMode, string prefix = null)
        {
            BuildMode = buildMode;
            Prefix = prefix;
        }

        public BuildMode BuildMode { get; private set; }
        public string Prefix { get; private set; }
    }
}
