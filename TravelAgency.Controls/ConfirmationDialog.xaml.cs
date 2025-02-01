using System.Windows;

namespace TravelAgency.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy ConfirmationDialog.xaml
    /// </summary>
    public partial class ConfirmationDialog : Window
    {
        public ConfirmationDialog(string itemName)
        {
            InitializeComponent();
            this.Title = itemName;
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
