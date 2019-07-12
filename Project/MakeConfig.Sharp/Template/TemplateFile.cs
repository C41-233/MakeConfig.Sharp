using System.IO;
using System.Reflection;
using MakeConfig.Output;

namespace MakeConfig.Template
{
    internal static class TemplateFile
    {

        public static void Copy(string name, IOutputWriter writer)
        {
            var thisExe = Assembly.GetExecutingAssembly();
            var stream = thisExe.GetManifestResourceStream(typeof(TemplateFile), name);
            if (stream == null)
            {
                throw new FileNotFoundException(name);
            }

            using (var reader = new StreamReader(stream))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    writer.WriteLine(line);
                }
            }
        }

    }
}
