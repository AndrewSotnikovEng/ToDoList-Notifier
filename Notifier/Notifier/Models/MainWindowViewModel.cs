using Notifier.Commands;
using Notifier.DataLayer;
using Notifier.Views;
using SimpleInstaller.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifier.ViewModels
{
    public class MainWindowViewModel : ViewModels.ViewModelBase
    {
        DBService service;

        public RelayCommand AddNewTaskCmd { get; }
        public RelayCommand MarkTaskAsDoneCmd { get; private set; }

        ObservableCollection<TaskModel> taskCombinedNames = new ObservableCollection<TaskModel>();

        TaskModel selectedTask;

        public TaskModel SelectedTask
        {
            get => selectedTask;
            set => selectedTask = value;
        }



        public ObservableCollection<TaskModel> TaskCombinedNames
        {
            get
            {
                taskCombinedNames.Clear();
                service.GetAllTasksFromCurrent(false).ForEach(x => taskCombinedNames.Add(x));

                return taskCombinedNames;
            }
            set
            {
                taskCombinedNames = value;
                OnPropertyChanged("TaskCombinedNames");
            }
        }

        public MainWindowViewModel()
        {
            service = new DBService(@"D:\Дело\das_code\dotnet\todo-list.accdb");

            AddNewTaskCmd = new RelayCommand( o => { AddNewTask(); }, AddNewTaskCanExecute);
            MarkTaskAsDoneCmd = new RelayCommand(o => { MarkTaskAsDone(); }, MarkTaskAsDoneCanExecute);

            MessengerStatic.TaskAdded += MessengerStatic_TaskAdded;

        }

        private void MessengerStatic_TaskAdded(object obj)
        {
            object o = TaskCombinedNames;
        }

        private void AddNewTask()
        {
            UserInput userInput = new UserInput();
            userInput.Show();
        }

        private bool MarkTaskAsDoneCanExecute(object arg)
        {
            bool result = false;
            if (SelectedTask != null)
            {
                result = true;
            }

            return result;
        }

        private void MarkTaskAsDone()
        {
            service.MarkTaskAsDone(SelectedTask.Id);
            TaskCombinedNames.Remove(SelectedTask);



        }

        private bool AddNewTaskCanExecute(object arg)
        {
            return true;
        }



    }
}
