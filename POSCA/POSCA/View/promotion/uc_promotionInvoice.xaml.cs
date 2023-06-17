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

namespace POSCA.View.promotion
{
    /// <summary>
    /// Interaction logic for uc_promotionInvoice.xaml
    /// </summary>
    public partial class uc_promotionInvoice : UserControl
    {

        public uc_promotionInvoice()
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
        private static uc_promotionInvoice _instance;
        public static uc_promotionInvoice Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_promotionInvoice();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string searchText = "";
        public static List<string> requiredControlList;
        private Promotion promotion = new Promotion();
        private PurchaseInvoice purchaseOrder = new PurchaseInvoice();
        private string _PromotionType = "quantity";
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

                
                #region loading
                loadingList = new List<keyValueBool>();
                bool isDone = true;

                loadingList.Add(new keyValueBool { key = "loading_RefrishLocations", value = false });

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
             
                
                FillCombo.fillPromotionTypes(cb_PromotionType);
                FillCombo.fillPromotionNatures(cb_PromotionNature);

                cb_PromotionType.SelectedIndex = 0;
                cb_PromotionType.SelectedValue = "quantity";

                //await setItemLocationsData();

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

            txt_invoiceTitle.Text = AppSettings.resourcemanager.GetString("PromotionalOffer");

            chk_barcode.Content = AppSettings.resourcemanager.GetString("trBarcode");
            chk_itemNum.Content = AppSettings.resourcemanager.GetString("ItemNumber");
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
            btn_stop.Content = AppSettings.resourcemanager.GetString("Terminate");

