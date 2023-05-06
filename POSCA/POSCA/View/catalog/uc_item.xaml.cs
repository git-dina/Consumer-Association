﻿using Microsoft.Win32;
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


                Keyboard.Focus(tb_Name);

                await FillCombo.fillCategorys(cb_CategoryId);
                await FillCombo.fillCountrys(cb_CountryId);
                await FillCombo.fillBrandsWithDefault(cb_BrandId);
                await FillCombo.fillSuppliers(cb_SupId);
                await FillCombo.fillUnits(cb_UnitId);

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
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trUpdate");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            dg_item.Columns[0].Header = AppSettings.resourcemanager.GetString("ItemNumber");
            dg_item.Columns[1].Header = AppSettings.resourcemanager.GetString("SupplierNumber");
            dg_item.Columns[2].Header = AppSettings.resourcemanager.GetString("trName");
            dg_item.Columns[3].Header = AppSettings.resourcemanager.GetString("Factor");
            dg_item.Columns[4].Header = AppSettings.resourcemanager.GetString("trPrice");
            dg_item.Columns[5].Header = AppSettings.resourcemanager.GetString("Cost");
            dg_item.Columns[6].Header = AppSettings.resourcemanager.GetString("Category");

            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");

            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            //tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            //tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            //tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            //tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            //tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {

                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                //{
                HelpClass.StartAwait(grid_main);

                item.ItemId = 0;
                if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                {
                    if (!tb_Factor.Text.Equals("0"))
                    {
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

                        FillCombo.itemList = await item.save(item);
                        if (FillCombo.itemList == null)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                             Clear();
                            await Search();
                        }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trFactorZeroError"), animation: ToasterAnimation.FadeIn);

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

                                FillCombo.itemList = await item.save(item);
                                if (FillCombo.itemList == null)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                                    Clear();
                                    await Search();
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
                            /*
                            FillCombo.itemList = await item.delete(item.ItemId, MainWindow.userLogin.userId);
                            if (FillCombo.itemList == null)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                item.ItemId = 0;
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                await Search();
                                Clear();
                            }
                            */
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

                tb_search.Text = "";
                searchText = "";
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
            this.DataContext = new Item();
            tb_ShortName.Clear();
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
                        FillCombo.itemList = await item.save(item);
                        await Search();
                        if (dg_item.SelectedIndex != -1)
                        {
                            item = FillCombo.itemList.Where(x => x.ItemId == item.ItemId).FirstOrDefault();
                        }
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

                w.itemAllowedTransactions = item.ItemAllowedTransactions;

                w.ShowDialog();
                if (w.isOk)
                {
                    item.ItemAllowedTransactions = w.itemAllowedTransactions;

                    if (item.ItemId != 0)
                    {
                        FillCombo.itemList = await item.save(item);
                        await Search();
                        if (dg_item.SelectedIndex != -1)
                        {
                            item = FillCombo.itemList.Where(x => x.ItemId == item.ItemId).FirstOrDefault();
                        }
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
                        FillCombo.itemList = await item.save(item);
                       
                        await Search();
                        if (dg_item.SelectedIndex != -1)
                        {
                            item = FillCombo.itemList.Where(x => x.ItemId == item.ItemId).FirstOrDefault();
                        }
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
                //if (cb_UnitId.SelectedIndex == -1)
                //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectSupplyUnitError"), animation: ToasterAnimation.FadeIn);
                //if (tb_Factor.Text.Equals("") || tb_Factor.Text.Equals("0"))
                //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trEnterFactorError"), animation: ToasterAnimation.FadeIn);
                //else if (cb_CategoryId.SelectedIndex == -1)
                //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectCategoryError"), animation: ToasterAnimation.FadeIn);
                //else if (tb_MainCost.Text.Equals("0") || tb_MainCost.Text.Equals(""))
                //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trEnterSupplyCostError"), animation: ToasterAnimation.FadeIn);
                if (item.ItemId == 0)
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSaveItemFirstError"), animation: ToasterAnimation.FadeIn);
                else
                {
                    HelpClass.StartAwait(grid_main);
                    Window.GetWindow(this).Opacity = 0.2;
                    wd_itemUnits w = new wd_itemUnits();

                    item.Factor = int.Parse(tb_Factor.Text);
                    item.UnitId = (long)cb_UnitId.SelectedValue;
                    item.CategoryId = (long)cb_CategoryId.SelectedValue;
                    item.MainCost = decimal.Parse(tb_MainCost.Text);
                    w.itemUnits = item.ItemUnits.ToList();
                    w.item = item;
                    w.ShowDialog();
                    if (w.isOk)
                    {
                        item.ItemUnits = w.itemUnits;

                        if (item.ItemId != 0)
                        {
                            FillCombo.itemList = await item.save(item);
                            await Search();
                            if (dg_item.SelectedIndex != -1)
                            {
                                item = FillCombo.itemList.Where(x => x.ItemId == item.ItemId).FirstOrDefault();
                            }
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
                cb_SupId.ItemsSource = FillCombo.suppliersList.Where(p => p.Name.ToLower().Contains(tb.Text.ToLower()) ||  p.SupId.ToString().Contains(tb.Text)).ToList();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Cb_SupId_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Cb_UnitId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                tb_Factor.Text = "";
                if (cb_UnitId.SelectedIndex > -1)
                {
                    item.UnitId = (long)cb_UnitId.SelectedValue;

                    var unit = FillCombo.unitList.Where(x => x.UnitId == (long)cb_UnitId.SelectedValue).FirstOrDefault();
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
                    if(item.ItemUnits == null || item.ItemUnits.Count == 0)
                    {
                        item.ItemUnits.Add(new ItemUnit()
                        {
                            UnitId = unit.UnitId,
                            ItemId = item.ItemId,
                            Factor = unit.Factor,
                            Cost = item.Cost,

                        }) ;
                    }
                }
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

        private void tb_Factor_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tb_Factor.IsFocused)
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
                if (!tb_Factor.Text.Equals(""))
                {
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
                }
            }
            catch { }
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
