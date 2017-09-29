using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BoSi_Reminder
{
    /// <summary>
    /// Interaction logic for CabinetWindow.xaml
    /// </summary>
    public partial class CabinetWindow : Window
    {
        public CabinetWindow()
        {
            InitializeComponent();
            CabinetViewModel = new CabinetViewModel();
            CabinetViewModel.RequestClose += Close;
            DataContext = CabinetViewModel;
        }

        private CabinetViewModel CabinetViewModel { get; set; }

       

        private void Close(bool isQuitApp)
        {
            if (!isQuitApp)
                this.Close();
            else
            {
                Environment.Exit(0);
            }
        }

        

        
private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DateBlock.Content = DateTime.Now.ToString("dd/MM/yyyy");
            this.UsernameBlock.Text = StationManager.CurrentUser.Name + " " + StationManager.CurrentUser.Surname;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DateBlock.Content = this.Calendar.SelectedDate.Value.ToString("dd/MM/yyyy");
        }
    }
}
