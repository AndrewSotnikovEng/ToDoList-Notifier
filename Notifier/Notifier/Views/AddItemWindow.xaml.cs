using Notifier.ViewModels;
using SimpleInstaller.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
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
            vm.UnwireStaticBus();
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
            if ( currentMode == Modes.Adding) //filtering
            {
                ExistedTasks.Visibility = Visibility.Visible;
                FilterTextBox.Visibility = Visibility.Visible;
                TaskNameBox.Visibility = Visibility.Hidden;                
                ToggleExistedBtn.Background = new SolidColorBrush(Colors.LemonChiffon);
                ((AddItemWindowViewModel)DataContext).CurrentMode = Modes.Selecting;
                FilterTextBox.Focus();
            } else //adding
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

        private void ProcessKeys(object sender, KeyEventArgs e)
        {
            AddItemWindowViewModel vm = (AddItemWindowViewModel)DataContext;
            IInputElement focuesControl = FocusManager.GetFocusedElement(this);
            if (e.Key == Key.Return)
            {
                Title = "Processing, please wait...";
                if(vm.AddNewTaskCanExecute(null))
                {
                    MessengerStatic.NotifyAboutTaskAddingByEnter();
                }
                System.Console.WriteLine("Did you press <Enter> by misake? Nothing was writeten in the field...");
                
            } else if  (e.Key == Key.Escape) {
            {
                this.Close();
            }
                
            } else if (Keyboard.Modifiers == ModifierKeys.Control && Keyboard.IsKeyDown(Key.T))
            {
                ToggleExistedBtn_Click(null, null);
            } else if (e.Key == Key.Tab)
            {
                if ( focuesControl.GetType() == typeof(TextBox)) 
                {
                    TextBox control = (TextBox) focuesControl;
                    if (control.Name == "FilterTextBox")
                    {
                        ExistedTasks.Focus();
                        ExistedTasks.IsDropDownOpen = true;
                    }
                }
            }

        }

        private void ExistedTasks_KeyUp(object sender, KeyEventArgs e)
        {
            IInputElement focuesControl = FocusManager.GetFocusedElement(this);
            if  (e.Key == Key.Escape)
            {
                FilterTextBox.Focus();
            } else if (e.Key == Key.Tab)
            {
                if (focuesControl.GetType() == typeof(ComboBox))
                {
                    //ComboBox control = (ComboBox)focuesControl;
                    //if (control.Name == "ExistedTasks")
                    //{
                    //    TargetSpace.Focus();
                    //    TargetSpace.IsDropDownOpen = true;
                    //} else if (control.Name == "TargetSpace")
                    //{
                    //    AddNewItemOkBtn.Focus();    
                    //}
                }
            }
        }
    }
}
