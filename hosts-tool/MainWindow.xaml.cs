using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Principal;

namespace hosts_tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // check service state
        }

        public bool isAdmin()
        {
            bool isElevated;
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            return isElevated;
        }

        public void backUp()
        {
            // copy old hosts to hosts.old

        }

        public void recover()
        {
            // copy hosts.old to hosts, replacing current hosts file
        }

        // download
        public async Task DownloadFileAsync()
        {
            string url = textBox.Text;
            using (var client = new WebClient())
            {
                await client.DownloadFileTaskAsync(
                    new Uri(url),
                    "hosts"
                    );
            }
        }

        public bool isLatest()
        {
            return true;
        }
        public void service()
        {

        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            // enable scheduled job
            MessageBox.Show("This feature is under development, check back later", "Hey!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This feature is under development, check back later", "Hey!", MessageBoxButton.OK, MessageBoxImage.Information);
            // disable scheduled job

        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            // updating hosts file
            /*if (!(isLatest()))
            {
                await DownloadFileAsync();
            }*/
            if (!(isAdmin()))
            {
                MessageBox.Show("You have to be admin to modify hosts file!", "Need Admin privilege!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            if (File.Exists("hosts"))
            {
                File.Delete("hosts");
            }

            try
            {
                await DownloadFileAsync();
            }
            catch
            {
                MessageBox.Show("Could not download hosts file from the source specified!", "Download failed!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            string target = System.Environment.SystemDirectory + "\\drivers\\etc\\hosts";
            if (File.Exists(target))
            {
                File.Delete(target);
            }

            try
            {
                File.Move("hosts", target);
                MessageBox.Show("If you see no errors, your hosts file has been updated", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Could not update your hosts file, please disable your Anti-Virus first", "Update failed!", MessageBoxButton.OK, MessageBoxImage.Error);
            }           
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // recovering hosts file
            this.Close();
            //MessageBox.Show("This feature is under development, check back later", "Hey!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
