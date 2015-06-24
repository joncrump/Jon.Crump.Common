using System.IO;

namespace TightlyCurly.Com.Framework.Helpers
{
    public class FileHelper : IFileHelper
    {
        public string ReadFileFromContents(string path)
        {
            Guard.EnsureIsNotNullOrEmpty("path", path);

            return File.ReadAllText(path);
        }

        public bool FileExists(string path)
        {
            Guard.EnsureIsNotNullOrEmpty("path", path);

            return File.Exists(path);
        }
    }
}
