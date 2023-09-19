using POSCA.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace POSCA.View.windows
{
    /// <summary>
    /// Interaction logic for wd_payTypes.xaml
    /// </summary>
    public partial class wd_payTypes : Window
    {

        public wd_payTypes()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name); }
        }



        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            isOk = false;
            this.Close();
        }

        public bool isOk { get; set; }
        public string invType { get; set; }


         private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load

            try
            {


                HelpClass.StartAwait(grid_main);



                #region translate

                if (AppSettings.lang.Equals("en"))
                {
                    //AppSettings.resourcemanager = new ResourceManager("POSCA.en_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    //AppSettings.resourcemanager = new ResourceManager("POSCA.ar_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }

                translate();
                #endregion

                await fillPaymentsTypes();
                HelpClass.EndAwait(grid_main);


                // in last
                await SetIsFocus();
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        async Task SetIsFocus()
        {
            await Task.Delay(0050);
            if (dg_payTypes.HasItems)
            {
                DataGridRow row = (DataGridRow)dg_payTypes.ItemContainerGenerator.ContainerFromIndex(0);
                if (row != null)
                {
                   
                    FocusManager.SetIsFocusScope(row, true);
                    FocusManager.SetFocusedElement(row, row);
                }
            }
        }

        private void translate()
        {
            txt_title.Text = AppSettings.resourcemanager.GetString("trPaymentMethods");
            txt_balanceTitle.Text = AppSettings.resourcemanager.GetString("trBalance");
            txt_paidTitle.Text = AppSettings.resourcemanager.GetString("paid");
            txt_valueTitle.Text = AppSettings.resourcemanager.GetString("trValue");
            txt_paymentTypesTitle.Text = AppSettings.resourcemanager.GetString("trPaymentMethods");
            txt_paid1Titles.Text = AppSettings.resourcemanager.GetString("paid"); 

            dg_payTypes.Columns[0].Header = AppSettings.resourcemanager.GetString("trPaymentType"); 

            dg_paid.Columns[0].Header = AppSettings.resourcemanager.GetString("SeuenceAbbrevation");
            dg_paid.Columns[1].Header = AppSettings.resourcemanager.GetString("trAmount");
            dg_paid.Columns[2].Header = AppSettings.resourcemanager.GetString("trPaymentType");
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
        }

        private async Task fillPaymentsTypes()
        {
            if (FillCombo.paymentTypeList == null)
                await FillCombo.RefreshPaymentTypes();

            dg_payTypes.ItemsSource = FillCombo.paymentTypeList;
            dg_payTypes.SelectedIndex = 0;

            object item = dg_payTypes.Items[0];
            dg_payTypes.SelectedItem = item;
            dg_payTypes.ScrollIntoView(item);
            var row = (DataGridRow)dg_payTypes.ItemContainerGenerator.ContainerFromIndex(0);
            //row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));


        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    //Btn_save_Click(null, null);
                }
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name); }
        }
        public static List<string> requiredControlList;
        #region events



        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {

             
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        #endregion

        #region validate - clearValidate - textChange - lostFocus - . . . . 

        string input;
        decimal _decimal = 0;
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {


                //only  digits
                TextBox textBox = sender as TextBox;
                HelpClass.InputJustNumber(ref textBox);
                if (textBox.Tag.ToString() == "int")
                {
                    Regex regex = new Regex("[^0-9]");
                    e.Handled = regex.IsMatch(e.Text);
                }
                else if (textBox.Tag.ToString() == "decimal")
                {
                    input = e.Text;
                    e.Handled = !decimal.TryParse(textBox.Text + input, out _decimal);

                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Code_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                //only english and digits
                Regex regex = new Regex("^[a-zA-Z0-9. -_?]*$");
                if (!regex.IsMatch(e.Text))
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        private void Spaces_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                e.Handled = e.Key == Key.Space;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void ValidateEmpty_TextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void validateEmpty_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        #endregion
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //if (timer != null)
                //    timer.Stop();
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private void dg_payTypes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {

                // if is return type
                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.0;
                wd_getInvNumber w = new wd_getInvNumber();
                w.ShowDialog();
                if (w.isOk)
                {
                   
                }
                Window.GetWindow(this).Opacity = 1;

                HelpClass.EndAwait(grid_main);

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
