using System;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Threading;
using System.Diagnostics;

namespace timedEvent
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public DispatcherTimer timer = new DispatcherTimer();

        private void browsefile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                filepath.Text = openFileDialog.FileName;
            }
        }

        private void startbutton_Click(object sender, RoutedEventArgs e)
        {
            startbutton.IsEnabled = false;
            browsefile.IsEnabled = false;
            starttimer.IsEnabled = false;
            closetimer.IsEnabled = false;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (Convert.ToDateTime(starttimer.Text).ToLongTimeString() == DateTime.Now.AddSeconds(0).ToLongTimeString())
            {
                Process process = Process.Start(filepath.Text);
            }
            if (Convert.ToDateTime(closetimer.Text).ToLongTimeString() == DateTime.Now.AddSeconds(0).ToLongTimeString())
            {
                if (closesoftware.IsChecked == true)
                {
                    foreach (Process p in Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(filepath.Text)))
                    {
                        try
                        {
                            p.Kill();
                            p.WaitForExit();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error acquired at: "+ex);
                        }
                    }
                    startbutton.IsEnabled = true;
                    browsefile.IsEnabled = true;
                    starttimer.IsEnabled = true;
                    closetimer.IsEnabled = true;
                    timer.Stop();
                }
                else
                {
                    var p = new ProcessStartInfo("shutdown", "/s /t 0");
                    p.CreateNoWindow = true;
                    p.UseShellExecute = false;
                    Process.Start(p);
                }
            }
        }
    }
}
