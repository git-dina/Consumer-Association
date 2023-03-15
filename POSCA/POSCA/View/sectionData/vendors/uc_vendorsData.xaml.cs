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
                requiredControlList = new List<string> { "Name", "ShortName", "SupplierTypeId" ,
                                        "SupplierGroupId","AssistantStartDate","FreePercentag",
                                        "DiscountPercentage"};
                //if (AppSettings.lang.Equals("en"))
                //{
                //grid_main.FlowDirection = FlowDirection.LeftToRight;
                //}
                //else
                //{
                grid_main.FlowDirection = FlowDirection.RightToLeft;
                //}
                translate();

               
                await FillCombo.fillAssistantWithDefault(cb_AssistantSupId);
                await FillCombo.fillSupplierTypes(cb_SupplierTypeId);
                await FillCombo.fillSupplierGroups(cb_SupplierGroupId);
                Keyboard.Focus(tb_Name);
                /*
                if (FillCombo.suppliersListAll is null)
                    await RefreshCustomersList();
                else
                    suppliers = FillCombo.suppliersListAll.ToList();
                */
                await Search();
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

            //// Title
            //if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate))
            //    txt_title.Text = AppSettings.resourcemanager.GetString(
            //   FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate
            //   );

            txt_title.Text = AppSettings.resourcemanager.GetString("Supplier");
            txt_Notes.Text = AppSettings.resourcemanager.GetString("trNote");
            txt_allowedOperationsButton.Text = AppSettings.resourcemanager.GetString("AllowedOperations");
            txt_supplierSectorButton.Text = AppSettings.resourcemanager.GetString("SupplierSectors");
            txt_documentsButton.Text = AppSettings.resourcemanager.GetString("MainDocuments");

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
            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trUpdate");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            dg_supplier.Columns[0].Header = AppSettings.resourcemanager.GetString("trNo");
            dg_supplier.Columns[1].Header = AppSettings.resourcemanager.GetString("trName");
            dg_supplier.Columns[2].Header = AppSettings.resourcemanager.GetString("ShortName");
            dg_supplier.Columns[3].Header = AppSettings.resourcemanager.GetString("Group");
            dg_supplier.Columns[4].Header = AppSettings.resourcemanager.GetString("trType");
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
                /*
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (supplier.supplierId > 0)
                    {
                        if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                        {
                            //payType
                            string payType = "";
                            if (cb_payType.SelectedIndex != -1)
                                payType = cb_payType.SelectedValue.ToString();

                            supplier.name = tb_name.Text;
                            supplier.company = tb_company.Text;
                            supplier.email = tb_email.Text;
                            supplier.address = tb_address.Text;
                            supplier.mobile = cb_areaMobile.Text + "-" + tb_mobile.Text;
                            if (!tb_phone.Text.Equals(""))
                                supplier.phone = cb_areaPhone.Text + "-" + cb_areaPhoneLocal.Text + "-" + tb_phone.Text;
                            if (!tb_fax.Text.Equals(""))
                                supplier.fax = cb_areaFax.Text + "-" + cb_areaFaxLocal.Text + "-" + tb_fax.Text;
                            supplier.payType = payType;
                            supplier.updateUserId = MainWindow.userLogin.userId;
                            supplier.notes = tb_notes.Text;

                            var s = await supplier.save(supplier);
                            if (s <= 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                                await RefreshCustomersList();
                                await Search();
                                FillCombo.suppliersList = suppliers.ToList();
                                if (openFileDialog.FileName != "")
                                {
                                    var supplierId = s;
                                    string b = await supplier.uploadImage(imgFileName, Md5Encription.MD5Hash("Inc-m" + supplierId.ToString()), supplierId);
                                    supplier.image = b;
                                    //isImgPressed = false;
                                    if (!b.Equals(""))
                                    {
                                        await getImg();
                                    }
                                    else
                                    {
                                        HelpClass.clearImg(btn_image);
                                    }
                                }
                            }
                        }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);

                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                */
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
                /*
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (supplier.supplierId != 0)
                    {
                        if ((!supplier.canDelete) && (supplier.isActive == 0))
                        {
                            #region
                            Window.GetWindow(this).Opacity = 0.2;
                            wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                            w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxActivate");
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion

                            if (w.isOk)
                                await activate();
                        }
                        else
                        {
                            #region
                            Window.GetWindow(this).Opacity = 0.2;
                            wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                            if (supplier.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                            if (!supplier.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDeactivate");
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion

                            if (w.isOk)
                            {
                                string popupContent = "";
                                if (supplier.canDelete) popupContent = AppSettings.resourcemanager.GetString("trPopDelete");
                                if ((!supplier.canDelete) && (supplier.isActive == 1)) popupContent = AppSettings.resourcemanager.GetString("trPopInActive");

                                var s = await supplier.delete(supplier.supplierId, MainWindow.userLogin.userId, supplier.canDelete);
                                if (s < 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    supplier.supplierId = 0;
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                    await RefreshCustomersList();
                                    await Search();
                                    Clear();
                                    FillCombo.suppliersList = suppliers.ToList();
                                }
                            }
                        }
                    }
                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                */
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
                /*
                if (dg_supplier.SelectedIndex != -1)
                {
                    supplier = dg_supplier.SelectedItem as Supplier;
                    this.DataContext = supplier;
                    if (supplier != null)
                    {
                        #region image
                        bool isModified = HelpClass.chkImgChng(supplier.image, (DateTime)supplier.updateDate, Global.TMPSuppliersFolder);
                        if (isModified)
                            getImg();
                        else
                            HelpClass.getLocalImg("Supplier", supplier.image, btn_image);
                        #endregion
                        //getImg();
                        #region delete
                        if (supplier.canDelete)
                            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
                        else
                        {
                            if (supplier.isActive == 0)
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trActive");
                            else
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trInActive");
                        }
                        #endregion
                        HelpClass.getMobile(supplier.mobile, cb_areaMobile, tb_mobile);
                        HelpClass.getPhone(supplier.phone, cb_areaPhone, cb_areaPhoneLocal, tb_phone);
                        HelpClass.getPhone(supplier.fax, cb_areaFax, cb_areaFaxLocal, tb_fax);
                    }
                }
                HelpClass.clearValidate(requiredControlList, this);
                */
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
            suppliersQuery = suppliers.Where(s =>
            s.SupId.ToString().Contains(searchText) ||
            s.Name.ToLower().Contains(searchText)
             || s.ShortName.ToLower().Contains(searchText)
             || s.SupplierGroup.ToLower().Contains(searchText)
             || s.SupplierType.ToLower().Contains(searchText)
            );
            RefreshCustomersView();
        }
        async Task<IEnumerable<Supplier>> RefreshSuppliersList()
        {

            await FillCombo.RefreshSuppliers();
            suppliers = FillCombo.suppliersList.ToList();

            return suppliers;
        }
        void RefreshCustomersView()
        {
            dg_supplier.ItemsSource = suppliersQuery;
            txt_count.Text = suppliersQuery.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new Supplier();
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");
            dp_AssistantStartDate.Text = DateTime.Now.ToString();
            tb_DiscountPercentage.Text = HelpClass.DecTostring(0);
            tb_FreePercentag.Text = HelpClass.DecTostring(0);
            /*
            #region mobile-Phone-fax-email
            brd_areaPhoneLocal.Visibility =
                brd_areaFaxLocal.Visibility = Visibility.Collapsed;
            cb_areaMobile.SelectedIndex = -1;
            cb_areaPhone.SelectedIndex = -1;
            cb_areaFax.SelectedIndex = -1;
            cb_areaPhoneLocal.SelectedIndex = -1;
            cb_areaFaxLocal.SelectedIndex = -1;
            tb_mobile.Clear();
            tb_phone.Clear();
            tb_fax.Clear();
            tb_email.Clear();
            #endregion
            */
            //#region image
            //HelpClass.clearImg(btn_image);
            //openFileDialog.FileName = "";
            //#endregion


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
        /*
        #region Phone
        int? countryid;
        private async void Cb_areaPhone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (cb_areaPhone.SelectedValue != null)
                {
                    if (cb_areaPhone.SelectedIndex >= 0)
                    {
                        countryid = int.Parse(cb_areaPhone.SelectedValue.ToString());
                        await FillCombo.fillCountriesLocal(cb_areaPhoneLocal, (long)countryid, brd_areaPhoneLocal);
                    }
                }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void Cb_areaFax_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (cb_areaFax.SelectedValue != null)
                {
                    if (cb_areaFax.SelectedIndex >= 0)
                    {
                        countryid = int.Parse(cb_areaFax.SelectedValue.ToString());
                        await FillCombo.fillCountriesLocal(cb_areaFaxLocal, (long)countryid, brd_areaFaxLocal);
                    }
                }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        #endregion
        */
        #region Image
        /*

        string imgFileName = "pic/no-image-icon-125x125.png";
        bool isImgPressed = false;
        OpenFileDialog openFileDialog = new OpenFileDialog();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        private void Btn_image_Click(object sender, RoutedEventArgs e)
        {
            //select image
            try
            {
                HelpClass.StartAwait(grid_main);
                isImgPressed = true;
                openFileDialog.Filter = "Images|*.png;*.jpg;*.bmp;*.jpeg;*.jfif";
                if (openFileDialog.ShowDialog() == true)
                {
                    HelpClass.imageBrush = new ImageBrush();
                    HelpClass.imageBrush.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Relative));
                    btn_image.Background = HelpClass.imageBrush;
                    imgFileName = openFileDialog.FileName;
                }
                else
                { }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

private async Task getImg()
{
try
{
HelpClass.StartAwait(grid_image, "forImage");
if (string.IsNullOrEmpty(supplier.image))
{
HelpClass.clearImg(btn_image);
}
else
{
byte[] imageBuffer = await supplier.downloadImage(supplier.image); // read this as BLOB from your DB

var bitmapImage = new BitmapImage();
if (imageBuffer != null)
{
using (var memoryStream = new MemoryStream(imageBuffer))
{
bitmapImage.BeginInit();
bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
bitmapImage.StreamSource = memoryStream;
bitmapImage.EndInit();
}

btn_image.Background = new ImageBrush(bitmapImage);
// configure trmporary path
string dir = Directory.GetCurrentDirectory();
string tmpPath = System.IO.Path.Combine(dir, Global.TMPSuppliersFolder, supplier.image);
openFileDialog.FileName = tmpPath;
}
else
HelpClass.clearImg(btn_image);
}
HelpClass.EndAwait(grid_image, "forImage");
}
catch
{
HelpClass.EndAwait(grid_image, "forImage");
}
}
*/

        #endregion

        private void Btn_contactDataButton_Click(object sender, RoutedEventArgs e)
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
                w.SupplierPhones = supplier.SupplierPhones;

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
        private void Btn_allowedOperations_Click(object sender, RoutedEventArgs e)
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

        private void Btn_supplierSector_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_documents_Click(object sender, RoutedEventArgs e)
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
                    supplier.SupplierDocuments = w.SupplierDocs;
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

    }
}