            txt_search.Text = AppSettings.resourcemanager.GetString("trSearch");
            txt_invoiceDetails.Text = AppSettings.resourcemanager.GetString("OfferDetails");
            txt_offerLocations.Text = AppSettings.resourcemanager.GetString("OfferLocations");


            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Ref, AppSettings.resourcemanager.GetString("RefHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_PromotionType, AppSettings.resourcemanager.GetString("OfferTypeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_LocationId, AppSettings.resourcemanager.GetString("trBranchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_PromotionNature, AppSettings.resourcemanager.GetString("OfferNatureHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_PromotionPercentage, AppSettings.resourcemanager.GetString("DiscountPercentageHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_PromotionDate, AppSettings.resourcemanager.GetString("OfferDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_PromotionStartDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_PromotionEndDate, AppSettings.resourcemanager.GetString("trEndDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));

           
            dg_invoiceDetails.Columns[1].Header = AppSettings.resourcemanager.GetString("ItemNumber");
            dg_invoiceDetails.Columns[2].Header = AppSettings.resourcemanager.GetString("Barcode");
            dg_invoiceDetails.Columns[3].Header = AppSettings.resourcemanager.GetString("itemName");
            dg_invoiceDetails.Columns[4].Header = AppSettings.resourcemanager.GetString("trUnit");
            dg_invoiceDetails.Columns[5].Header = AppSettings.resourcemanager.GetString("Factor");
            col_mainCost.Header = AppSettings.resourcemanager.GetString("trCost");
            dg_invoiceDetails.Columns[7].Header = AppSettings.resourcemanager.GetString("basePrice");
            dg_invoiceDetails.Columns[8].Header = AppSettings.resourcemanager.GetString("OfferPrice");
            dg_invoiceDetails.Columns[9].Header = AppSettings.resourcemanager.GetString("trQuantity");
            dg_invoiceDetails.Columns[10].Header = AppSettings.resourcemanager.GetString("IsBlocked");
        
            btn_newDraft.ToolTip = AppSettings.resourcemanager.GetString("trNew");
            btn_promotionOrders.ToolTip = AppSettings.resourcemanager.GetString("PromotionOrders");
            btn_printInvoice.ToolTip = AppSettings.resourcemanager.GetString("trPrint");
        }

        #region Loading

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
        List<PromotionLocations> listItemLocations = new List<PromotionLocations>();
        private async Task setItemLocationsData()
        {

            if (FillCombo.locationsList == null)
                await FillCombo.RefreshLocations();

            listItemLocations = new List<PromotionLocations>();
            foreach (var row in FillCombo.locationsList)
            {
                var isSelected = false;
                if (promotion.PromotionLocations != null )
                {
                    var selected = promotion.PromotionLocations.Where(x => x.LocationId == row.LocationId).FirstOrDefault();

                    if (selected != null)
                    {
                        isSelected = true;
                    }
                }
                listItemLocations.Add(new PromotionLocations()
                {
                    LocationId = row.LocationId,
                    LocationName = row.Name,
                    IsSelected = isSelected,
                });

            }
            dg_itemLocation.ItemsSource = listItemLocations;
            dg_itemLocation.Items.Refresh();
        }
        List<keyValueBool> loadingList;
        List<string> catchError = new List<string>();
        int catchErrorCount = 0;
        /*
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

        */
        #endregion
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private void ControlsEditable()
        {
          
            if (promotion.PromotionId.Equals(0))
            {
                dg_invoiceDetails.Columns[0].Visibility = Visibility.Visible;
                col_IsItemStoped.Visibility = Visibility.Collapsed;
                col_quantity.IsReadOnly = false;

                btn_save.IsEnabled = true;
                btn_stop.IsEnabled = false;
                btn_printInvoice.IsEnabled = false;
                cb_PromotionType.IsEnabled = true;
                cb_LocationId.IsEnabled = true;
                cb_PromotionNature.IsEnabled = true;
                tb_PromotionPercentage.IsEnabled = true;
                dp_PromotionDate.IsEnabled = true;
                dp_PromotionStartDate.IsEnabled = true;
                dp_PromotionEndDate.IsEnabled = true;

                chb_locIsSelected.IsEnabled = true;

                //check promotion status first
                switch (_PromotionType)
                {

                    case "quantity":

                        tb_PromotionPercentage.Visibility = Visibility.Collapsed;
                        col_quantity.Visibility = Visibility.Visible;
                        col_promotionPrice.IsReadOnly = false;
                        brd_branchId.Visibility = Visibility.Collapsed;
                        brd_locationsGrid.Visibility = Visibility.Visible;
                        break;
                    default:
                        tb_PromotionPercentage.Visibility = Visibility.Visible;
                        col_quantity.Visibility = Visibility.Collapsed;
                        col_promotionPrice.IsReadOnly = true;
                        brd_branchId.Visibility = Visibility.Visible;
                        brd_locationsGrid.Visibility = Visibility.Collapsed;

                        break;

                }
            }
            else
            {
                dg_invoiceDetails.Columns[0].Visibility = Visibility.Collapsed;
                col_IsItemStoped.Visibility = Visibility.Visible;
                col_promotionPrice.IsReadOnly = true;
                col_quantity.IsReadOnly = true;

                btn_save.IsEnabled = true;
                btn_stop.IsEnabled = true;
                btn_printInvoice.IsEnabled = true;
                cb_PromotionType.IsEnabled = false;
                cb_LocationId.IsEnabled = false;
                cb_PromotionNature.IsEnabled = false;
                tb_PromotionPercentage.IsEnabled = false;
                dp_PromotionDate.IsEnabled = false;
                dp_PromotionStartDate.IsEnabled = false;
                dp_PromotionEndDate.IsEnabled = false;

                chb_locIsSelected.IsEnabled = false;

                //check promotion status first
                switch (_PromotionType)
                {

                    case "quantity":

                        tb_PromotionPercentage.Visibility = Visibility.Collapsed;
                        col_quantity.Visibility = Visibility.Visible;
                        brd_branchId.Visibility = Visibility.Collapsed;
                        brd_locationsGrid.Visibility = Visibility.Visible;

                        break;
                    default:
                        tb_PromotionPercentage.Visibility = Visibility.Visible;
                        col_quantity.Visibility = Visibility.Collapsed;
                        brd_branchId.Visibility = Visibility.Visible;
                        brd_locationsGrid.Visibility = Visibility.Collapsed;

                        break;

                }

                if(promotion.IsStoped == true)
                {
                    btn_save.IsEnabled = false;
                    btn_stop.IsEnabled = false;
                    col_IsItemStoped.IsReadOnly = true;
                }
            }
        }

        #endregion
        #region events
        private void  Btn_newDraft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private bool canAddInvoice()
        {
            bool canAdd = true;
            if (billDetails.Count == 0)
            {
                canAdd = false;
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trOfferWithoutItemsError"), animation: ToasterAnimation.FadeIn);
            }
            else if (_PromotionType == "quantity")
            {
                var lst = (List < PromotionLocations >) dg_itemLocation.ItemsSource;
                var isSelected = lst.Where(x => x.IsSelected == true).FirstOrDefault();
                if(isSelected == null)
                {
                    canAdd = false;
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectLocationError"), animation: ToasterAnimation.FadeIn);
                }
            }
            return canAdd;
        }
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (canAddInvoice())
                {
                    if (HelpClass.validate(requiredControlList, this))
                    {
                        await addInvoice();

                    }
                    else
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("saveNotDoneEmptyFields"), animation: ToasterAnimation.FadeIn);
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

        private async Task addInvoice()
        {

            //promotion.LocationId = (long)cb_LocationId.SelectedValue;
            if (promotion.PromotionId < 0)
                promotion.PromotionId = 0;

            promotion.PromotionType = _PromotionType;
            promotion.PromotionNature = cb_PromotionNature.SelectedValue.ToString();

            promotion.PromotionDate = (DateTime)dp_PromotionDate.SelectedDate;
            promotion.PromotionStartDate = (DateTime)dp_PromotionStartDate.SelectedDate;
            promotion.PromotionEndDate = (DateTime)dp_PromotionEndDate.SelectedDate;

            if(tb_PromotionPercentage.Text != "")
                promotion.PromotionPercentage = decimal.Parse(tb_PromotionPercentage.Text);

            promotion.Notes = tb_Notes.Text;

            promotion.CreateUserId = MainWindow.userLogin.userId;
            promotion.UpdateUserId = MainWindow.userLogin.userId;

            promotion.PromotionDetails = billDetails;
            if (_PromotionType == "quantity")
                promotion.PromotionLocations = listItemLocations.Where(x => x.IsSelected == true).ToList();
            else
            {
                promotion.PromotionLocations = new List<PromotionLocations>();
                promotion.PromotionLocations.Add(new PromotionLocations() { LocationId = (long)cb_LocationId.SelectedValue, IsSelected = true });
            }
            promotion = await promotion.SavePromotion(promotion);

            if (promotion.PromotionId == 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            if (promotion.PromotionId == -5)//الباركود عليه عرض ضمن نفس الفترة
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trBarcodeOfferError"), animation: ToasterAnimation.FadeIn);
            else
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                fillPromotionInputs(promotion);
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
                        PromotionDetails row = (PromotionDetails)dg_invoiceDetails.SelectedItems[0];
                        int index = dg_invoiceDetails.SelectedIndex;

                        // remove item from bill
                        billDetails.RemoveAt(index);

                        if (!forceCancelEdit)
                        {
                            dg_invoiceDetails.IsEnabled = false;
                            RefreshInvoiceDetailsDataGrid();
                        }
                        // calculate new total

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

        private  void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
 
                 Clear();
            }
            catch (Exception ex)
            {

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

            this.DataContext = new Promotion();

            promotion = new Promotion();
            purchaseOrder = new PurchaseInvoice();

            billDetails = new List<PromotionDetails>();

            await setItemLocationsData();

            if (!forceCancelEdit)
            {
                dg_invoiceDetails.IsEnabled = false;
                RefreshInvoiceDetailsDataGrid();
            }

          
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
                //if (location == null)
                //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectLocationError"), animation: ToasterAnimation.FadeIn);
                //else if (supplier == null)
                //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectSupplierError"), animation: ToasterAnimation.FadeIn);
                //else
                {
                    HelpClass.StartAwait(grid_main);

                    List<Item> itemLst = new List<Item>();
                    List<PromotionDetails> promotionDetails = new List<PromotionDetails>();
                    Item item1 = null;

                    string barcode = "";
                    if (chk_itemNum.IsChecked == true)
                    {
                        itemLst = await FillCombo.item.GetItemByCodeOrName(tb_search.Text);
                        if (itemLst.Count == 1)
                        {
                            item1 = itemLst[0];
                        }

                    }
                    else
                    {
                        barcode = tb_search.Text;
                        item1 = await FillCombo.item.GetItemByBarcode(tb_search.Text);
                    }


                    if (itemLst.Count > 1)
                    {
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_addPurchaseItems w = new wd_addPurchaseItems();
                        w.items = itemLst.ToList();
                      //  w.supId = (long)cb_SupId.SelectedValue;
                       // w.locationId = (long)cb_LocationId.SelectedValue;

                        w.ShowDialog();
                        if (w.isOk)
                        {
                            item1 = w.item;
                        }
                    }


                    if (item1 != null)
                    {
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_itemUnitsPromotion w = new wd_itemUnitsPromotion();
                        w.item = item1;
                        w.promotionType = _PromotionType;
                        w.selectedBarcode = barcode;
                        w.ShowDialog();

                        if (w.isOk)
                        {
                            var lst = new List<PromotionDetails>();
                            
                            if (_PromotionType == "percentage")
                                w.promotionDetails = calculatePromotionPrice(w.promotionDetails);
                            foreach(var row in w.promotionDetails)
                            {
                                if (row.IsSelected == true)
                                    lst.Add(row);
                            }

                            //    int maxQty = 0;
                            //    int minQty = 0;
                            //    if (w.newPromotionItem.MaxQty != null)
                            //        maxQty = (int)w.newPromotionItem.MaxQty;
                            //    if (w.newPromotionItem.MinQty != null)
                            //        minQty = (int)w.newPromotionItem.MinQty;
                            //    w.newPromotionItem.Cost = (w.newPromotionItem.MainCost * maxQty) + ((w.newPromotionItem.MainCost / (int)w.newPromotionItem.Factor) * minQty);
                            //    w.newPromotionItem.Price = ((int)w.newPromotionItem.Factor * w.newPromotionItem.MainPrice * maxQty) + (w.newPromotionItem.MainPrice * minQty);

                            //    if (w.newPromotionItem.MinQty == null)
                            //        w.newPromotionItem.MinQty = 0;
                            //    if (w.newPromotionItem.FreeQty == null)
                            //        w.newPromotionItem.FreeQty = 0;
                            addItemToBill(lst);
                           
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

        private List<PromotionDetails> calculatePromotionPrice(List<PromotionDetails> details)
        {
            decimal promotionPercentage = 0;
            if(tb_PromotionPercentage.Text !="")
                promotionPercentage= decimal.Parse(tb_PromotionPercentage.Text);
            foreach(var item in details)
            {
                item.PromotionPrice = item.MainPrice - HelpClass.calcPercentage(item.MainPrice, promotionPercentage);
            }
            return details;
        }
        List<PromotionDetails> billDetails = new List<PromotionDetails>();
        private void addItemToBill(List<PromotionDetails> promotionDetails)
        {
            foreach (var row in promotionDetails)
            {
                int index = billDetails.IndexOf(billDetails.Where(p => p.UnitId == row.UnitId && p.ItemId == row.ItemId && p.Barcode == row.Barcode).FirstOrDefault());

                if (index == -1)//item doesn't exist in bill
                {
                    billDetails.Add(row);

                }
                else // item exist prevoiusly in list
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trItemExistInOrderError"), animation: ToasterAnimation.FadeIn);

                }
            }
            if (!forceCancelEdit)
            {
                dg_invoiceDetails.IsEnabled = false;
                RefreshInvoiceDetailsDataGrid();
            }
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

        //            promotion.LocationId = purchaseOrder.LocationId;
        //            promotion.SupId = purchaseOrder.SupId;
        //            promotion.PurchaseInvNumber = purchaseOrder.InvNumber;

        //          promotion.NetInvoice= purchaseOrder.CostNet;

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

        private void Btn_promotionOrders_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_promotions w = new wd_promotions();
                w.windowTitle = AppSettings.resourcemanager.GetString("PromotionalOffers");
                w.ShowDialog();
                if (w.isOk)
                {
                    promotion = w.promotion;
                    _PromotionType = promotion.PromotionType;
                    fillPromotionInputs(promotion);
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

        public void fillPromotionInputs(Promotion invoice)
        {
            
            this.DataContext = invoice;

            cb_LocationId.SelectedValue = invoice.PromotionLocations[0].LocationId;
            buildInvoiceDetails(invoice);
            setItemLocationsData();
            ControlsEditable();
           
        }
       
        private void buildInvoiceDetails(Promotion invoice)
        {

            billDetails = invoice.PromotionDetails.ToList();
            //dg_invoiceDetails.ItemsSource = invoice.PromotionDetails;
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
                    msg = await printInvoice(promotion);
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
        public async Task<string> printInvoice(Promotion prInvoice)
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
                rep.DataSources.Add(new ReportDataSource("DataSetPromotionDetails", promotion.PromotionDetails));

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
        private void dp_PromotionDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dp_PromotionDate.SelectedDate != null && dp_SupInvoiceDate.SelectedDate != null)
                    if (dp_PromotionDate.SelectedDate.Value.Date > dp_SupInvoiceDate.SelectedDate.Value.Date)
                        dp_SupInvoiceDate.SelectedDate = dp_PromotionDate.SelectedDate.Value.Date;

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        */

       



     


      

        private void tb_FreePercentage_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
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
                //Grid.SetColumn(brd_grid2_0, 1);

                Grid.SetColumn(brd_grid0_1, 0);
                Grid.SetColumn(brd_grid1_1, 0);
                Grid.SetColumn(brd_grid2_1, 0);
            }
            else
            {
                Grid.SetColumn(brd_grid0_0, 0);
                Grid.SetColumn(brd_grid1_0, 0);
                //Grid.SetColumn(brd_grid2_0, 0);

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
                    var res = await promotion.deletePromotionInv(promotion.PromotionId, MainWindow.userLogin.userId);

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
    

        private async void Btn_posting_Click(object sender, RoutedEventArgs e)
        {
            /*
            try
            {
                HelpClass.StartAwait(grid_main);

                if (promotion.PromotionStatus == "notCarriedOver")
                {
                    promotion = await promotion.SavePromotionOrder(promotion);

                }
                else if (promotion.PromotionStatus == "locationTransfer")
                {
                    promotion = await promotion.SavePromotionOrder(promotion);
                }



                if (promotion.PromotionId == 0)
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                else
                {
                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                    fillPromotionInputs(promotion);
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

        private  void cb_PromotionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                 Clear();
                _PromotionType = cb_PromotionType.SelectedValue.ToString();
                if (_PromotionType == "quantity")
                    requiredControlList = new List<string> { "PromotionDate", "PromotionStartDate","PromotionEndDate",
                                                                "PromotionNature","PromotionQuantity"};
                else
                    requiredControlList = new List<string> { "PromotionDate", "PromotionStartDate","PromotionEndDate",
                                                "PromotionNature","PromotionPercentage","LocationId"};
                ControlsEditable();
            }
            catch { }
        }

        Supplier supplier;
        private void Cb_SupId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
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
            */
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
            /*
             * 
            try
            {
                ValidateEmpty_TextChange(sender, e);

                if (tb_AmountDifference.Text != "")
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
            */
        }

        private async void tb_PurchaseInvNumber_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            try
            {
                if (e.Key == Key.Return && tb_PurchaseInvNumber.Text != "")
                {
                    HelpClass.StartAwait(grid_main);
                    purchaseOrder = await FillCombo.purchaseInvoice.getPurchaseOrderByNum(tb_PurchaseInvNumber.Text);

                    promotion = new Promotion();
                    promotion.LocationId = purchaseOrder.LocationId;
                    promotion.SupId = purchaseOrder.SupId;
                    promotion.PurchaseInvNumber = purchaseOrder.InvNumber;

                    promotion.NetInvoice = purchaseOrder.CostNet;

                    // doesn't have any promotion

                    if (purchaseOrder.PromotionDocuments == null)
                        await fillOrderInputs(purchaseOrder);
                    else
                    {
                        foreach (var row in purchaseOrder.PurchaseDetails)
                        {
                            var minQty = 0;
                            var maxQty = 0;
                            foreach (var row1 in purchaseOrder.PromotionDocuments)
                            {
                                foreach (var item in row1.PromotionDetails)
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
                                breakNum = 1 + (int)Math.Ceiling((decimal)diffMin / (decimal)row.Factor);
                                row.MinQty = row.Factor + diffMin;
                            }
                            else
                                row.MinQty = row.Factor - diffMin;

                            row.MaxQty = row.MaxQty - maxQty - breakNum;
                            await fillOrderInputs(purchaseOrder);
                        }
                    }
                    HelpClass.EndAwait(grid_main);
                }
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

        private void dp_PromotionStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dp_PromotionStartDate.SelectedDate != null && dp_PromotionEndDate.SelectedDate != null)
                    if (dp_PromotionStartDate.SelectedDate.Value.Date > dp_PromotionEndDate.SelectedDate.Value.Date)
                        dp_PromotionEndDate.SelectedDate = dp_PromotionStartDate.SelectedDate.Value.Date;

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private async void Btn_stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                promotion = await FillCombo.Promotion.TerminateOffer(promotion.PromotionId, MainWindow.userLogin.userId);
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);

                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void tb_PromotionPercentage_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                calculatePromotionPrice(billDetails);

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
    }
}
