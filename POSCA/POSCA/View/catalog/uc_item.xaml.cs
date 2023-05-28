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

namespace POSCA.View.catalog
{
    /// <summary>
    /// Interaction logic for uc_item.xaml
    /// </summary>
    public partial class uc_item : UserControl
    {

        public uc_item()
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
        private static uc_item _instance;
        public static uc_item Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_item();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        Item item = new Item();
        Category category = new Category();

        IEnumerable<Item> itemsQuery;
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
                requiredControlList = new List<string> { "Name" ,"ShortName", "EngName", "UnitId" , "Factor",
                                                "CategoryId","CountryId", "SupId"};
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


                Keyboard.Focus(tb_Name);
                #region loading
                loadingList = new List<keyValueBool>();
                bool isDone = true;

                loadingList.Add(new keyValueBool { key = "loading_RefrishCategories", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefrishCountries", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefrishBrands", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefrishSuppliers", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefrishUnits", value = false });

                loading_RefrishCategories();
                loading_RefrishCountries();
                loading_RefrishBrands();
                loading_RefrishSuppliers();
                loading_RefrishUnits();

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
                //await FillCombo.fillCategorys(cb_CategoryId);
                // await FillCombo.fillCountrys(cb_CountryId);
                // await FillCombo.fillBrandsWithDefault(cb_BrandId);
                //await FillCombo.fillSuppliers(cb_SupId);
                //await FillCombo.fillUnits(cb_UnitId);

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

