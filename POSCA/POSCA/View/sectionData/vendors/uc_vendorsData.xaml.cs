using netoaster;
using POSCA.Classes;
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
using Microsoft.Win32;
using System.IO;
using POSCA.View.windows;
//using POSCA.View.windows;

//using Microsoft.Reporting.WinForms;

namespace POSCA.View.sectionData
{
    /// <summary>
    /// Interaction logic for uc_vendorsData.xaml
    /// </summary>
    public partial class uc_vendorsData : UserControl
    {
        public uc_vendorsData()
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
        private static uc_vendorsData _instance;
        public static uc_vendorsData Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_vendorsData();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        //string basicsPermission = "suppliers_basics";
        Supplier supplier = new Supplier();
        IEnumerable<Supplier> suppliersQuery;
        IEnumerable<Supplier> suppliers;
        byte tgl_supplierState;
        string searchText = "";
        public static List<string> requiredControlList;

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "Name", "ShortName", "SupplierTypeId" 
                                        ,"SupplierGroupId","AssistantStartDate","FreePercentag",
                                        "DiscountPercentage"};
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


                await FillCombo.fillAssistantWithDefault(cb_AssistantSupId);
                await FillCombo.fillSupplierTypes(cb_SupplierTypeId);
                await FillCombo.fillSupplierGroups(cb_SupplierGroupId);
                await FillCombo.fillBanksWithDefault(cb_BankId);

                Keyboard.Focus(tb_Name);

                swapToData();
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

