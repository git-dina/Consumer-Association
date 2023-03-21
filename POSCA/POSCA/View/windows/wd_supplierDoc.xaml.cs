using Microsoft.Win32;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace POSCA.View.windows
{
    /// <summary>
    /// Interaction logic for wd_supplierDoc.xaml
    /// </summary>
    public partial class wd_supplierDoc : Window
    {

        public wd_supplierDoc()
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
        List<SupplierDoc> listSupplierDoc = new List<SupplierDoc>();
        List<SupplierDocType> listSupplierDocType = new List<SupplierDocType>();
        //public static DispatcherTimer timer;

        public List<SupplierDoc> SupplierDocs { get; set; }
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

                if (AppSettings.lang.Equals("en"))
                {
                    AppSettings.resourcemanager = new ResourceManager("POSCA.en_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    AppSettings.resourcemanager = new ResourceManager("POSCA.ar_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }

                translate();
                #endregion

                await fillDocTypeCombo();
                setDocsData();
                //#region fill combo SupplierDocType
                //listSupplierDocType = new List<SupplierDocType>();
                //listSupplierDocType.Add(new SupplierDocType() { TypeId = 1, Name = "SupplierDocType1" });
                //listSupplierDocType.Add(new SupplierDocType() { TypeId = 2, Name = "SupplierDocType2" });
                //cb_supplierDocType.DisplayMemberPath = "Name";
                //cb_supplierDocType.SelectedValuePath = "TypeId";
                //cb_supplierDocType.ItemsSource = listSupplierDocType;
                //#endregion

                //listSupplierDoc = new List<SupplierDoc>();
                //listSupplierDoc.Add(new SupplierDoc() { TypeId = 1, StartDate = DateTime.Now.Date, EndDate = DateTime.Now.Date });
                //listSupplierDoc.Add(new SupplierDoc() { TypeId = 2, StartDate = DateTime.Now.Date, EndDate = DateTime.Now.Date });
                //dg_supplierDoc.ItemsSource = listSupplierDoc;


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
            txt_title.Text = AppSettings.resourcemanager.GetString("MainDocuments");
            txt_supplierDoc.Text = AppSettings.resourcemanager.GetString("SupplierDocs");
           
            dg_supplierDoc.Columns[0].Header = AppSettings.resourcemanager.GetString("DocumentType");
            dg_supplierDoc.Columns[1].Header = AppSettings.resourcemanager.GetString("trStartDate");
            dg_supplierDoc.Columns[2].Header = AppSettings.resourcemanager.GetString("trEndDate");

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

        }

        private async Task fillDocTypeCombo()
        {
            if (FillCombo.supplierDocTypeList is null)
                await FillCombo.RefreshSupplierDocTypes();

            cb_supplierDocType.DisplayMemberPath = "Name";
            cb_supplierDocType.SelectedValuePath = "TypeId";
            cb_supplierDocType.ItemsSource = FillCombo.supplierDocTypeList;
        }
        private void setDocsData()
        {      
            dg_supplierDoc.ItemsSource = SupplierDocs;
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
        private async void Dg_supplierDoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                SupplierDocs = (List<SupplierDoc>)dg_supplierDoc.ItemsSource ;

                isOk = true;
                this.Close();
                // HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        OpenFileDialog openFileDialog = new OpenFileDialog();
        private void uploadSupplierDocRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        btn_addSupplierDoc.IsEnabled = false;
                        dg_supplierDoc.IsEnabled = false;

                        SupplierDoc row = (SupplierDoc)dg_supplierDoc.SelectedItems[0];


                        if (openFileDialog.ShowDialog() == true)
                        {
                            row.DocPath = openFileDialog.FileName;
                            row.IsEdited = true;
                        }
                        btn_addSupplierDoc.IsEnabled = true;
                        dg_supplierDoc.IsEnabled = true;

                    }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
      

        private void Btn_addSupplierDoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_addSupplierDoc.IsEnabled = false;
                dg_supplierDoc.IsEnabled = false;
                listSupplierDoc.Add(new SupplierDoc());
                RefreshSupplierDocDataGrid();
            }
            catch (Exception ex)
            {
                btn_addSupplierDoc.IsEnabled = true;
                dg_supplierDoc.IsEnabled = true;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void deleteSupplierDocRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {

                        btn_addSupplierDoc.IsEnabled = false;
                        dg_supplierDoc.IsEnabled = false;
                        SupplierDoc row = (SupplierDoc)dg_supplierDoc.SelectedItems[0];
                        listSupplierDoc.Remove(row);
                        RefreshSupplierDocDataGrid();
                    }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                btn_addSupplierDoc.IsEnabled = true;
                dg_supplierDoc.IsEnabled = true;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public void RefreshSupplierDocDataGrid()
        {
            try
            {
                dg_supplierDoc.CancelEdit();
                dg_supplierDoc.ItemsSource = listSupplierDoc;
                dg_supplierDoc.Items.Refresh();

                dg_supplierDoc.IsEnabled = true;
                btn_addSupplierDoc.IsEnabled = true;
            }
            catch (Exception ex)
            {
                dg_supplierDoc.IsEnabled = true;
                btn_addSupplierDoc.IsEnabled = true;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
        }

        //void timer_Tick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        RefreshSupplierDocDataGrid();
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
        //    }
        //}

        //private void DatePicker_CalendarOpened(object sender, RoutedEventArgs e)
        //{
        //    if (timer != null)
        //        timer.Stop();
        //}

        //private void DatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        //{
        //    if (timer != null)
        //        timer.Start();
        //}
    }
}
