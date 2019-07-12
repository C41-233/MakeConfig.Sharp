using System;
using System.Security.AccessControl;
using MakeConfig.Output;

namespace MakeConfig.Utils
{
    public static class StringUtils
    {

        public static string RemoveFirst(this string self, string value)
        {
            if (!self.StartsWith(value))
            {
                return self;
            }

            return self.Substring(value.Length);
        }

        public static string RemoveLast(this string self, string value)
        {
            if (!self.EndsWith(value))
            {
                return self;
            }

            return self.Substring(0, self.Length - value.Length);
        }

        public static string RemoveBoth(this string self, string begin, string end)
        {
            if (!self.StartsWith(begin))
            {
                return RemoveLast(self, end);
            }

            if (!self.EndsWith(end))
            {
                return RemoveFirst(self, begin);
            }

            return self.Substring(begin.Length, self.Length - begin.Length - end.Length);
        }

        public static string[] SplitAt(this string self, int index)
        {
            var rst = new string[2];
            rst[0] = self.Substring(0, index);
            rst[1] = self.Substring(index + 1);
            return rst;
        }

        public static void Block(this IOutputWriter self, Action action)
        {
            self.BeginBlock();
            action();
            self.EndBlock();
        }

    }
}
