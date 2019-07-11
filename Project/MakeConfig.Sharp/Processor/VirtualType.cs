using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeConfig.Processor
{
    internal abstract class VirtualType
    {
        public abstract string Name { get; }
    }

    internal sealed class CustomType : VirtualType
    {
        public override string Name { get; }
    }

    internal sealed class RuntimeType : VirtualType
    {

        private readonly Type type;
        
        public RuntimeType(Type type)
        {
            this.type = type;
        }

        public override string Name
        {
            get
            {
                if (type == typeof(string))
                {
                    return "string";
                }
                return type.Name;
            }
        }
    }

}
