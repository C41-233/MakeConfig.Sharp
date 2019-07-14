using System;
using System.IO;
using System.Text;

namespace MakeConfig.Output
{
    internal class FileWriter : OutputWriterBase, IDisposable
    {

        private readonly StreamWriter writer;

        public FileWriter(string file)
        {
            var fs = new FileStream(file, FileMode.Create);
            writer = new StreamWriter(fs, Encoding.UTF8);
        }


        public void Dispose()
        {
            writer.Dispose();
        }

        protected override void AppendLine()
        {
            writer.WriteLine();
        }

        protected override void Append(string value)
        {
            writer.Write(value);
        }
    }
}