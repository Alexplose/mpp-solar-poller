using System.ComponentModel;
using System.IO;

namespace MppSolarPoller.Common
{
    public interface ICommand
    {
        string CommandName { get; set; }
        int ResponseSize { get; set; }

        event PropertyChangedEventHandler PropertyChanged;

        void Parse(string rawData);
        void ProcessCommand(Stream hidStream);
        string ReadCommand(Stream hidStream);
    }
}