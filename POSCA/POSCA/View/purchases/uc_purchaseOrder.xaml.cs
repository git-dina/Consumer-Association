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
                    //AppSettings.resourcemanager = new ResourceManager("POSCA.en_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    //AppSettings.resourcemanager = new ResourceManager("POSCA.ar_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();

                dp_OrderDate.SelectedDate = DateTime.Now;
                dp_OrderRecieveDate.SelectedDate = DateTime.Now;
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
            txt_CostAfterDiscountTitle.Text = AppSettings.resourcemanager.GetString("DiscountValue");
            txt_FreePercentageTitle.Text = AppSettings.resourcemanager.GetString("FreePercentag");
            txt_FreeValueTitle.Text = AppSettings.resourcemanager.GetString("FreeValue");
            txt_ConsumerDiscountTitle.Text = AppSettings.resourcemanager.GetString("ConsumerDiscount");
            txt_CostNetTitle.Text = AppSettings.resourcemanager.GetString("NetCost");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_LocationId, AppSettings.resourcemanager.GetString("trBranchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_InvStatus, AppSettings.resourcemanager.GetString("OrderStatusHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_InvNumber, AppSettings.resourcemanager.GetString("OrderNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_SupId, AppSettings.resourcemanager.GetString("SupplierHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_OrderDate, AppSettings.resourcemanager.GetString("DocumentDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_OrderRecieveDate, AppSettings.resourcemanager.GetString("RequestedReceiptDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            dg_invoiceDetails.Columns[1].Header = AppSettings.resourcemanager.GetString("ItemNumber");
            dg_invoiceDetails.Columns[2].Header = AppSettings.resourcemanager.GetString("Barcode");
            dg_invoiceDetails.Columns[3].Header = AppSettings.resourcemanager.GetString("ItemDescription");
            dg_invoiceDetails.Columns[4].Header = AppSettings.resourcemanager.GetString("Factor");
            dg_invoiceDetails.Columns[5].Header = AppSettings.resourcemanager.GetString("trCost");
            dg_invoiceDetails.Columns[6].Header = AppSettings.resourcemanager.GetString("MaxFactorQty");
            dg_invoiceDetails.Columns[7].Header = AppSettings.resourcemanager.GetString("LessFactorQty");
            dg_invoiceDetails.Columns[8].Header = AppSettings.resourcemanager.GetString("Free");
            dg_invoiceDetails.Columns[9].Header = AppSettings.resourcemanager.GetString("ConsumerDiscountTitle");
            dg_invoiceDetails.Columns[10].Header = AppSettings.resourcemanager.GetString("trPrice");
            dg_invoiceDetails.Columns[11].Header = AppSettings.resourcemanager.GetString("trBalance");
            dg_invoiceDetails.Columns[12].Header = AppSettings.resourcemanager.GetString("TotalCost");
            dg_invoiceDetails.Columns[13].Header = AppSettings.resourcemanager.GetString("trTotalPrice");


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
        
        //bool loadingSuccess_RefrishItems = false;
        //async void loading_RefrishItems()
        //{
        //    try
        //    {
        //        await RefreshItemsList();
        //        if (FillCombo.itemList is null)
        //            await RefreshItemsList();
        //        else
        //            loadingSuccess_RefrishItems = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        catchError.Add("loading_RefrishItems");
        //        catchErrorCount++;
        //        if (catchErrorCount > 50)
        //        {
        //            loadingSuccess_RefrishItems = true;
        //        }
        //        else
        //            loading_RefrishItems();
        //        HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
        //    }
        //    if (loadingSuccess_RefrishItems)
        //        foreach (var item in loadingList)
        //        {
        //            if (item.key.Equals("loading_RefrishItems"))
        //            {
        //                item.value = true;
        //                break;
        //            }
        //        }
        //}

        //async Task<IEnumerable<Item>> RefreshItemsList()
        //{

        //    await FillCombo.RefreshItems();
        //    return FillCombo.itemList;

        //}
        #endregion
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private void ControlsEditable()
        {
            if(purchaseInvoice.PurchaseId.Equals(0))
            {
                btn_printInvoice.IsEnabled = false;
            }
        }

        #endregion
        #region events
        private void Btn_newDraft_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {

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
        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {

                HelpClass.StartAwait(grid_main);

                //tb_search.Text = "";
                //searchText = "";
                //await RefreshBanksList();
                //await Search();

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
        void Clear()
        {
            this.DataContext = new PurchaseInvoice();

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

                    Item item1 = new Item();
                    string barcode = "";

                    if (chk_itemNum.IsChecked == true)
                    {
                        item1 =await  FillCombo.item.GetItemByCode(tb_search.Text, location.LocationId, supplier.SupId);
                        //item1 = FillCombo.itemList.Where(x => x.Code == tb_search.Text && x.SupId == supplier.SupId 
                        //                && x.ItemLocations.Any(u => u.LocationId == location.LocationId)).FirstOrDefault();
                       if(item1 != null)
                            barcode = item1.ItemUnits.FirstOrDefault().Barcode;
                    }
                    else
                    {
                        item1 = await FillCombo.item.GetItemByBarcode(tb_search.Text, location.LocationId, supplier.SupId);
                        //item1 = FillCombo.itemList.Where(m => m.ItemUnits.Any(u => u.Barcode.ToLower() == tb_search.Text.ToLower())).FirstOrDefault();
                        barcode = tb_search.Text;
                    }

                    if (item1 != null)
                    {
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_addPurchaseItem w = new wd_addPurchaseItem();

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
                        };
                        w.ShowDialog();

                        if (w.isOk)
                        {
                            w.newPurchaseItem.Cost = (w.newPurchaseItem.MaxQty - w.newPurchaseItem.MinQty) * w.newPurchaseItem.MainCost;
                            w.newPurchaseItem.Price = (w.newPurchaseItem.MaxQty - w.newPurchaseItem.MinQty) * w.newPurchaseItem.MainPrice;
                            
                            addItemToBill(w.newPurchaseItem);

                        }
                        Window.GetWindow(this).Opacity = 1;
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trItemNotFoundError"), animation: ToasterAnimation.FadeIn);

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
                dg_invoiceDetails.Items.Clear();
                dg_invoiceDetails.ItemsSource = billDetails;
                refreshValues();
            }
            else // item exist prevoiusly in list
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trItemExistInOrderError"), animation: ToasterAnimation.FadeIn);

            }
        }

        decimal _TotalCost = 0;
        decimal _TotalPrice = 0;
        decimal _CostAfterDiscount = 0;
        decimal _ConsumerDiscount = 0;
    
        private void refreshValues()
        {
            _TotalCost = 0;
            _TotalPrice = 0;
            _CostAfterDiscount = 0;
            _ConsumerDiscount = 0;
            foreach(var row in billDetails)
            {
                _TotalCost += row.Cost;
                _TotalPrice += row.Price;
                _ConsumerDiscount += row.ConsumerDiscount;
            }

            txt_TotalCost.Text = HelpClass.DecTostring(_TotalCost);
            txt_TotalPrice.Text = HelpClass.DecTostring(_TotalPrice);
            txt_ConsumerDiscount.Text = HelpClass.DecTostring(_ConsumerDiscount);

            //cost after discount
            var discount = HelpClass.calcPercentage(_TotalCost, 100 + supplier.DiscountPercentage);
            _CostAfterDiscount = discount;
            txt_CostAfterDiscount.Text = HelpClass.DecTostring(_CostAfterDiscount);

            //free quantity
            decimal freePercentage = 0;
            decimal freeValue = 0;
            if(tb_FreePercentage.Text !="")
            {
                freePercentage = decimal.Parse(tb_FreePercentage.Text);
                freeValue = HelpClass.calcPercentage(_TotalCost, freePercentage);
            }
            tb_FreePercentage.Text = HelpClass.DecTostring(freeValue);

            decimal netCost = _TotalCost - discount - freeValue;
        }
        private void Btn_invoices_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_draft_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_printInvoice_Click(object sender, RoutedEventArgs e)
        {

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
                cb_SupId.ItemsSource = FillCombo.suppliersList.Where(p => p.Name.ToLower().Contains(tb.Text.ToLower()) || p.SupId.ToString().Contains(tb.Text)).ToList();
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
        }
    }
}