            txt_title.Text = AppSettings.resourcemanager.GetString("Supplier");
            txt_Notes.Text = AppSettings.resourcemanager.GetString("trNote");
            txt_contactDataButton.Text = AppSettings.resourcemanager.GetString("ContactData");
            txt_allowedOperationsButton.Text = AppSettings.resourcemanager.GetString("AllowedOperations");
            txt_supplierSectorButton.Text = AppSettings.resourcemanager.GetString("SupplierSectors");
            txt_documentsButton.Text = AppSettings.resourcemanager.GetString("MainDocuments");
            txt_contactData.Text = AppSettings.resourcemanager.GetString("ContactData");
            txt_bankData.Text = AppSettings.resourcemanager.GetString("BankData");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Code, AppSettings.resourcemanager.GetString("trNoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ShortName, AppSettings.resourcemanager.GetString("ShortNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Name, AppSettings.resourcemanager.GetString("trNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_SupplierTypeId, AppSettings.resourcemanager.GetString("SupplierTypeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_SupplierGroupId, AppSettings.resourcemanager.GetString("SupplierGroupHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_AssistantSupId, AppSettings.resourcemanager.GetString("AssistantSupplierHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_AssistantStartDate, AppSettings.resourcemanager.GetString("StartMigrationHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_DiscountPercentage, AppSettings.resourcemanager.GetString("DiscountPercentageHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_FreePercentag, AppSettings.resourcemanager.GetString("FreePercentagHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_address, AppSettings.resourcemanager.GetString("trAdressHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_PurchaseOrderNotes, AppSettings.resourcemanager.GetString("PurchaseOrderNotesHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("GeneralNotesHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Email, AppSettings.resourcemanager.GetString("trEmailHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_BOX, AppSettings.resourcemanager.GetString("BOXHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_BankId, AppSettings.resourcemanager.GetString("trBankHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_BankAccount, AppSettings.resourcemanager.GetString("BankAccountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_AccountCode, AppSettings.resourcemanager.GetString("AccountCodetHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_SupNODays, AppSettings.resourcemanager.GetString("SupNODaysHint"));

            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trSave");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");
            btn_updateGrid.Content = AppSettings.resourcemanager.GetString("trUpdate");

            dg_supplier.Columns[0].Header = AppSettings.resourcemanager.GetString("trNo");
            dg_supplier.Columns[1].Header = AppSettings.resourcemanager.GetString("trName");
            dg_supplier.Columns[2].Header = AppSettings.resourcemanager.GetString("ShortName");
            dg_supplier.Columns[3].Header = AppSettings.resourcemanager.GetString("Group");
            dg_supplier.Columns[4].Header = AppSettings.resourcemanager.GetString("trType");
            dg_supplier.Columns[5].Header = AppSettings.resourcemanager.GetString("IsBlocked");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");

            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            //tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            //tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            //tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            //tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            //tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");
            //tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

            //txt_active.Text = AppSettings.resourcemanager.GetString("trActive_");
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
               // if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);

                    supplier = new Supplier();
                    if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                    {
                        supplier.ShortName = tb_ShortName.Text;
                        supplier.Name = tb_Name.Text;

                        if (cb_AssistantSupId.SelectedIndex > 0)
                            supplier.AssistantSupId = (long)cb_AssistantSupId.SelectedValue;
                        else
                            supplier.AssistantSupId = null;

                        if (cb_SupplierTypeId.SelectedIndex >= 0)
                            supplier.SupplierTypeId = (int)cb_SupplierTypeId.SelectedValue;


                         if (cb_SupplierGroupId.SelectedIndex >= 0)
                            supplier.SupplierGroupId = (int)cb_SupplierGroupId.SelectedValue;

                        supplier.AssistantStartDate =DateTime.Parse( dp_AssistantStartDate.Text);
                        supplier.FreePercentag =decimal.Parse( tb_FreePercentag.Text);
                        supplier.DiscountPercentage =decimal.Parse( tb_DiscountPercentage.Text);
                        supplier.Address = tb_address.Text;
                        supplier.LicenseId = tb_LicenseId.Text;

                        if (cb_BankId.SelectedIndex > 0)
                            supplier.BankId = (long)cb_BankId.SelectedValue;

                        supplier.BankAccount = tb_BankAccount.Text;
                        if (tb_SupNODays.Text != "")
                            supplier.SupNODays = int.Parse(tb_SupNODays.Text);

                        supplier.Email = tb_Email.Text;
                        supplier.BOX = tb_BOX.Text;

                        supplier.Notes = tb_Notes.Text;
                        supplier.PurchaseOrderNotes = tb_PurchaseOrderNotes.Text;

                        if (dp_AssistantStartDate.SelectedDate != null)
                            supplier.AssistantStartDate = (DateTime)dp_AssistantStartDate.SelectedDate;
                        else
                            supplier.AssistantStartDate = null;

                        supplier.CreateUserId = MainWindow.userLogin.userId;

                       FillCombo.suppliersList = await supplier.save(supplier);
                        if (FillCombo.suppliersList == null)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                            Clear();
                            await Search();

                        }
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
                //{
                    HelpClass.StartAwait(grid_main);
                    if (supplier.SupId > 0)
                    {
                    if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                    {
                        supplier.ShortName = tb_ShortName.Text;
                        supplier.Name = tb_Name.Text;

                        if (cb_AssistantSupId.SelectedIndex > 0)
                            supplier.AssistantSupId = (long)cb_AssistantSupId.SelectedValue;
                        else
                            supplier.AssistantSupId = null;

                        if (cb_SupplierTypeId.SelectedIndex >= 0)
                            supplier.SupplierTypeId = (int)cb_SupplierTypeId.SelectedValue;


                        if (cb_SupplierGroupId.SelectedIndex >= 0)
                            supplier.SupplierGroupId = (int)cb_SupplierGroupId.SelectedValue;

                        supplier.AssistantStartDate = DateTime.Parse(dp_AssistantStartDate.Text);
                        supplier.FreePercentag = decimal.Parse(tb_FreePercentag.Text);
                        supplier.DiscountPercentage = decimal.Parse(tb_DiscountPercentage.Text);
                        supplier.Address = tb_address.Text;
                        supplier.LicenseId = tb_LicenseId.Text;
                        if (cb_BankId.SelectedIndex > 0)
                            supplier.BankId = (long)cb_BankId.SelectedValue;

                        supplier.BankAccount = tb_BankAccount.Text;
                        if (tb_SupNODays.Text != "")
                            supplier.SupNODays = int.Parse(tb_SupNODays.Text);

                        supplier.Email = tb_Email.Text;
                        supplier.BOX = tb_BOX.Text;
                        supplier.Notes = tb_Notes.Text;
                        supplier.PurchaseOrderNotes = tb_PurchaseOrderNotes.Text;

                        if (dp_AssistantStartDate.SelectedDate != null)
                            supplier.AssistantStartDate = (DateTime)dp_AssistantStartDate.SelectedDate;
                        else
                            supplier.AssistantStartDate = null;
                        supplier.CreateUserId = MainWindow.userLogin.userId;

                        FillCombo.suppliersList = await supplier.save(supplier);
                        if (FillCombo.suppliersList == null)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                            await Search();
                            if (dg_supplier.SelectedIndex != -1)
                            {
                                supplier = dg_supplier.SelectedItem as Supplier;
                               
                            }
                        }
                    }
                }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);

                    HelpClass.EndAwait(grid_main);
                //}
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
                    if (supplier.SupId != 0)
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
                            FillCombo.suppliersList = await supplier.delete(supplier.SupId, MainWindow.userLogin.userId);
                            if (FillCombo.suppliersList == null)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                supplier.SupId = 0;
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                await Search();
                                Clear();
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
        //private async Task activate()
        //{//activate
        //    supplier.isActive = 1;
        //    var s = await supplier.save(supplier);
        //    if (s <= 0)
        //        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
        //    else
        //    {
        //        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopActive"), animation: ToasterAnimation.FadeIn);
        //        await RefreshCustomersList();
        //        await Search();
        //    }
        //}
        #endregion
        #region events
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
        /*
        private async void Tgl_isActive_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.suppliersListAll != null)
                    suppliers = FillCombo.suppliersListAll.ToList();
                if (suppliers is null)
                    await RefreshCustomersList();
                tgl_supplierState = 1;
                await Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void Tgl_isActive_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (suppliers is null)
                    await RefreshCustomersList();
                tgl_supplierState = 0;
                await Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        */
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
        private async void Dg_supplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection

                

                if (dg_supplier.SelectedIndex != -1)
                {
                    supplier = dg_supplier.SelectedItem as Supplier;
                    this.DataContext = supplier;

                    tb_DiscountPercentage.Text = HelpClass.DecTostring(supplier.DiscountPercentage);
                    tb_FreePercentag.Text = HelpClass.DecTostring(supplier.FreePercentag);

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

                tb_search.Text = "";
                searchText = "";
                await RefreshSuppliersList();
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
            if (FillCombo.suppliersList is null)
                await RefreshSuppliersList();
            searchText = tb_search.Text.ToLower();
            suppliersQuery = FillCombo.suppliersList.Where(s =>
            s.SupId.ToString().Contains(searchText) ||
            s.Name.ToLower().Contains(searchText)
             || s.ShortName.ToLower().Contains(searchText)
             || s.SupplierGroup.ToLower().Contains(searchText)
             || s.SupplierType.ToLower().Contains(searchText)
            ).ToList() ;
            RefreshSuppliersView();
        }
        async Task<IEnumerable<Supplier>> RefreshSuppliersList()
        {

            await FillCombo.RefreshSuppliers();
            suppliers = FillCombo.suppliersList.ToList();

            return suppliers;
        }
        void RefreshSuppliersView()
        {
            dg_supplier.ItemsSource = null;
            dg_supplier.ItemsSource = suppliersQuery;
            txt_count.Text = suppliersQuery.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new Supplier();
            dg_supplier.SelectedIndex = -1;
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");
            dp_AssistantStartDate.Text = DateTime.Now.ToString();
            tb_DiscountPercentage.Text = HelpClass.DecTostring(0);
            tb_FreePercentag.Text = HelpClass.DecTostring(0);
            long lastSupId = 0;
            if(FillCombo.suppliersList != null && FillCombo.suppliersList.Count > 0)
                lastSupId = FillCombo.suppliersList.Select(x => x.SupId).Max();
            lastSupId++;
            tb_Code.Text = lastSupId.ToString();
            // last 
            p_error_Email.Visibility = Visibility.Collapsed;

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
     

        private async void Btn_contactDataButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_supplierContactData w = new wd_supplierContactData();

                w.BankId = supplier.BankId;
                w.BankAccount = supplier.BankAccount;
                w.AccountCode = supplier.AccountCode;
                w.SupNODays = supplier.SupNODays;
                w.Email = supplier.Email;
                w.BOX = supplier.BOX;
                w.SupplierPhones = supplier.SupplierPhones.ToList();

                w.ShowDialog();
                if(w.isOk)
                {
                    supplier.BankId = w.BankId;
                    supplier.BankAccount = w.BankAccount;
                    supplier.AccountCode = w.AccountCode;
                    supplier.SupNODays = w.SupNODays;
                    supplier.Email = w.Email;
                    supplier.BOX = w.BOX;
                    supplier.SupplierPhones = w.SupplierPhones;

                    if (supplier.SupId != 0)
                    {
                        FillCombo.suppliersList = await supplier.save(supplier);
                        
                        await Search();
                        if (dg_supplier.SelectedIndex != -1)
                        {
                            supplier = FillCombo.suppliersList.Where(x => x.SupId == supplier.SupId).FirstOrDefault();

                        }
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
        private async void Btn_allowedOperations_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_supplierAllowedOperations w = new wd_supplierAllowedOperations();
                w.IsAllowedPO = supplier.IsAllowedPO;
                w.IsAllowedReceipt = supplier.IsAllowedReceipt;
                w.IsAllowedDirectReturn = supplier.IsAllowedDirectReturn;
                w.IsAllowedReturnDiscount = supplier.IsAllowedReturnDiscount;
                w.IsAllowCashingChecks = supplier.IsAllowCashingChecks;
                w.ShowDialog();

                if (w.isOk)
                {
                    supplier.IsAllowedPO = w.IsAllowedPO;
                    supplier.IsAllowedReceipt = w.IsAllowedReceipt;
                    supplier.IsAllowedDirectReturn = w.IsAllowedDirectReturn;
                    supplier.IsAllowedReturnDiscount = w.IsAllowedReturnDiscount;
                    supplier.IsAllowCashingChecks = w.IsAllowCashingChecks;

                    if (supplier.SupId != 0)
                    {
                        FillCombo.suppliersList = await supplier.save(supplier);
                        
                        await Search();
                        if (dg_supplier.SelectedIndex != -1)
                        {
                            supplier = FillCombo.suppliersList.Where(x => x.SupId == supplier.SupId).FirstOrDefault();

                        }
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

        private async void Btn_supplierSector_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_supplierSectors w = new wd_supplierSectors();

                w.SupplierSectors = supplier.SupplierSectors.ToList();
                w.SupplierSectorSpecifies = supplier.supplierSectorSpecifies.ToList();

                w.ShowDialog();
                if (w.isOk)
                {
                    supplier.SupplierSectors = w.SupplierSectors;
                    supplier.supplierSectorSpecifies = w.SupplierSectorSpecifies;
                    if (supplier.SupId != 0)
                    {
                        FillCombo.suppliersList = await supplier.save(supplier);
                        await Search();
                        if (dg_supplier.SelectedIndex != -1)
                        {
                            supplier = FillCombo.suppliersList.Where(x => x.SupId == supplier.SupId).FirstOrDefault();

                        }
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

        private async void Btn_documents_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_supplierDoc w = new wd_supplierDoc();

                w.SupplierDocs = supplier.SupplierDocuments;

                w.ShowDialog();
                if (w.isOk)
                {
                    supplier.SupplierDocuments = w.SupplierDocs.ToList();
                    if (supplier.SupId != 0)
                    {
                        foreach(var row in supplier.SupplierDocuments)
                        {
                            if(row.IsEdited)
                            {
                                row.DocTitle = System.IO.Path.GetFileNameWithoutExtension(row.DocPath);
                                var ext = row.DocPath.Substring(row.DocPath.LastIndexOf('.'));
                                var extension = ext.ToLower();
                                row.DocName = row.DocTitle.ToLower() + MainWindow.userLogin.userId + row.TypeId ;
                                string b = await supplier.uploadDocument(row.DocPath,row.DocName);
                                row.DocName = row.DocName +  ext;
                            }
                        }
                        FillCombo.suppliersList = await supplier.save(supplier);
                        
                        await Search();
                        if (dg_supplier.SelectedIndex != -1)
                        {
                            supplier = FillCombo.suppliersList.Where(x => x.SupId == supplier.SupId).FirstOrDefault();
                            //supplier = dg_supplier.SelectedItem as Supplier;
                        }
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

        private void Btn_addSupplierType_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Btn_addBank_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Tb_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(supplier.SupId == 0)
                tb_ShortName.Text = tb_Name.Text;
        }

        /*
         #region report
         //report  parameters
         ReportCls reportclass = new ReportCls();
         LocalReport rep = new LocalReport();
         //  SaveFileDialog saveFileDialog = new SaveFileDialog();

         // end report parameters
         public void BuildReport()
         {

             List<ReportParameter> paramarr = new List<ReportParameter>();

             string addpath;
             bool isArabic = ReportCls.checkLang();
             if (isArabic)
             {
                 addpath = @"\Reports\SectionData\persons\Ar\ArSuppliers.rdlc";
             }
             else
             {
                 addpath = @"\Reports\SectionData\persons\En\EnSuppliers.rdlc";
             }
             string searchval = "";
             string stateval = "";
             //filter   
             stateval = tgl_isActive.IsChecked == true ? AppSettings.resourcemanagerreport.GetString("trActive_")
               : AppSettings.resourcemanagerreport.GetString("trNotActive");
             paramarr.Add(new ReportParameter("stateval", stateval));
             paramarr.Add(new ReportParameter("trActiveState", AppSettings.resourcemanagerreport.GetString("trState")));
             paramarr.Add(new ReportParameter("trSearch", AppSettings.resourcemanagerreport.GetString("trSearch")));
             searchval = tb_search.Text;
             paramarr.Add(new ReportParameter("searchVal", searchval));
             //end filter
             string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

             clsReports.SupplierReport(suppliersQuery, rep, reppath, paramarr);
             clsReports.setReportLanguage(paramarr);
             clsReports.Header(paramarr);

             rep.SetParameters(paramarr);

             rep.Refresh();

         }
         private void Btn_pdf_Click(object sender, RoutedEventArgs e)
         {//pdf
             try
             {

                 HelpClass.StartAwait(grid_main);

                 if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                 {
                     #region
                     BuildReport();

                     saveFileDialog.Filter = "PDF|*.pdf;";

                     if (saveFileDialog.ShowDialog() == true)
                     {
                         string filepath = saveFileDialog.FileName;
                         LocalReportExtensions.ExportToPDF(rep, filepath);
                     }
                     #endregion
                 }
                 else
                     Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                 HelpClass.EndAwait(grid_main);
             }
             catch (Exception ex)
             {

                 HelpClass.EndAwait(grid_main);
                 HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
             }
         }

         private void Btn_print_Click(object sender, RoutedEventArgs e)
         {
             try
             {

                 HelpClass.StartAwait(grid_main);
                 if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                 {

                     #region
                     BuildReport();
                     LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, AppSettings.rep_print_count == null ? short.Parse("1") : short.Parse(AppSettings.rep_print_count));
                     #endregion
                 }
                 else
                     Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


                 HelpClass.EndAwait(grid_main);
             }
             catch (Exception ex)
             {

                 HelpClass.EndAwait(grid_main);
                 HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
             }

         }

         private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
         {
             try
             {

                 HelpClass.StartAwait(grid_main);

                 if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                 {
                     #region
                     BuildReport();
                     this.Dispatcher.Invoke(() =>
                     {
                         saveFileDialog.Filter = "EXCEL|*.xls;";
                         if (saveFileDialog.ShowDialog() == true)
                         {
                             string filepath = saveFileDialog.FileName;
                             LocalReportExtensions.ExportToExcel(rep, filepath);
                         }
                     });
                     #endregion
                 }
                 else
                     Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


                 HelpClass.EndAwait(grid_main);
             }
             catch (Exception ex)
             {

                 HelpClass.EndAwait(grid_main);

                 HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
             }
         }

         private void Btn_preview_Click(object sender, RoutedEventArgs e)
         {
             try
             {

                 HelpClass.StartAwait(grid_main);
                 if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                 {
                     #region
                     Window.GetWindow(this).Opacity = 0.2;

                     string pdfpath = "";
                     //
                     pdfpath = @"\Thumb\report\temp.pdf";
                     pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                     BuildReport();

                     LocalReportExtensions.ExportToPDF(rep, pdfpath);
                     wd_previewPdf w = new wd_previewPdf();
                     w.pdfPath = pdfpath;
                     if (!string.IsNullOrEmpty(w.pdfPath))
                     {
                         w.ShowDialog();
                         w.wb_pdfWebViewer.Dispose();


                     }
                     Window.GetWindow(this).Opacity = 1;
                     #endregion
                 }
                 else
                     Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


                 HelpClass.EndAwait(grid_main);
             }
             catch (Exception ex)
             {

                 Window.GetWindow(this).Opacity = 1;
                 HelpClass.EndAwait(grid_main);
                 HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
             }

         }
         private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
         {//pie
             try
             {
                 HelpClass.StartAwait(grid_main);

                 if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                 {
                     #region
                     Window.GetWindow(this).Opacity = 0.2;
                     win_lvc win = new win_lvc(suppliersQuery, 1, false);
                     win.ShowDialog();
                     Window.GetWindow(this).Opacity = 1;
                     #endregion
                 }
                 else
                     Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                 HelpClass.EndAwait(grid_main);
             }
             catch (Exception ex)
             {
                 Window.GetWindow(this).Opacity = 1;
                 HelpClass.EndAwait(grid_main);
                 HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
             }

         }
         #endregion
         */

        private void btn_columnSwap_Click(object sender, RoutedEventArgs e)
        {
            ColumnDefinition cd_gridMain3 = new ColumnDefinition();
            cd_gridMain3.Width = cd_gridMain1.Width;
            cd_gridMain1.Width = cd_gridMain2.Width;
            cd_gridMain2.Width = cd_gridMain3.Width;
        }
        private async void btn_updateGrid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);

                 var lst = (List<Supplier>)dg_supplier.ItemsSource;
                    FillCombo.suppliersList = await supplier.editBlocked(lst,MainWindow.userLogin.userId);
                    if (FillCombo.suppliersList == null)
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    else
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                        Clear();
                        await Search();

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

        private async void Btn_swapToSearch_Click(object sender, RoutedEventArgs e)
        {
            cd_gridMain1.Width = new GridLength(1, GridUnitType.Star);
            cd_gridMain2.Width = new GridLength(0, GridUnitType.Star);

            await Search();
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
    }
}
