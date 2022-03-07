using Notifier.ViewModels;
using SimpleInstaller.ViewModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Notifier.Views
{
    /// <summary>
    /// Interaction logic for UserInput.xaml
    /// </summary>
    public partial class AddItemWindow : Window
    {
        public AddItemWindow()
        {
            InitializeComponent();
            
            DataContext = new AddItemWindowViewModel();
            MessengerStatic.TaskAdded += MessengerStatic_TaskAdded;
            Closing += AddItemWindow_Closing;
        }

        private void AddItemWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AddItemWindowViewModel vm = (AddItemWindowViewModel)DataContext;
            if (!vm.IsFinished)
            {
                e.Cancel = true;
            }
        }

        private void MessengerStatic_TaskAdded(object obj)
        {
            AddItemWindowViewModel vm = (AddItemWindowViewModel)DataContext;
            if (vm.IsFinished)
            {
                Close();
            }
        }

        private void ToggleExistedBtn_Click(object sender, RoutedEventArgs e)
        {
            Modes currentMode = ((AddItemWindowViewModel) DataContext).CurrentMode;
            const string DEFAULT_COLOR = "#FFDDDDDD";
            if ( currentMode == Modes.Adding)
            {
                ExistedTasks.Visibility = Visibility.Visible;
                FilterTextBox.Visibility = Visibility.Visible;
                TaskNameBox.Visibility = Visibility.Hidden;
                
                ToggleExistedBtn.Background = new SolidColorBrush(Colors.LemonChiffon);
                ((AddItemWindowViewModel)DataContext).CurrentMode = Modes.Selecting;
            } else
            {
                ExistedTasks.Visibility = Visibility.Hidden;
                FilterTextBox.Visibility = Visibility.Hidden;
                TaskNameBox.Visibility = Visibility.Visible;
                var converter = new BrushConverter();
                ToggleExistedBtn.Background = (Brush)converter.ConvertFromString(DEFAULT_COLOR);
                ((AddItemWindowViewModel)DataContext).CurrentMode = Modes.Adding;

                MessengerStatic.NotifyShowExistedTasks(null);
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.Title = "Processing, please wait...";
                MessengerStatic.NotifyAboutTaskAddingByEnter();
            }
        }
    }
}
