using netoaster;
using POSCA.Classes;
using POSCA.Classes.ApiClasses;
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

namespace POSCA.View.windows
{
    /// <summary>
    /// Interaction logic for wd_payAmount.xaml
    /// </summary>
    public partial class wd_payAmount : Window
    {

        public wd_payAmount()
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

        public SalesPayment payment { get; set; }
        public decimal Remain { get; set; }
        public bool isOk { get; set; }
        public static List<string> requiredControlList;

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load

            try
            {
                MainWindow.mainWindow.KeyUp += UserControl_KeyUp;

                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "amount", };

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

                tb_amount.Text = Remain.ToString();
                HelpClass.EndAwait(grid_main);
                Keyboard.Focus(tb_amount);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }


        private void translate()
        {
            txt_title.Text = payment.PaymentTypeName;
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_amount, AppSettings.resourcemanager.GetString("trAmountHint"));

            btn_save.Content = AppSettings.resourcemanager.GetString("trOK");

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


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                MainWindow.mainWindow.KeyUp -= UserControl_KeyUp;
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
        #region events
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
        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                if (HelpClass.validate(requiredControlList, this))
                {
                    if (decimal.Parse(tb_amount.Text) != 0)
                    {
                        payment.Amount = decimal.Parse(tb_amount.Text);
                        if(payment.Amount > Remain)
                        {
                            Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trTheRemine")+" "+ (payment.Amount - Remain).ToString(), animation: ToasterAnimation.FadeIn);
                        }
                        isOk = true;
                        this.Close();
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("ZeroAmountAlert"), animation: ToasterAnimation.FadeIn);
                }
                else
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("saveNotDoneEmptyFields"), animation: ToasterAnimation.FadeIn);
                }
                HelpClass.EndAwait(grid_main);
                Keyboard.Focus(tb_amount);

            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Escape)
                {
                    Btn_colse_Click(null, null);
                }
                else if (e.Key == Key.Return)
                    Btn_save_Click(null, null);
                else if (e.Key == Key.Decimal)
                {
                    if (tb_amount.Text != "")
                        tb_amount.Text = (decimal.Parse(tb_amount.Text) * 100).ToString();
                    Keyboard.Focus(tb_amount);

                }
                else if (e.Key == Key.Multiply)
                {
                    if (tb_amount.Text != "")
                        tb_amount.Text = (decimal.Parse(tb_amount.Text) * 1000).ToString();
                    Keyboard.Focus(tb_amount);
                }

            }
            catch
            {

            }
        }

        private void tb_amount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ValidateEmpty_TextChange(sender, e);
            }
            catch
            {

            }

        }
    }
}
