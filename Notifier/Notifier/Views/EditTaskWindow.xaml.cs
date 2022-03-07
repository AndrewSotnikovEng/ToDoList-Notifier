using Notifier.Commands;
using Notifier.Models;
using Notifier.ViewModels;
using SimpleInstaller.ViewModel;
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
using System.Windows.Shapes;

namespace Notifier.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        public EditTaskWindow(TaskModel selectedTeas)
        {
            InitializeComponent();
            DataContext = new EditTaskViewModel(selectedTeas);

            MessengerStatic.EditWindowClosed += MessengerStatic_EditWindowClosed;
        }

        private void MessengerStatic_EditWindowClosed(object obj)
        {
            this.Close();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.Title = "Processing, please wait...";
                MessengerStatic.NotifyAboutTaskEditingByEnter();
            }
        }
    }
}
