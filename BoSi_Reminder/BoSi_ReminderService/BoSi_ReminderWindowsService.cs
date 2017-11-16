using System;
using Tools;
using System.ServiceProcess;
using System.Diagnostics;
using System.ServiceModel;

namespace BoSi_ReminderService
{
    public class BoSi_ReminderWindowsService : ServiceBase
    {
            internal const string CurrentServiceName = "BoSi_ReminderService";
            internal const string CurrentServiceDisplayName = "BoSi_Reminder Service";
            internal const string CurrentServiceSource = "BoSi_ReminderServiceSource";
            internal const string CurrentServiceLogName = "BoSi_ReminderServiceLogName";
            internal const string CurrentServiceDescription = "BoSi_Reminder for learning purposes.";
            private ServiceHost _serviceHost = null;
            private EventLog _serviceEventLog;
            private void InitializeComponent()
            {
                _serviceEventLog = new EventLog();
                ((System.ComponentModel.ISupportInitialize)(_serviceEventLog)).BeginInit();
                ServiceName = CurrentServiceName;
                ((System.ComponentModel.ISupportInitialize)(_serviceEventLog)).EndInit();
            }
            public BoSi_ReminderWindowsService()
            {
                ServiceName = CurrentServiceName;
                InitializeComponent();
                try
                {
                    if (!EventLog.SourceExists(CurrentServiceSource))
                        EventLog.CreateEventSource(CurrentServiceSource, CurrentServiceLogName);
                    _serviceEventLog.Source = CurrentServiceSource;
                    _serviceEventLog.Log = CurrentServiceLogName;
                    _serviceEventLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
                    _serviceEventLog.MaximumKilobytes = 8192;
                }
                catch (Exception ex)
                {
                    LogWriter.LogWrite("Failed To Initialize Log", ex);
                }
                try
                {
                    AppDomain.CurrentDomain.UnhandledException += UnhandledException;
                    try
                    {
                        _serviceEventLog.WriteEntry("Initialization");
                    }
                    catch (Exception ex)
                    {
                        LogWriter.LogWrite("",ex);
                    }
                }
                catch (Exception ex)
                {
                    _serviceEventLog.WriteEntry(string.Format("Initialization{0}{1}ex.StackTrace = {2}{1}ex.InnerException.Message = {3}", ex.Message, Environment.NewLine, ex.StackTrace, (ex.InnerException == null ? "null" : ex.InnerException.Message)),
                        EventLogEntryType.Error);
                    LogWriter.LogWrite("Initialization", ex);
                }
            }

            protected override void OnStart(string[] args)
            {
                LogWriter.LogWrite("OnStart");
                RequestAdditionalTime(120 * 1000);
#if DEBUG
                //for (int i = 0; i < 100; i++)
                //{
                //    Thread.Sleep(1000);
                //}
#endif
                try
                {
                    if (_serviceHost != null)
                        _serviceHost.Close();
                }
                catch
                {
                }
                try
                {
                    _serviceHost = new ServiceHost(typeof(BoSi_ReminderService));
                    _serviceHost.Open();
                }
                catch (Exception ex)
                {
                    _serviceEventLog.WriteEntry(string.Format("Opening The Host: {0}", ex.Message), EventLogEntryType.Error);
                    LogWriter.LogWrite("OnStart", ex);
                    throw;
                }
                LogWriter.LogWrite("Service Started");
            }

            protected override void OnStop()
            {
                LogWriter.LogWrite("OnStop");
                RequestAdditionalTime(120 * 1000);
#if DEBUG
                //Thread.Sleep(10000);
#endif
                try
                {
                    _serviceHost.Close();
                }
                catch (Exception ex)
                {
                    _serviceEventLog.WriteEntry(string.Format("Trying To Stop The Host Listener{0}", ex.Message),
                        EventLogEntryType.Error);
                    LogWriter.LogWrite("Trying To Stop The Host Listener", ex);
                }
                _serviceEventLog.WriteEntry("Service Stopped", EventLogEntryType.Information);
                LogWriter.LogWrite("Service Stopped");
            }

            private void UnhandledException(object sender, UnhandledExceptionEventArgs args)
            {
                var ex = (Exception)args.ExceptionObject;
                _serviceEventLog.WriteEntry(string.Format("UnhandledException {0} ex.Message = {1}{0} ex.StackTrace = {2}", Environment.NewLine, ex.Message, ex.StackTrace),
                    EventLogEntryType.Error);

                LogWriter.LogWrite("UnhandledException", ex);
            }
    }
}
