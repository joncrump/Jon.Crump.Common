namespace TightlyCurly.Com.Framework.Helpers
{
    public interface IFileHelper
    {
        string ReadFileFromContents(string path);
        bool FileExists(string path);
    }
}
