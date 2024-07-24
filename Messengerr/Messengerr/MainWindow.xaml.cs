using System.Windows;

namespace Messenger
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UsernameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UsernamePlaceholder.Visibility = string.IsNullOrWhiteSpace(UsernameTextBox.Text) ? Visibility.Visible : Visibility.Hidden;
        }

        private void IpAddressTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            IpPlaceholder.Visibility = string.IsNullOrWhiteSpace(IpAddressTextBox.Text) ? Visibility.Visible : Visibility.Hidden;
        }

        private void CreateChat_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                MessageBox.Show("Введите имя пользователя.");
                return;
            }

            ChatWindow chatWindow = new ChatWindow(true, UsernameTextBox.Text, null);
            chatWindow.Show();
            this.Close();
        }

        private void JoinChat_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) || string.IsNullOrWhiteSpace(IpAddressTextBox.Text))
            {
                MessageBox.Show("Введите имя пользователя и IP-адрес.");
                return;
            }

            ChatWindow chatWindow = new ChatWindow(false, UsernameTextBox.Text, IpAddressTextBox.Text);
            chatWindow.Show();
            this.Close();
        }
    }
}
