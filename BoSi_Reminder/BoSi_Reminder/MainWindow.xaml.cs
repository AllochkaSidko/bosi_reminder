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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BoSi_Reminder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            InitializeComponent();
            AppDomain.CurrentDomain.ProcessExit += (sender, args) => OnExit(sender, args);
              
        }

        private void OnExit(object obj, EventArgs a)
        {
           
            Environment.Exit(0);
        }
    }
}