using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WinForms;
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
using System.Threading;
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

namespace POSCA.View.sales
{
    /// <summary>
    /// Interaction logic for uc_salesInvoice.xaml
    /// </summary>
    public partial class uc_salesInvoice : UserControl
    {

        public uc_salesInvoice()
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
        private static uc_salesInvoice _instance;
        public static uc_salesInvoice Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_salesInvoice();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string searchText = "";
        public static List<string> requiredControlList;
        private Receipt receipt = new Receipt();
        private SalesInvoice salesInvoice = new SalesInvoice();
        private string _ReceiptType = "salesInvoices";
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

                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();

                /*
                #region loading
                loadingList = new List<keyValueBool>();
                bool isDone = true;

                loadingList.Add(new keyValueBool { key = "loading_RefrishSuppliers", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefrishLocations", value = false });

                loading_RefrishSuppliers();
                loading_RefrishLocations();

                do
                {
                    isDone = true;
                    foreach (var item in loadingList)
                    {
                        if (item.value == false)
                        {
                            isDone = false;
                            break;
                        }
                    }
                    if (!isDone)
                    {
                        await Task.Delay(0500);
                    }
                }
                while (!isDone);
                #endregion
                */

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

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            txt_invoiceTitle.Text = AppSettings.resourcemanager.GetString("SalesInvoice");
            txt_invoiceDetails.Text = AppSettings.resourcemanager.GetString("InvoiceDetails");
            txt_InvNumberTitle.Text = AppSettings.resourcemanager.GetString("trInvoiceNumber");
            txt_LocationTitle.Text = AppSettings.resourcemanager.GetString("Branch");
            txt_MachineTitle.Text = AppSettings.resourcemanager.GetString("Machine");
            txt_ShiftTitle.Text = AppSettings.resourcemanager.GetString("Shift");
            txt_LastInvTitle.Text = AppSettings.resourcemanager.GetString("LastInvoice");
            txt_CustomerTitle.Text = AppSettings.resourcemanager.GetString("IsCustomer");
            txt_CustomerBalanceTitle.Text = AppSettings.resourcemanager.GetString("trBalance");
            txt_payType.Text = AppSettings.resourcemanager.GetString("trPaymentMethods");


            txt_QuantityTitle.Text = AppSettings.resourcemanager.GetString("trAmount");
            txt_TotalTitle.Text = AppSettings.resourcemanager.GetString("TotalInvoice");

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
           
           
            dg_invoiceDetails.Columns[1].Header = AppSettings.resourcemanager.GetString("SeuenceAbbrevation");
            dg_invoiceDetails.Columns[2].Header = AppSettings.resourcemanager.GetString("trBarcode");
            dg_invoiceDetails.Columns[3].Header = AppSettings.resourcemanager.GetString("itemName");
            dg_invoiceDetails.Columns[5].Header = AppSettings.resourcemanager.GetString("trAmount");
            dg_invoiceDetails.Columns[6].Header = AppSettings.resourcemanager.GetString("trPrice");
            dg_invoiceDetails.Columns[7].Header = AppSettings.resourcemanager.GetString("trTotal");
           
            col_paymentType.Header = AppSettings.resourcemanager.GetString("trPaymentType");
            col_amount.Header = AppSettings.resourcemanager.GetString("trPaymentType");

            btn_newDraft.ToolTip = AppSettings.resourcemanager.GetString("trNew");
            btn_receiptOrders.ToolTip = AppSettings.resourcemanager.GetString("trSalesInvoices");
            // btn_salesInvoices.ToolTip = AppSettings.resourcemanager.GetString("SalesInvoices");
            btn_printInvoice.ToolTip = AppSettings.resourcemanager.GetString("trPrint");
        }

