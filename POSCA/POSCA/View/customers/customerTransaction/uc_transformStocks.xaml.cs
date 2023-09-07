using Microsoft.Win32;
using netoaster;
using POSCA.Classes;
using POSCA.Classes.ApiClasses;
using POSCA.View.windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POSCA.View.customers.customerTransaction
{
    /// <summary>
    /// Interaction logic for uc_transformStocks.xaml
    /// </summary>
    public partial class uc_transformStocks : UserControl
    {

        public uc_transformStocks()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private static uc_transformStocks _instance;
        public static uc_transformStocks Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_transformStocks();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        CustomerTransaction customerTransaction = new CustomerTransaction();
        Customer customer1 = new Customer();
        Customer customer2 = new Customer();
        List<CustomerTransaction> customerTransactions;

        public static List<string> requiredControlList;

        public void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "BoxNumber", "ToBoxNumber", "TransactionDate", "TransactionStocksCount", "ApprovalNumber", "MeetingDate", "BondNo", "BondDate" };
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
                swapToData();


                Keyboard.Focus(dp_TransactionDate);


                Clear();
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

            txt_title.Text = AppSettings.resourcemanager.GetString("TransformStocks");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            txt_search.Text = AppSettings.resourcemanager.GetString("trSearch");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_TransactionDate, AppSettings.resourcemanager.GetString("TransactionDateHint"));

            txt_fromCustomer.Text = AppSettings.resourcemanager.GetString("FromCustomer");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_BoxNumber, AppSettings.resourcemanager.GetString("BoxNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerStatus, AppSettings.resourcemanager.GetString("CustomerStatusHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerName, AppSettings.resourcemanager.GetString("CustomerNameHint"));
            txt_StocksCountTitle.Text = AppSettings.resourcemanager.GetString("StocksCount");
            
            txt_toCustomer.Text = AppSettings.resourcemanager.GetString("ToCustomer");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ToBoxNumber, AppSettings.resourcemanager.GetString("BoxNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ToCustomerStatus, AppSettings.resourcemanager.GetString("CustomerStatusHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ToCustomerName, AppSettings.resourcemanager.GetString("CustomerNameHint"));
            txt_ToStocksCountTitle.Text = AppSettings.resourcemanager.GetString("StocksCount");


            txt_transformInformation.Text = AppSettings.resourcemanager.GetString("TransferInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_TransactionStocksCount, AppSettings.resourcemanager.GetString("TransactionStocksCountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ApprovalNumber, AppSettings.resourcemanager.GetString("BoardApprovalNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_MeetingDate, AppSettings.resourcemanager.GetString("SessionDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_BondNo, AppSettings.resourcemanager.GetString("BondNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_BondDate, AppSettings.resourcemanager.GetString("BondDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));


            dg_customerTransaction.Columns[0].Header = AppSettings.resourcemanager.GetString("TransactionDate");
            dg_customerTransaction.Columns[1].Header = AppSettings.resourcemanager.GetString("CustomerNo");
            dg_customerTransaction.Columns[2].Header = AppSettings.resourcemanager.GetString("CustomerName");
            dg_customerTransaction.Columns[3].Header = AppSettings.resourcemanager.GetString("TransactionStocksCount");
            dg_customerTransaction.Columns[4].Header = AppSettings.resourcemanager.GetString("StocksPrice");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");

            //txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trSave");
            //txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);


                    if (HelpClass.validate(requiredControlList, this))
                    {
                        customerTransaction.TransactionType = "transform";
                        customerTransaction.BoxNumber = customer1.BoxNumber;
                        customerTransaction.CustomerId = customer1.CustomerId;
                        customerTransaction.ToBoxNumber = customer2.BoxNumber;
                        customerTransaction.ToCustomerId = customer2.CustomerId;
                        customerTransaction.TransactionDate = dp_TransactionDate.SelectedDate;
                        customerTransaction.TransactionStocksCount = int.Parse(tb_TransactionStocksCount.Text);
                        customerTransaction.StocksCount = int.Parse(txt_StocksCount.Text);
                        customerTransaction.ToStocksCount = int.Parse(txt_ToStocksCount.Text);
                        customerTransaction.StocksPrice = 5;
                        customerTransaction.ApprovalNumber = tb_ApprovalNumber.Text;
                        customerTransaction.MeetingDate = dp_MeetingDate.SelectedDate;
                        customerTransaction.BondNo =int.Parse( tb_BondNo.Text);
                        customerTransaction.BondDate = dp_BondDate.SelectedDate;
                        customerTransaction.Notes = tb_Notes.Text;

                        customerTransaction.UpdateUserId = MainWindow.userLogin.UserId;

                        var res = await customerTransaction.TransformStocks(customerTransaction);
                        if (res == 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                            Clear();
                            //await Search();

                        }
                    }
                    else
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("saveNotDoneEmptyFields"), animation: ToasterAnimation.FadeIn);
                    }


                    HelpClass.EndAwait(grid_main);
                }
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void Btn_delete_Click(object sender, RoutedEventArgs e)
        {//delete
            try
            {
                // if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    /*
                    if (customerTransaction.CustomerTransactionId != 0)
                    {
                        #region
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                        w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");

                        w.ShowDialog();
                        Window.GetWindow(this).Opacity = 1;
                        #endregion

                        if (w.isOk)
                        {
                            FillCombo.customerTransactionList = await customerTransaction.delete(customerTransaction.CustomerTransactionId, MainWindow.userLogin.UserId);
                            if (FillCombo.customerTransactionList == null)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                customerTransaction.CustomerTransactionId = 0;
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                await Search();
                                await Clear();
                            }
                        }

                    }
                    */
                    HelpClass.EndAwait(grid_main);
                }
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        #endregion
        #region events

        private async void Btn_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                await Search();

            }
            catch
            {

            }
        }

        private void tb_search_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    Btn_search_Click(btn_search, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                await Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private  void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Clear();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void Dg_customerTransaction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection

                if (dg_customerTransaction.SelectedIndex != -1)
                {
                    customerTransaction = dg_customerTransaction.SelectedItem as CustomerTransaction;
                    this.DataContext = customerTransaction;
                }
                HelpClass.clearValidate(requiredControlList, this);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {

                HelpClass.StartAwait(grid_main);

                // await RefreshCustomerTransactionsList();
                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion
        #region Refresh & Search
        async Task Search()
        {
            customerTransactions = await customerTransaction.SearchTransactions("transform",tb_search.Text);
            RefreshCustomerTransactionsView();
        }
        //async Task<IEnumerable<CustomerTransaction>> RefreshCustomerTransactionsList()
        //{

        //    await customerTransaction.get(true);

        //    return FillCombo.customerTransactionList;

        //    return customerTransactions;
        //}
        void RefreshCustomerTransactionsView()
        {
            dg_customerTransaction.ItemsSource = customerTransactions;
            txt_count.Text = customerTransactions.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void  Clear()
        {
            customerTransaction = new CustomerTransaction();

            this.DataContext = new CustomerTransaction();
            dg_customerTransaction.SelectedIndex = -1;

            #region clear inputs
            tb_BoxNumber.Text = "";
            tb_CustomerName.Text = "";
            tb_CustomerStatus.Text = "";
            tb_ToBoxNumber.Text = "";
            tb_ToCustomerName.Text = "";
            tb_ToCustomerStatus.Text = "";
            txt_StocksCount.Text = "";
            txt_ToStocksCount.Text = "";

            #endregion
            inputEditable();
            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }
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


        #region swap
        private void btn_columnSwap_Click(object sender, RoutedEventArgs e)
        {
            ColumnDefinition cd_gridMain3 = new ColumnDefinition();
            cd_gridMain3.Width = cd_gridMain1.Width;
            cd_gridMain1.Width = cd_gridMain2.Width;
            cd_gridMain2.Width = cd_gridMain3.Width;
        }

        private void Btn_swapToSearch_Click(object sender, RoutedEventArgs e)
        {
            cd_gridMain1.Width = new GridLength(1, GridUnitType.Star);
            cd_gridMain2.Width = new GridLength(0, GridUnitType.Star);

            //if (tb_search.Text != "")
            //    Btn_search_Click(null, null);
            //else
            //{
            //    customerTransactions = new List<CustomerTransaction>();
            //    RefreshCustomerTransactionsView();
            //}

        }
        void swapToData()
        {
            cd_gridMain1.Width = new GridLength(0, GridUnitType.Star);
            cd_gridMain2.Width = new GridLength(1, GridUnitType.Star);

            this.DataContext = customerTransaction;

            dp_TransactionDate.SelectedDate = customerTransaction.TransactionDate;
            tb_BoxNumber.Text = customerTransaction.BoxNumber.ToString();
            tb_CustomerName.Text = customerTransaction.CustomerName;
            tb_ToBoxNumber.Text = customerTransaction.ToBoxNumber.ToString();
            tb_ToCustomerName.Text = customerTransaction.ToCustomerName;
            txt_StocksCount.Text = customerTransaction.StocksCount.ToString();
            txt_ToStocksCount.Text = customerTransaction.ToStocksCount.ToString();

            inputEditable();
        }

        private void inputEditable()
        {
            if (customerTransaction.TransactionId == 0)
            {
                tb_BoxNumber.IsEnabled = true;
                tb_ToBoxNumber.IsEnabled = true;
                tb_TransactionStocksCount.IsEnabled = true;
                tb_ApprovalNumber.IsEnabled = true;
                tb_BondNo.IsEnabled = true;
                tb_Notes.IsEnabled = true;
                dp_BondDate.IsEnabled = true;
                dp_MeetingDate.IsEnabled = true;
                btn_update.IsEnabled = true;
            }
            else
            {
                tb_BoxNumber.IsEnabled = false;
                tb_ToBoxNumber.IsEnabled = false;
                tb_TransactionStocksCount.IsEnabled = false;
                tb_ApprovalNumber.IsEnabled = false;
                tb_BondNo.IsEnabled = false;
                tb_Notes.IsEnabled = false;
                dp_BondDate.IsEnabled = false;
                dp_MeetingDate.IsEnabled = false;
                btn_update.IsEnabled = false;
            }
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            swapToData();
        }

        #endregion

        private void Btn_archiving_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_updateIBAN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_personalDocuments_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_pastProfits_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tb_TransactionStocksCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ValidateEmpty_TextChange(sender, e);
                int stocksCount = 0;
                if (tb_TransactionStocksCount.Text != "")
                    stocksCount = int.Parse(tb_TransactionStocksCount.Text);

                //check stock count
                var difference = int.Parse(txt_StocksCount.Text) - stocksCount;
                if (difference < 5)
                {
                    tb_TransactionStocksCount.Text = "";
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("StocksNumberNotAllowed"), animation: ToasterAnimation.FadeIn);
                }

            }
            catch { }
        }

        private async void tb_BoxNumber_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return && tb_BoxNumber.Text != "")
                {
                    customer1 = null;
                    HelpClass.StartAwait(grid_main);

                    customer1 = await FillCombo.customer.GetByBoxNumber(long.Parse(tb_BoxNumber.Text));

                    if (customer1 != null)
                    {
                        if (customer1.CustomerStatus != "continouse")
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CustomerNotContinouse"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            tb_CustomerName.Text = customer1.Name;
                            tb_CustomerStatus.Text = AppSettings.resourcemanager.GetString(customer1.CustomerStatus) ;
                            txt_StocksCount.Text = customer1.AllStocksCount.ToString();


                        }
                    }
                    else
                    {
                        tb_BoxNumber.Text = "";
                        tb_CustomerName.Text = "";
                        tb_CustomerStatus.Text = "";
                        txt_StocksCount.Text = "";
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("NumberNotTrue"), animation: ToasterAnimation.FadeIn);
                    }
                    HelpClass.EndAwait(grid_main);

                }
            }
            catch
            {
                HelpClass.EndAwait(grid_main);
            }
        }

        private async void tb_ToBoxNumber_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return && tb_ToBoxNumber.Text != "")
                {
                    if (tb_BoxNumber.Text.Equals(tb_ToBoxNumber.Text))
                    {
                        tb_ToBoxNumber.Text = "";
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CannotTransferToSameBox"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                    {
                        customer2 = null;
                        HelpClass.StartAwait(grid_main);

                        customer2 = await FillCombo.customer.GetByBoxNumber(long.Parse(tb_ToBoxNumber.Text));

                        if (customer2 != null)
                        {
                            if (customer2.CustomerStatus != "continouse")
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CustomerNotContinouse"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                tb_ToCustomerName.Text = customer2.Name;
                                tb_ToCustomerStatus.Text = AppSettings.resourcemanager.GetString(customer2.CustomerStatus);
                                txt_ToStocksCount.Text = customer2.AllStocksCount.ToString();


                            }
                        }
                        else
                        {
                            tb_ToBoxNumber.Text = "";
                            tb_ToCustomerName.Text = "";
                            tb_ToCustomerStatus.Text = "";
                            txt_ToStocksCount.Text = "";
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("NumberNotTrue"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    HelpClass.EndAwait(grid_main);

                }
            }
            catch
            {
                HelpClass.EndAwait(grid_main);
            }
        }
       
    }
}
