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
using System.Windows.Threading;

namespace POSCA.View.windows
{
    /// <summary>
    /// Interaction logic for wd_supplierContactData.xaml
    /// </summary>
    public partial class wd_supplierContactData : Window
    {

        public wd_supplierContactData()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name); }
        }
        BrushConverter bc = new BrushConverter();



        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            isOk = false;
            this.Close();
        }
        public List<SupplierPhone> SupplierPhones = new List<SupplierPhone>();

        public Nullable<long> BankId { get; set; }
        public string BankAccount { get; set; }
        public Nullable<int> SupNODays { get; set; }
        public int? AccountCode { get; set; }
        public string Email { get; set; }
        public string BOX { get; set; }
        public bool isOk { get; set; }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load

            try
            {


                HelpClass.StartAwait(grid_main);

                //timer = new DispatcherTimer();
                //timer.Interval = TimeSpan.FromSeconds(1);
                //timer.Tick += timer_Tick;
                //timer.Start();


                #region translate

                //if (AppSettings.lang.Equals("en"))
                //{
                //    grid_main.FlowDirection = FlowDirection.LeftToRight;
                //}
                //else
                //{
                grid_main.FlowDirection = FlowDirection.RightToLeft;
                //}

                translate();
                #endregion

                await FillCombo.fillBanksWithDefault(cb_BankId);
                await fillPhoneCombo();

                setContactData();

                //SupplierPhones = new List<SupplierPhone>();
                //SupplierPhones.Add(new SupplierPhone() { PhoneTypeId = 1, PhoneNumber = "PhoneNumber1", PersonName = "PersonName1" });
                //SupplierPhones.Add(new SupplierPhone() { PhoneTypeId = 2, PhoneNumber = "PhoneNumber2", PersonName = "PersonName2" });
                //dg_supplierPhone.ItemsSource = SupplierPhones;


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
            //

            txt_title.Text = AppSettings.resourcemanager.GetString("ContactData");
            txt_contactData.Text = AppSettings.resourcemanager.GetString("ContactData");
            txt_bankData.Text = AppSettings.resourcemanager.GetString("BankData");
            txt_phonesData.Text = AppSettings.resourcemanager.GetString("Phones");
            
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Email, AppSettings.resourcemanager.GetString("trEmailHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_BOX, AppSettings.resourcemanager.GetString("BOXHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_BankId, AppSettings.resourcemanager.GetString("trBankHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_BankAccount, AppSettings.resourcemanager.GetString("BankAccountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_AccountCode, AppSettings.resourcemanager.GetString("AccountCodetHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_SupNODays, AppSettings.resourcemanager.GetString("SupNODaysHint"));

            dg_supplierPhone.Columns[0].Header = AppSettings.resourcemanager.GetString("PhoneType");
            dg_supplierPhone.Columns[1].Header = AppSettings.resourcemanager.GetString("PhoneNumber");
            dg_supplierPhone.Columns[2].Header = AppSettings.resourcemanager.GetString("ContactName");
            //txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            //txt_updateButton.Text = AppSettings.resourcemanager.GetString("trUpdate");
            //txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");
        }
        private async Task fillPhoneCombo()
        {
            if (FillCombo.phoneTypeList is null)
                await FillCombo.RefreshPhoneTypes();

            cb_phoneType.DisplayMemberPath = "Name";
            cb_phoneType.SelectedValuePath = "PhoneTypeId";
            cb_phoneType.ItemsSource = FillCombo.phoneTypeList;
        }
        private void setContactData()
        {
            if (BankId != null)
                cb_BankId.SelectedValue = (long)BankId;

            tb_BankAccount.Text = BankAccount;
            if(AccountCode != 0)
                tb_AccountCode.Text = AccountCode.ToString();
            if(SupNODays != null)
                tb_SupNODays.Text = SupNODays.ToString();

            tb_Email.Text = Email;
            tb_BOX.Text = BOX;

            dg_supplierPhone.ItemsSource = SupplierPhones;
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
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                /*
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);

                    supplier = new Supplier();
                    if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                    {
                        //payType
                        string payType = "";
                        if (cb_payType.SelectedIndex != -1)
                            payType = cb_payType.SelectedValue.ToString();

                        //tb_code.Text = await supplier.generateCodeNumber("v");
                        supplier.code = await supplier.generateCodeNumber("v");
                        supplier.name = tb_name.Text;
                        supplier.company = tb_company.Text;
                        supplier.address = tb_address.Text;
                        supplier.email = tb_email.Text;
                        supplier.mobile = cb_areaMobile.Text + "-" + tb_mobile.Text;
                        if (!tb_phone.Text.Equals(""))
                            supplier.phone = cb_areaPhone.Text + "-" + cb_areaPhoneLocal.Text + "-" + tb_phone.Text;
                        if (!tb_fax.Text.Equals(""))
                            supplier.fax = cb_areaFax.Text + "-" + cb_areaFaxLocal.Text + "-" + tb_fax.Text;
                        supplier.type = "v";
                        supplier.accType = "";
                        supplier.balance = 0;
                        supplier.balanceType = 0;
                        supplier.payType = payType;
                        supplier.createUserId = MainWindow.userLogin.userId;
                        supplier.updateUserId = MainWindow.userLogin.userId;
                        supplier.notes = tb_notes.Text;
                        supplier.isActive = 1;

                        var s = await supplier.save(supplier);
                        if (s <= 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                            if (openFileDialog.FileName != "")
                            {
                                var supplierId = s;
                                string b = await supplier.uploadImage(imgFileName,
                                    Md5Encription.MD5Hash("Inc-m" + supplierId.ToString()), supplierId);
                                supplier.image = b;
                            }

                            Clear();
                            await RefreshCustomersList();
                            await Search();
                            FillCombo.suppliersList = suppliers.ToList();
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
        private async void Dg_supplierPhone_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
        #endregion

        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new Supplier();
            //txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            // last 
            HelpClass.clearValidate(requiredControlList, this);
            p_error_Email.Visibility = Visibility.Collapsed;
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

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // HelpClass.StartAwait(grid_main);
                if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                {

                    if (cb_BankId.SelectedIndex > 0)
                        BankId = (long)cb_BankId.SelectedValue;

                    BankAccount = tb_BankAccount.Text;
                    // AccountCode = int.Parse( tb_AccountCode.Text);
                    if (tb_SupNODays.Text != "")
                        SupNODays = int.Parse(tb_SupNODays.Text);

                    Email = tb_Email.Text;
                    BOX = tb_BOX.Text;
                    SupplierPhones = (List<SupplierPhone>)dg_supplierPhone.ItemsSource;
                    isOk = true;
                    this.Close();
                }
                // HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_addBank_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_addSupplierPhone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_addSupplierPhone.IsEnabled = false;
                dg_supplierPhone.IsEnabled = false;
                SupplierPhones.Add(new SupplierPhone());
                RefreshSupplierPhoneDataGrid();
            }
            catch (Exception ex)
            {
                dg_supplierPhone.IsEnabled = true;
                btn_addSupplierPhone.IsEnabled = true;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void deleteSupplierPhoneRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {

                        btn_addSupplierPhone.IsEnabled = false;
                        dg_supplierPhone.IsEnabled = false;
                        SupplierPhone row = (SupplierPhone)dg_supplierPhone.SelectedItems[0];
                        SupplierPhones.Remove(row);
                        RefreshSupplierPhoneDataGrid();
                    }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                dg_supplierPhone.IsEnabled = true;
                btn_addSupplierPhone.IsEnabled = true;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public void RefreshSupplierPhoneDataGrid()
        {
            try
            {
                dg_supplierPhone.CancelEdit();
                dg_supplierPhone.ItemsSource = SupplierPhones;
                dg_supplierPhone.Items.Refresh();

                dg_supplierPhone.IsEnabled = true;
                btn_addSupplierPhone.IsEnabled = true;
            }
            catch (Exception ex)
            {
                dg_supplierPhone.IsEnabled = true;
                btn_addSupplierPhone.IsEnabled = true;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
        }
        //void timer_Tick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        RefreshSupplierPhoneDataGrid();
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
        //    }
        //}
        //private void DataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        //{
        //    if (timer != null)
        //        timer.Stop();
        //}
        //private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        //{
        //    if (timer != null)
        //        timer.Start();
        //}
    }
}
