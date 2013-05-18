using System;
using System.ComponentModel;
using System.Diagnostics;

namespace FMUtils.TaskRUninstaller
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var args = Environment.GetCommandLineArgs();

            if (args.Length != 4)
                return;

            var self = args[0];
            var action = args[1];
            var name = args[2];
            var path = args[3];

            switch (action)
            {
                case "install":
                    // non-task invokation:
                    // set up a scheduled task named `taskname` that runs `program_path`

                    Helper.Install(self, "rununinstall", name, path);
                    break;

                case "runinstall":
                    // task invokation:
                    // try and run the `program_path`, but if it doesn't exist then uninstall the task and delete runinstaller.exe

                    try
                    {
                        Process.Start(path);
                    }
                    catch (Win32Exception e)
                    {
                        Helper.Uninstall(name);
                        Helper.Suicide(self, name);
                    }
                    break;
            }
        }
    }
}
