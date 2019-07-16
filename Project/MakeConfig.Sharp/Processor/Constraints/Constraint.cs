using System;
using MakeConfig.Processor.Types;

namespace MakeConfig.Processor.Constraints
{


    internal static class Constraint
    {

        public static IConstraint[] Parse(string value)
        {
            value = value.Trim();
            if (string.IsNullOrEmpty(value))
            {
                return new IConstraint[0];
            }
            var tokens = value.Split('|');
            var rst = new IConstraint[tokens.Length];
            for (var i = 0; i < tokens.Length; i++)
            {
                var constraint = ParseInternal(tokens[i]);
                rst[i] = constraint ?? throw MakeConfigException.IllegalConstraint(tokens[i]);
            }
            return rst;
        }

        private static IConstraint ParseInternal(string value)
        {
            if (!ImportTypePool.TryGetType(value, out var type))
            {
                return null;
            }
            return new ImportTypeConstraint(type);
        }

    }

    internal interface IConstraint
    {
    }

    internal sealed class ImportTypeConstraint : IConstraint
    {

        public Type Type { get; }

        public ImportTypeConstraint(Type type)
        {
            Type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj is ImportTypeConstraint other)
            {
                return Equals(other);
            }

            return false;
        }

        private bool Equals(ImportTypeConstraint other)
        {
            return Type == other.Type;
        }

        public override int GetHashCode()
        {
            return Type?.GetHashCode() ?? 0;
        }
    }

}