        #region Loading
        /*
        List<keyValueBool> loadingList;
        List<string> catchError = new List<string>();
        int catchErrorCount = 0;

        bool loadingSuccess_RefrishSuppliers = false;
        async void loading_RefrishSuppliers()
        {
            try
            {
                await FillCombo.fillUnBlockedSuppliers(cb_SupId);
                if (FillCombo.suppliersList is null)
                    loading_RefrishSuppliers();
                else
                    loadingSuccess_RefrishSuppliers = true;
            }
            catch (Exception ex)
            {
                catchError.Add("loading_RefrishSuppliers");
                catchErrorCount++;
                if (catchErrorCount > 50)
                {
                    loadingSuccess_RefrishSuppliers = true;
                }
                else
                    loading_RefrishSuppliers();
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
            if (loadingSuccess_RefrishSuppliers)
                foreach (var item in loadingList)
                {
                    if (item.key.Equals("loading_RefrishSuppliers"))
                    {
                        item.value = true;
                        break;
                    }
                }
        }

        bool loadingSuccess_RefrishLocations = false;
        async void loading_RefrishLocations()
        {
            try
            {
                await FillCombo.fillLocations(cb_LocationId);
                if (FillCombo.locationsList is null)
                    loading_RefrishLocations();
                else
                    loadingSuccess_RefrishLocations = true;
            }
            catch (Exception ex)
            {
                catchError.Add("loading_RefrishLocations");
                catchErrorCount++;
                if (catchErrorCount > 50)
                {
                    loadingSuccess_RefrishLocations = true;
                }
                else
                    loading_RefrishLocations();
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
            if (loadingSuccess_RefrishLocations)
                foreach (var item in loadingList)
                {
                    if (item.key.Equals("loading_RefrishLocations"))
                    {
                        item.value = true;
                        break;
                    }
                }
        }

        */
        #endregion

        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private void ControlsEditable()
        {
            /*
            if (receipt.ReceiptId.Equals(0))
            {
                btn_posting.Visibility = Visibility.Collapsed;
                btn_printInvoice.IsEnabled = false;
                tb_InvoiceAmount.IsEnabled = true;
                tb_AmountDifference.IsEnabled = true;
                tb_SupInvoiceNum.IsEnabled = true;
                tb_FreePercentage.IsEnabled = true;
                dp_ReceiptDate.IsEnabled = true;
                dp_SupInvoiceDate.IsEnabled = true;
                cb_ReceiptType.IsEnabled = true;
                //check receipt status first
                switch (_ReceiptType)
                {

                    case "salesInvoices": //supplying order is approved
                        btn_save.IsEnabled = true;
                        col_mainCost.IsReadOnly = true;
                        col_maxQty.Visibility = Visibility.Visible;

                        if (tgl_IsRecieveAll.IsChecked == true)
                        {
                            dg_invoiceDetails.Columns[0].Visibility = Visibility.Collapsed;
                            col_freeQty.IsReadOnly = true;
                            col_maxQty.IsReadOnly = true;
                            col_minQty.IsReadOnly = true;
                        }
                        else
                        {
                            dg_invoiceDetails.Columns[0].Visibility = Visibility.Visible;
                            col_freeQty.IsReadOnly = false;
                            col_maxQty.IsReadOnly = false;
                            col_minQty.IsReadOnly = false;
                        }

                        grd_salesNum.IsEnabled = true;
                        sp_IsRecieveAll.Visibility = Visibility.Visible;
                        brd_salesNumber.Visibility = Visibility.Visible;
                        brd_CustomFreeType.Visibility = Visibility.Collapsed;

                        brd_grid0_0.IsEnabled = false;
                        cb_LocationId.IsEnabled = false;
                        cb_SupId.IsEnabled = false;
                        break;
                    case "free":
                    case "freeVegetables":
                    case "customFree":
                        dg_invoiceDetails.Columns[0].Visibility = Visibility.Visible;

                        col_freeQty.IsReadOnly = false;
                        col_maxQty.IsReadOnly = true;
                        col_minQty.IsReadOnly = false;

                        col_maxQty.Visibility = Visibility.Collapsed;
                        brd_grid0_0.IsEnabled = true;
                        cb_LocationId.IsEnabled = true;
                        cb_SupId.IsEnabled = true;

                        if (_ReceiptType == "customFree")
                            brd_CustomFreeType.Visibility = Visibility.Visible;
                        else
                            brd_CustomFreeType.Visibility = Visibility.Collapsed;

                        if (_ReceiptType == "vegetable" || _ReceiptType == "freeVegetables")
                            col_mainCost.IsReadOnly = false;
                        else
                            col_mainCost.IsReadOnly = true;

                        if (receipt.SalesId != null)
                        {
                            sp_IsRecieveAll.Visibility = Visibility.Visible;
                            brd_salesNumber.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            sp_IsRecieveAll.Visibility = Visibility.Collapsed;
                            brd_salesNumber.Visibility = Visibility.Collapsed;
                        }
                        break;
                    default:
                        dg_invoiceDetails.Columns[0].Visibility = Visibility.Visible;
                        col_maxQty.Visibility = Visibility.Visible;

                        col_freeQty.IsReadOnly = false;
                        col_maxQty.IsReadOnly = false;
                        col_minQty.IsReadOnly = false;

                        //grd_salesNum.Visibility = Visibility.Collapsed;


                        brd_grid0_0.IsEnabled = true;
                        cb_LocationId.IsEnabled = true;
                        cb_SupId.IsEnabled = true;

                        if (_ReceiptType == "customFree")
                            brd_CustomFreeType.Visibility = Visibility.Visible;
                        else
                            brd_CustomFreeType.Visibility = Visibility.Collapsed;

                        if (_ReceiptType == "vegetable" || _ReceiptType == "freeVegetables")
                            col_mainCost.IsReadOnly = false;
                        else
                            col_mainCost.IsReadOnly = true;

                        if (receipt.SalesId != null)
                        {
                            //grd_salesNum.Visibility = Visibility.Visible;
                            sp_IsRecieveAll.Visibility = Visibility.Visible;
                            brd_salesNumber.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            //grd_salesNum.Visibility = Visibility.Collapsed;
                            sp_IsRecieveAll.Visibility = Visibility.Collapsed;
                            brd_salesNumber.Visibility = Visibility.Collapsed;
                        }

                        break;

                }
            }
            else
            {
                switch (_ReceiptType)
                {

                    case "salesInvoices": //supplying order is approved

                        sp_IsRecieveAll.Visibility = Visibility.Visible;
                        brd_salesNumber.Visibility = Visibility.Visible;
                        brd_CustomFreeType.Visibility = Visibility.Collapsed;
                        col_maxQty.Visibility = Visibility.Visible;

                        break;
                    case "free":
                    case "freeVegetables":
                    case "customFree":
                        col_maxQty.Visibility = Visibility.Collapsed;

                        sp_IsRecieveAll.Visibility = Visibility.Collapsed;
                        brd_salesNumber.Visibility = Visibility.Collapsed;

                        if (_ReceiptType == "customFree")
                            brd_CustomFreeType.Visibility = Visibility.Visible;
                        else
                            brd_CustomFreeType.Visibility = Visibility.Collapsed;
                        break;
                    default:
                        col_maxQty.Visibility = Visibility.Visible;

                        sp_IsRecieveAll.Visibility = Visibility.Collapsed;
                        brd_salesNumber.Visibility = Visibility.Collapsed;

                        if (_ReceiptType == "customFree")
                            brd_CustomFreeType.Visibility = Visibility.Visible;
                        else
                            brd_CustomFreeType.Visibility = Visibility.Collapsed;
                        break;

                }
                dg_invoiceDetails.Columns[0].Visibility = Visibility.Collapsed;

                col_freeQty.IsReadOnly = true;
                col_maxQty.IsReadOnly = true;
                col_minQty.IsReadOnly = true;
                col_mainCost.IsReadOnly = true;
                grd_salesNum.IsEnabled = false;

                brd_grid0_0.IsEnabled = false;

                cb_ReceiptType.IsEnabled = false;
                cb_LocationId.IsEnabled = false;
                cb_SupId.IsEnabled = false;
                cb_CustomFreeType.IsEnabled = false;
                tb_InvoiceAmount.IsEnabled = false;
                tb_AmountDifference.IsEnabled = false;
                tb_SupInvoiceNum.IsEnabled = false;
                tb_FreePercentage.IsEnabled = false;
                dp_ReceiptDate.IsEnabled = false;
                dp_SupInvoiceDate.IsEnabled = false;
                grd_salesNum.IsEnabled = false;

                btn_save.IsEnabled = false;
                btn_printInvoice.IsEnabled = true;

                //btn posting
                if (receipt.ReceiptStatus == "notCarriedOver")
                {
                    btn_posting.Visibility = Visibility.Visible;
                    btn_posting.Content = AppSettings.resourcemanager.GetString("Posting");
                }
                else if (receipt.ReceiptStatus == "locationTransfer")
                {
                    btn_posting.Visibility = Visibility.Visible;
                    btn_posting.Content = AppSettings.resourcemanager.GetString("CancelPosting");
                }
                else
                {
                    btn_posting.Visibility = Visibility.Collapsed;
                }
            }

            */
        }

