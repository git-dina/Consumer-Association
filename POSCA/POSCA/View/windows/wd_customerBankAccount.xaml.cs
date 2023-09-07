using netoaster;
using POSCA.Classes;
using POSCA.Classes.ApiClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
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
    /// Interaction logic for wd_customerBankAccount.xaml
    /// </summary>
    public partial class wd_customerBankAccount : Window
    {

        public wd_customerBankAccount()
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

        IEnumerable<CustomerBankAccount> customerBankAccounts;
        CustomerBankAccount customerBankAccount = new CustomerBankAccount();
        public long customerId { get; set; }
        public string customerName { get; set; }
        public int? oldBankId { get; set; }
        public string oldIBAN { get; set; }
        public bool isOk { get; set; }
        public static List<string> requiredControlList;

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load

            try
            {


                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "NewIBAN", "NewBankId"};

                #region translate

                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }

                translate();
                #endregion
                tb_OldIBAN.Text = oldIBAN;
                tb_CustomerName.Text = customerName;
                await FillCombo.fillCustomerBanksWithDefault(cb_NewBankId);

                await fillCustomerBankAccounts();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }


        private void translate()
        {
            txt_title.Text = AppSettings.resourcemanager.GetString("ChangingBankAccountInformation");
            txt_BankAccountInformation.Text = AppSettings.resourcemanager.GetString("BankAccount");
            //txt_packageInformation.Text = AppSettings.resourcemanager.GetString("PackageInformation");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerName, AppSettings.resourcemanager.GetString("CustomerNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_OldIBAN, AppSettings.resourcemanager.GetString("OldIBANHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_NewIBAN, AppSettings.resourcemanager.GetString("NewIBANHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_NewBankId, AppSettings.resourcemanager.GetString("trBankHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            dg_customerBankAccount.Columns[0].Header = AppSettings.resourcemanager.GetString("trNo");
            dg_customerBankAccount.Columns[1].Header = AppSettings.resourcemanager.GetString("OldIBAN");
            dg_customerBankAccount.Columns[2].Header = AppSettings.resourcemanager.GetString("NewIBAN");
            dg_customerBankAccount.Columns[3].Header = AppSettings.resourcemanager.GetString("OldBank");
            dg_customerBankAccount.Columns[4].Header = AppSettings.resourcemanager.GetString("NewBank");
            dg_customerBankAccount.Columns[5].Header = AppSettings.resourcemanager.GetString("trNote");
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

        }

       private async Task fillCustomerBankAccounts()
        {
            customerBankAccounts = await customerBankAccount.GetByCustomerId(customerId);
            refreshBankAccountsView();
        }

        private void refreshBankAccountsView()
        {
            dg_customerBankAccount.ItemsSource = customerBankAccounts;
            dg_customerBankAccount.Items.Refresh();
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
        private bool isValidIBAN()
        {
            if (tb_NewIBAN.Text != "" && tb_NewIBAN.Text.Length < 30)
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("IBANLengthAlert"), animation: ToasterAnimation.FadeIn);

                return false;
            }
            else return true;
        }
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                if (HelpClass.validate(requiredControlList, this))
                {
                    if (isValidIBAN())
                    {
                        customerBankAccount.CustomerId = customerId;
                        customerBankAccount.OldIBAN = oldIBAN;
                        customerBankAccount.OldBankId = oldBankId;
                        customerBankAccount.NewIBAN = tb_NewIBAN.Text;
                        customerBankAccount.NewBankId = (int)cb_NewBankId.SelectedValue;
                        customerBankAccount.CreateUserId = MainWindow.userLogin.UserId;

                        customerBankAccount.Notes = tb_Notes.ToString();

                        customerBankAccounts = await customerBankAccount.save(customerBankAccount);
                        if (customerBankAccounts != null)
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                            refreshBankAccountsView();
                        }
                        else
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                        }
                    }
                }
                else
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("saveNotDoneEmptyFields"), animation: ToasterAnimation.FadeIn);
                }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Dg_customerBankAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
