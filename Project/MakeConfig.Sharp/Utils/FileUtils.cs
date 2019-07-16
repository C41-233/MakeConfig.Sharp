using System.IO;

namespace MakeConfig.Utils
{
    internal static class FileUtils
    {

        public static string GetAbsolutePath(this FileInfo self)
        {
            return Path.GetFullPath(self.ToString());
        }

    }
}
