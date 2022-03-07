using Notifier.Commands;
using Notifier.DataLayer;

using SimpleInstaller.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

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

        Modes _currentMode = Modes.Adding;
        public Modes CurrentMode
        {
            get
            {
                return _currentMode;
            }
            set
            {
                _currentMode = value;
                OnPropertyChanged("ButtonModePath");
            }
        }
        public string ButtonModePath
        {
            get
            {
                string buttonModePath = CurrentMode == Modes.Adding ? "/resources/adding_mode_icon.png" : "/resources/selecting_mode_icon.png";
                return buttonModePath;
            }
        }


        ObservableCollection<TaskModel> existedTasks = new ObservableCollection<TaskModel>();
        private ICollectionView existedTasksView;


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


        private string filterWord;
        public string FilterWord
        {
            get
            {
                return filterWord;
            }
            set
            {
                if (value != filterWord)
                {
                    filterWord = value;
                    existedTasksView.Refresh();
                    OnPropertyChanged("FilterWord");
                }
            }
        }


        public string TaskName { get => taskName; set { taskName = value; OnPropertyChanged("TaskName"); } }
        public string TargetSpace { get => targetSpace; set { targetSpace = value; OnPropertyChanged("TargetSpace"); } }


        public RelayCommand AddNewTaskCmd { get; }

        public AddItemWindowViewModel()
        {
            service = new DBService(@"D:\Дело\das_code\dotnet\todo-list.accdb");
            AddNewTaskCmd = new RelayCommand(o => { AddNewTask(); }, AddNewTaskCanExecute);
            MessengerStatic.ShowExistedTasksQueried += MessengerStatic_ShowExistedTasksQueried;
            MessengerStatic.TaskAddedByEnter += MessengerStatic_TaskAddedByEnter;

            WireFilter();
        }

        private void MessengerStatic_TaskAddedByEnter(object obj)
        {
            AddNewTask();
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

        private void WireFilter()
        {
            existedTasksView = CollectionViewSource.GetDefaultView(existedTasks);
            existedTasksView.Filter = o => String.IsNullOrEmpty(FilterWord) ?
                    true : Regex.IsMatch(((TaskModel)o).Name, $"{FilterWord}", RegexOptions.IgnoreCase);
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

        public bool IsFinished
        {
            get
            {
                bool result = false;
                try
                {
                    service.CloseConnection();
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
