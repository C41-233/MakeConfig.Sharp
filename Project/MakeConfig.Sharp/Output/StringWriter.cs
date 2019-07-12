using System;
using System.IO;
using System.Text;

namespace MakeConfig.Output
{

    public interface IOutputWriter
    {
        void Write(string value);
        void WriteLine();
        void WriteLine(string value);
        void BeginBlock();
        void EndBlock();
    }

    internal abstract class OutputWriterBase : IOutputWriter
    {

        private int block = 0;
        private bool newLine = true;

        public void Write(string value)
        {
            if (newLine)
            {
                newLine = false;
                WriteBlocks();
            }

            Append(value);
        }

        public void WriteLine()
        {
            newLine = true;
            AppendLine();
        }

        public void WriteLine(string value)
        {
            if (newLine)
            {
                WriteBlocks();
            }

            newLine = true;
            Append(value);
            AppendLine();
        }

        private void WriteBlocks()
        {
            for (var i = 0; i < block; i++)
            {
                Append("    ");
            }
        }

        public void BeginBlock()
        {
            block++;
        }

        public void EndBlock()
        {
            block--;
        }

        protected abstract void AppendLine();

        protected abstract void Append(string value);

    }

    internal class StringWriter : OutputWriterBase
    {

        private readonly StringBuilder sb = new StringBuilder();

        public override string ToString()
        {
            return sb.ToString();
        }

        protected override void AppendLine()
        {
            sb.AppendLine();
        }

        protected override void Append(string value)
        {
            sb.Append(value);
        }
    }

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
