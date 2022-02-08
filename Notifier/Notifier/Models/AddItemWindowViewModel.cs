using Notifier.Commands;
using Notifier.DataLayer;

using SimpleInstaller.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifier.ViewModels
{
    enum Modes
    {
        Selecting,
        Adding
    }

    class AddItemWindowViewModel : ViewModelBase
    {
        private string taskName;
        private string targetSpace;
        DBService service;
        public Modes CurrentMode { get; set; } = Modes.Adding;


        ObservableCollection<TaskModel> existedTasks = new ObservableCollection<TaskModel>();

        public ObservableCollection<TaskModel> ExistedTasks
        {
            get
            {
                existedTasks.Clear();
                service.GetAllTasks(false).ForEach(x => existedTasks.Add(x));

                return existedTasks;
            }
            set
            {
                existedTasks = value;
                OnPropertyChanged("ExistedTasks");
            }
        }

        public TaskModel SelectedComboItem { get; set; }


        public string TaskName { get => taskName; set { taskName = value; OnPropertyChanged("TaskName"); } }
        public string TargetSpace { get => targetSpace; set { targetSpace = value; OnPropertyChanged("TargetSpace"); } }


        public RelayCommand AddNewTaskCmd { get; }

        public AddItemWindowViewModel()
        {
            service = new DBService(@"D:\Дело\das_code\dotnet\todo-list.accdb");
            AddNewTaskCmd = new RelayCommand(o => { AddNewTask(); }, AddNewTaskCanExecute);
            MessengerStatic.ShowExistedTasksQueried += MessengerStatic_ShowExistedTasksQueried;

        }

        private void MessengerStatic_ShowExistedTasksQueried(object obj)
        {
            ExistedTasks.Clear();
            service.GetAllTasks(true).ForEach(x => ExistedTasks.Add(x));
        }

        private bool AddNewTaskCanExecute(object arg)
        {
            bool result = false;
            switch (CurrentMode)
            {
                case Modes.Selecting:
                    if (TargetSpace != null && (TargetSpace.Contains("Ящик") || TargetSpace.Contains("Стол")))
                    {
                        if (SelectedComboItem != null)
                        {
                            result = true;
                        };

                    }
                    break;
                case Modes.Adding:
                    if (!string.IsNullOrEmpty(TaskName))
                    {
                        result = true;
                    };
                    break;
            }

            return result;
        }

        private void AddNewTask()
        {

            switch (CurrentMode)
            {
                case Modes.Selecting:
                    service.MoveTaskToSpace(SelectedComboItem.Id, targetSpace);
                    break;
                case Modes.Adding:
                    service.CreateNewTask(taskName, targetSpace);
                    break;
            }
            MessengerStatic.NotifyTaskAdded(null);
        }
    }
}
