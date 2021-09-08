using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace ProcessWatcher
{
    class EventWatcher
    {
        private MainForm outputForm;
        private ManagementEventWatcher startProcWatcher;
        private ManagementEventWatcher endProcWatcher;

        public EventWatcher(MainForm targetForm)
        {
            outputForm = targetForm;
            WatchForProcessStart();
            WatchForProcessEnd();
        }

        public void StopAllWatchers()
        {
            startProcWatcher.Stop();
            endProcWatcher.Stop();
        }

        public void WatchForProcessStart()
        {
            string queryString =
                "SELECT TargetInstance" +
                "  FROM __InstanceCreationEvent " +
                "WITHIN  .025 " +
                " WHERE TargetInstance ISA 'Win32_Process' "
                + "   AND TargetInstance.Name like '%'";

            string scope = @"\\.\root\CIMV2";

            startProcWatcher = new ManagementEventWatcher(scope, queryString);
            startProcWatcher.EventArrived += ProcessStarted;
            startProcWatcher.Start();
            
        }
        
        private void ProcessStarted(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
            string processName = targetInstance.Properties["Name"].Value.ToString();
            string exePath = targetInstance.Properties["ExecutablePath"].Value.ToString();
            string action = "Открыто";
            Console.WriteLine(String.Format("{0}   |\t   {1}   |\t   {2}   |\t   {3}", DateTime.Now, action, processName, exePath));
            outputForm.displayDelegate(String.Format("{0}   |\t   {1}   |\t   {2}   |\t   {3}   ", DateTime.Now, action, processName, exePath));
        }

        private void WatchForProcessEnd()
        {
            string queryString =
                "SELECT TargetInstance" +
                "  FROM __InstanceDeletionEvent " +
                "WITHIN  .025 " +
                " WHERE TargetInstance ISA 'Win32_Process' "
                + "   AND TargetInstance.Name like '%'";
            string scope = @"\\.\root\CIMV2";

            endProcWatcher = new ManagementEventWatcher(scope, queryString);
            endProcWatcher.EventArrived += ProcessEnded;
            endProcWatcher.Start();
        }

        private void ProcessEnded(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
            string processName = targetInstance.Properties["Name"].Value.ToString();
            string action = "Закрыто";

            Console.WriteLine(String.Format("{0}   |   {1}   |   {2}", DateTime.Now, action, processName));
            outputForm.displayDelegate(String.Format("{0}   |\t   {1}   |\t   {2}", DateTime.Now, action, processName));

        }
    }
}
