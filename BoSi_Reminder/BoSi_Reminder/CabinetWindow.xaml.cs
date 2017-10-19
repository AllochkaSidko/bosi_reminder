using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            InitializeComponent();

            CabinetViewModel = new CabinetViewModel();

            CabinetViewModel.RequestClose += Close;
            CabinetViewModel.UpdateList += UpdateItemSource;
            CabinetViewModel.FillDates += Fill;

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
        
        //заповнення масиву для виділення дат, на які встановлено нагадування
        private void Fill()
        {
            foreach (var d in StationManager.CurrentUser?.Reminders)
                Calendar.SelectedDates.Add(d.ReactDate);
        }

        private void UpdateItemSource()
        {
            ListBox.Items.Refresh();
        }

       

       
       
       

        //---------------ВИПРАВЛЕНА ПОМИЛКА---------------//
        //відображення дати нагадування при натисненні на ньому в списку, коли увімкнений режим "Показати все"
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Reminder reminder = (Reminder)ListBox.SelectedItem;

           // if (isDisplayAll)
           // {
             //   DateBlock.Content = reminder?.ReactDate.Date.ToString("dd/MM/yyyy");
            //}
        }
    }
}
