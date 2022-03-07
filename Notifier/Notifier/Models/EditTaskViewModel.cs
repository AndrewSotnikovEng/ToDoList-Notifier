using Notifier.Commands;
using Notifier.DataLayer;
using Notifier.ViewModels;
using SimpleInstaller.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifier.Models
{
    class EditTaskViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        

        DBService service;
        public EditTaskViewModel(TaskModel selectedItem)
        {
            Id = selectedItem.Id;
            Name = selectedItem.Name;
            Description = selectedItem.Description;

            OkBtnCmd = new RelayCommand(o => { RenameTask();  });
            CancelBtnCmd = new RelayCommand(o => { Cancel();  });
            MessengerStatic.TaskEditedByEnter += MessengerStatic_TaskEditedByEnter;

            service = new DBService(@"D:\Дело\das_code\dotnet\todo-list.accdb");

        }

        private void MessengerStatic_TaskEditedByEnter(object obj)
        {
            RenameTask();
        }

        private void Cancel()
        {
            MessengerStatic.NotifyEditWindowClosing();
        }

        private void RenameTask()
        {

            service.EditTask(Id, Name, Description);
            MessengerStatic.NotifyEditWindowClosing();
        }

        public RelayCommand OkBtnCmd { get; }
        public RelayCommand CancelBtnCmd { get; }
    }
}
