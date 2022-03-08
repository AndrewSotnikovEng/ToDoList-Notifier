using Notifier.Commands;
using Notifier.DataLayer;
using Notifier.Models;
using Notifier.Views;
using SimpleInjector;
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
        DBService _dbService;


        public RelayCommand AddNewTaskCmd { get; }
        public RelayCommand MarkTaskAsDoneCmd { get; private set; }
        public RelayCommand RefreshListCmd { get; private set; }

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
                _dbService.GetAllTasksFromCurrent(false).ForEach(x => taskCombinedNames.Add(x));

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
            AddNewTaskCmd = new RelayCommand( o => { AddNewTask(); }, AddNewTaskCanExecute);
            MarkTaskAsDoneCmd = new RelayCommand(o => { MarkTaskAsDone(); }, MarkTaskAsDoneCanExecute);
            RefreshListCmd = new RelayCommand(o => { RefreshList(); }, RefreshListCanExecute);

            MessengerStatic.TaskAdded += MessengerStatic_TaskAdded;

            SharedData.InitContainer();
            SharedData.InitDbService();

            _dbService = SharedData.container.GetInstance<DBService>();

        }


        private bool RefreshListCanExecute(object arg)
        {
            return true;
        }

        private void RefreshList()
        {
            taskCombinedNames.Clear();
            _dbService.GetAllTasksFromCurrent(false).ForEach(x => taskCombinedNames.Add(x));
        }

        private void MessengerStatic_TaskAdded(object obj)
        {
            object o = TaskCombinedNames;
        }

        private void AddNewTask()
        {
            AddItemWindow userInput = new AddItemWindow();
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
            _dbService.MarkTaskAsDone(SelectedTask.Id);
            TaskCombinedNames.Remove(SelectedTask);

        }

        private bool AddNewTaskCanExecute(object arg)
        {
            return true;
        }

        public bool IsFinished
        {
            get
            {
                bool result = false;
                try
                {
                    _dbService.CloseConnection();
                    result = true;
                }
                catch (Exception)
                {
                    throw;
                }
                return result;
            }
        }
    }
}
