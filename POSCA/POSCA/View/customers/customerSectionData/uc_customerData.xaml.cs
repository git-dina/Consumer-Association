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

namespace POSCA.View.customers.customerSectionData
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
        IEnumerable<Customer> customersQuery;
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


                Keyboard.Focus(tb_Name);

                FillCombo.fillGender(cb_Gender);
                FillCombo.fillMartialStatus(cb_MaritalStatus);
                await FillCombo.fillJobsWithDefault(cb_JobId);
                await FillCombo.fillAreasWithDefault(cb_AreaId);
                await FillCombo.fillKinshipTiessWithDefault(cb_KinshipId);
                await Search();
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

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CurrentPurchses, AppSettings.resourcemanager.GetString("CurrentCustomerPurHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_BoxNumber, AppSettings.resourcemanager.GetString("BoxNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Name, AppSettings.resourcemanager.GetString("CustomerNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Family, AppSettings.resourcemanager.GetString("FamilyHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_Gender, AppSettings.resourcemanager.GetString("GenderHint"));
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
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_CustomerStatus, AppSettings.resourcemanager.GetString("CustomerStatusHint"));
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


            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trSave");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            dg_customer.Columns[0].Header = AppSettings.resourcemanager.GetString("CustomerNo");
            dg_customer.Columns[1].Header = AppSettings.resourcemanager.GetString("BoxNumber");
            dg_customer.Columns[2].Header = AppSettings.resourcemanager.GetString("CustomerName");
            dg_customer.Columns[3].Header = AppSettings.resourcemanager.GetString("CurrentCustomerPur");
            dg_customer.Columns[4].Header = AppSettings.resourcemanager.GetString("SharesCount");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");


            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);

                    customer = new Customer();
                    if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                    {

                        customer.Name = tb_Name.Text;
                        customer.Notes = tb_Notes.Text;
                       
                        customer.CreateUserId = MainWindow.userLogin.userId;

                        FillCombo.customerList = await customer.save(customer);
                        if (FillCombo.customerList == null)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                            await Clear();
                            await Search();
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
                        if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                        {
                            customer.Name = tb_Name.Text;
                            customer.Notes = tb_Notes.Text;
                           
                            customer.UpdateUserId = MainWindow.userLogin.userId;

                            FillCombo.customerList = await customer.save(customer);
                            if (FillCombo.customerList == null)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                                await Search();

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
        private async void Btn_delete_Click(object sender, RoutedEventArgs e)
        {//delete
            try
            {
                // if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (customer.CustomerId != 0)
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
                            FillCombo.customerList = await customer.delete(customer.CustomerId, MainWindow.userLogin.userId);
                            if (FillCombo.customerList == null)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                customer.CustomerId = 0;
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                await Search();
                                await Clear();
                            }
                        }

                    }
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
                //if (tb_search.Text != "")
                //{
                //dina search
                //suppliers = await FillCombo.supplier.searchSuppliers(tb_search.Text);
                //RefreshSuppliersView();

                await Search();


                //}
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
            //search
            if (FillCombo.customerList is null)
                await RefreshCustomersList();
            searchText = tb_search.Text.ToLower();
            customersQuery = FillCombo.customerList.Where(s =>
            s.Name.ToLower().Contains(searchText)
            ).ToList();
            RefreshCustomersView();
        }
        async Task<IEnumerable<Customer>> RefreshCustomersList()
        {
            await FillCombo.RefreshCustomers();

            return FillCombo.customerList;
        }
        void RefreshCustomersView()
        {
            dg_customer.ItemsSource = customersQuery;
            txt_count.Text = customersQuery.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        async Task Clear()
        {
            this.DataContext = new Customer();
            dg_customer.SelectedIndex = -1;

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

        private void btn_updateIBAN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_personalDocuments_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
