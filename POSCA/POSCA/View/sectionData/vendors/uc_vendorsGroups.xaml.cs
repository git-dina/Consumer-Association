using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POSCA.View.sectionData.vendors
{
    /// <summary>
    /// Interaction logic for uc_vendorsGroups.xaml
    /// </summary>
    public partial class uc_vendorsGroups : UserControl
    {

        public uc_vendorsGroups()
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
        private static uc_vendorsGroups _instance;
        public static uc_vendorsGroups Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_vendorsGroups();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        SupplierGroup supplierGroup = new SupplierGroup();
        IEnumerable<SupplierGroup> supplierGroupsQuery;
        IEnumerable<SupplierGroup> supplierGroups;
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
                requiredControlList = new List<string> { "Name" };
                //if (AppSettings.lang.Equals("en"))
                //{
                //    grid_main.FlowDirection = FlowDirection.LeftToRight;
                //}
                //else
                //{
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                //}
                translate();


                //FillCombo.FillDefaultPayType_cashBalanceCardMultiple(cb_payType);
                Keyboard.Focus(tb_Name);
                /*
                if (FillCombo.supplierGroupsListAll is null)
                    await RefreshCustomersList();
                else
                    supplierGroups = FillCombo.supplierGroupsListAll.ToList();
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

            //txt_title.Text = AppSettings.resourcemanager.GetString("trSupplierGroup");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_code, AppSettings.resourcemanager.GetString("trCodeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Name, AppSettings.resourcemanager.GetString("trNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));
            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trUpdate");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            dg_supplierGroup.Columns[0].Header = AppSettings.resourcemanager.GetString("trName");
            dg_supplierGroup.Columns[1].Header = AppSettings.resourcemanager.GetString("trNote");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");

            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            //tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            //tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            //tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            //tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            //tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

            //txt_active.Text = AppSettings.resourcemanager.GetString("trActive_");
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                /*
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);

                    supplierGroup = new SupplierGroup();
                    if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                    {
                        //payType
                        string payType = "";
                        if (cb_payType.SelectedIndex != -1)
                            payType = cb_payType.SelectedValue.ToString();

                        //tb_code.Text = await supplierGroup.generateCodeNumber("v");
                        supplierGroup.code = await supplierGroup.generateCodeNumber("v");
                        supplierGroup.name = tb_name.Text;
                        supplierGroup.company = tb_company.Text;
                        supplierGroup.address = tb_address.Text;
                        supplierGroup.email = tb_email.Text;
                        supplierGroup.mobile = cb_areaMobile.Text + "-" + tb_mobile.Text;
                        if (!tb_phone.Text.Equals(""))
                            supplierGroup.phone = cb_areaPhone.Text + "-" + cb_areaPhoneLocal.Text + "-" + tb_phone.Text;
                        if (!tb_fax.Text.Equals(""))
                            supplierGroup.fax = cb_areaFax.Text + "-" + cb_areaFaxLocal.Text + "-" + tb_fax.Text;
                        supplierGroup.type = "v";
                        supplierGroup.accType = "";
                        supplierGroup.balance = 0;
                        supplierGroup.balanceType = 0;
                        supplierGroup.payType = payType;
                        supplierGroup.createUserId = MainWindow.userLogin.userId;
                        supplierGroup.updateUserId = MainWindow.userLogin.userId;
                        supplierGroup.notes = tb_notes.Text;
                        supplierGroup.isActive = 1;

                        var s = await supplierGroup.save(supplierGroup);
                        if (s <= 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                            if (openFileDialog.FileName != "")
                            {
                                var supplierGroupId = s;
                                string b = await supplierGroup.uploadImage(imgFileName,
                                    Md5Encription.MD5Hash("Inc-m" + supplierGroupId.ToString()), supplierGroupId);
                                supplierGroup.image = b;
                            }

                            Clear();
                            await RefreshCustomersList();
                            await Search();
                            FillCombo.supplierGroupsList = supplierGroups.ToList();
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
                    if (supplierGroup.supplierGroupId > 0)
                    {
                        if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                        {
                            //payType
                            string payType = "";
                            if (cb_payType.SelectedIndex != -1)
                                payType = cb_payType.SelectedValue.ToString();

                            supplierGroup.name = tb_name.Text;
                            supplierGroup.company = tb_company.Text;
                            supplierGroup.email = tb_email.Text;
                            supplierGroup.address = tb_address.Text;
                            supplierGroup.mobile = cb_areaMobile.Text + "-" + tb_mobile.Text;
                            if (!tb_phone.Text.Equals(""))
                                supplierGroup.phone = cb_areaPhone.Text + "-" + cb_areaPhoneLocal.Text + "-" + tb_phone.Text;
                            if (!tb_fax.Text.Equals(""))
                                supplierGroup.fax = cb_areaFax.Text + "-" + cb_areaFaxLocal.Text + "-" + tb_fax.Text;
                            supplierGroup.payType = payType;
                            supplierGroup.updateUserId = MainWindow.userLogin.userId;
                            supplierGroup.notes = tb_notes.Text;

                            var s = await supplierGroup.save(supplierGroup);
                            if (s <= 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                                await RefreshCustomersList();
                                await Search();
                                FillCombo.supplierGroupsList = supplierGroups.ToList();
                                if (openFileDialog.FileName != "")
                                {
                                    var supplierGroupId = s;
                                    string b = await supplierGroup.uploadImage(imgFileName, Md5Encription.MD5Hash("Inc-m" + supplierGroupId.ToString()), supplierGroupId);
                                    supplierGroup.image = b;
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
                    if (supplierGroup.supplierGroupId != 0)
                    {
                        if ((!supplierGroup.canDelete) && (supplierGroup.isActive == 0))
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
                            if (supplierGroup.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                            if (!supplierGroup.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDeactivate");
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion

                            if (w.isOk)
                            {
                                string popupContent = "";
                                if (supplierGroup.canDelete) popupContent = AppSettings.resourcemanager.GetString("trPopDelete");
                                if ((!supplierGroup.canDelete) && (supplierGroup.isActive == 1)) popupContent = AppSettings.resourcemanager.GetString("trPopInActive");

                                var s = await supplierGroup.delete(supplierGroup.supplierGroupId, MainWindow.userLogin.userId, supplierGroup.canDelete);
                                if (s < 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    supplierGroup.supplierGroupId = 0;
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                    await RefreshCustomersList();
                                    await Search();
                                    Clear();
                                    FillCombo.supplierGroupsList = supplierGroups.ToList();
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
        private async void Dg_supplierGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                /*
                if (dg_supplierGroup.SelectedIndex != -1)
                {
                    supplierGroup = dg_supplierGroup.SelectedItem as SupplierGroup;
                    this.DataContext = supplierGroup;
                    if (supplierGroup != null)
                    {
                        #region image
                        bool isModified = HelpClass.chkImgChng(supplierGroup.image, (DateTime)supplierGroup.updateDate, Global.TMPSupplierGroupsFolder);
                        if (isModified)
                            getImg();
                        else
                            HelpClass.getLocalImg("SupplierGroup", supplierGroup.image, btn_image);
                        #endregion
                        //getImg();
                        #region delete
                        if (supplierGroup.canDelete)
                            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
                        else
                        {
                            if (supplierGroup.isActive == 0)
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trActive");
                            else
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trInActive");
                        }
                        #endregion
                        HelpClass.getMobile(supplierGroup.mobile, cb_areaMobile, tb_mobile);
                        HelpClass.getPhone(supplierGroup.phone, cb_areaPhone, cb_areaPhoneLocal, tb_phone);
                        HelpClass.getPhone(supplierGroup.fax, cb_areaFax, cb_areaFaxLocal, tb_fax);
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
            if (supplierGroups is null)
                await RefreshCustomersList();
            searchText = tb_search.Text.ToLower();
            supplierGroupsQuery = supplierGroups.Where(s =>
            s.Name.ToLower().Contains(searchText) 
            );
            RefreshCustomersView();
        }
        async Task<IEnumerable<SupplierGroup>> RefreshCustomersList()
        {
            /*
            await FillCombo.RefreshSupplierGroupsAll();
            supplierGroups = FillCombo.supplierGroupsListAll.ToList();
            */
            return supplierGroups;
        }
        void RefreshCustomersView()
        {
            dg_supplierGroup.ItemsSource = supplierGroupsQuery;
            txt_count.Text = supplierGroupsQuery.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new SupplierGroup();


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
      



    }
}
