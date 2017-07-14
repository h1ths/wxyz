using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace uvwxyz
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        /// <summary>Interaction logic for App.xaml</summary>
            #region Constants and Fields

            /// <summary>The event mutex name.</summary>
            private const string UniqueEventName = "{7BD73A70-7E98-40B0-8AB8-DB3A0FD493B4}";

            /// <summary>The unique mutex name.</summary>
            private const string UniqueMutexName = "{4DAC5E1C-1AC1-4035-985E-6F8E28A9C46A}";

            /// <summary>The event wait handle.</summary>
            private EventWaitHandle eventWaitHandle;

            /// <summary>The mutex.</summary>
            private Mutex mutex;

            #endregion

            // regionMethods

            /// <summary>The app on startup.</summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The e.</param>
            private void AppOnStartup(object sender, StartupEventArgs e)
            {
                bool isOwned;
                this.mutex = new Mutex(true, UniqueMutexName, out isOwned);
                this.eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, UniqueEventName);

                // So, R# would not give a warning that this variable is not used.
                GC.KeepAlive(this.mutex);

                if (isOwned)
                {
                    // Spawn a thread which will be waiting for our event
                    var thread = new Thread(
                        () =>
                        {
                            while (this.eventWaitHandle.WaitOne())
                            {
                                Current.Dispatcher.BeginInvoke(
                                    (Action)(() => ((MainWindow)Current.MainWindow).BringToForeground()));
                            }
                        });

                    // It is important mark it as background otherwise it will prevent app from exiting.
                    thread.IsBackground = true;

                    thread.Start();
                return;
                }
            else
            { 
                // Notify other instance so it could bring itself to foreground.
                this.eventWaitHandle.Set();

                // Terminate this instance.
                this.Shutdown();
            }
        }

        //endregion


    }
}
