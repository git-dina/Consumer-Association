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

namespace POSCA.View.customers
{
    /// <summary>
    /// Interaction logic for uc_fundChange.xaml
    /// </summary>
    public partial class uc_fundChange : UserControl
    {

        public uc_fundChange()
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
        private static uc_fundChange _instance;
        public static uc_fundChange Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_fundChange();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        FundChange fundChange = new FundChange();
        Customer customer;
        Customer customer2;
        List<FundChange> fundChanges;
        string searchText = "";
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
                requiredControlList = new List<string> { "CustomerId", "ChangeDate" };
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();
                swapToData();


                Keyboard.Focus(cb_ChangeType);
                FillCombo.fillChangeBoxType(cb_ChangeType);

                //await Search();
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
            txt_title.Text = AppSettings.resourcemanager.GetString("FundNumberChange");
            
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            txt_search.Text = AppSettings.resourcemanager.GetString("trSearch");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_ChangeType, AppSettings.resourcemanager.GetString("ChangeTypeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_ChangeDate, AppSettings.resourcemanager.GetString("ProcessDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerId, AppSettings.resourcemanager.GetString("CustomerNoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_SecondCustomerId, AppSettings.resourcemanager.GetString("CustomerNoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_OldFundNumber, AppSettings.resourcemanager.GetString("BoxNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_NewFundNumber, AppSettings.resourcemanager.GetString("BoxNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerName, AppSettings.resourcemanager.GetString("CustomerNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_SecondCustomerName, AppSettings.resourcemanager.GetString("CustomerNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerStatus, AppSettings.resourcemanager.GetString("CustomerStatusHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_SecondCustomerStatus, AppSettings.resourcemanager.GetString("CustomerStatusHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_JoinDate, AppSettings.resourcemanager.GetString("JoinDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_SecondJoinDate, AppSettings.resourcemanager.GetString("JoinDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CivilNum, AppSettings.resourcemanager.GetString("CivilNumHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_SecondCivilNum, AppSettings.resourcemanager.GetString("CivilNumHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_MobileNumber, AppSettings.resourcemanager.GetString("MobileNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_SecondMobileNumber, AppSettings.resourcemanager.GetString("MobileNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Reason, AppSettings.resourcemanager.GetString("TransferReasonHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_EmptyFundNumber, AppSettings.resourcemanager.GetString("UnloadedBoxNoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ChangeToFundNumber, AppSettings.resourcemanager.GetString("NewFundNumberHint"));


            txt_clearBoxNumberTitle.Text = AppSettings.resourcemanager.GetString("ClearBoxNumber");
            txt_changeBoxNumberTitle.Text = AppSettings.resourcemanager.GetString("ChangeBoxNumber");
            txt_switchBoxNumberTitle.Text = AppSettings.resourcemanager.GetString("OtherContributorData");



            dg_fundChange.Columns[0].Header = AppSettings.resourcemanager.GetString("ChangeType");
            dg_fundChange.Columns[1].Header = AppSettings.resourcemanager.GetString("CustomerNo");
            dg_fundChange.Columns[2].Header = AppSettings.resourcemanager.GetString("CustomerName");
            dg_fundChange.Columns[3].Header = AppSettings.resourcemanager.GetString("BoxNumber");
            dg_fundChange.Columns[4].Header = AppSettings.resourcemanager.GetString("NewFundNumber");
            dg_fundChange.Columns[5].Header = AppSettings.resourcemanager.GetString("ProcessDate");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");

            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trSave");

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
                        FundChange secondFundChange = new FundChange();

                        fundChange.ChangeType = cb_ChangeType.SelectedValue.ToString();
                        fundChange.ChangeDate = dp_ChangeDate.SelectedDate;
                        fundChange.CustomerId = long.Parse(tb_CustomerId.Text);
                        fundChange.OldFundNumber = long.Parse( tb_OldFundNumber.Text);
                        fundChange.Reason = tb_Reason.Text;
                        switch (cb_ChangeType.SelectedValue)
                        {
                            case "change":
                                fundChange.NewFundNumber = long.Parse(tb_ChangeToFundNumber.Text);
                                break;
                            case "exchange":
                                fundChange.SecondCustomerId = long.Parse(tb_SecondCustomerId.Text);
                                fundChange.NewFundNumber = long.Parse(tb_NewFundNumber.Text);

                                secondFundChange.ChangeType = cb_ChangeType.SelectedValue.ToString();
                                secondFundChange.ChangeDate = dp_ChangeDate.SelectedDate;
                                secondFundChange.CustomerId = long.Parse(tb_SecondCustomerId.Text);
                                secondFundChange.SecondCustomerId = long.Parse(tb_CustomerId.Text);
                                secondFundChange.OldFundNumber = long.Parse(tb_NewFundNumber.Text);
                                secondFundChange.NewFundNumber = long.Parse(tb_OldFundNumber.Text);
                                secondFundChange.Reason = tb_Reason.Text;
                                secondFundChange.CreateUserId = MainWindow.userLogin.userId;

                                break;
                            case "emptying":
                                fundChange.NewFundNumber = long.Parse(tb_EmptyFundNumber.Text);

                                break;

                        }

                        fundChange.CreateUserId = MainWindow.userLogin.userId;

                        var res = await fundChange.Save(fundChange,secondFundChange);
                        if (res == 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                            Clear();

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

        private void Btn_clear_Click(object sender, RoutedEventArgs e)
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
        private async void Dg_fundChange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection

                if (dg_fundChange.SelectedIndex != -1)
                {
                    fundChange = dg_fundChange.SelectedItem as FundChange;

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

                // await RefreshFundChangesList();
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
            /*
            fundChanges = await fundChange.SearchTransactions("add", tb_search.Text);
            RefreshFundChangesView();
            */
        }
        //async Task<IEnumerable<FundChange>> RefreshFundChangesList()
        //{

        //    await fundChange.get(true);

        //    return FillCombo.fundChangeList;

        //    return fundChanges;
        //}
        void RefreshFundChangesView()
        {
            dg_fundChange.ItemsSource = fundChanges;
            txt_count.Text = fundChanges.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        private void Clear()
        {
          
            fundChange = new FundChange();
            this.DataContext = new FundChange();
            dg_fundChange.SelectedIndex = -1;

            #region clear inputs
            tb_CustomerId.Text = "";
            tb_CustomerName.Text = "";
            dp_JoinDate.SelectedDate = null;
            tb_OldFundNumber.Text ="";
            tb_CustomerStatus.Text = "";
            dp_JoinDate.SelectedDate = null;
            tb_CivilNum.Text ="";
            tb_MobileNumber.Text = "";
            tb_Reason.Text = "";

            tb_SecondCustomerId.Text = "";
            tb_SecondCivilNum.Text = "";
            tb_SecondCustomerName.Text = "";
            tb_SecondCustomerStatus.Text = "";
            tb_SecondMobileNumber.Text = "";
            tb_NewFundNumber.Text = "";
            dp_SecondJoinDate.SelectedDate = null;

            tb_EmptyFundNumber.Text = "";
            tb_ChangeToFundNumber.Text = "";
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
            //    fundChanges = new List<FundChange>();
            //    RefreshFundChangesView();
            //}

        }
        void swapToData()
        {
            cd_gridMain1.Width = new GridLength(0, GridUnitType.Star);
            cd_gridMain2.Width = new GridLength(1, GridUnitType.Star);

            this.DataContext = fundChange;
            /*
            tb_CustomerId.Text = fundChange.CustomerId.ToString();
            dp_TransactionDate.SelectedDate = fundChange.TransactionDate;
            tb_CustomerName.Text = fundChange.CustomerName;
            dp_JoinDate.SelectedDate = fundChange.JoinDate;
            tb_StocksCount.Text = fundChange.StocksCount.ToString();
            tb_TotalPrice.Text = HelpClass.DecTostring(fundChange.StocksCount * fundChange.StocksPrice);
            if (fundChange.JoinDate != null)
            {
                DateTime zeroTime = new DateTime(1, 1, 1);
                TimeSpan span1 = DateTime.Now - (DateTime)fundChange.JoinDate;
                txt_JoinYear.Text = ((zeroTime + span1).Year - 1).ToString();
                txt_JoinMonth.Text = ((zeroTime + span1).Month - 1).ToString();
                txt_JoinDay.Text = ((zeroTime + span1).Day).ToString();
            }
            */
            inputEditable();
        }

        private void inputEditable()
        {
            if (fundChange.Id == 0)
            {
                tb_CustomerId.IsEnabled = true;
                tb_SecondCustomerId.IsEnabled = true;
                tb_Reason.IsEnabled = true;
                dp_ChangeDate.IsEnabled = true;
                cb_ChangeType.IsEnabled = true;
                btn_clearSecond.IsEnabled = true;
                btn_update.IsEnabled = true;
            }
            else
            {
                tb_CustomerId.IsEnabled = false;
                tb_SecondCustomerId.IsEnabled = false;
                tb_Reason.IsEnabled = false;
                dp_ChangeDate.IsEnabled = false;
                cb_ChangeType.IsEnabled = false;
                btn_clearSecond.IsEnabled = false;
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

        private async void tb_CustomerId_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return && tb_CustomerId.Text != "")
                {

                    customer = null;
                    tb_ChangeToFundNumber.Text = "";
                    HelpClass.StartAwait(grid_main);
                    customer = await FillCombo.customer.GetById(long.Parse(tb_CustomerId.Text));

                    switch (cb_ChangeType.SelectedValue)
                    {
                        case "emptying":
                            if (customer != null && (customer.CustomerStatus != "withdrawn" || customer.IsArchived == true))
                            {
                                tb_CustomerId.Text = "";
                                tb_CustomerName.Text = "";
                                tb_OldFundNumber.Text = "";
                                tb_CustomerStatus.Text = "";
                                dp_JoinDate.SelectedDate = null;
                                tb_CivilNum.Text = "";
                                tb_MobileNumber.Text = "";

                                customer = null;
                                
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("BoxCannotBeDumped"), animation: ToasterAnimation.FadeIn);
                                HelpClass.EndAwait(grid_main);
                                return;
                            }
                            else if (customer != null)
                            {
                                var maxDumped = await fundChange.GetMaxDumpedBoxNum();
                                tb_EmptyFundNumber.Text = maxDumped.ToString();
                            }

                            break;
                        default:
                         
                            break;
                    }
                   

                    if (customer != null)
                    {
                        if (customer.CustomerStatus != "continouse")
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CustomerNotContinouse"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            tb_CustomerName.Text = customer.Name;
                            tb_OldFundNumber.Text = customer.BoxNumber.ToString();
                            tb_CustomerStatus.Text = AppSettings.resourcemanager.GetString(customer.CustomerStatus);
                            dp_JoinDate.SelectedDate = customer.JoinDate;
                            tb_CivilNum.Text = customer.CivilNum;
                            tb_MobileNumber.Text = customer.customerAddress.MobileNumber;

                        }
                    }
                    else
                    {
                        tb_CustomerId.Text = "";
                        tb_CustomerName.Text = "";
                        tb_OldFundNumber.Text = "";
                        tb_CustomerStatus.Text = "";
                        dp_JoinDate.SelectedDate = null;
                        tb_CivilNum.Text = "";
                        tb_MobileNumber.Text = "";
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

        private async void tb_SecondCustomerId_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return && tb_CustomerId.Text == "")
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("SelectFirstContributor"), animation: ToasterAnimation.FadeIn);
                }
                else if (e.Key == Key.Return &&  tb_CustomerId.Text.Equals(tb_SecondCustomerId.Text))
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("SecondDifferentFromFirst"), animation: ToasterAnimation.FadeIn);
                }
               else if (e.Key == Key.Return && tb_SecondCustomerId.Text != "")
                {

                    customer2 = null;
                    HelpClass.StartAwait(grid_main);
                    customer2 = await FillCombo.customer.GetById(long.Parse(tb_SecondCustomerId.Text));

                  

                    if (customer2 != null)
                    {
                        if (customer2.CustomerStatus != "continouse")
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CustomerNotContinouse"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            tb_SecondCustomerName.Text = customer2.Name;
                            tb_NewFundNumber.Text = customer2.BoxNumber.ToString();
                            tb_SecondCustomerStatus.Text = AppSettings.resourcemanager.GetString(customer2.CustomerStatus);
                            dp_SecondJoinDate.SelectedDate = customer2.JoinDate;
                            tb_SecondCivilNum.Text = customer2.CivilNum;
                            tb_SecondMobileNumber.Text = customer2.customerAddress.MobileNumber;

                        }
                    }
                    else
                    {
                        tb_SecondCustomerId.Text = "";
                        tb_SecondCustomerName.Text = "";
                        tb_NewFundNumber.Text = "";
                        tb_SecondCustomerStatus.Text = "";
                        dp_SecondJoinDate.SelectedDate = null;
                        tb_SecondCivilNum.Text = "";
                        tb_SecondMobileNumber.Text = "";
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

        private void Btn_clearSecond_Click(object sender, RoutedEventArgs e)
        {
            tb_SecondCustomerId.Text = "";
            tb_SecondCivilNum.Text = "";
            tb_SecondCustomerName.Text = "";
            tb_SecondCustomerStatus.Text = "";
            tb_SecondMobileNumber.Text = "";
            tb_NewFundNumber.Text = "";
            dp_SecondJoinDate.SelectedDate = null;
        }

        private void cb_ChangeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cb_ChangeType.SelectedValue)
            {
                case "change":
                    sp_changeBoxNumber.Visibility = Visibility.Visible;
                    sp_clearBoxNumber.Visibility = Visibility.Collapsed;
                    sp_switchBoxNumber.Visibility = Visibility.Collapsed;

                    requiredControlList = new List<string> { "CustomerId", "ChangeDate", "ChangeToFundNumber" };
                    break;
                     case "exchange":
                    sp_changeBoxNumber.Visibility = Visibility.Collapsed;
                    sp_clearBoxNumber.Visibility = Visibility.Collapsed;
                    sp_switchBoxNumber.Visibility = Visibility.Visible;
                    requiredControlList = new List<string> { "CustomerId", "ChangeDate", "SecondCustomerId" };

                    break;
                case "emptying":
                    sp_changeBoxNumber.Visibility = Visibility.Collapsed;
                    sp_clearBoxNumber.Visibility = Visibility.Visible;
                    sp_switchBoxNumber.Visibility = Visibility.Collapsed;
                    requiredControlList = new List<string> { "CustomerId", "ChangeDate" };

                    break;

            }
        }

        private async void tb_ChangeToFundNumber_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return && tb_ChangeToFundNumber.Text != "")
                {
                    if (tb_ChangeToFundNumber.Text.Equals(tb_OldFundNumber.Text))
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("EnterDifferentBoxNumber"), animation: ToasterAnimation.FadeIn);
                        tb_ChangeToFundNumber.Text = "";
                    }

                    else
                    {
                        var isValid = await FillCombo.customer.CheckBoxNumber(long.Parse(tb_ChangeToFundNumber.Text), customer.CustomerId);
                        if (!isValid)
                        {
                            tb_ChangeToFundNumber.Text = "";

                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("FundNumNotAvailable"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
