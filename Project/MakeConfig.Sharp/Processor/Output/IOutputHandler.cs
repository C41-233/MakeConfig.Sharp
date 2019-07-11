using MakeConfig.Excel;

namespace MakeConfig.Processor.Output
{

    public interface IOutputHandler
    {

        void Output(VirtualDataTable table);

    }

}
