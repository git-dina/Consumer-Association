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

namespace POSCA.View.purchases
{
    /// <summary>
    /// Interaction logic for uc_purchaseOrder.xaml
    /// </summary>
    public partial class uc_purchaseOrder : UserControl
    {

        public uc_purchaseOrder()
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
        private static uc_purchaseOrder _instance;
        public static uc_purchaseOrder Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_purchaseOrder();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string searchText = "";
        public static List<string> requiredControlList;
        private PurchaseInvoice purchaseInvoice = new PurchaseInvoice();
        private string _InvType = "sod";
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
                requiredControlList = new List<string> { "LocationId", "SupplierId" };
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
                // loadingList.Add(new keyValueBool { key = "loading_RefrishItems", value = false });

                loading_RefrishSuppliers();
                loading_RefrishLocations();
                //loading_RefrishItems();

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

                FillCombo.fillPurchaseOrderStatus(cb_InvStatus);
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
            //txt_newDraft.Text = AppSettings.resourcemanager.GetString("trNew");
            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("ProcurementRequest");
            txt_invoiceDetails.Text = AppSettings.resourcemanager.GetString("OrderDetails");
            txt_TotalCostTitle.Text = AppSettings.resourcemanager.GetString("TotalCost");
            txt_TotalPriceTitle.Text = AppSettings.resourcemanager.GetString("trTotalPrice");
            txt_EnterpriseDiscountTitle.Text = AppSettings.resourcemanager.GetString("EnterpriseDiscount");
            txt_DiscountValueTitle.Text = AppSettings.resourcemanager.GetString("DiscountValue");
            txt_FreePercentageTitle.Text = AppSettings.resourcemanager.GetString("FreePercentag");
            txt_FreeValueTitle.Text = AppSettings.resourcemanager.GetString("FreeValue");
            txt_ConsumerDiscountTitle.Text = AppSettings.resourcemanager.GetString("ConsumerDiscount");
            txt_CostNetTitle.Text = AppSettings.resourcemanager.GetString("NetCost");
            txt_isApproved.Text = AppSettings.resourcemanager.GetString("Approval");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_LocationId, AppSettings.resourcemanager.GetString("trBranchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_InvStatus, AppSettings.resourcemanager.GetString("OrderStatusHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_InvNumber, AppSettings.resourcemanager.GetString("OrderNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_SupId, AppSettings.resourcemanager.GetString("SupplierHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_OrderDate, AppSettings.resourcemanager.GetString("DocumentDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_OrderRecieveDate, AppSettings.resourcemanager.GetString("RequestedReceiptDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            dg_invoiceDetails.Columns[1].Header = AppSettings.resourcemanager.GetString("ItemNumber");
            dg_invoiceDetails.Columns[2].Header = AppSettings.resourcemanager.GetString("itemName");
            dg_invoiceDetails.Columns[3].Header = AppSettings.resourcemanager.GetString("Barcode");
            dg_invoiceDetails.Columns[4].Header = AppSettings.resourcemanager.GetString("ItemDescription");
            dg_invoiceDetails.Columns[5].Header = AppSettings.resourcemanager.GetString("Factor");
            dg_invoiceDetails.Columns[6].Header = AppSettings.resourcemanager.GetString("trCost");
            dg_invoiceDetails.Columns[7].Header = AppSettings.resourcemanager.GetString("trPrice");
            dg_invoiceDetails.Columns[8].Header = AppSettings.resourcemanager.GetString("MaxFactorQty");
            dg_invoiceDetails.Columns[9].Header = AppSettings.resourcemanager.GetString("LessFactorQty");
            dg_invoiceDetails.Columns[10].Header = AppSettings.resourcemanager.GetString("Free");
            dg_invoiceDetails.Columns[11].Header = AppSettings.resourcemanager.GetString("ConsumerDiscountTitle");
            dg_invoiceDetails.Columns[12].Header = AppSettings.resourcemanager.GetString("trBalance");
            dg_invoiceDetails.Columns[13].Header = AppSettings.resourcemanager.GetString("TotalCost");
            dg_invoiceDetails.Columns[14].Header = AppSettings.resourcemanager.GetString("trTotalPrice");


            btn_newDraft.ToolTip = AppSettings.resourcemanager.GetString("trNew");
            btn_drafts.ToolTip = AppSettings.resourcemanager.GetString("NotApproved");
            btn_invoices.ToolTip = AppSettings.resourcemanager.GetString("Approved");
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


        #endregion
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private void ControlsEditable()
        {

            switch (_InvType)
            {
                case "sod":
                    cb_LocationId.IsEnabled = true;
                    cb_SupId.IsEnabled = true;
                    tgl_isApproved.IsEnabled = true;
                    tb_FreePercentage.IsEnabled = true;
                    btn_save.IsEnabled = true;
                    dp_OrderDate.IsEnabled = true;
                    dp_OrderRecieveDate.IsEnabled = true;
                    btn_deleteInvoice.Visibility = Visibility.Visible;
                    dg_invoiceDetails.Columns[0].Visibility = Visibility.Visible;
                    dg_invoiceDetails.Columns[8].IsReadOnly = false;
                    dg_invoiceDetails.Columns[9].IsReadOnly = false;
                    dg_invoiceDetails.Columns[10].IsReadOnly = false;
                    break;
                case "soa": //supplying order is approved
                    if (purchaseInvoice.InvStatus == "opened")
                    {
                        cb_LocationId.IsEnabled = true;
                        cb_SupId.IsEnabled = true;
                        tgl_isApproved.IsEnabled = false;
                        tb_FreePercentage.IsEnabled = true;
                        btn_save.IsEnabled = true;
                        dp_OrderDate.IsEnabled = true;
                        dp_OrderRecieveDate.IsEnabled = true;
                        btn_deleteInvoice.Visibility = Visibility.Visible;
                        dg_invoiceDetails.Columns[0].Visibility = Visibility.Visible;
                        dg_invoiceDetails.Columns[8].IsReadOnly = false;
                        dg_invoiceDetails.Columns[9].IsReadOnly = false;
                        dg_invoiceDetails.Columns[10].IsReadOnly = false;

                    }
                    else
                    {
                        cb_LocationId.IsEnabled = false;
                        cb_SupId.IsEnabled = false;
                        tgl_isApproved.IsEnabled = false;
                        tb_FreePercentage.IsEnabled = false;
                        btn_save.IsEnabled = false;
                        dp_OrderDate.IsEnabled = false;
                        dp_OrderRecieveDate.IsEnabled = false;
                        btn_deleteInvoice.Visibility = Visibility.Collapsed;
                        dg_invoiceDetails.Columns[0].Visibility = Visibility.Collapsed;
                        dg_invoiceDetails.Columns[8].IsReadOnly = true;
                        dg_invoiceDetails.Columns[9].IsReadOnly = true;
                        dg_invoiceDetails.Columns[10].IsReadOnly = true;

                    }
                    break;

            }

            if (purchaseInvoice.PurchaseId.Equals(0))
            {
                btn_printInvoice.IsEnabled = false;
                tgl_isApproved.IsEnabled = false;
                btn_deleteInvoice.Visibility = Visibility.Collapsed;
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

            if (billDetails.Count > 0 && (_InvType == "sod" || (_InvType == "soa" && purchaseInvoice.InvStatus == "opened")))
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
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (billDetails.Count == 0)
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trOrderWithoutItemsError"), animation: ToasterAnimation.FadeIn);
                else if (HelpClass.validate(requiredControlList, this))
                {
                    await addInvoice();

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
            purchaseInvoice.LocationId = (long)cb_LocationId.SelectedValue;
            purchaseInvoice.SupId = (long)cb_SupId.SelectedValue;
            purchaseInvoice.InvStatus = cb_InvStatus.SelectedValue.ToString();
            purchaseInvoice.InvType = _InvType;
            purchaseInvoice.OrderDate = (DateTime)dp_OrderDate.SelectedDate;
            purchaseInvoice.OrderRecieveDate = (DateTime)dp_OrderRecieveDate.SelectedDate;
            purchaseInvoice.Notes = tb_Notes.Text;
            purchaseInvoice.SupplierNotes = supplier.Notes;
            purchaseInvoice.SupplierPurchaseNotes = supplier.PurchaseOrderNotes;

            purchaseInvoice.TotalCost = decimal.Parse(txt_TotalCost.Text);
            purchaseInvoice.TotalPrice = decimal.Parse(txt_TotalPrice.Text);
            purchaseInvoice.CoopDiscount = decimal.Parse(txt_EnterpriseDiscount.Text);
            purchaseInvoice.DiscountValue = decimal.Parse(txt_DiscountValue.Text);
            if (tb_FreePercentage.Text != "")
                purchaseInvoice.FreePercentage = decimal.Parse(tb_FreePercentage.Text);
            purchaseInvoice.FreeValue = decimal.Parse(txt_FreeValue.Text);
            purchaseInvoice.ConsumerDiscount = decimal.Parse(txt_ConsumerDiscount.Text);
            purchaseInvoice.CostNet = decimal.Parse(txt_CostNet.Text);


            purchaseInvoice.CreateUserId = MainWindow.userLogin.userId;

            purchaseInvoice.PurchaseDetails = billDetails;
            purchaseInvoice = await purchaseInvoice.SaveSupplyingOrder(purchaseInvoice);

            if (purchaseInvoice.PurchaseId == 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            else
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                fillOrderInputs(purchaseInvoice);

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
                        PurchaseInvDetails row = (PurchaseInvDetails)dg_invoiceDetails.SelectedItems[0];
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
                    //HelpClass.StartAwait(grid_main);
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
                    //HelpClass.EndAwait(grid_main);

                }
            }
            catch (Exception ex)
            {
                //HelpClass.EndAwait(grid_main);
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
            await Task.Delay(0050);
            this.DataContext = new PurchaseInvoice();

            purchaseInvoice = new PurchaseInvoice();

            billDetails = new List<PurchaseInvDetails>();
            //dg_invoiceDetails.ItemsSource = billDetails;
            //dg_invoiceDetails.Items.Refresh();
            if (!forceCancelEdit)
            {
                dg_invoiceDetails.IsEnabled = false;
                RefreshInvoiceDetailsDataGrid();
            }

            await Task.Delay(0050);
            dp_OrderDate.SelectedDate = DateTime.Now;
            dp_OrderRecieveDate.SelectedDate = DateTime.Now;

            txt_TotalCost.Text = HelpClass.DecTostring(0);
            txt_TotalPrice.Text = HelpClass.DecTostring(0);
            txt_EnterpriseDiscount.Text = HelpClass.DecTostring(0);
            txt_EnterpriseDiscount.Text = HelpClass.DecTostring(0);
            txt_DiscountValue.Text = HelpClass.DecTostring(0);
            tb_FreePercentage.Text = HelpClass.DecTostring(0);
            txt_FreeValue.Text = HelpClass.DecTostring(0);
            txt_ConsumerDiscount.Text = HelpClass.DecTostring(0);
            txt_CostNet.Text = HelpClass.DecTostring(0);

            cb_InvStatus.SelectedValue = "opened";
            _InvType = "sod";
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
                        itemLst = await FillCombo.item.GetItemByCodeOrName(tb_search.Text, location.LocationId, supplier.SupId, "orders");
                        if (itemLst.Count == 1)
                        {
                            item1 = itemLst[0];
                            barcode = item1.ItemUnits.FirstOrDefault().Barcode;
                        }

                    }
                    else
                    {
                        item1 = await FillCombo.item.GetItemByBarcode(tb_search.Text, location.LocationId, supplier.SupId, "orders");
                        barcode = tb_search.Text;
                    }

                    if (itemLst.Count > 1)
                    {
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_addPurchaseItems w = new wd_addPurchaseItems();
                        w.items = itemLst.ToList();
                        w.supId = (long)cb_SupId.SelectedValue;
                        w.locationId = (long)cb_LocationId.SelectedValue;
                        w.itemsFor = "orders";

                        w.ShowDialog();
                        if (w.isOk)
                        {
                            item1 = w.item;
                            barcode = item1.ItemUnits.FirstOrDefault().Barcode;
                        }
                        Window.GetWindow(this).Opacity = 1;
                    }


                    if (item1 != null)
                    {
                        if (item1.ItemStatus != "normal")
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trItemStatusNotNormalError"), animation: ToasterAnimation.FadeIn);

                        }
                        else
                        {
                            Window.GetWindow(this).Opacity = 0.2;
                            wd_addPurchaseItem w = new wd_addPurchaseItem();

                            long balance = item1.ItemLocations.Where(x => x.LocationId == location.LocationId).FirstOrDefault().Balance;
                            w.newPurchaseItem = new PurchaseInvDetails()
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
                                    if (w.newPurchaseItem.MaxQty != null)
                                        maxQty = (int)w.newPurchaseItem.MaxQty;
                                    if (w.newPurchaseItem.MinQty != null)
                                        minQty = (int)w.newPurchaseItem.MinQty;
                                    w.newPurchaseItem.Cost = (w.newPurchaseItem.MainCost * maxQty) + ((w.newPurchaseItem.MainCost / (int)w.newPurchaseItem.Factor) * minQty);
                                    w.newPurchaseItem.Price = ((int)w.newPurchaseItem.Factor * w.newPurchaseItem.MainPrice * maxQty) + (w.newPurchaseItem.MainPrice * minQty);

                                    if (w.newPurchaseItem.MinQty == null)
                                        w.newPurchaseItem.MinQty = 0;
                                    if (w.newPurchaseItem.FreeQty == null)
                                        w.newPurchaseItem.FreeQty = 0;
                                    addItemToBill(w.newPurchaseItem);
                                }
                                else
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMoreTwentyItemsError"), animation: ToasterAnimation.FadeIn);

                            }
                            Window.GetWindow(this).Opacity = 1;
                        }
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
        List<PurchaseInvDetails> billDetails = new List<PurchaseInvDetails>();
        private void addItemToBill(PurchaseInvDetails purchaseInvDetails)
        {
            int index = billDetails.IndexOf(billDetails.Where(p => p.ItemId == purchaseInvDetails.ItemId).FirstOrDefault());

            if (index == -1)//item doesn't exist in bill
            {
                billDetails.Add(purchaseInvDetails);

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
                _ConsumerDiscount = row.ConsumerDiscount;
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
        }
        private void Btn_invoices_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_purchaseInv w = new wd_purchaseInv();

                string invoiceType = "soa";
                w.invoiceType = invoiceType;
                w.isApproved = true;
                w.windowTitle = AppSettings.resourcemanager.GetString("ProcurementRequests");
                w.ShowDialog();
                if (w.isOk)
                {
                    _InvType = "soa";
                    purchaseInvoice = w.purchaseInvoice;
                    fillOrderInputs(purchaseInvoice);
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

        private void Btn_draft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_purchaseInv w = new wd_purchaseInv();

                string invoiceType = "sod";
                w.invoiceType = invoiceType;
                w.isApproved = false;
                w.windowTitle = AppSettings.resourcemanager.GetString("ProcurementRequests");

                w.ShowDialog();
                if (w.isOk)
                {
                    _InvType = "sod";
                    purchaseInvoice = w.purchaseInvoice;
                    fillOrderInputs(purchaseInvoice);
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

        public void fillOrderInputs(PurchaseInvoice invoice)
        {

            this.DataContext = invoice;

            txt_TotalCost.Text = HelpClass.DecTostring(purchaseInvoice.TotalCost);
            txt_TotalPrice.Text = HelpClass.DecTostring(purchaseInvoice.TotalPrice);
            txt_EnterpriseDiscount.Text = HelpClass.DecTostring(purchaseInvoice.CoopDiscount);
            txt_DiscountValue.Text = HelpClass.DecTostring(purchaseInvoice.DiscountValue);
            tb_FreePercentage.Text = HelpClass.DecTostring(purchaseInvoice.FreePercentage);
            txt_FreeValue.Text = HelpClass.DecTostring(purchaseInvoice.FreeValue);
            txt_ConsumerDiscount.Text = HelpClass.DecTostring(purchaseInvoice.ConsumerDiscount);
            txt_CostNet.Text = HelpClass.DecTostring(purchaseInvoice.CostNet);

            if (purchaseInvoice.IsApproved == true)
                txt_isApproved.Text = AppSettings.resourcemanager.GetString("Approved");
            else
                txt_isApproved.Text = AppSettings.resourcemanager.GetString("Approval");
            buildInvoiceDetails(purchaseInvoice);

            ControlsEditable();

        }

        private void buildInvoiceDetails(PurchaseInvoice invoice)
        {
            billDetails = invoice.PurchaseDetails.ToList();
            //dg_invoiceDetails.ItemsSource = invoice.PurchaseDetails;
            //dg_invoiceDetails.Items.Refresh();
            if (!forceCancelEdit)
            {
                dg_invoiceDetails.IsEnabled = false;
                RefreshInvoiceDetailsDataGrid();
            }
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
                    msg = await printInvoice(purchaseInvoice);
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
        public async Task<string> printInvoice(PurchaseInvoice prInvoice)
        {
            string msg = "";
            try
            {
                int sequence = 0;
                foreach (var row in prInvoice.PurchaseDetails)
                {
                    row.Sequence = sequence++;
                }

                List<ReportParameter> paramarr = new List<ReportParameter>();

                rep.ReportPath = reportclass.GetSupplyingOrderRdlcpath();

                // ReportsConfig.setReportLanguage(paramarr);

                reportclass.fillSupplyingOrderReport(prInvoice, rep, paramarr);
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
                //this.Dispatcher.Invoke(() =>
                //{
                //    Toaster.ShowWarning(Window.GetWindow(this), message: "Not completed", animation: ToasterAnimation.FadeIn);

                //});
                msg = "trNotCompleted";
            }
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

        private void dp_OrderDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dp_OrderDate.SelectedDate != null && dp_OrderRecieveDate.SelectedDate != null)
                    if (dp_OrderDate.SelectedDate.Value.Date > dp_OrderRecieveDate.SelectedDate.Value.Date)
                        dp_OrderRecieveDate.SelectedDate = dp_OrderDate.SelectedDate.Value.Date;

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

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

        Supplier supplier;
        private void Cb_SupId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                supplier = FillCombo.suppliersList.Where(x => x.SupId == (long)cb_SupId.SelectedValue).FirstOrDefault();

                txt_EnterpriseDiscount.Text = HelpClass.DecTostring(supplier.DiscountPercentage);
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

        private async void tgl_isApproved_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                #region
                Window.GetWindow(this).Opacity = 0.2;
                wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxApproveOrder");

                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
                #endregion
                if (w.isOk)
                {
                    var res = await purchaseInvoice.approveSupplyingOrder(purchaseInvoice.PurchaseId, MainWindow.userLogin.userId);

                    if (res != 0)
                    {
                        _InvType = "soa";
                        purchaseInvoice.IsApproved = true;
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                        fillOrderInputs(purchaseInvoice);
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                }
            }
            catch
            {

            }
        }

        private async void Btn_deleteInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
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
                    var res = await purchaseInvoice.deletePurchaseInv(purchaseInvoice.PurchaseId, MainWindow.userLogin.userId);

                    if (res != 0)
                    {

                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);
                        await Clear();
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                }
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

                PurchaseInvDetails row = e.Row.Item as PurchaseInvDetails;
                int index = billDetails.IndexOf(billDetails.Where(p => p.ItemId == row.ItemId).FirstOrDefault());

                int maxQty = 0;
                int minQty = 0;
                int freeQty = 0;

                if (columnName == AppSettings.resourcemanager.GetString("MaxFactorQty") && !t.Text.Equals(""))
                    maxQty = textValue;
                else
                    maxQty = (int)row.MaxQty;

                if (columnName == AppSettings.resourcemanager.GetString("LessFactorQty") && !t.Text.Equals(""))
                {
                    if (textValue >= row.Factor)
                    {
                        textValue = 0;
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMinQtyMoreFactorError"), animation: ToasterAnimation.FadeIn);
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

                // dg_invoiceDetails.ItemsSource = billDetails;
                //dg_invoiceDetails.Items.Refresh();
                if (!forceCancelEdit)
                {
                    dg_invoiceDetails.IsEnabled = false;
                    RefreshInvoiceDetailsDataGrid();
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
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
