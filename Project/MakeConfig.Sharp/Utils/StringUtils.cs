using System;
using MakeConfig.Output;

namespace MakeConfig.Utils
{
    public static class StringUtils
    {

        public static string RemoveLast(this string self, string value)
        {
            if (!self.EndsWith(value))
            {
                return self;
            }

            return self.Substring(0, self.Length - value.Length);
        }

        public static void Block(this IOutputWriter self, Action action)
        {
            self.BeginBlock();
            action();
            self.EndBlock();
        }

    }
}