        #endregion
        #region events
        private void Btn_newDraft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // HelpClass.StartAwait(grid_main);
                Clear();
                //await addDraft();

                //HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                //HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        //private async Task addDraft()
        //{

        //    if (billDetails.Count > 0 && _ReceiptType == "po")
        //    {
        //        #region Accept
        //        MainWindow.mainWindow.Opacity = 0.2;
        //        wd_acceptCancelPopup w = new wd_acceptCancelPopup();
        //        w.contentText = AppSettings.resourcemanager.GetString("trSaveOrderNotification");
        //        w.ShowDialog();
        //        MainWindow.mainWindow.Opacity = 1;
        //        #endregion
        //        if (w.isOk)
        //        {
        //            await addInvoice();
        //        }
        //        else
        //            await Clear();
        //    }
        //    else
        //        await Clear();
        //}

        private bool canAddInvoice()
        {
            bool canAdd = true;
            /*
            decimal invoiceAmount = decimal.Parse(tb_InvoiceAmount.Text);
            decimal diff = decimal.Parse(tb_AmountDifference.Text);
            decimal costNet = decimal.Parse(txt_CostNet.Text);

            if (costNet != (invoiceAmount + diff))
                return false;
            */
            return canAdd;
        }
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (billDetails.Count == 0)
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trOrderWithoutItemsError"), animation: ToasterAnimation.FadeIn);
                else if (HelpClass.validate(requiredControlList, this))
                {
                    if (canAddInvoice())
                        await addInvoice();
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trInvoiceAmountError"), animation: ToasterAnimation.FadeIn);

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

        private async Task addInvoice()
        {
            /*
            receipt.LocationId = (long)cb_LocationId.SelectedValue;
            receipt.SupId = (long)cb_SupId.SelectedValue;
            receipt.ReceiptType = _ReceiptType;
            receipt.InvType = "receipt";
            if (salesInvoice.SalesId != 0)
                receipt.SalesId = salesInvoice.SalesId;

            receipt.ReceiptStatus = "notCarriedOver";

            receipt.ReceiptDate = (DateTime)dp_ReceiptDate.SelectedDate;
            receipt.SupInvoiceDate = (DateTime)dp_SupInvoiceDate.SelectedDate;
            receipt.SupInvoiceNum = tb_SupInvoiceNum.Text;
            receipt.SupplierNotes = tb_SupplierNotes.Text;
            receipt.SupplierSalesNotes = tb_SupplierSalesNotes.Text;
            receipt.SupplierNotes = supplier.Notes;
            receipt.SupplierSalesNotes = supplier.SalesInvoiceNotes;

            if (tgl_IsRecieveAll.IsChecked == true)
                receipt.IsRecieveAll = true;
            else
                receipt.IsRecieveAll = false;

            receipt.InvoiceAmount = decimal.Parse(tb_InvoiceAmount.Text);
            receipt.AmountDifference = decimal.Parse(tb_AmountDifference.Text);

            receipt.TotalCost = decimal.Parse(txt_TotalCost.Text);
            receipt.TotalPrice = decimal.Parse(txt_TotalPrice.Text);

            receipt.CoopDiscount = supplier.DiscountPercentage;
            receipt.DiscountValue = decimal.Parse(txt_DiscountValue.Text);

            if (tb_FreePercentage.Text != "")
                receipt.FreePercentage = decimal.Parse(tb_FreePercentage.Text);
            receipt.FreeValue = decimal.Parse(txt_FreeValue.Text);
            receipt.ConsumerDiscount = decimal.Parse(txt_ConsumerDiscount.Text);
            receipt.CostNet = decimal.Parse(txt_CostNet.Text);

            receipt.CreateUserId = MainWindow.userLogin.userId;

            receipt.ReceiptDetails = billDetails;
            receipt = await receipt.SaveReceiptOrder(receipt);

            if (receipt.ReceiptId == 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            else
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                fillReceiptInputs(receipt);
            }
            */
        }
        #region datagrid events
        void deleteRowFromInvoiceItems(object sender, RoutedEventArgs e)
        {
            try
            {
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        SalesInvoiceDetails row = (SalesInvoiceDetails)dg_invoiceDetails.SelectedItems[0];
                        int index = dg_invoiceDetails.SelectedIndex;

                        // remove item from bill
                        billDetails.RemoveAt(index);

                        //dg_invoiceDetails.Items.Clear();
                        //dg_invoiceDetails.ItemsSource = billDetails;
                        //dg_invoiceDetails.Items.Refresh();
                        if (!forceCancelEdit)
                        {
                            dg_invoiceDetails.IsEnabled = false;
                            RefreshInvoiceDetailsDataGrid();
                        }
                        // calculate new total
                        refreshValues();
                    }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion

        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //await Search();
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
                // HelpClass.StartAwait(grid_main);
                Clear();
                // HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                // HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /*
        private async void searchType_check(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox cb = sender as CheckBox;
                if (cb.IsFocused)
                {
                    if (cb.IsChecked == true)
                    {
                        if (cb.Name == "chk_barcode")
                        {
                            chk_itemNum.IsChecked = false;
                        }
                        else if (cb.Name == "chk_itemNum")
                        {
                            chk_barcode.IsChecked = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void searchType_uncheck(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox cb = sender as CheckBox;
                if (cb.IsFocused)
                {
                    if (cb.Name == "chk_barcode")
                        chk_barcode.IsChecked = true;
                    else if (cb.Name == "chk_itemNum")
                        chk_itemNum.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        */
        #endregion
        #region Refresh & Search

        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {

            this.DataContext = new Receipt();

            receipt = new Receipt();
            salesInvoice = new SalesInvoice();

            billDetails = new List<SalesInvoiceDetails>();
            //dg_invoiceDetails.ItemsSource = billDetails;
            //dg_invoiceDetails.Items.Refresh();
            if (!forceCancelEdit)
            {
                dg_invoiceDetails.IsEnabled = false;
                RefreshInvoiceDetailsDataGrid();
            }
            //await Task.Delay(0050);
            //dp_ReceiptDate.SelectedDate = DateTime.Now;
            //dp_SupInvoiceDate.SelectedDate = DateTime.Now;

           




            // _ReceiptType = "salesInvoices";
            ControlsEditable();

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

        private async Task search()
        {

            try
            {
               
                if(tb_search.Text != "")
                {
                    HelpClass.StartAwait(grid_main);

                    Item item1 = null;


                    item1 = await FillCombo.item.GetItemByBarcode(tb_search.Text, AppSettings.locationId);                                  

                    if (item1 != null)
                    {
                        //add item to invoice
                       
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trItemNotFoundError"), animation: ToasterAnimation.FadeIn);

                    tb_search.Text = "";

                    HelpClass.EndAwait(grid_main);
                }
            }
            catch (Exception ex)
            {

                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        List<SalesInvoiceDetails> billDetails = new List<SalesInvoiceDetails>();
        private void addItemToBill(SalesInvoiceDetails salesInvoiceDetails)
        {
            /*
            int index = billDetails.IndexOf(billDetails.Where(p => p.ItemId == salesInvoiceDetails.ItemId).FirstOrDefault());

            if (index == -1)//item doesn't exist in bill
            {
                billDetails.Add(salesInvoiceDetails);

                //dg_invoiceDetails.ItemsSource = billDetails;
                //dg_invoiceDetails.Items.Refresh();
                if (!forceCancelEdit)
                {
                    dg_invoiceDetails.IsEnabled = false;
                    RefreshInvoiceDetailsDataGrid();
                }
                refreshValues();
            }
            else // item exist prevoiusly in list
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trItemExistInOrderError"), animation: ToasterAnimation.FadeIn);

            }
            */
        }

        decimal _TotalCost = 0;
        decimal _TotalPrice = 0;
        decimal _DiscountValue = 0;
        decimal _ConsumerDiscount = 0;

        private void refreshValues()
        {
            /*
            _TotalCost = 0;
            _TotalPrice = 0;
            _DiscountValue = 0;
            _ConsumerDiscount = 0;
            foreach (var row in billDetails)
            {
                _TotalCost += row.Cost * ((int)row.MinQty + ((int)row.MaxQty * (int)row.Factor));
                _TotalPrice += row.Price * ((int)row.MinQty + ((int)row.MaxQty * (int)row.Factor));
                _ConsumerDiscount = (decimal)row.ConsumerDiscount;
            }

            txt_TotalCost.Text = HelpClass.DecTostring(_TotalCost);
            txt_TotalPrice.Text = HelpClass.DecTostring(_TotalPrice);
            txt_ConsumerDiscount.Text = HelpClass.DecTostring(_ConsumerDiscount);

            //cost after discount
            var discount = HelpClass.calcPercentage(_TotalCost, supplier.DiscountPercentage);
            _DiscountValue = discount;
            txt_DiscountValue.Text = HelpClass.DecTostring(_DiscountValue);

            //free quantity
            decimal freePercentage = 0;
            decimal freeValue = 0;
            if (tb_FreePercentage.Text != "")
            {
                freePercentage = decimal.Parse(tb_FreePercentage.Text);
                freeValue = HelpClass.calcPercentage(_TotalPrice, freePercentage);
            }
            txt_FreeValue.Text = HelpClass.DecTostring(freeValue);

            decimal netCost = _TotalCost - discount;
            txt_CostNet.Text = HelpClass.DecTostring(netCost);
            tb_NetInvoice.Text = HelpClass.DecTostring(netCost);
            */

        }

        private void Btn_receiptOrders_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_receiptInv w = new wd_receiptInv();
                w.invType = "receipt";
                w.ShowDialog();
                if (w.isOk)
                {
                    receipt = w.receipt;
                    _ReceiptType = receipt.ReceiptType;
                    fillReceiptInputs(receipt);
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

        public async Task fillOrderInputs(SalesInvoice invoice)
        {

            this.DataContext = receipt;

            /*
             txt_TotalCost.Text = HelpClass.DecTostring(invoice.TotalCost);
            txt_TotalPrice.Text = HelpClass.DecTostring(invoice.TotalPrice);
            txt_EnterpriseDiscount.Text = HelpClass.DecTostring(invoice.CoopDiscount);
            txt_DiscountValue.Text = HelpClass.DecTostring(invoice.DiscountValue);
            tb_FreePercentage.Text = HelpClass.DecTostring(invoice.FreePercentage);
            txt_FreeValue.Text = HelpClass.DecTostring(invoice.FreeValue);
            txt_ConsumerDiscount.Text = HelpClass.DecTostring(invoice.ConsumerDiscount);
            txt_CostNet.Text = HelpClass.DecTostring(invoice.CostNet);
            */
            await buildSalesInvoiceDetails(invoice);

            ControlsEditable();

        }
        public void fillReceiptInputs(Receipt invoice)
        {

            this.DataContext = invoice;
            /*
            txt_TotalCost.Text = HelpClass.DecTostring(receipt.TotalCost);
            txt_TotalPrice.Text = HelpClass.DecTostring(receipt.TotalPrice);
            txt_EnterpriseDiscount.Text = HelpClass.DecTostring(receipt.CoopDiscount);
            txt_DiscountValue.Text = HelpClass.DecTostring(receipt.DiscountValue);
            tb_FreePercentage.Text = HelpClass.DecTostring(receipt.FreePercentage);
            txt_FreeValue.Text = HelpClass.DecTostring(receipt.FreeValue);
            txt_ConsumerDiscount.Text = HelpClass.DecTostring(receipt.ConsumerDiscount);
            txt_CostNet.Text = HelpClass.DecTostring(receipt.CostNet);
            */
            buildInvoiceDetails(invoice);

            ControlsEditable();

        }
        private async Task buildSalesInvoiceDetails(SalesInvoice invoice)
        {
            /*
            billDetails = new List<SalesInvoiceDetails>();
            foreach (var row in invoice.SalesDetails)
            {
                billDetails.Add(new SalesInvoiceDetails()
                {

                    Balance = row.Balance,
                    Barcode = row.Barcode,
                    ConsumerDiscount = row.ConsumerDiscount,
                    CoopDiscount = row.CoopDiscount,
                    Cost = row.Cost,
                    Factor = row.Factor,
                    FreeQty = (int)row.FreeQty,
                    ItemCode = row.ItemCode,
                    ItemName = row.ItemName,
                    ItemId = row.ItemId,
                    MainCost = row.MainCost,
                    MainPrice = row.MainPrice,
                    ItemNotes = row.ItemNotes,
                    MaxQty = (int)row.MaxQty,
                    MinQty = (int)row.MinQty,
                    Price = row.Price,
                    CreateUserId = MainWindow.userLogin.userId,
                });
            }

           


            if (!forceCancelEdit)
            {
                dg_invoiceDetails.IsEnabled = false;
                RefreshInvoiceDetailsDataGrid();
            }

            */
        }
        private void buildInvoiceDetails(Receipt invoice)
        {
            /*
            billDetails = invoice.ReceiptDetails.ToList();
            if (!forceCancelEdit)
            {
                dg_invoiceDetails.IsEnabled = false;
                RefreshInvoiceDetailsDataGrid();
            }
            */
        }

        #region print
        private void btn_printInvoice_Click(object sender, RoutedEventArgs e)
        {
            //try
            {
                if (sender != null)
                    HelpClass.StartAwait(grid_main);

                ////////////////
                Thread t1 = new Thread(async () =>
                {
                    string msg = "";
                    msg = await printInvoice(receipt);
                    if (msg == "")
                    {

                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString(msg), animation: ToasterAnimation.FadeIn);
                        });
                    }
                });
                t1.Start();
                /////////////////

                if (sender != null)
                    HelpClass.EndAwait(grid_main);
            }
            //catch (Exception ex)
            //{
            //    if (sender != null)
            //        HelpClass.EndAwait(grid_main);
            //    HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            //}
        }

        LocalReport rep = new LocalReport();
        ReportCls reportclass = new ReportCls();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        public async Task<string> printInvoice(Receipt prInvoice)
        {
            string msg = "";

            try
            {
                int sequence = 0;
                foreach (var row in prInvoice.ReceiptDetails)
                {
                    row.Sequence = sequence++;
                }

                List<ReportParameter> paramarr = new List<ReportParameter>();

                rep.ReportPath = reportclass.GetReceiptOrderRdlcpath();

                // ReportsConfig.setReportLanguage(paramarr);

                reportclass.fillReceiptOrderReport(prInvoice, rep, paramarr);
                rep.SetParameters(paramarr);
                rep.Refresh();

                //copy count
                saveFileDialog.Filter = "PDF|*.pdf;";

                this.Dispatcher.Invoke(() =>
                {

                    if (saveFileDialog.ShowDialog() == true)
                    {

                        string filepath = saveFileDialog.FileName;
                        ReportsConfig.ExportToPDF(rep, filepath);
                    }
                });


            }
            catch (Exception ex)
            {
                msg = "trNotCompleted";
            }

            return msg;

        }
        #endregion
        private async void tb_search_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    await search();
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }



        private void btn_columnSwap_Click(object sender, RoutedEventArgs e)
        {
            ColumnDefinition cd_gridMain3 = new ColumnDefinition();
            cd_gridMain3.Width = cd_gridMain1.Width;
            cd_gridMain1.Width = cd_gridMain2.Width;
            cd_gridMain2.Width = cd_gridMain3.Width;

            if (Grid.GetColumn(brd_grid0_0) == 0)
            {
                Grid.SetColumn(brd_grid0_0, 1);
                Grid.SetColumn(brd_grid1_0, 1);
                Grid.SetColumn(brd_grid2_0, 1);

                Grid.SetColumn(brd_grid0_1, 0);
                Grid.SetColumn(brd_grid1_1, 0);
                Grid.SetColumn(brd_grid2_1, 0);
            }
            else
            {
                Grid.SetColumn(brd_grid0_0, 0);
                Grid.SetColumn(brd_grid1_0, 0);
                Grid.SetColumn(brd_grid2_0, 0);

                Grid.SetColumn(brd_grid0_1, 1);
                Grid.SetColumn(brd_grid1_1, 1);
                Grid.SetColumn(brd_grid2_1, 1);
            }
        }



        private async void Btn_deleteInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
                #region
                Window.GetWindow(this).Opacity = 0.2;
                wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");

                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
                #endregion
                if (w.isOk)
                {
                    var res = await receipt.deleteReceiptInv(receipt.ReceiptId, MainWindow.userLogin.userId);

                    if (res != 0)
                    {

                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);
                        await Clear();
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                }
                */
            }
            catch
            {

            }
        }

        private async void dg_invoiceDetails_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            /*
            try
            {
                HelpClass.StartAwait(grid_main);

                TextBox t = e.EditingElement as TextBox;  // Assumes columns are all TextBoxes
                decimal textValue = 0;
                if (t.Text != "")
                    textValue = decimal.Parse(t.Text);

                var columnName = e.Column.Header.ToString();

                SalesInvoiceDetails row = e.Row.Item as SalesInvoiceDetails;
                int index = billDetails.IndexOf(billDetails.Where(p => p.ItemId == row.ItemId).FirstOrDefault());

                int maxQty = 0;
                int minQty = 0;
                int freeQty = 0;
                decimal mainCost = 0;
                decimal mainPrice = 0;

                if (columnName == AppSettings.resourcemanager.GetString("MaxFactorQty") && !t.Text.Equals(""))
                {
                    if (_ReceiptType == "salesInvoices")
                    {
                        var it = salesInvoice.SalesDetails.Where(x => x.ItemId == row.ItemId).FirstOrDefault();
                        if (it.MaxQty < textValue)
                        {

                            textValue = (int)it.MaxQty;
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trExceedOrderqauntityError"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    maxQty = (int)textValue;
                }
                else
                    maxQty = (int)row.MaxQty;

                if (columnName == AppSettings.resourcemanager.GetString("LessFactorQty") && !t.Text.Equals(""))
                {
                    if (_ReceiptType == "salesInvoices")
                    {
                        if (textValue >= row.Factor)
                        {
                            textValue = 0;
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMinQtyMoreFactorError"), animation: ToasterAnimation.FadeIn);
                        }
                        var it = salesInvoice.SalesDetails.Where(x => x.ItemId == row.ItemId).FirstOrDefault();
                        if (it.MaxQty < textValue)
                        {

                            textValue = (int)it.MinQty;
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trExceedOrderqauntityError"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    minQty = (int)textValue;
                }
                else
                    minQty = (int)row.MinQty;

                if (columnName == AppSettings.resourcemanager.GetString("Free") && !t.Text.Equals(""))
                    freeQty = (int)textValue;
                else
                    freeQty = (int)row.FreeQty;


                if (columnName == AppSettings.resourcemanager.GetString("trCost") && !t.Text.Equals(""))
                {
                    var item = await FillCombo.item.GetItemByCodeOrName(row.ItemCode, (long)cb_LocationId.SelectedValue, (long)cb_SupId.SelectedValue, _ReceiptType);
                    mainCost = textValue;
                    mainPrice = mainCost / (int)row.Factor * (1 + HelpClass.calcPercentage(1, item[0].Category.ProfitPercentage));
                }
                else
                {
                    mainCost = (decimal)row.MainCost;
                    mainPrice = (decimal)row.MainPrice;
                }

                decimal cost = (mainCost * maxQty) + ((mainCost / (int)row.Factor) * minQty);
                decimal price = ((int)row.Factor * mainPrice * maxQty) + (mainPrice * minQty);

                billDetails[index].MainCost = mainCost;
                billDetails[index].MainPrice = mainPrice;
                billDetails[index].MinQty = minQty;
                billDetails[index].MaxQty = maxQty;
                billDetails[index].FreeQty = freeQty;
                billDetails[index].Cost = cost;
                billDetails[index].Price = price;

                refreshValues();

                //dg_invoiceDetails.ItemsSource = billDetails;
                //dg_invoiceDetails.Items.Refresh();
                if (!forceCancelEdit)
                {
                    dg_invoiceDetails.IsEnabled = false;
                    RefreshInvoiceDetailsDataGrid();
                }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            */
        }


    

      



        bool forceCancelEdit = false;
        public void RefreshInvoiceDetailsDataGrid()
        {
            try
            {
                forceCancelEdit = true;
                dg_invoiceDetails.CancelEdit();
                dg_invoiceDetails.ItemsSource = billDetails;
                dg_invoiceDetails.Items.Refresh();

                dg_invoiceDetails.IsEnabled = true;
                forceCancelEdit = false;

            }
            catch (Exception ex)
            {
                dg_invoiceDetails.IsEnabled = true;
                forceCancelEdit = false;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
        }

       
    }
}
