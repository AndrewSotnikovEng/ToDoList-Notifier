using Notifier.ViewModels;
using SimpleInstaller.ViewModel;
using System.Windows;
using System.Windows.Media;

namespace Notifier.Views
{
    /// <summary>
    /// Interaction logic for UserInput.xaml
    /// </summary>
    public partial class UserInput : Window
    {
        public UserInput()
        {
            InitializeComponent();
            
            DataContext = new AddItemWindowViewModel();
            MessengerStatic.TaskAdded += MessengerStatic_TaskAdded;
        }

        private void MessengerStatic_TaskAdded(object obj)
        {
            this.Close();
        }

        private void ToggleExistedBtn_Click(object sender, RoutedEventArgs e)
        {
            Modes currentMode = ((AddItemWindowViewModel) DataContext).CurrentMode;
            const string DEFAULT_COLOR = "#FFDDDDDD";
            if ( currentMode == Modes.Adding)
            {
                ExistedTasks.Visibility = Visibility.Visible;
                TaskNameBox.Visibility = Visibility.Hidden;
                ToggleExistedBtn.Background = new SolidColorBrush(Colors.LemonChiffon);
                ((AddItemWindowViewModel)DataContext).CurrentMode = Modes.Selecting;
            } else
            {
                ExistedTasks.Visibility = Visibility.Hidden;
                TaskNameBox.Visibility = Visibility.Visible;
                var converter = new BrushConverter();
                ToggleExistedBtn.Background = (Brush)converter.ConvertFromString(DEFAULT_COLOR);
                ((AddItemWindowViewModel)DataContext).CurrentMode = Modes.Adding;

                MessengerStatic.NotifyShowExistedTasks(null);
            }
        }
    }
}
