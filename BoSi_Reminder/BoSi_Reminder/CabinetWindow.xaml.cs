using System;
using System.Windows;
using System.Windows.Controls;


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

        //оновлення елементів ListBox
        private void UpdateItemSource()
        {
            ListBox.Items.Refresh();
        }
    }
}
