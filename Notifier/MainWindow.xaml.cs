using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Notifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskNotifier taskbarNotifier;
        private bool reallyCloseWindow = false;
        public TaskNotifier TaskbarNotifier
        {
            get { return this.taskbarNotifier; }
        }
        public MainWindow()
        {
            InitializeComponent();
            this.taskbarNotifier = new TaskNotifier();
            this.taskbarNotifier.Show();
            var dueTime = TimeSpan.FromSeconds(5);
            var interval = TimeSpan.FromSeconds(5);

            // TODO: Add a CancellationTokenSource and supply the token here instead of None.
            RunPeriodicAsync(Act, dueTime, interval, CancellationToken.None);
        }

        private static async Task RunPeriodicAsync(Action onTick,
                                                   TimeSpan dueTime,
                                                   TimeSpan interval,
                                                   CancellationToken token)
        {
            // Initial wait time before we begin the periodic loop.
            if (dueTime > TimeSpan.Zero)
                await Task.Delay(dueTime, token);

            // Repeat this loop until cancelled.
            while (!token.IsCancellationRequested)
            {
                // Call our onTick function.
                onTick?.Invoke();

                // Wait to repeat again.
                if (interval > TimeSpan.Zero)
                    await Task.Delay(interval, token);
            }
        }

        private void Act()
        {
            this.taskbarNotifier.NotifyContent.Add(new NotifyObject("GH", "GH"));
            this.taskbarNotifier.Notify();
        }
    }
}
