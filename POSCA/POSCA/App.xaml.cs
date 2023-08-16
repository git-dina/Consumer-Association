using POSCA.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace POSCA
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                EventManager.RegisterClassHandler(typeof(TextBox), TextBox.KeyDownEvent, new KeyEventHandler(NextControl_KeyDown));
                EventManager.RegisterClassHandler(typeof(ComboBox), ComboBox.KeyDownEvent, new KeyEventHandler(NextControl_KeyDown));
                EventManager.RegisterClassHandler(typeof(DatePicker), DatePicker.KeyDownEvent, new KeyEventHandler(NextControl_KeyDown));
                EventManager.RegisterClassHandler(typeof(ToggleButton), ToggleButton.KeyDownEvent, new KeyEventHandler(NextControl_KeyDown));

                MainWindow window = new MainWindow();
                MainWindow.Show();

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        void NextControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender.GetType().Name == "TextBox")
            {
                if (e.Key == Key.Enter & (sender as TextBox).AcceptsReturn == false & !(sender as TextBox).Name.Contains("tb_search") & !(sender as TextBox).Name.Contains("tb_PurchaseInvNumber") & !(sender as TextBox).Name.Contains("tb_IBAN")  ) MoveToNextUIElement(e);
            }
            else if (sender.GetType().Name == "ComboBox")
            {
                if (e.Key == Key.Enter ) MoveToNextUIElement(e);
            }
            else if (sender.GetType().Name == "DatePickerTextBox")
            {
                if (e.Key == Key.Enter ) MoveToNextUIElement(e);
            }
            else if (sender.GetType().Name == "ToggleButton")
            {
                if (e.Key == Key.Enter ) MoveToNextUIElement(e);
            }
        }


        void MoveToNextUIElement(KeyEventArgs e)
        {
            // Creating a FocusNavigationDirection object and setting it to a
            // local field that contains the direction selected.
            FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;

            // MoveFocus takes a TraveralReqest as its argument.
            TraversalRequest request = new TraversalRequest(focusDirection);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                if (elementWithFocus.MoveFocus(request)) e.Handled = true;
            }
        }
    }
}
