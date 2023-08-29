using Microsoft.Win32;
using netoaster;
using POSCA.Classes;
using POSCA.Classes.ApiClasses;
using POSCA.View.customers.customerSectionData;
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
    /// Interaction logic for uc_customerData.xaml
    /// </summary>
    public partial class uc_customerData : UserControl
    {

        public uc_customerData()
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
        private static uc_customerData _instance;
        public static uc_customerData Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_customerData();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        Customer customer = new Customer();
        IEnumerable<Customer> customers;
        string searchText = "";
        public static List<string> requiredControlList;

        public void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "Name", "BoxNumber","CivilNum", "AutomtedNumber" , "MobileNumber" };
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


                Keyboard.Focus(tb_BoxNumber);

                FillCombo.fillGender(cb_Gender);
                FillCombo.fillMartialStatus(cb_MaritalStatus);
                FillCombo.fillMemberNature(cb_MemberNature);
                await FillCombo.fillJobsWithDefault(cb_JobId);
                await FillCombo.fillAreasWithDefault(cb_AreaId);
                await FillCombo.fillKinshipTiessWithDefault(cb_KinshipId);
                await FillCombo.fillCustomerBanksWithDefault(cb_BankId);

                await Clear();
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

            txt_title.Text = AppSettings.resourcemanager.GetString("trCustomer");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            txt_search.Text = AppSettings.resourcemanager.GetString("trSearch");


            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CurrentPurchses, AppSettings.resourcemanager.GetString("CurrentCustomerPurHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_BoxNumber, AppSettings.resourcemanager.GetString("BoxNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerId, AppSettings.resourcemanager.GetString("CustomerIdHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Name, AppSettings.resourcemanager.GetString("CustomerNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Family, AppSettings.resourcemanager.GetString("FamilyHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_Gender, AppSettings.resourcemanager.GetString("GenderHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_WithdrawnDate, AppSettings.resourcemanager.GetString("WithdrawnDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CheckNumber, AppSettings.resourcemanager.GetString("CheckNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_InvoiceName, AppSettings.resourcemanager.GetString("CustomerInvNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_MaritalStatus, AppSettings.resourcemanager.GetString("MaritalStatusHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_JobId, AppSettings.resourcemanager.GetString("trJobHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_DOB, AppSettings.resourcemanager.GetString("DOBHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CivilNum, AppSettings.resourcemanager.GetString("NationalIDHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            txt_FamilyCardHolder.Text = AppSettings.resourcemanager.GetString("FamilyCardHolder");
            
            txt_addressInformation.Text = AppSettings.resourcemanager.GetString("AddressInfo");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_AutomtedNumber, AppSettings.resourcemanager.GetString("AutomtedNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_AreaId, AppSettings.resourcemanager.GetString("trArea"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_SectionId, AppSettings.resourcemanager.GetString("trSectionHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Street, AppSettings.resourcemanager.GetString("StreetHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_HouseNumber, AppSettings.resourcemanager.GetString("HouseNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Floor, AppSettings.resourcemanager.GetString("FloorHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_AvenueNumber, AppSettings.resourcemanager.GetString("AvenueNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Plot, AppSettings.resourcemanager.GetString("PlotHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_MailBox, AppSettings.resourcemanager.GetString("MailBoxHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_PostalCode, AppSettings.resourcemanager.GetString("PostalCodeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Employer, AppSettings.resourcemanager.GetString("EmployerHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Guardian, AppSettings.resourcemanager.GetString("GuardianHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_HomePhone, AppSettings.resourcemanager.GetString("HomePhoneHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_WorkPhone, AppSettings.resourcemanager.GetString("WorkPhoneHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_MobileNumber, AppSettings.resourcemanager.GetString("MobileNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_MobileNumber2, AppSettings.resourcemanager.GetString("MobileNumber2Hint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_WorkAddress, AppSettings.resourcemanager.GetString("WorkAddressHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_KinshipId, AppSettings.resourcemanager.GetString("KinshipRelationshipHint"));


            txt_joinInformation.Text = AppSettings.resourcemanager.GetString("AddressInfo");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerStatus, AppSettings.resourcemanager.GetString("CustomerStatusHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_MemberNature, AppSettings.resourcemanager.GetString("MemberNatureHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_SessionNumber, AppSettings.resourcemanager.GetString("SessionNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_JoinDate, AppSettings.resourcemanager.GetString("JoinDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ReceiptVoucherNumber, AppSettings.resourcemanager.GetString("ReceiptVoucherNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_ReceiptVoucherDate, AppSettings.resourcemanager.GetString("ReceiptVoucherDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_JoiningSharesCount, AppSettings.resourcemanager.GetString("JoiningSharesCountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_SharesCount, AppSettings.resourcemanager.GetString("SharesCountBiginningYearHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_IBAN, AppSettings.resourcemanager.GetString("IBANHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_BankId, AppSettings.resourcemanager.GetString("trBankHint"));

            txt_CalculateEarnings.Text = AppSettings.resourcemanager.GetString("CalculateEarningsForMember");
            txt_RegisteredInMinistry.Text = AppSettings.resourcemanager.GetString("CalculateEarningsForMember");
            txt_StopOnPOS.Text = AppSettings.resourcemanager.GetString("StopOnPOS");
            txt_DataCompleted.Text = AppSettings.resourcemanager.GetString("DataCompleted");
            txt_PrintNameOnInv.Text = AppSettings.resourcemanager.GetString("PrintNameOnInv");


            txt_personalDocumentsButton.Text = AppSettings.resourcemanager.GetString("PersonalDocuments");
            txt_pastProfitsButton.Text = AppSettings.resourcemanager.GetString("PastProfits");

            btn_archiving.Content = AppSettings.resourcemanager.GetString("trArchive");

            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trSave");
            //txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            dg_customer.Columns[0].Header = AppSettings.resourcemanager.GetString("CustomerNo");
            dg_customer.Columns[1].Header = AppSettings.resourcemanager.GetString("BoxNumber");
            dg_customer.Columns[2].Header = AppSettings.resourcemanager.GetString("CustomerName");
            dg_customer.Columns[3].Header = AppSettings.resourcemanager.GetString("CivilNo");
            dg_customer.Columns[4].Header = AppSettings.resourcemanager.GetString("CurrentCustomerPur");
            dg_customer.Columns[5].Header = AppSettings.resourcemanager.GetString("SharesCount");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");


            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
        }

        private void inputEditable()
        {
            if(customer.CustomerId.Equals(0))
            {
                btn_archiving.IsEnabled = false;
                tb_BoxNumber.IsEnabled = true;
                tb_IBAN.IsEnabled = true;
                cb_BankId.IsEnabled = true;
                sp_withdrawnData.Visibility = Visibility.Collapsed;
            }
            else
            {
                tb_IBAN.IsEnabled = false;
                cb_BankId.IsEnabled = false;

                if (customer.CanArchive)
                    btn_archiving.IsEnabled = true;
                else
                    btn_archiving.IsEnabled = false;

                if (customer.WithdrawnDate != null)
                    sp_withdrawnData.Visibility = Visibility.Visible;
                else
                    sp_withdrawnData.Visibility = Visibility.Collapsed;
                tb_BoxNumber.IsEnabled = false;
            }

            tb_CustomerStatus.Foreground = tb_CustomerStatus.Text.Equals(AppSettings.resourcemanager.GetString("withdrawn")) ? Brushes.Red : Brushes.Black;
        }


        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh

        private void AssignValueToObject()
        {
            customer.Name = tb_Name.Text;
            customer.BoxNumber = long.Parse(tb_BoxNumber.Text);
            customer.CivilNum = tb_CivilNum.Text;
            customer.Family = tb_Family.Text;
            customer.InvoiceName = tb_InvoiceName.Text;
            customer.IBAN = tb_IBAN.Text;

            if (cb_Gender.SelectedValue != null)
                customer.Gender = cb_Gender.SelectedValue.ToString();
            if (cb_MaritalStatus.SelectedValue != null)
                customer.MaritalStatus = cb_MaritalStatus.SelectedValue.ToString();
            if (cb_JobId.SelectedValue != null)
                customer.JobId = (int)cb_JobId.SelectedValue;
            if (dp_DOB.SelectedDate != null)
                customer.DOB = dp_DOB.SelectedDate;
            if (cb_MemberNature.SelectedValue != null)
                customer.MemberNature = cb_MemberNature.SelectedValue.ToString();
            if (tb_SessionNumber.Text != "")
                customer.SessionNumber = int.Parse(tb_SessionNumber.Text);
            if (dp_JoinDate.SelectedDate != null)
                customer.JoinDate = dp_JoinDate.SelectedDate;
            if (tb_ReceiptVoucherNumber.Text != "")
                customer.ReceiptVoucherNumber = int.Parse(tb_ReceiptVoucherNumber.Text);
            if (dp_ReceiptVoucherDate.SelectedDate != null)
                customer.ReceiptVoucherDate = dp_ReceiptVoucherDate.SelectedDate;
            if (tb_SharesCount.Text != "")
                customer.SharesCount = int.Parse(tb_SharesCount.Text);
            if (cb_BankId.SelectedValue != null)
                customer.BankId = (int)cb_BankId.SelectedValue;
            if (tgl_CalculateEarnings.IsChecked == true)
                customer.CalculateEarnings = true;
            else
                customer.CalculateEarnings = false;

            if (tgl_DataCompleted.IsChecked == true)
                customer.DataCompleted = true;
            else
                customer.DataCompleted = false;

            if (tgl_PrintNameOnInv.IsChecked == true)
                customer.PrintNameOnInv = true;
            else
                customer.PrintNameOnInv = false;

            if (tgl_RegisteredInMinistry.IsChecked == true)
                customer.RegisteredInMinistry = true;
            else
                customer.RegisteredInMinistry = false;

            if (tgl_StopOnPOS.IsChecked == true)
                customer.StopOnPOS = true;
            else
                customer.StopOnPOS = false;

            customer.Notes = tb_Notes.Text;

            customer.CreateUserId = MainWindow.userLogin.userId;

            //customer ddress
            if (customer.customerAddress == null)
                customer.customerAddress = new CustomerAddress();
            customer.customerAddress.AutomtedNumber = tb_AutomtedNumber.Text;
            if (cb_AreaId.SelectedValue != null)
                customer.customerAddress.AreaId = (int)cb_AreaId.SelectedValue;
            if (cb_SectionId.SelectedValue != null)
                customer.customerAddress.SectionId = (int)cb_SectionId.SelectedValue;
            customer.customerAddress.Street = tb_Street.Text;
            customer.customerAddress.HouseNumber = tb_HouseNumber.Text;
            if (tb_Floor.Text != "")
                customer.customerAddress.Floor = int.Parse(tb_Floor.Text);
            customer.customerAddress.AvenueNumber = tb_AvenueNumber.Text;
            customer.customerAddress.Plot = tb_Plot.Text;
            customer.customerAddress.MailBox = tb_MailBox.Text;
            customer.customerAddress.PostalCode = tb_PostalCode.Text;
            customer.customerAddress.Employer = tb_Employer.Text;
            customer.customerAddress.Guardian = tb_Guardian.Text;
            customer.customerAddress.HomePhone = tb_HomePhone.Text;
            customer.customerAddress.WorkPhone = tb_WorkPhone.Text;
            customer.customerAddress.MobileNumber = tb_MobileNumber.Text;
            customer.customerAddress.MobileNumber2 = tb_MobileNumber2.Text;
            customer.customerAddress.WorkAddress = tb_WorkAddress.Text;
            if (cb_KinshipId.SelectedValue != null)
                customer.customerAddress.KinshipId = (int)cb_KinshipId.SelectedValue;
        }

        private bool validateCustomerData()
        {
            if(isValidCivilNum())
            {
                if(isValidCustomerName())
                if(isValidIBAN())
                {
                    return true;
                }
            }
            return false;
        }
        private bool isValidCivilNum()
        {
            if (tb_CivilNum.Text.Length < 12)
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CivilNumLengthAlert"), animation: ToasterAnimation.FadeIn);

                return false;
            }
            else return true;
        } 
        private bool isValidCustomerName()
        {
            if (tb_Name.Text.Trim().Split(' ').Length < 3)
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CustomerNameLengthAlert"), animation: ToasterAnimation.FadeIn);

                return false;
            }
            else return true;
        } 
        
        private bool isValidIBAN()
        {
            if (tb_IBAN.Text != "" && tb_IBAN.Text.Length < 30)
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("IBANLengthAlert"), animation: ToasterAnimation.FadeIn);

                return false;
            }
            else return true;
        }
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);

                    customer = new Customer();
                    if (HelpClass.validate(requiredControlList, this) )
                    {
                        if (validateCustomerData())
                        {
                            AssignValueToObject();

                            customer = await customer.save(customer);
                            if (customer.CustomerId == 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                                await Clear();
                            }
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
        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (customer.CustomerId > 0)
                    {
                        if (HelpClass.validate(requiredControlList, this) )
                        {
                            if (validateCustomerData())
                            {
                                AssignValueToObject();

                                customer = await customer.save(customer);
                                if (customer.CustomerId == 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);

                                }
                            }
                        }
                        else
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("saveNotDoneEmptyFields"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);

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

        private async void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                await Clear();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void Dg_customer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection

                if (dg_customer.SelectedIndex != -1)
                {
                    customer = dg_customer.SelectedItem as Customer;
                    this.DataContext = customer;

                    tb_CurrentPurchses.Text = HelpClass.DecTostring(customer.CurrentPurchses);
                    tb_JoiningSharesCount.Text = customer.JoiningSharesCount.ToString();
                    tb_CustomerStatus.Text = AppSettings.resourcemanager.GetString(customer.CustomerStatus);
                    tb_AutomtedNumber.Text = customer.customerAddress.AutomtedNumber.ToString();
                    cb_AreaId.SelectedValue = customer.customerAddress.AreaId;

                    #region section
                    if (cb_AreaId.SelectedValue != null)
                    {
                        var area = FillCombo.areaList.Where(x => x.AreaId == (int)cb_AreaId.SelectedValue).FirstOrDefault();

                        var lst = area.Sections.ToList();
                        POSCA.Classes.ApiClasses.Section sup = new POSCA.Classes.ApiClasses.Section();
                        sup.Name = "-";
                        sup.SectionId = 0;
                        lst.Insert(0, sup);

                        cb_SectionId.ItemsSource = lst;
                        cb_SectionId.SelectedValuePath = "SectionId";
                        cb_SectionId.DisplayMemberPath = "Name";
                    }
                    cb_SectionId.SelectedValue = customer.customerAddress.SectionId;
                    #endregion
                    tb_Street.Text = customer.customerAddress.Street;
                    tb_HouseNumber.Text = customer.customerAddress.HouseNumber;
                    tb_Floor.Text = customer.customerAddress.Floor.ToString();
                    tb_AvenueNumber.Text = customer.customerAddress.AvenueNumber;
                    tb_Plot.Text = customer.customerAddress.Plot;
                    tb_MailBox.Text = customer.customerAddress.MailBox;
                    tb_PostalCode.Text = customer.customerAddress.PostalCode;
                    tb_Employer.Text = customer.customerAddress.Employer;
                    tb_Guardian.Text = customer.customerAddress.Guardian;
                    tb_HomePhone.Text = customer.customerAddress.HomePhone;
                    tb_WorkPhone.Text = customer.customerAddress.WorkPhone;
                    tb_MobileNumber.Text = customer.customerAddress.MobileNumber;
                    tb_MobileNumber2.Text = customer.customerAddress.MobileNumber2;
                    tb_WorkAddress.Text = customer.customerAddress.WorkAddress;
                    cb_KinshipId.SelectedValue = customer.customerAddress.KinshipId;

                    inputEditable();
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

                //tb_search.Text = "";
                //searchText = "";
                await RefreshCustomersList();
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
            HelpClass.StartAwait(grid_main);
            customers = await FillCombo.customer.SearchCustomers(tb_search.Text);
            RefreshCustomersView();
            HelpClass.EndAwait(grid_main);
        }
        async Task<IEnumerable<Customer>> RefreshCustomersList()
        {
            await FillCombo.RefreshCustomers();

            return FillCombo.customerList;
        }
        void RefreshCustomersView()
        {
            dg_customer.ItemsSource = customers;
            txt_count.Text = customers.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        async Task Clear()
        {
            requiredControlList = new List<string> { "Name", "BoxNumber", "CivilNum", "AutomtedNumber", "MobileNumber" };

            customer = new Customer();
            this.DataContext = new Customer();
            dg_customer.SelectedIndex = -1;

            //var maxId = await FillCombo.customer.GetMaxFundNum();
            //tb_BoxNumber.Text = maxId.ToString();
             var maxCustomerId = await FillCombo.customer.GetMaxCustomerId();
            tb_CustomerId.Text = maxCustomerId.ToString();

            // nameFirstChange = true;
            //clear address info
            tb_CustomerStatus.Text = AppSettings.resourcemanager.GetString("continouse");
            tb_CurrentPurchses.Text = "";
            tb_Street.Text = "";
            tb_HouseNumber.Text = "";
            tb_Floor.Text ="";
            tb_AvenueNumber.Text = "";
            tb_Plot.Text = "";
            tb_MailBox.Text ="";
            tb_PostalCode.Text = "";
            tb_Employer.Text = "";
            tb_Guardian.Text = "";
            tb_HomePhone.Text = "";
            tb_WorkPhone.Text = "";
            tb_MobileNumber.Text = "";
            tb_MobileNumber2.Text = "";
            tb_WorkAddress.Text = "";
            cb_KinshipId.SelectedValue = null;
            // last 
            inputEditable();

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
            //    customers = new List<Customer>();
            //    RefreshCustomersView();
            //}

        }
        void swapToData()
        {
            cd_gridMain1.Width = new GridLength(0, GridUnitType.Star);
            cd_gridMain2.Width = new GridLength(1, GridUnitType.Star);
           
        }


        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            swapToData();
        }

        #endregion

        private void Btn_archiving_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btn_addIBAN_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_userControl w = new wd_userControl();

                w.grid_uc.Children.Add(uc_customerBank.Instance);
                w.ShowDialog();
                uc_customerBank.Instance.UserControl_Unloaded(uc_customerBank.Instance, null);
                //await FillCombo.RefreshSupplierTypes();
                //await FillCombo.fillSupplierTypes(cb_SupplierTypeId);

                //if (w.isOk)
                //{

                //}
                Window.GetWindow(this).Opacity = 1;

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private async void Btn_personalDocuments_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_customerDocument w = new wd_customerDocument();

                w.CustomerDocuments = customer.customerDocuments;
                
                w.ShowDialog();
                if (w.isOk)
                {
                    customer.customerDocuments = w.CustomerDocuments.ToList();
                    if (customer.CustomerId != 0)
                    {
                        foreach (var row in customer.customerDocuments)
                        {
                            if (row.IsEdited)
                            {
                                row.DocTitle = System.IO.Path.GetFileNameWithoutExtension(row.DocPath);
                                var ext = row.DocPath.Substring(row.DocPath.LastIndexOf('.'));
                                var extension = ext.ToLower();
                                row.DocName = row.DocTitle.ToLower() + MainWindow.userLogin.userId ;
                                string b = await customer.uploadDocument(row.DocPath, row.DocName);
                                row.DocName = row.DocName + ext;
                            }
                        }
                        customer = await customer.save(customer);

                        
                    }
                }
                Window.GetWindow(this).Opacity = 1;

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_pastProfits_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cb_AreaId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(cb_AreaId.SelectedValue != null)
                {
                    var area = FillCombo.areaList.Where(x => x.AreaId == (int)cb_AreaId.SelectedValue).FirstOrDefault();

                    var lst = area.Sections.ToList();
                   POSCA.Classes.ApiClasses.Section sup = new POSCA.Classes.ApiClasses.Section();
                    sup.Name = "-";
                    sup.SectionId = 0;
                    lst.Insert(0, sup);

                    cb_SectionId.ItemsSource = lst;
                    cb_SectionId.SelectedValuePath = "SectionId";
                    cb_SectionId.DisplayMemberPath = "Name";
                }
            }
            catch
            {

            }
        }

        
        private async void tb_BoxNumber_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return && tb_BoxNumber.Text !="")
                {
                    
                    var isValid = await FillCombo.customer.CheckBoxNumber(long.Parse(tb_BoxNumber.Text),customer.CustomerId);
                    if (!isValid)
                    {
                        tb_BoxNumber.Text = "";
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("FundNumNotAvailable"), animation: ToasterAnimation.FadeIn);
                    }

                    // last
                    App.MoveToNextUIElement(e);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        //bool nameFirstChange = true;
        //private void tb_Name_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        validateEmpty_LostFocus(sender, e);
        //        if (nameFirstChange && customer.CustomerId == 0)
        //        {
        //            nameFirstChange = false;
        //            tb_InvoiceName.Text = tb_Name.Text;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
        //    }
        //}

        private void tb_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);

                TextBox textBox = sender as TextBox;

                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex(@"[ ]{2,}", options);
                textBox.Text = regex.Replace(textBox.Text, @" ");
                //only  digits

                var txt = textBox.Text.Trim();
                int countSpaces = txt.Count(Char.IsWhiteSpace);
                if (countSpaces > 2)
                {
                    int k = txt.LastIndexOf(" ", txt.LastIndexOf(" ") + 1);
                    String res = txt.Substring(0, k);
                    textBox.Text = res;
                }
                tb_InvoiceName.Text = tb_Name.Text;

                tb_Family.Text = textBox.Text.Split(' ').Last();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void tb_IBAN_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ValidateEmpty_TextChange(null, null);
                if(tb_IBAN.Text != "")
                    requiredControlList = new List<string> { "Name", "BoxNumber", "CivilNum", "AutomtedNumber", "MobileNumber","BankId" };
                else
                    requiredControlList = new List<string> { "Name", "BoxNumber", "CivilNum", "AutomtedNumber", "MobileNumber" };
            }
            catch
            {

            }
        }

        private void cb_BankId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ValidateEmpty_TextChange(null, null);
                if (cb_BankId.SelectedIndex > 0)
                    requiredControlList = new List<string> { "Name", "BoxNumber", "CivilNum", "AutomtedNumber", "MobileNumber", "IBAN" };
                else
                    requiredControlList = new List<string> { "Name", "BoxNumber", "CivilNum", "AutomtedNumber", "MobileNumber" };
            }
            catch
            {

            }
        }
    }
}
