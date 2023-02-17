using Notifier.Commands;
using Notifier.DataLayer;
using Notifier.Models;
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
        string _taskName;
        public string TaskName { 
            get => _taskName; 
            set { _taskName = value; }
        }


        string _targetSpace;
        public string TargetSpace { 
            get => _targetSpace == null ? "" : _targetSpace; 
            set { _targetSpace = value; OnPropertyChanged("TargetSpace"); } 
        }

        DBService _dbService;

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
                _dbService.GetAllTasks(false).ForEach(x => existedTasks.Add(x));

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


        public RelayCommand AddNewTaskCmd { get; }

        public AddItemWindowViewModel()
        {
            AddNewTaskCmd = new RelayCommand(o => { AddNewTask(); }, AddNewTaskCanExecute);

            _dbService = SharedData.container.GetInstance<DBService>();
            SharedData.InitDbService();
            
            WireFilter();
            WireStaticBus();
        }

        private void MessengerStatic_TaskAddedByEnter(object obj)
        {
            AddNewTask();
        }

        private void MessengerStatic_ShowExistedTasksQueried(object obj)
        {
            ExistedTasks.Clear();
            _dbService.GetAllTasks(true).ForEach(x => ExistedTasks.Add(x));
        }

        public bool AddNewTaskCanExecute(object arg)
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

        public void WireStaticBus()
        {
            MessengerStatic.ShowExistedTasksQueried += MessengerStatic_ShowExistedTasksQueried;
            MessengerStatic.TaskAddedByEnter += MessengerStatic_TaskAddedByEnter;
        }

        public void UnwireStaticBus()
        {
            MessengerStatic.ShowExistedTasksQueried -= MessengerStatic_ShowExistedTasksQueried;
            MessengerStatic.TaskAddedByEnter -= MessengerStatic_TaskAddedByEnter;
        }

        private void AddNewTask()
        {
            switch (CurrentMode)
            {
                case Modes.Selecting:
                    _dbService.MoveTaskToSpace(SelectedComboItem.Id, TargetSpace);
                    break;
                case Modes.Adding:
                    _dbService.CreateNewTask(TaskName, TargetSpace);
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
