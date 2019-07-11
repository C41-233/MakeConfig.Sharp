using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeConfig.Output
{

    public interface IOutputWriter
    {
        void WriteLine(string value);
        void BeginBlock();
        void EndBlock();
    }

    internal class StringWriter : IOutputWriter
    {

        private readonly StringBuilder sb = new StringBuilder();

        private int block = 0;

        public void WriteLine(string value)
        {
            WriteBlocks();
            sb.AppendLine(value);
        }

        private void WriteBlocks()
        {
            for (var i = 0; i < block; i++)
            {
                sb.Append("    ");
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

        public override string ToString()
        {
            return sb.ToString();
        }
    }

}
