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

namespace POSCA.View.receipts
{
    /// <summary>
    /// Interaction logic for uc_receiptInvoice.xaml
    /// </summary>
    public partial class uc_receiptInvoice : UserControl
    {

        public uc_receiptInvoice()
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
        private static uc_receiptInvoice _instance;
        public static uc_receiptInvoice Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_receiptInvoice();
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
        private PurchaseInvoice purchaseOrder= new PurchaseInvoice();
        private string _ReceiptType = "purchaseOrders";
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
                requiredControlList = new List<string> { "LocationId", "SupplierId","SupInvoiceNum",
                                                "InvoiceAmount", "AmountDifference" };
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();


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
              
                FillCombo.fillReceiptsTypes(cb_ReceiptType);
                FillCombo.fillReceiptStatus(cb_ReceiptStatus);

                cb_ReceiptType.SelectedIndex = 0;
                cb_ReceiptType.SelectedValue = "purchaseOrders";
                //await Search();
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

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            chk_barcode.Content = AppSettings.resourcemanager.GetString("trBarcode");
            chk_itemNum.Content = AppSettings.resourcemanager.GetString("ItemNumber");
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

            txt_search.Text = AppSettings.resourcemanager.GetString("trSearch");
            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("Receipt");
            txt_invoiceDetails.Text = AppSettings.resourcemanager.GetString("OrderDetails");
            txt_TotalCostTitle.Text = AppSettings.resourcemanager.GetString("TotalCost");
            txt_TotalPriceTitle.Text = AppSettings.resourcemanager.GetString("trTotalPrice");
            txt_EnterpriseDiscountTitle.Text = AppSettings.resourcemanager.GetString("EnterpriseDiscount");
            txt_DiscountValueTitle.Text = AppSettings.resourcemanager.GetString("DiscountValue");
            txt_FreePercentageTitle.Text = AppSettings.resourcemanager.GetString("FreePercentag");
            txt_FreeValueTitle.Text = AppSettings.resourcemanager.GetString("FreeValue");
            txt_ConsumerDiscountTitle.Text = AppSettings.resourcemanager.GetString("ConsumerDiscount");
            txt_CostNetTitle.Text = AppSettings.resourcemanager.GetString("NetCost");
            txt_IsRecieveAll.Text = AppSettings.resourcemanager.GetString("ReceiptAll");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_LocationId, AppSettings.resourcemanager.GetString("trBranchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_ReceiptStatus, AppSettings.resourcemanager.GetString("DocumentStatusHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_ReceiptType, AppSettings.resourcemanager.GetString("ReceiptTypeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_InvNumber, AppSettings.resourcemanager.GetString("VouchernoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_PurchaseInvNumber, AppSettings.resourcemanager.GetString("PurchaseOrderNumHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_SupId, AppSettings.resourcemanager.GetString("SupplierHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_SupInvoiceNum, AppSettings.resourcemanager.GetString("trInvoiceNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_ReceiptDate, AppSettings.resourcemanager.GetString("ReceiptDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_SupInvoiceDate, AppSettings.resourcemanager.GetString("trInvoiceDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_InvoiceAmount, AppSettings.resourcemanager.GetString("InvoiceAmountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_AmountDifference, AppSettings.resourcemanager.GetString("ApproximateVariancesHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_NetInvoice, AppSettings.resourcemanager.GetString("NetInvoiceHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_SupplierNotes, AppSettings.resourcemanager.GetString("SupplierNotesHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_SupplierPurchaseNotes, AppSettings.resourcemanager.GetString("SupplierOrderNotesHint"));

            dg_invoiceDetails.Columns[1].Header = AppSettings.resourcemanager.GetString("ItemNumber");
            dg_invoiceDetails.Columns[2].Header = AppSettings.resourcemanager.GetString("itemName");
     
            dg_invoiceDetails.Columns[3].Header = AppSettings.resourcemanager.GetString("Factor");
            dg_invoiceDetails.Columns[4].Header = AppSettings.resourcemanager.GetString("trCost");
            dg_invoiceDetails.Columns[5].Header = AppSettings.resourcemanager.GetString("trPrice");
            dg_invoiceDetails.Columns[6].Header = AppSettings.resourcemanager.GetString("MaxFactorQty");
            dg_invoiceDetails.Columns[7].Header = AppSettings.resourcemanager.GetString("LessFactorQty");
            dg_invoiceDetails.Columns[8].Header = AppSettings.resourcemanager.GetString("Free");
            dg_invoiceDetails.Columns[9].Header = AppSettings.resourcemanager.GetString("ConsumerDiscountTitle");    
            dg_invoiceDetails.Columns[10].Header = AppSettings.resourcemanager.GetString("TotalCost");
            dg_invoiceDetails.Columns[11].Header = AppSettings.resourcemanager.GetString("trTotalPrice");


            btn_newDraft.ToolTip = AppSettings.resourcemanager.GetString("trNew");
            btn_receiptOrders.ToolTip = AppSettings.resourcemanager.GetString("ReceiptOrders");
           // btn_purchaseOrders.ToolTip = AppSettings.resourcemanager.GetString("PurchaseOrders");
            btn_printInvoice.ToolTip = AppSettings.resourcemanager.GetString("trPrint");
        }

        #region Loading
        List<keyValueBool> loadingList;
        List<string> catchError = new List<string>();
        int catchErrorCount = 0;

        bool loadingSuccess_RefrishSuppliers = false;
        async void loading_RefrishSuppliers()
        {
            try
            {
                await FillCombo.fillSuppliers(cb_SupId);
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


        #endregion
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private void ControlsEditable()
        {
            //check receipt status first
            switch (_ReceiptType)
            {

                case "purchaseOrders": //supplying order is approved
                    btn_save.IsEnabled = true;
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

                    // btn_purchaseOrders.Visibility = Visibility.Visible;
                    grd_purchaseNum.Visibility = Visibility.Visible;
                    sp_IsRecieveAll.Visibility = Visibility.Visible;

                    brd_grid0_0.IsEnabled = false;
                    cb_LocationId.IsEnabled = false;
                    cb_SupId.IsEnabled = false;
                    break;
                default:
                    dg_invoiceDetails.Columns[0].Visibility = Visibility.Visible;

                    col_freeQty.IsReadOnly = false;
                    col_maxQty.IsReadOnly = false;
                    col_minQty.IsReadOnly = false;

                    //btn_purchaseOrders.Visibility = Visibility.Collapsed;
                  
                    sp_IsRecieveAll.Visibility = Visibility.Collapsed;

                    brd_grid0_0.IsEnabled = true;
                    cb_LocationId.IsEnabled = true;
                    cb_SupId.IsEnabled = true;
                    if(receipt.PurchaseId != null)
                        grd_purchaseNum.Visibility = Visibility.Visible;
                    else
                        grd_purchaseNum.Visibility = Visibility.Collapsed;

                    break;

            }

            if (receipt.ReceiptId.Equals(0))
            {
                btn_printInvoice.IsEnabled = false;
            }
            else
            {
                btn_printInvoice.IsEnabled = true;

            }


        }

        #endregion
        #region events
        private async void Btn_newDraft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                await addDraft();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private async Task addDraft()
        {

            if (billDetails.Count > 0 && _ReceiptType == "po")
            {
                #region Accept
                MainWindow.mainWindow.Opacity = 0.2;
                wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                w.contentText = AppSettings.resourcemanager.GetString("trSaveOrderNotification");
                w.ShowDialog();
                MainWindow.mainWindow.Opacity = 1;
                #endregion
                if (w.isOk)
                {
                    await addInvoice();
                }
                else
                    await Clear();
            }
            else
                await Clear();
        }

        private bool canAddInvoice()
        {
            bool canAdd = true;
            decimal invoiceAmount = decimal.Parse(tb_InvoiceAmount.Text);
            decimal diff = decimal.Parse(tb_AmountDifference.Text);
            decimal costNet = decimal.Parse(txt_CostNet.Text);

            if (costNet != (invoiceAmount + diff))
                return false;
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
                   if( canAddInvoice())
                    await addInvoice();
                   else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trInvoiceAmountError"), animation: ToasterAnimation.FadeIn);

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
           
            receipt.LocationId = (long)cb_LocationId.SelectedValue;
            receipt.SupId = (long)cb_SupId.SelectedValue;
            receipt.ReceiptType = _ReceiptType;

            if (purchaseOrder.PurchaseId != 0)
                receipt.PurchaseId = purchaseOrder.PurchaseId;

            receipt.ReceiptStatus = "notCarriedOver";

            receipt.ReceiptDate = (DateTime)dp_ReceiptDate.SelectedDate;
            receipt.SupInvoiceDate = (DateTime)dp_SupInvoiceDate.SelectedDate;
            receipt.SupInvoiceNum = tb_SupInvoiceNum.Text;
            receipt.SupplierNotes = tb_SupplierNotes.Text;
            receipt.SupplierPurchaseNotes = tb_SupplierPurchaseNotes.Text;
            receipt.SupplierNotes = supplier.Notes;
            receipt.SupplierPurchaseNotes = supplier.PurchaseOrderNotes;

            if (tgl_IsRecieveAll.IsChecked == true)
                receipt.IsRecieveAll = true;
            else
                receipt.IsRecieveAll = false;

            receipt.InvoiceAmount = decimal.Parse( tb_InvoiceAmount.Text);
            receipt.AmountDifference = decimal.Parse( tb_AmountDifference.Text);

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
           
        }
        #region datagrid events
        void deleteRowFromInvoiceItems(object sender, RoutedEventArgs e)
        {
            try
            {
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        RecieptDetails row = (RecieptDetails)dg_invoiceDetails.SelectedItems[0];
                        int index = dg_invoiceDetails.SelectedIndex;

                        // remove item from bill
                        billDetails.RemoveAt(index);

                        //dg_invoiceDetails.Items.Clear();
                        dg_invoiceDetails.ItemsSource = billDetails;
                        dg_invoiceDetails.Items.Refresh();
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

        #endregion
        #region Refresh & Search

        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        async Task Clear()
        {

            this.DataContext = new Receipt();

            receipt = new Receipt();
            purchaseOrder = new PurchaseInvoice();

            billDetails = new List<RecieptDetails>();
            dg_invoiceDetails.ItemsSource = billDetails;
            dg_invoiceDetails.Items.Refresh();

            //await Task.Delay(0050);
            //dp_ReceiptDate.SelectedDate = DateTime.Now;
            //dp_SupInvoiceDate.SelectedDate = DateTime.Now;

            txt_TotalCost.Text = HelpClass.DecTostring(0);
            txt_TotalPrice.Text = HelpClass.DecTostring(0);
            txt_EnterpriseDiscount.Text = HelpClass.DecTostring(0);
            txt_EnterpriseDiscount.Text = HelpClass.DecTostring(0);
            txt_DiscountValue.Text = HelpClass.DecTostring(0);
            tb_FreePercentage.Text = HelpClass.DecTostring(0);
            txt_FreeValue.Text = HelpClass.DecTostring(0);
            txt_ConsumerDiscount.Text = HelpClass.DecTostring(0);
            txt_CostNet.Text = HelpClass.DecTostring(0);

            col_freeQty.IsReadOnly = false;
            col_maxQty.IsReadOnly = false;
            col_minQty.IsReadOnly = false;

            
           // _ReceiptType = "purchaseOrders";
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

        private async void Btn_search_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                if (location == null)
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectLocationError"), animation: ToasterAnimation.FadeIn);
                else if (supplier == null)
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectSupplierError"), animation: ToasterAnimation.FadeIn);
                else
                {
                    HelpClass.StartAwait(grid_main);

                    List<Item> itemLst = new List<Item>();
                    Item item1 = null;
                    string barcode = "";

                    if (chk_itemNum.IsChecked == true)
                    {
                        itemLst = await FillCombo.item.GetItemByCodeOrName(tb_search.Text, location.LocationId, supplier.SupId,_ReceiptType);
                        if (itemLst.Count == 1)
                        {
                            item1 = itemLst[0];
                            barcode = item1.ItemUnits.FirstOrDefault().Barcode;
                        }

                    }
                    else
                    {
                        item1 = await FillCombo.item.GetItemByBarcode(tb_search.Text, location.LocationId, supplier.SupId,_ReceiptType);
                        barcode = tb_search.Text;
                    }

                    if (itemLst.Count > 1)
                    {
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_addPurchaseItems w = new wd_addPurchaseItems();
                        w.items = itemLst.ToList();
                        w.supId = (long)cb_SupId.SelectedValue;
                        w.locationId = (long)cb_LocationId.SelectedValue;

                        w.ShowDialog();
                        if (w.isOk)
                        {
                            item1 = w.item;
                            barcode = item1.ItemUnits.FirstOrDefault().Barcode;
                        }
                    }


                    if (item1 != null)
                    {
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_addPurchaseItem w = new wd_addPurchaseItem();
                        long balance = item1.ItemLocations.Where(x => x.LocationId == location.LocationId).FirstOrDefault().Balance;

                        w.newReceiptItem = new RecieptDetails()
                        {
                            ItemId = item1.ItemId,
                            ItemCode = item1.Code,
                            ItemName = item1.Name,
                            ItemUnit = item1.ItemUnit,
                            Factor = item1.Factor,
                            MainCost = item1.MainCost,
                            CoopDiscount = supplier.DiscountPercentage,
                            ConsumerDiscount = item1.ConsumerDiscPerc,
                            MainPrice = item1.Price,
                            Barcode = barcode,
                            Balance = balance,
                        };
                        w.ShowDialog();

                        if (w.isOk)
                        {
                            //check billDetails count
                            if (billDetails.Count < 20)
                            {
                                int maxQty = 0;
                                int minQty = 0;
                                if (w.newReceiptItem.MaxQty != null)
                                    maxQty = (int)w.newReceiptItem.MaxQty;
                                if (w.newReceiptItem.MinQty != null)
                                    minQty = (int)w.newReceiptItem.MinQty;
                                w.newReceiptItem.Cost = (w.newReceiptItem.MainCost * maxQty) + ((w.newReceiptItem.MainCost / (int)w.newReceiptItem.Factor) * minQty);
                                w.newReceiptItem.Price = ((int)w.newReceiptItem.Factor * w.newReceiptItem.MainPrice * maxQty) + (w.newReceiptItem.MainPrice * minQty);

                                if (w.newReceiptItem.MinQty == null)
                                    w.newReceiptItem.MinQty = 0;
                                if (w.newReceiptItem.FreeQty == null)
                                    w.newReceiptItem.FreeQty = 0;
                                addItemToBill(w.newReceiptItem);
                            }
                            else
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMoreTwentyItemsError"), animation: ToasterAnimation.FadeIn);

                        }
                        Window.GetWindow(this).Opacity = 1;
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
        List<RecieptDetails> billDetails = new List<RecieptDetails>();
        private void addItemToBill(RecieptDetails recieptDetails)
        {
            int index = billDetails.IndexOf(billDetails.Where(p => p.ItemId == recieptDetails.ItemId).FirstOrDefault());

            if (index == -1)//item doesn't exist in bill
            {
                billDetails.Add(recieptDetails);

                dg_invoiceDetails.ItemsSource = billDetails;
                dg_invoiceDetails.Items.Refresh();
                refreshValues();
            }
            else // item exist prevoiusly in list
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trItemExistInOrderError"), animation: ToasterAnimation.FadeIn);

            }
        }

        decimal _TotalCost = 0;
        decimal _TotalPrice = 0;
        decimal _DiscountValue = 0;
        decimal _ConsumerDiscount = 0;

        private void refreshValues()
        {
          
            _TotalCost = 0;
            _TotalPrice = 0;
            _DiscountValue = 0;
            _ConsumerDiscount = 0;
            foreach (var row in billDetails)
            {
                _TotalCost += row.Cost;
                _TotalPrice += row.Price;
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
          
        }
        //private void Btn_purchaseOrders_Click(object sender, RoutedEventArgs e)
        //{
        
        //    try
        //    {
        //        HelpClass.StartAwait(grid_main);
        //        Window.GetWindow(this).Opacity = 0.2;
        //        wd_purchaseInv w = new wd_purchaseInv();

        //        string invoiceType = "po";
        //        w.invoiceType = invoiceType;
        //        w.invoiceStatus = "orderPlaced";

        //        w.ShowDialog();
        //        if (w.isOk)
        //        {
        //            purchaseOrder = w.purchaseInvoice;

        //            receipt.LocationId = purchaseOrder.LocationId;
        //            receipt.SupId = purchaseOrder.SupId;
        //            receipt.PurchaseInvNumber = purchaseOrder.InvNumber;
                   
        //          receipt.NetInvoice= purchaseOrder.CostNet;

        //            fillOrderInputs(purchaseOrder);
        //        }
        //        Window.GetWindow(this).Opacity = 1;

        //        HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {

        //        Window.GetWindow(this).Opacity = 1;
        //        HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
        //    }
           
        //}

        private void Btn_receiptOrders_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_receiptInv w = new wd_receiptInv();

                w.ShowDialog();
                if (w.isOk)
                {
                    receipt = w.receipt;
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

        public async Task fillOrderInputs(PurchaseInvoice invoice)
        {
           
            this.DataContext = receipt;

            txt_TotalCost.Text = HelpClass.DecTostring(invoice.TotalCost);
            txt_TotalPrice.Text = HelpClass.DecTostring(invoice.TotalPrice);
            txt_EnterpriseDiscount.Text = HelpClass.DecTostring(invoice.CoopDiscount);
            txt_DiscountValue.Text = HelpClass.DecTostring(invoice.DiscountValue);
            tb_FreePercentage.Text = HelpClass.DecTostring(invoice.FreePercentage);
            txt_FreeValue.Text = HelpClass.DecTostring(invoice.FreeValue);
            txt_ConsumerDiscount.Text = HelpClass.DecTostring(invoice.ConsumerDiscount);
            txt_CostNet.Text = HelpClass.DecTostring(invoice.CostNet);

            await buildPurchaseOrderDetails(invoice);

            ControlsEditable();

        }
        public void fillReceiptInputs(Receipt invoice)
        {
           
            this.DataContext = invoice;

            txt_TotalCost.Text = HelpClass.DecTostring(receipt.TotalCost);
            txt_TotalPrice.Text = HelpClass.DecTostring(receipt.TotalPrice);
            txt_EnterpriseDiscount.Text = HelpClass.DecTostring(receipt.CoopDiscount);
            txt_DiscountValue.Text = HelpClass.DecTostring(receipt.DiscountValue);
            tb_FreePercentage.Text = HelpClass.DecTostring(receipt.FreePercentage);
            txt_FreeValue.Text = HelpClass.DecTostring(receipt.FreeValue);
            txt_ConsumerDiscount.Text = HelpClass.DecTostring(receipt.ConsumerDiscount);
            txt_CostNet.Text = HelpClass.DecTostring(receipt.CostNet);

            buildInvoiceDetails(invoice);

            ControlsEditable();
        
        }
        private async Task buildPurchaseOrderDetails(PurchaseInvoice invoice)
        {
            billDetails = new List<RecieptDetails>();
            foreach (var row in invoice.PurchaseDetails)
            {
                billDetails.Add(new RecieptDetails()
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
                    MaxQty =(int) row.MaxQty,
                    MinQty = (int)row.MinQty,
                    Price = row.Price,
                    CreateUserId = MainWindow.userLogin.userId,
                });
            }
          
            dg_invoiceDetails.ItemsSource = billDetails;
            HelpClass.StartAwait(grid_main);
            dg_invoiceDetails.IsEnabled = false;
            await Task.Delay(1000);
            dg_invoiceDetails.Items.Refresh();
            dg_invoiceDetails.IsEnabled = true;
            HelpClass.EndAwait(grid_main);
           

        }
        private void buildInvoiceDetails(Receipt invoice)
        {
            
            billDetails = invoice.ReceiptDetails.ToList();
            dg_invoiceDetails.ItemsSource = invoice.ReceiptDetails;
            dg_invoiceDetails.Items.Refresh();
          
        }

        #region print
        private void btn_printInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                if (sender != null)
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        LocalReport rep = new LocalReport();
        ReportCls reportclass = new ReportCls();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        public async Task<string> printInvoice(Receipt prInvoice)
        {
            string msg = "";
            /*
            try
            {
                //ReportsConfig reportConfig = new ReportsConfig();
                List<ReportParameter> paramarr = new List<ReportParameter>();

                rep.ReportPath = reportclass.GetSupplyingOrderRdlcpath();

                ReportsConfig.setReportLanguage(paramarr);
                ReportsConfig.InvoiceHeader(paramarr);
                reportclass.fillSupplyingOrderReport(prInvoice, rep, paramarr);

                rep.EnableExternalImages = true;
                rep.DataSources.Clear();
                rep.DataSources.Add(new ReportDataSource("DataSetReceiptDetails", receipt.ReceiptDetails));

                rep.EnableExternalImages = true;

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
                //this.Dispatcher.Invoke(() =>
                //{
                //    Toaster.ShowWarning(Window.GetWindow(this), message: "Not completed", animation: ToasterAnimation.FadeIn);

                //});
                msg = "trNotCompleted";
            }
            */
            return msg;

        }
        #endregion
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
        /*
        private void dp_ReceiptDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dp_ReceiptDate.SelectedDate != null && dp_SupInvoiceDate.SelectedDate != null)
                    if (dp_ReceiptDate.SelectedDate.Value.Date > dp_SupInvoiceDate.SelectedDate.Value.Date)
                        dp_SupInvoiceDate.SelectedDate = dp_ReceiptDate.SelectedDate.Value.Date;

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        */

        private void cb_SupId_KeyUp(object sender, KeyEventArgs e)
        {

            try
            {
                var tb = cb_SupId.Template.FindName("PART_EditableTextBox", cb_SupId) as TextBox;
                tb.FontFamily = Application.Current.Resources["Font-cairo-regular"] as FontFamily;
                cb_SupId.ItemsSource = FillCombo.suppliersList.Where(p => p.Name.ToLower().Contains(tb.Text.ToLower()) || p.SupCode.ToString().Contains(tb.Text)).ToList();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }



        private void cb_LocationId_KeyUp(object sender, KeyEventArgs e)
        {

            try
            {
                var tb = cb_LocationId.Template.FindName("PART_EditableTextBox", cb_LocationId) as TextBox;
                tb.FontFamily = Application.Current.Resources["Font-cairo-regular"] as FontFamily;
                cb_LocationId.ItemsSource = FillCombo.locationsList.Where(p => p.Name.ToLower().Contains(tb.Text.ToLower()) || p.LocationId.ToString().Contains(tb.Text)).ToList();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        Location location;
        private void cb_LocationId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                location = FillCombo.locationsList.Where(x => x.LocationId == (long)cb_LocationId.SelectedValue).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void tb_FreePercentage_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                refreshValues();
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

        private void dg_invoiceDetails_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                TextBox t = e.EditingElement as TextBox;  // Assumes columns are all TextBoxes
                int textValue = 0;
                if (t.Text != "")
                    textValue = int.Parse(t.Text);

                var columnName = e.Column.Header.ToString();

                RecieptDetails row = e.Row.Item as RecieptDetails;
                int index = billDetails.IndexOf(billDetails.Where(p => p.ItemId == row.ItemId).FirstOrDefault());

                int maxQty = 0;
                int minQty = 0;
                int freeQty = 0;

                if (columnName == AppSettings.resourcemanager.GetString("MaxFactorQty") && !t.Text.Equals(""))
                {                 
                    if(_ReceiptType == "purchaseOrders")
                    {
                        var it = purchaseOrder.PurchaseDetails.Where(x => x.ItemId == row.ItemId).FirstOrDefault();
                        if(it.MaxQty < textValue)
                        {

                            textValue = (int)it.MaxQty;
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trExceedOrderqauntityError"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    maxQty = textValue;
                }
                else
                    maxQty = (int)row.MaxQty;

                if (columnName == AppSettings.resourcemanager.GetString("LessFactorQty") && !t.Text.Equals(""))
                {
                    if (_ReceiptType == "purchaseOrders")
                    {
                        if (textValue >= row.Factor)
                        {
                            textValue = 0;
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMinQtyMoreFactorError"), animation: ToasterAnimation.FadeIn);
                        }
                        var it = purchaseOrder.PurchaseDetails.Where(x => x.ItemId == row.ItemId).FirstOrDefault();
                        if (it.MaxQty < textValue)
                        {

                            textValue = (int)it.MinQty;
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trExceedOrderqauntityError"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    minQty = textValue;
                }
                else
                    minQty = (int)row.MinQty;

                if (columnName == AppSettings.resourcemanager.GetString("Free") && !t.Text.Equals(""))
                    freeQty = textValue;
                else
                    freeQty = (int)row.FreeQty;

                decimal cost = (row.MainCost * maxQty) + ((row.MainCost / (int)row.Factor) * minQty);
                decimal price = ((int)row.Factor * row.MainPrice * maxQty) + (row.MainPrice * minQty);

                billDetails[index].MinQty = minQty;
                billDetails[index].MaxQty = maxQty;
                billDetails[index].FreeQty = freeQty;
                billDetails[index].Cost = cost;
                billDetails[index].Price = price;

                refreshValues();

                dg_invoiceDetails.ItemsSource = billDetails;
                //dg_invoiceDetails.Items.Refresh();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_posting_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void cb_ReceiptType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                await Clear();
                _ReceiptType = cb_ReceiptType.SelectedValue.ToString();
                ControlsEditable();
            }
            catch { }
        }

        Supplier supplier;
        private void Cb_SupId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                supplier = FillCombo.suppliersList.Where(x => x.SupId == (long)cb_SupId.SelectedValue).FirstOrDefault();

                txt_EnterpriseDiscount.Text = HelpClass.DecTostring(supplier.DiscountPercentage);
                tb_SupplierNotes.Text = supplier.Notes;
                tb_SupplierPurchaseNotes.Text = supplier.PurchaseOrderNotes;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void tgl_IsRecieveAll_Checked(object sender, RoutedEventArgs e)
        {
            ControlsEditable();
        }

        private void tgl_IsRecieveAll_Unchecked(object sender, RoutedEventArgs e)
        {
            ControlsEditable();
      
        }

        private void tb_AmountDifference_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ValidateEmpty_TextChange(sender, e);
              
                if(tb_AmountDifference.Text != "")
                {
                    var diff = decimal.Parse(tb_AmountDifference.Text);
                    if (diff > 50)
                    {
                        tb_AmountDifference.Text = "0";
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trAmountDifferenceError"), animation: ToasterAnimation.FadeIn);
                    }
                }

            }
            catch { }
        }

        private async void tb_PurchaseInvNumber_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return && tb_PurchaseInvNumber.Text!= "")
                {
                    purchaseOrder = await FillCombo.purchaseInvoice.getPurchaseOrderByNum(tb_PurchaseInvNumber.Text);

                    receipt = new Receipt();
                    receipt.LocationId = purchaseOrder.LocationId;
                    receipt.SupId = purchaseOrder.SupId;
                    receipt.PurchaseInvNumber = purchaseOrder.InvNumber;

                    receipt.NetInvoice = purchaseOrder.CostNet;

                    // doesn't have any receipt
                    
                    if(purchaseOrder.ReceiptDocuments == null)
                        await fillOrderInputs(purchaseOrder);
                    else
                    {
                        foreach(var row in purchaseOrder.PurchaseDetails)
                        {
                            var minQty = 0;
                            var maxQty = 0;
                            foreach(var row1 in purchaseOrder.ReceiptDocuments)
                            {
                                foreach (var item in row1.ReceiptDetails)
                                {
                                    if (item.ItemId == row.ItemId)
                                    {
                                        minQty += (int)item.MinQty;
                                        maxQty += (int)item.MaxQty;
                                    }
                                }
                            }

                            var diffMin = row.MinQty - minQty;
                            int breakNum = 0;
                            if (diffMin < 0)
                            {
                                 breakNum = 1+(int)Math.Ceiling((decimal)diffMin / (decimal)row.Factor);
                                row.MinQty = row.Factor + diffMin;
                            }
                            else
                                row.MinQty = row.Factor - diffMin;

                            row.MaxQty = row.MaxQty - maxQty - breakNum;
                            await fillOrderInputs(purchaseOrder);
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
