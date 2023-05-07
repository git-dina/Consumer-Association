using Microsoft.Win32;
using netoaster;
using POSCA.Classes;
using POSCA.Classes.ApiClasses;
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
    /// Interaction logic for wd_itemUnits.xaml
    /// </summary>
    public partial class wd_itemUnits : Window
    {

        public wd_itemUnits()
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

        //List<ItemUnit> listItemUnit = new List<ItemUnit>();
       public List<ItemUnit> itemUnits = new List<ItemUnit>();
        List<Unit> listUnit = new List<Unit>();

       // public List<ItemUnit> ItemUnits { get; set; }

        public Item item { get; set; }      
        public bool isOk { get; set; }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load

            try
            {


                HelpClass.StartAwait(grid_main);


                #region translate

                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }

                translate();
                #endregion

                await FillCombo.fillUnits(cb_unit);
                fillBarcodeTypeCombo();
                setItemUnitsData();


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
            txt_title.Text = AppSettings.resourcemanager.GetString("ItemUnits");
            txt_itemUnit.Text = AppSettings.resourcemanager.GetString("ItemUnits");

            dg_itemUnit.Columns[0].Header = AppSettings.resourcemanager.GetString("Unit");
            dg_itemUnit.Columns[1].Header = AppSettings.resourcemanager.GetString("Barcode");
            dg_itemUnit.Columns[2].Header = AppSettings.resourcemanager.GetString("BarcodeType");
            dg_itemUnit.Columns[3].Header = AppSettings.resourcemanager.GetString("Factor");
            dg_itemUnit.Columns[4].Header = AppSettings.resourcemanager.GetString("Cost");
            dg_itemUnit.Columns[5].Header = AppSettings.resourcemanager.GetString("SalePrice");
            dg_itemUnit.Columns[6].Header = AppSettings.resourcemanager.GetString("IsBlocked");

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

        }
    
       private void fillBarcodeTypeCombo()
        {
            if (FillCombo.barcodeTypeList is null)
                 FillCombo.RefreshBarcodeTypes();

            cb_barcodeType.DisplayMemberPath = "value";
            cb_barcodeType.SelectedValuePath = "key";
            cb_barcodeType.ItemsSource = FillCombo.barcodeTypeList;
        }
        private void setItemUnitsData()
        {
            dg_itemUnit.ItemsSource = itemUnits;
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
     
        private async void Dg_itemUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                itemUnits = (List<ItemUnit>)dg_itemUnit.ItemsSource;
                var isEmpty = itemUnits.Where(x => x.UnitId == null || x.BarcodeType == null || x.fa).FirstOrDefault();
                if (isEmpty != null)
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trRowWithEmptyError"), animation: ToasterAnimation.FadeIn);
                else
                {
                    isOk = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

      
        private void Btn_addItemUnit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_addItemUnit.IsEnabled = false;
                dg_itemUnit.IsEnabled = false;
                itemUnits = (List<ItemUnit>)dg_itemUnit.ItemsSource;
                var isEmpty = itemUnits.Where(x => x.UnitId == null || x.BarcodeType == null).FirstOrDefault();
                if (isEmpty != null)
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trRowWithEmptyError"), animation: ToasterAnimation.FadeIn);
                else
                {
                    itemUnits.Add(new ItemUnit() { ItemId = item.ItemId });
                    RefreshItemUnitDataGrid();
                }
                btn_addItemUnit.IsEnabled = true;
                dg_itemUnit.IsEnabled = true;
            }
            catch (Exception ex)
            {
                btn_addItemUnit.IsEnabled = true;
                dg_itemUnit.IsEnabled = true;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void deleteItemUnitRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {

                        btn_addItemUnit.IsEnabled = false;
                        dg_itemUnit.IsEnabled = false;
                        ItemUnit row = (ItemUnit)dg_itemUnit.SelectedItems[0];
                        itemUnits = (List<ItemUnit>)dg_itemUnit.ItemsSource;
                        itemUnits.Remove(row);
                        RefreshItemUnitDataGrid();
                    }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                btn_addItemUnit.IsEnabled = true;
                dg_itemUnit.IsEnabled = true;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public void RefreshItemUnitDataGrid()
        {
            try
            {
                dg_itemUnit.CancelEdit();
                dg_itemUnit.ItemsSource = itemUnits;
                dg_itemUnit.Items.Refresh();
                dg_itemUnit.Items.Refresh();

                dg_itemUnit.IsEnabled = true;
                btn_addItemUnit.IsEnabled = true;
            }
            catch (Exception ex)
            {
                dg_itemUnit.IsEnabled = true;
                btn_addItemUnit.IsEnabled = true;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
        }

        private void Cb_barcodeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var cmb = sender as ComboBox;
                if (dg_itemUnit.SelectedIndex != -1 && cmb != null)
                {
                    int _datagridSelectedIndex = dg_itemUnit.SelectedIndex;
                    var itemUnit = (ItemUnit)dg_itemUnit.SelectedItem;
                    string barcodeTypeValue = (string)cmb.SelectedValue;
                    var _barcodeType = (keyValueString)cmb.SelectedItem;

                    var lst = (List<ItemUnit>)dg_itemUnit.ItemsSource;
                    if (barcodeTypeValue != "external" && barcodeTypeValue != "")
                    {
                        lst.Remove(itemUnit);
                        foreach (var row in lst)
                        {
                            if (row.UnitId == itemUnit.UnitId && row.BarcodeType == itemUnit.BarcodeType)
                            {
                                cmb.SelectedIndex = -1;
                                cmb.SelectedItem = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Cb_unit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var cmb = sender as ComboBox;
                if (dg_itemUnit.SelectedIndex != -1 && cmb != null)
                {
                    int _datagridSelectedIndex = dg_itemUnit.SelectedIndex;
                    var itemUnit = (ItemUnit)dg_itemUnit.SelectedItem ;
                    int unitId = int.Parse(cmb.SelectedValue.ToString());
                    var _unit = (Unit)cmb.SelectedItem;
                   var lst  = (List<ItemUnit>)dg_itemUnit.ItemsSource;
                    if (itemUnit.BarcodeType != "external" && itemUnit.BarcodeType != "")
                    {
                        lst.Remove(itemUnit);
                        foreach (var row in lst)
                        {
                            if (row.UnitId == unitId && row.BarcodeType == itemUnit.BarcodeType)
                            {
                                cmb.SelectedIndex = -1;
                                cmb.SelectedItem = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Tb_Factor_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
