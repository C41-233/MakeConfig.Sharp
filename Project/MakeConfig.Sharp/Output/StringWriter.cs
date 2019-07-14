using System.Text;

namespace MakeConfig.Output
{
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
}
