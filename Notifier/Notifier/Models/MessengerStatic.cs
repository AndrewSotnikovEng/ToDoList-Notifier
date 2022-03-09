using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInstaller.ViewModel
{
    public static class MessengerStatic
    {
        public static event Action<object> TaskAdded;
        public static void NotifyTaskAdded(object o)
            => TaskAdded?.Invoke(o);


        public static event Action<object> NewTaskQueried;
        public static void NotifyShowNewTaskWindow(object o = null)
            => NewTaskQueried?.Invoke(o);


        public static event Action<object> ShowExistedTasksQueried;
        public static void NotifyShowExistedTasks(object o)
            => ShowExistedTasksQueried?.Invoke(o);


        public static event Action<object> EditWindowClosed;
        public static void NotifyEditWindowClosing()
            => EditWindowClosed?.Invoke(null);


        public static event Action<object> TaskEditedByEnter;
        public static void NotifyAboutTaskEditingByEnter(object data = null)
            => TaskEditedByEnter?.Invoke(data);


        public static event Action<object> TaskAddedByEnter;
        public static void NotifyAboutTaskAddingByEnter(object data = null)
            => TaskAddedByEnter?.Invoke(data);
    }
}

