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
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using Notifier.DataLayer;
using Notifier.ViewModels;
using System.ComponentModel;
using System.Collections.ObjectModel;
using SimpleInstaller.ViewModel;

namespace Notifier.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {

            InitializeComponent();
            

            DataContext = new MainWindowViewModel();
            MessengerStatic.NewTaskQueried += MessengerStatic_NewTaskQueried;
            //Closing += MainWindowClosing;



        }

        private void MessengerStatic_NewTaskQueried(object obj)
        {
            var dialog = new UserInput();

        }

        //private void MainWindowClosing(object sender, CancelEventArgs e)
        //{
        //    this.Close();
        //}



        //void LoadTasksFromDb()
        //{
        //    foreach (var item in service.GetAllTaskNamesFromCurrent(true))
        //    {
        //        tasksListBox.Items.Add(item);
        //    } 
        //}

        private void taskListBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {

        }


        //void UpdateListBox()
        //{
        //    tasksListBox.Items.Clear();
        //    LoadTasksFromDb();
        //    tasksListBox.UpdateLayout();
        //}

        private void DoneBtn_Click(object sender, RoutedEventArgs e)
        {

        }




        //private void RegreshBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    UpdateListBox();
        //}
    }
}
