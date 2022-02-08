using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifier.ViewModels
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string CombinedName
        {
            get { return Name + " : " + Id; }
        }

        public TaskModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
