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

namespace Notifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DBService service;

        public MainWindow()
        {

            InitializeComponent();
            service = new DBService(@"D:\Дело\das_code\dotnet\todo-list.accdb");
            LoadTasksFromDb();

        }

        void LoadTasksFromDb()
        {
            foreach (var item in service.GetAllTaskNamesFromCurrent(true))
            {
                tasksListBox.Items.Add(item);
            }

 
        }

        private void taskListBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new UserInput();
            if (dialog.ShowDialog() == true)
            {

            }

            if (!string.IsNullOrEmpty(dialog.ResponseTextBox.Text))
            {
                string taskName = dialog.ResponseTextBox.Text;
                if (System.Windows.MessageBox.Show("Do you want to add new record?",
                        "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    if (dialog.CurrentComboBox.IsChecked == true)
                    {
                        service.CreateNewTask(taskName, true);                        
                    } else
                    {
                        service.CreateNewTask(taskName, false);
                    }
                    UpdateListBox();
                }
                else
                { }
            }
        }


        void UpdateListBox()
        {
            tasksListBox.Items.Clear();
            LoadTasksFromDb();
            tasksListBox.UpdateLayout();
        }

        private void DoneBtn_Click(object sender, RoutedEventArgs e)
        {
            //string selectedItem;
            //if (!String.IsNullOrEmpty(tasksListBox.SelectedItem.ToString()))
            //{
            //    selectedItem = tasksListBox.SelectedItem.ToString();
            //}
            //else
            //{
            //    return;
            //}


            //int taskId = Int32.Parse(selectedItem.Split(':')[0]);

            //SQL = $"SELECT IsTemplate FROM Tasks WHERE ID ={taskId}";
            //command = new OleDbCommand(SQL, connection);
            //OleDbDataReader reader = command.ExecuteReader();
            //reader.Read();
            //bool isTemplate = reader["IsTemplate"].ToString() == "True" ? true : false;

            //string curDate = $"{DateTime.Now.Year.ToString("00")}/{DateTime.Now.Month.ToString("00")}/{DateTime.Now.Day.ToString("00")}";
            //if (isTemplate) //if tempalte
            //{
            //    SQL = $"UPDATE Tasks SET CompletionDate = FORMAT(#{curDate}# , 'yyyy/mm/dd') WHERE ID = {taskId}";
            //    command = new OleDbCommand(SQL, connection);
            //    reader = command.ExecuteReader();
            //    reader.Read();

            //} else //if not tempalte
            //{                

            //    SQL = $"UPDATE Tasks SET Completed = True, CompletionDate = FORMAT(#{curDate}# , 'yyyy/mm/dd') WHERE ID = {taskId}";
            //    command = new OleDbCommand(SQL, connection);
            //    reader = command.ExecuteReader();
            //    reader.Read();

            //}
            //SQL = $"DELETE FROM Contexts WHERE TaskID = {taskId}";
            //command = new OleDbCommand(SQL, connection);
            //reader = command.ExecuteReader();
            //reader.Read();


            //UpdateListBox();
        }

        private void RegreshBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateListBox();
        }
    }
}
