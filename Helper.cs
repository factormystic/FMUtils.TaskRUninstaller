using System;
using System.Diagnostics;
using System.IO;
using Scheduler = Microsoft.Win32.TaskScheduler;

namespace FMUtils.TaskRUninstaller
{
    public class Helper
    {
        public static bool IsTaskInstalled(string name)
        {
            using (var ts = new Scheduler.TaskService())
                return ts.FindTask(name) != null;
        }

        public static void Install(string self, string action, string name, string path)
        {
            if (IsTaskInstalled(name))
                return;

            using (var ts = new Scheduler.TaskService())
            {
                var task = ts.NewTask();

                task.Principal.RunLevel = Scheduler.TaskRunLevel.Highest;

                task.Settings.DisallowStartIfOnBatteries = false;
                task.Settings.ExecutionTimeLimit = TimeSpan.Zero;

                task.Triggers.Add(new Scheduler.LogonTrigger() { UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name });
                task.Actions.Add(new Scheduler.ExecAction(self, action + " " + name + " " + path));

                ts.RootFolder.RegisterTaskDefinition(name, task);
            }
        }

        public static void Uninstall(string name)
        {
            if (!IsTaskInstalled(name))
                return;

            using (var ts = new Scheduler.TaskService())
                ts.RootFolder.DeleteTask(name);
        }

        public static bool Toggle(string self, string name, string path)
        {
            if (IsTaskInstalled(name))
            {
                Uninstall(name);
                return false;
            }
            else
            {
                Install(self, "runinstall", name, path);
                return true;
            }
        }

        internal static void Suicide(string self, string name)
        {
            var ParentDir = Directory.GetParent(self);

            if (ParentDir.Name != name)
                return;

            Process.Start(new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = "/c timeout 5 & rmdir /q /s " + ParentDir.FullName,

                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            });
        }
    }
}
