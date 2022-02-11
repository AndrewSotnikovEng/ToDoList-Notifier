using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Notifier.ViewModels;
using SimpleInstaller.ViewModel;

namespace Notifier.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = new MainWindowViewModel();
            MessengerStatic.NewTaskQueried += MessengerStatic_NewTaskQueried;
            Closing += MainWindowClosing;

        }

        private void MainWindowClosing(object sender, CancelEventArgs e)
        {
            MainWindowViewModel vm = (MainWindowViewModel)DataContext;
            if(!vm.IsFinished)
            {
                e.Cancel = true;
            }
        }

        private void MessengerStatic_NewTaskQueried(object obj)
        {
            var dialog = new AddItemWindow();
        }


        private void taskListBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

    }
}
