using System.IO;
using System.Reflection;
using MakeConfig.Output;

namespace MakeConfig.Template
{
    internal static class TemplateFile
    {

        private static readonly TemplateFileFormat Format = new TemplateFileFormat();

        private static readonly ConfigStub configStub = new ConfigStub();

        public static void Copy(string name, IOutputWriter writer)
        {
            var thisExe = Assembly.GetExecutingAssembly();
            var stream = thisExe.GetManifestResourceStream($"MakeConfig.Template.{name}.template");
            if (stream == null)
            {
                throw new FileNotFoundException(name);
            }

            using (var reader = new StreamReader(stream))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    line = line
                        .Replace("{", "{{")
                        .Replace("}", "}}")
                        .Replace("#{{", "{")
                        .Replace("}}#", "}");
                    writer.WriteLine(string.Format(Format, line, configStub));
                }
            }
        }

    }
}
