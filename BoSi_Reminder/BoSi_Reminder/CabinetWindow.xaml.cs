using System;
using System.Windows;
using DBAdapter;
using Tools;


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
            CabinetViewModel.RequestVisibilityChange += (x) => Visibility = x;

            DataContext = CabinetViewModel;
        }

        private CabinetViewModel CabinetViewModel { get; }

        private void Close(bool isQuitApp)
        {
            if (!isQuitApp)
                Close();
            else
            {
                Environment.Exit(0);
            }
        }
        
        //заповнення масиву для виділення дат, на які встановлено нагадування
        private void Fill()
        {
            try
            {
                foreach (var d in EntityWraper.GetAllRemindsCurrUser(StationManager.CurrentUser))
                    Calendar.SelectedDates.Add(d.ReactDate);
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("Exception in Fill() method while getting reminders", ex);
            }
        }

        //оновлення елементів ListBox
        private void UpdateItemSource()
        {
            ListBox.Items.Refresh();
        }
    }
}