            txt_title.Text = AppSettings.resourcemanager.GetString("Items");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Code, AppSettings.resourcemanager.GetString("trNoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Name, AppSettings.resourcemanager.GetString("trNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ShortName, AppSettings.resourcemanager.GetString("ShortNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_EngName, AppSettings.resourcemanager.GetString("EnglishNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_CategoryId, AppSettings.resourcemanager.GetString("CategoryHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_CountryId, AppSettings.resourcemanager.GetString("CountryHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_BrandId, AppSettings.resourcemanager.GetString("BrandHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_SupId, AppSettings.resourcemanager.GetString("SupplierHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_SupSectorId, AppSettings.resourcemanager.GetString("SupplierSectorHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CommitteeNo, AppSettings.resourcemanager.GetString("CommitteeNoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_UnitId, AppSettings.resourcemanager.GetString("SupplyUnitHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Factor, AppSettings.resourcemanager.GetString("FactorHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_MainCost, AppSettings.resourcemanager.GetString("SupplyCostHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ConsumerProfitPerc, AppSettings.resourcemanager.GetString("ProfitMarginHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_MainPrice, AppSettings.resourcemanager.GetString("PieceSellingHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ConsumerDiscPerc, AppSettings.resourcemanager.GetString("ConsumerDiscountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Cost, AppSettings.resourcemanager.GetString("NetCostHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Price, AppSettings.resourcemanager.GetString("NetPieceSellingHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_WholesaleProfitPerc, AppSettings.resourcemanager.GetString("WholesaleProfitMarginHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_WholesalePrice, AppSettings.resourcemanager.GetString("WholesalePriceHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            txt_IsSpecialOffer.Text = AppSettings.resourcemanager.GetString("SpecialOffer");
            txt_IsWeight.Text = AppSettings.resourcemanager.GetString("IsWeight");

            txt_unitsButton.Text = AppSettings.resourcemanager.GetString("trUnits");
            txt_itemGeneralizationButton.Text = AppSettings.resourcemanager.GetString("Generalization");
            txt_allowedOperationsButton.Text = AppSettings.resourcemanager.GetString("ItemMovements");
            txt_supplyingItemButton.Text = AppSettings.resourcemanager.GetString("ExtraInformation");

            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trSave");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            dg_item.Columns[0].Header = AppSettings.resourcemanager.GetString("ItemNumber");
            dg_item.Columns[1].Header = AppSettings.resourcemanager.GetString("SupplierNumber");
            dg_item.Columns[2].Header = AppSettings.resourcemanager.GetString("trName");
            dg_item.Columns[3].Header = AppSettings.resourcemanager.GetString("Factor");
            dg_item.Columns[4].Header = AppSettings.resourcemanager.GetString("trPrice");
            dg_item.Columns[5].Header = AppSettings.resourcemanager.GetString("Cost");
            dg_item.Columns[6].Header = AppSettings.resourcemanager.GetString("Category");

            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");

            //tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            //tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            //tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            //tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            //tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            //tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

        }

        #region Loading
        List<keyValueBool> loadingList;
        List<string> catchError = new List<string>();
        int catchErrorCount = 0;

        bool loadingSuccess_RefrishCategories = false;
        async void loading_RefrishCategories()
        {
            try
            {
                await FillCombo.fillCategorys(cb_CategoryId);
                if (FillCombo.categoryList is null)
                    loading_RefrishCategories();
                else
                    loadingSuccess_RefrishCategories = true;
            }
            catch (Exception ex)
            {
                catchError.Add("loading_RefrishCategories");
                catchErrorCount++;
                if (catchErrorCount > 50)
                {
                    loadingSuccess_RefrishCategories = true;
                }
                else
                    loading_RefrishCategories();
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
            if (loadingSuccess_RefrishCategories)
                foreach (var item in loadingList)
                {
                    if (item.key.Equals("loading_RefrishCategories"))
                    {
                        item.value = true;
                        break;
                    }
                }
        }
        
        bool loadingSuccess_RefrishCountries = false;
        async void loading_RefrishCountries()
        {
            try
            {
                await FillCombo.fillCountrys(cb_CountryId);
                if (FillCombo.countryList is null)
                    loading_RefrishCountries();
                else
                    loadingSuccess_RefrishCountries = true;
            }
            catch (Exception ex)
            {
                catchError.Add("loading_RefrishCountries");
                catchErrorCount++;
                if (catchErrorCount > 50)
                {
                    loadingSuccess_RefrishCountries = true;
                }
                else
                    loading_RefrishCountries();
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
            if (loadingSuccess_RefrishCountries)
                foreach (var item in loadingList)
                {
                    if (item.key.Equals("loading_RefrishCountries"))
                    {
                        item.value = true;
                        break;
                    }
                }
        } 
        
        bool loadingSuccess_RefrishBrands = false;
        async void loading_RefrishBrands()
        {
            try
            {
                await FillCombo.fillBrandsWithDefault(cb_BrandId);
                if (FillCombo.brandList is null)
                    loading_RefrishBrands();
                else
                    loadingSuccess_RefrishBrands = true;
            }
            catch (Exception ex)
            {
                catchError.Add("loading_RefrishBrands");
                catchErrorCount++;
                if (catchErrorCount > 50)
                {
                    loadingSuccess_RefrishBrands = true;
                }
                else
                    loading_RefrishBrands();
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
            if (loadingSuccess_RefrishBrands)
                foreach (var item in loadingList)
                {
                    if (item.key.Equals("loading_RefrishBrands"))
                    {
                        item.value = true;
                        break;
                    }
                }
        } 
        
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
        
        bool loadingSuccess_RefrishUnits = false;
        async void loading_RefrishUnits()
        {
            try
            {
                await FillCombo.fillUnits(cb_UnitId);
                if (FillCombo.unitList is null)
                    loading_RefrishUnits();
                else
                    loadingSuccess_RefrishUnits = true;
            }
            catch (Exception ex)
            {
                catchError.Add("loading_RefrishUnits");
                catchErrorCount++;
                if (catchErrorCount > 50)
                {
                    loadingSuccess_RefrishUnits = true;
                }
                else
                    loading_RefrishUnits();
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
            if (loadingSuccess_RefrishUnits)
                foreach (var item in loadingList)
                {
                    if (item.key.Equals("loading_RefrishUnits"))
                    {
                        item.value = true;
                        break;
                    }
                }
        }
        #endregion
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {

                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                //{
                HelpClass.StartAwait(grid_main);

                item.ItemId = 0;
                if (HelpClass.validate(requiredControlList, this) )
                {
                    if (tb_Factor.Text.Equals("0"))
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trFactorZeroError"), animation: ToasterAnimation.FadeIn);
                    else if(item.ItemUnits.Count == 0 ||item.ItemUnits.Where(x =>x.Barcode == "" || x.Barcode == null).FirstOrDefault() != null)
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trItemBarcodError"), animation: ToasterAnimation.FadeIn);
                    else if (item.ItemLocations == null || item.ItemLocations.Count() == 0)
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("GoToItemTransactionsError"), animation: ToasterAnimation.FadeIn);
                    
                    else if(item.ItemStatus =="" || item.ItemStatus == null
                            || item.ItemReceiptType =="" || item.ItemReceiptType == null
                            || item.ItemType =="" || item.ItemType == null
                            || item.ItemTransactionType == "" || item.ItemTransactionType == null
                            || item.PackageWeight == null
                            || item.PackageUnit == null)

                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("AdditionalInfoError"), animation: ToasterAnimation.FadeIn);

                    else
                    {
                        item.Code = tb_Code.Text;
                        item.Name = tb_Name.Text;
                        item.ShortName = tb_ShortName.Text;
                        item.EngName = tb_EngName.Text;
                        item.CategoryId = (long)cb_CategoryId.SelectedValue;
                        item.CountryId = (long)cb_CountryId.SelectedValue;

                        if (cb_BrandId.SelectedIndex > 0)
                            item.BrandId = (int)cb_BrandId.SelectedValue;

                        item.SupId = (long)cb_SupId.SelectedValue;
                        if(cb_SupSectorId.SelectedIndex > 0)
                            item.SupSectorId = (long)cb_SupSectorId.SelectedValue;
                        item.Factor = int.Parse(tb_Factor.Text);
                        if (tb_CommitteeNo.Text != "")
                            item.CommitteeNo = int.Parse(tb_CommitteeNo.Text);
                        if (tgl_IsWeight.IsChecked == true)
                            item.IsWeight = true;
                        if (tgl_IsSpecialOffer.IsChecked == true)
                            item.IsSpecialOffer = true;
                        item.Notes = tb_Notes.Text;
                        item.CreateUserId = MainWindow.userLogin.userId;

                        var item1 = await item.save(item);
                        if (item1 == null)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {

                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                             Clear();
                            //await Search();
                        }
                    }
            

                }
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
        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {

                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (item.ItemId > 0)
                    {
                        if (HelpClass.validate(requiredControlList, this) )
                        {
                            if (!tb_Factor.Text.Equals("0"))
                            {
                                item.Code = tb_Code.Text;
                                item.Name = tb_Name.Text;
                                item.ShortName = tb_ShortName.Text;
                                item.EngName = tb_EngName.Text;
                                item.CategoryId = (long)cb_CategoryId.SelectedValue;
                                item.CountryId = (long)cb_CountryId.SelectedValue;

                                if (cb_BrandId.SelectedIndex > 0)
                                    item.BrandId = (int)cb_BrandId.SelectedValue;

                                item.SupId = (long)cb_SupId.SelectedValue;
                                if (cb_SupSectorId.SelectedIndex > 0)
                                    item.SupSectorId = (long)cb_SupSectorId.SelectedValue;
                                item.Factor = int.Parse(tb_Factor.Text);
                                if (tb_CommitteeNo.Text != "")
                                    item.CommitteeNo = int.Parse(tb_CommitteeNo.Text);
                                if (tgl_IsWeight.IsChecked == true)
                                    item.IsWeight = true;
                                if (tgl_IsSpecialOffer.IsChecked == true)
                                    item.IsSpecialOffer = true;
                                item.Notes = tb_Notes.Text;
                                item.CreateUserId = MainWindow.userLogin.userId;

                                var item1 = await item.save(item);
                                if (item1 == null)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                                    Clear();
                                    //await Search();
                                }
                            }
                            else
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trFactorZeroError"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);

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
        private async void Btn_delete_Click(object sender, RoutedEventArgs e)
        {//delete
            try
            {
                // if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (item.ItemId != 0)
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
                           
                            var res = await item.delete(item.ItemId, MainWindow.userLogin.userId);
                            if (res == 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {

                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);
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

        #endregion
        #region events

        private async void Btn_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //dina search

                HelpClass.StartAwait(grid_main);
                itemsQuery = await FillCombo.item.searchItems(tb_search.Text);
                RefreshItemsView();
                HelpClass.EndAwait(grid_main);
            }
            catch
            {
                HelpClass.EndAwait(grid_main);
            }
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
        private async void Dg_item_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection

                if (dg_item.SelectedIndex != -1)
                {
                    item = dg_item.SelectedItem as Item;
                    this.DataContext = item;
                    tb_Code.Text = item.Code.ToString();
                    tb_Factor.Text = item.Factor.ToString();
                    tb_ShortName.Text = item.ShortName;
                    tb_ConsumerProfitPerc.Text = item.ConsumerProfitPerc.ToString();
                    tb_MainPrice.Text = item.MainPrice.ToString();
                    tb_ConsumerDiscPerc.Text = item.ConsumerDiscPerc.ToString();
                    tb_Cost.Text = item.Cost.ToString();
                    tb_Price.Text = item.Price.ToString();
                    tb_WholesaleProfitPerc.Text = item.WholesaleProfitPerc.ToString();
                    tb_WholesalePrice.Text = item.WholesalePrice.ToString();
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

                //tb_search.Text = "";
                //searchText = "";
                await RefreshItemsList();
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
        
            if (FillCombo.itemList is null)
                await RefreshItemsList();
            searchText = tb_search.Text.ToLower();
            itemsQuery = FillCombo.itemList.Where(s =>
            s.Name.ToLower().Contains(searchText)
            || s.ShortName.ToLower().Contains(searchText)
            || s.EngName.ToLower().Contains(searchText)
            || s.CategoryName.ToLower().Contains(searchText)
            || s.Code.ToLower().Contains(searchText)
            || s.SupId.ToString() == searchText
            || s.Factor.ToString() == searchText
            || s.Price.ToString() == searchText
            || s.Cost.ToString() == searchText
            || s.Supplier.Name.ToLower().Contains(searchText)

            ).ToList();
         
            RefreshItemsView();
        }
        async Task<IEnumerable<Item>> RefreshItemsList()
        {

            await FillCombo.RefreshItems();
            return FillCombo.itemList;
    
        }

        void RefreshItemsView()
        {
            dg_item.ItemsSource = itemsQuery;
            txt_count.Text = itemsQuery.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            item = new Item();
            this.DataContext = new Item();
            tb_ShortName.Clear();
            tb_Code.Text = "";
            tb_Factor.Text = "";
            tb_ConsumerProfitPerc.Text = "0";
            tb_ConsumerDiscPerc.Text = "0";
            tb_WholesaleProfitPerc.Text = "0";
            nameFirstChange = true;
            dg_item.SelectedIndex = -1;

            tb_Factor.IsEnabled = true;
            category = new Category();
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

        bool nameFirstChange = true;
        private void Tb_Name_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                validateEmpty_LostFocus(sender, e);
                if (nameFirstChange && item.ItemId == 0)
                {
                    nameFirstChange = false;
                    tb_ShortName.Text = tb_Name.Text;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        #endregion

        private void Btn_addCountry_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_addBrand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_addSup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_addSupSectorId_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Btn_supplyingItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_supplyingItem w = new wd_supplyingItem();

                w.itemStatus = item.ItemStatus;
                w.itemRecieptType = item.ItemReceiptType;
                w.itemType = item.ItemType;
                w.itemTransactionType = item.ItemTransactionType;

                w.packageWeight =item.PackageWeight;
                w.packageUnit = item.PackageUnit;
                w.ShowDialog();
                if (w.isOk)
                {
                    item.ItemStatus = w.itemStatus;
                    item.ItemReceiptType = w.itemRecieptType;
                    item.ItemType = w.itemType;
                    item.ItemTransactionType = w.itemTransactionType;

                    item.PackageWeight = w.packageWeight;
                    item.PackageUnit = w.packageUnit;
                    if(item.ItemId != 0)
                    {
                        item = await item.save(item);
                      
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
                wd_itemAllowedOperations w = new wd_itemAllowedOperations();

                if(item.ItemAllowedTransactions != null)
                    w.itemAllowedTransactions = item.ItemAllowedTransactions.ToList();
                if (item.ItemLocations != null)
                w.itemLocations = item.ItemLocations.ToList();
                w.ShowDialog();
                if (w.isOk)
                {
                    item.ItemAllowedTransactions = w.itemAllowedTransactions;
                    item.ItemLocations = w.itemLocations;
                    if (item.ItemId != 0)
                    {
                       item = await item.save(item);

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

        private async void Btn_itemGeneralization_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_itemGeneralization w = new wd_itemGeneralization();

                if (item.ItemGeneralizations != null)
                    w.itemGeneralizations = item.ItemGeneralizations.ToList();

                w.ShowDialog();
                if (w.isOk)
                {
                    item.ItemGeneralizations = w.itemGeneralizations;

                    if (item.ItemId != 0)
                    {
                        item = await item.save(item);

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

        private async void Btn_units_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cb_UnitId.SelectedIndex == -1)
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectSupplyUnitError"), animation: ToasterAnimation.FadeIn);
                if (tb_Factor.Text.Equals("") || tb_Factor.Text.Equals("0"))
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trEnterFactorError"), animation: ToasterAnimation.FadeIn);
                else if (cb_CategoryId.SelectedIndex == -1)
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectCategoryError"), animation: ToasterAnimation.FadeIn);
                else if (tb_MainCost.Text.Equals("0") || tb_MainCost.Text.Equals(""))
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trEnterSupplyCostError"), animation: ToasterAnimation.FadeIn);
                else
                {
                    HelpClass.StartAwait(grid_main);
                    Window.GetWindow(this).Opacity = 0.2;
                    wd_itemUnits w = new wd_itemUnits();

                    item.Factor = int.Parse(tb_Factor.Text);
                    item.UnitId = (long)cb_UnitId.SelectedValue;
                    item.CategoryId = (long)cb_CategoryId.SelectedValue;
                    item.MainCost = decimal.Parse(tb_MainCost.Text);
                    w.itemUnits = item.ItemUnits.Select(x => new ItemUnit()
                    {
                        Barcode = x.Barcode,
                        BarcodeType = x.BarcodeType,
                        Cost=x.Cost,
                        SalePrice = x.SalePrice,
                        Factor = x.Factor,
                        IsBlocked = x.IsBlocked,
                        ItemId=x.ItemId,
                        ItemUnitId = x.ItemUnitId,
                        UnitId=x.UnitId,
                    }).ToList();
                    w.item = item;
                    w.ShowDialog();
                    if (w.isOk)
                    {
                        item.ItemUnits = w.itemUnits;

                        if (item.ItemId != 0)
                        {
                           item = await item.save(item);
                          
                        }
                    }
                    Window.GetWindow(this).Opacity = 1;

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

        private void Btn_addCategory_Click(object sender, RoutedEventArgs e)
        {

        }
        private void cb_SupId_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                var tb = cb_SupId.Template.FindName("PART_EditableTextBox", cb_SupId) as TextBox;
                tb.FontFamily = Application.Current.Resources["Font-cairo-regular"] as FontFamily;
                cb_SupId.ItemsSource = FillCombo.suppliersList.Where(p => p.Name.ToLower().Contains(tb.Text.ToLower()) 
                                        ||  p.SupCode.ToString().Contains(tb.Text)).ToList();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async  void Cb_SupId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                var supplier = FillCombo.suppliersList.Where(x => x.SupId == (long)cb_SupId.SelectedValue).FirstOrDefault();

                var lst = supplier.SupplierSectors.ToList();
                var sup = new SupplierSector();
                sup.SupSectorName = "-";
                sup.SupSectorId = 0;
                lst.Insert(0, sup);

                cb_SupSectorId.ItemsSource = lst;
                cb_SupSectorId.SelectedValuePath = "SupSectorId";
                cb_SupSectorId.DisplayMemberPath = "SupSectorName";
                cb_SupSectorId.SelectedIndex = -1;
                cb_SupSectorId.Items.Refresh();
                //generate item number
                if(item.ItemId == 0 || supplier.SupId != item.SupId)
                    tb_Code.Text= await generateItemCode(supplier.SupId);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async Task<string> generateItemCode(long supId)
        {
            return await FillCombo.item.generateItemCode(supId);
            //long maxId = 0;
            //if (FillCombo.itemList.Count > 0)
            //    maxId = FillCombo.itemList.Select(x => x.ItemId).Max();
            //maxId++;
            //var itemCode = supId.ToString().PadLeft(4, '0') + maxId.ToString().PadLeft(4, '0');

            //return itemCode;
        }
        private void Cb_UnitId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                tb_Factor.Text = "";
                if (cb_UnitId.SelectedIndex > -1)
                {
                    long oldId = 0;
                    if (item.UnitId != null)
                        oldId = (long)item.UnitId;

                    item.UnitId = (long)cb_UnitId.SelectedValue;

                    var unit = FillCombo.unitList.Where(x => x.UnitId == (long)cb_UnitId.SelectedValue).FirstOrDefault();
                    item.ItemUnit = unit.Name;
                    if (unit.Name.ToLower().Trim().Equals(AppSettings.resourcemanager.GetString("piece")))
                    {
                        tb_Factor.Text = "1";
                        tb_Factor.IsEnabled = false;
                    }
                    else
                    {
                        tb_Factor.IsEnabled = true;
                    }

                    calculatePeicePrice();
                    if(item.ItemUnits == null || item.ItemUnits.Count == 0 || oldId != item.UnitId)
                    {
                        item.ItemUnits = new List<ItemUnit>();
                        int factor = 0;
                        if (tb_Factor.Text != "")
                            factor =int.Parse( tb_Factor.Text);
                        item.ItemUnits.Add(new ItemUnit()
                        {

                            UnitId = unit.UnitId,
                            ItemId = item.ItemId,
                            Barcode = "",
                            BarcodeType = "",
                            Factor = factor,
                            Cost = item.Cost,
                            SalePrice = item.Price *factor,
                            CreateUserId= MainWindow.userLogin.userId,
                            UpdateUserId = MainWindow.userLogin.userId,
                        }) ;
                    }
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void cb_CategoryId_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                var tb = cb_CategoryId.Template.FindName("PART_EditableTextBox", cb_CategoryId) as TextBox;
                tb.FontFamily = Application.Current.Resources["Font-cairo-regular"] as FontFamily;
                cb_CategoryId.ItemsSource = FillCombo.categoryList.Where(p => p.Name.ToLower().Contains(tb.Text.ToLower()) ).ToList();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Cb_CategoryId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                category = FillCombo.categoryList.Where(x => x.CategoryId == (long)cb_CategoryId.SelectedValue).FirstOrDefault();

                tb_ConsumerProfitPerc.Text = category.ProfitPercentage.ToString();
                tb_ConsumerDiscPerc.Text = category.DiscountPercentage.ToString();
                tb_WholesaleProfitPerc.Text = category.WholesalePercentage.ToString();
                item.ConsumerProfitPerc = category.ProfitPercentage;
                item.ConsumerDiscPerc = category.DiscountPercentage;
                item.WholesaleProfitPerc = category.WholesalePercentage;
                calculatePeicePrice();
             
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void cb_CountryId_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                var tb = cb_CountryId.Template.FindName("PART_EditableTextBox", cb_CountryId) as TextBox;
                tb.FontFamily = Application.Current.Resources["Font-cairo-regular"] as FontFamily;
                cb_CountryId.ItemsSource = FillCombo.countryList.Where(p => p.CountryName.ToLower().Contains(tb.Text.ToLower())).ToList();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void cb_BrandId_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                var tb = cb_BrandId.Template.FindName("PART_EditableTextBox", cb_BrandId) as TextBox;
                tb.FontFamily = Application.Current.Resources["Font-cairo-regular"] as FontFamily;
                var lst = FillCombo.brandList.Where(p => p.Name.ToLower().Contains(tb.Text.ToLower())).ToList();

                Brand sup = new Brand();
                sup.Name = "-";
                sup.BrandId = 0;
                lst.Insert(0, sup);

                cb_BrandId.ItemsSource = lst;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void tb_Factor_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tb_Factor.IsFocused)
                {
                    ValidateEmpty_TextChange(sender, e);
                    if(cb_UnitId.SelectedValue != null && tb_Factor.Text != "") 
                    foreach(var row in item.ItemUnits)
                    {
                        if (row.UnitId == (long)cb_UnitId.SelectedValue)
                            row.Factor = int.Parse(tb_Factor.Text);
                    }
                    calculatePeicePrice();
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Tb_MainCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tb_MainCost.IsFocused)
                {
                    ValidateEmpty_TextChange(sender, e);
                    calculatePeicePrice();
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        decimal peicePrice = 0;
        decimal finalPrice = 0;
        private void calculatePeicePrice()
        {
            try
            {
                if (tb_Factor.Text.Equals(""))
                    tb_Factor.Text = "0";
                //if (!tb_Factor.Text.Equals(""))
               // {
                    int factor = int.Parse(tb_Factor.Text);
                    if (factor != 0)
                    {
                        decimal cost = decimal.Parse(tb_MainCost.Text);
                        //سعر بيع الحبة
                        if (category.ProfitPercentage != 0)
                            peicePrice = cost / factor * (1+ HelpClass.calcPercentage(1, category.ProfitPercentage));
                        else
                            peicePrice = cost / factor;

                        //صافي بيع الحبة
                        finalPrice = peicePrice;
                        if (category.DiscountPercentage != 0)
                        {
                            var discount = peicePrice - HelpClass.calcPercentage(peicePrice, 100+ category.DiscountPercentage);
                            finalPrice = peicePrice - Math.Abs( discount);
                        }
                        if (finalPrice < 0)
                            finalPrice = 0;

                        //wholesale price سعر الجملة
                        var wholesalePrice = cost / factor * (1 + HelpClass.calcPercentage(1, category.WholesalePercentage));

                        tb_MainPrice.Text = HelpClass.DecTostring(peicePrice);
                        tb_Cost.Text = HelpClass.DecTostring(cost);
                        tb_Price.Text = HelpClass.DecTostring(finalPrice);
                        tb_WholesalePrice.Text = HelpClass.DecTostring(wholesalePrice);

                        item.MainPrice = peicePrice;
                        item.MainCost = cost;
                        item.Cost = cost;
                        item.Price = finalPrice;
                        item.WholesalePrice = wholesalePrice;

                    }
                if (item.ItemUnits != null && item.ItemUnits.Count > 0 )
                {
                    foreach(var row in item.ItemUnits)
                    {
                        if(row.UnitId == item.UnitId)
                        {
                            row.Factor = factor;
                            row.Cost = item.Cost;
                            row.SalePrice = item.Price * factor;
                        }
                    }                 
                }
                // }

            }
            catch { }
        }

     

        #region swap
        private void btn_columnSwap_Click(object sender, RoutedEventArgs e)
        {
            ColumnDefinition cd_gridMain3 = new ColumnDefinition();
            cd_gridMain3.Width = cd_gridMain1.Width;
            cd_gridMain1.Width = cd_gridMain2.Width;
            cd_gridMain2.Width = cd_gridMain3.Width;
        }

        private void Btn_swapToSearch_Click(object sender, RoutedEventArgs e)
        {
            cd_gridMain1.Width = new GridLength(1, GridUnitType.Star);
            cd_gridMain2.Width = new GridLength(0, GridUnitType.Star);
            if(tb_search.Text != "")
                Btn_search_Click(null, null);
            else
            {
                itemsQuery = new List<Item>();
                RefreshItemsView();
            }
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

        #endregion
    }
}
