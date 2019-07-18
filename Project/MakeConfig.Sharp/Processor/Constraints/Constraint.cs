
namespace MakeConfig.Processor.Constraints
{

    internal class ConstraintDescription
    {
    }

    internal static class Constraint
    {

        public static ConstraintDescription Parse(string value)
        {
            value = value.Trim();
            var description = new ConstraintDescription();
            if (string.IsNullOrEmpty(value))
            {
                return description;
            }
            var tokens = value.Split('|');
            foreach (var token in tokens)
            {
                ParseInternal(token, description);
            }
            return description;
        }

        private static void ParseInternal(string value, ConstraintDescription description)
        {
            value = value.Trim();
        }

    }

    internal interface IConstraint
    {
    }
}
