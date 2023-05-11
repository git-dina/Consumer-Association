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
                fillBarcodeTypeCombo();

                await FillCombo.fillUnits(cb_unit);
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
            dg_itemUnit.Columns[1].Header = AppSettings.resourcemanager.GetString("BarcodeType");
            dg_itemUnit.Columns[2].Header = AppSettings.resourcemanager.GetString("Barcode");
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
                var isEmpty = itemUnits.Where(x => x.UnitId == null || x.BarcodeType == null
                                || x.Factor == 0||x.Barcode == null || x.Barcode == "").FirstOrDefault();
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
                var isEmpty = itemUnits.Where(x => x.UnitId == null || x.BarcodeType == null || x.BarcodeType ==""
                                || x.Factor == 0 || x.Barcode == null || x.Barcode =="").FirstOrDefault();
                if (isEmpty != null)
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trRowWithEmptyError"), animation: ToasterAnimation.FadeIn);
                else
                {
                    itemUnits.Add(new ItemUnit()
                    {
                        ItemId = item.ItemId,
                        Barcode = "",
                        BarcodeType = "",
                        Factor = 0,
                        UnitId = item.UnitId,
                        Cost = 0,
                        SalePrice = 0,
                        CreateUserId = MainWindow.userLogin.userId,
                        UpdateUserId = MainWindow.userLogin.userId,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                    }) ;
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
                if (cmb.IsMouseOver)
                {
                    //e.Handled = true;
                    if (dg_itemUnit.SelectedIndex != -1 && cmb != null && cmb.SelectedValue != null)
                    {
                        int _datagridSelectedIndex = dg_itemUnit.SelectedIndex;
                        var itemUnit = (ItemUnit)dg_itemUnit.SelectedItem;
                        string barcodeTypeValue = (string)cmb.SelectedValue;
                        itemUnit.BarcodeType = barcodeTypeValue;
                        var _barcodeType = (keyValueString)cmb.SelectedItem;

                        itemUnits = (List<ItemUnit>)dg_itemUnit.ItemsSource;

                        bool valid = true;
                        if (barcodeTypeValue != "external" )
                        {
                            var lst = itemUnits.Select(x => new ItemUnit()
                            {
                                Barcode = x.Barcode,
                                BarcodeType = x.BarcodeType,
                                Cost = x.Cost,
                                SalePrice = x.SalePrice,
                                Factor = x.Factor,
                                IsBlocked = x.IsBlocked,
                                ItemId = x.ItemId,
                                ItemUnitId = x.ItemUnitId,
                                UnitId = x.UnitId,
                                CreateDate = x.CreateDate,
                                UpdateDate=x.UpdateDate,
                                CreateUserId=x.CreateUserId,
                                UpdateUserId=x.UpdateUserId
                            }).ToList();
                            var u = lst.Where(x => x.UnitId == itemUnit.UnitId && x.BarcodeType == itemUnit.BarcodeType).FirstOrDefault();
                            lst.Remove(u);
                            foreach (var row in lst)
                            {
                                if (row.UnitId == itemUnit.UnitId && row.BarcodeType == itemUnit.BarcodeType)
                                {
                                    valid = false;

                                    cmb.SelectedIndex = -1;
                                    cmb.SelectedItem = null;
                                    cmb.SelectedValue = null;
                                    break;
                                }
                            }
                        }
                        if (valid)
                        {
                            itemUnit.BarcodeType = barcodeTypeValue;
                            itemUnit.Barcode = generateBarcode(barcodeTypeValue);
                        }
                        else
                        {
                            itemUnit.BarcodeType = "";
                            itemUnit.Barcode = "";
                        }
                        RefreshItemUnitDataGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private string generateBarcode(string barcodeTypeValue)
        {
            string barcode = "";
            switch (barcodeTypeValue)
            {
                case "external":
                   
                    break;
                case "internal":
                    var lst = (List<ItemUnit>)dg_itemUnit.ItemsSource;
                    var barcodeNum = lst.Where(x => x.BarcodeType == "internal").Select(x => x.Barcode).Max();
                    int num = 0;
                    if (barcodeNum != null && barcodeNum != "")
                        num = int.Parse(barcodeNum.Substring(0, 4));
                    num++;
                    barcode = "1000"+num.ToString().PadLeft(4, '0') + item.ItemId.ToString().PadLeft(4, '0');
                    break;
                case "isWeight":
                     lst = (List<ItemUnit>)dg_itemUnit.ItemsSource;
                     barcodeNum = lst.Where(x => x.BarcodeType == "isWeight").Select(x => x.Barcode).Max();
                     num = 0;
                    if (barcodeNum != null && barcodeNum != "")
                        num = int.Parse(barcodeNum);
                    num++;
                    barcode = num.ToString().PadLeft(4, '0');
                    break;
                default:
                    break;
            }
            return barcode;
        }
        private void Cb_unit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var cmb = sender as ComboBox;
                e.Handled = true;

                if (cmb.IsMouseOver)
                {
                    if (dg_itemUnit.SelectedIndex != -1 && cmb != null && cmb.SelectedValue != null)
                    {
                        int _datagridSelectedIndex = dg_itemUnit.SelectedIndex;
                        var itemUnit = (ItemUnit)dg_itemUnit.SelectedItem;
                        int unitId = int.Parse(cmb.SelectedValue.ToString());
                        itemUnit.UnitId = unitId;
                        var _unit = (Unit)cmb.SelectedItem;
                        var lst = (List<ItemUnit>)dg_itemUnit.ItemsSource;
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
                        RefreshItemUnitDataGrid();

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
            try
            {
                var txt = sender as TextBox;
                e.Handled = true;
                if (txt.IsFocused)
                {
                    int _datagridSelectedIndex = dg_itemUnit.SelectedIndex;
                    var itemUnit = (ItemUnit)dg_itemUnit.SelectedItem;

                    if (itemUnit.UnitId == item.UnitId)
                    {
                        txt.Text = item.Factor.ToString();                  
                    }
                    int factor = int.Parse(txt.Text);
                    if(factor > item.Factor)
                    {
                        txt.Text = "0";
                        factor = 0;
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trFactorMaxError")+"("+item.Factor+")", animation: ToasterAnimation.FadeIn);

                    }
                    itemUnit.Factor = factor;
                   // RefreshItemUnitDataGrid();
                }
                    
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var txt = sender as TextBox;
                e.Handled = true;
                //if (txt.IsFocused)
                {
                    int _datagridSelectedIndex = dg_itemUnit.SelectedIndex;
                    var itemUnit = (ItemUnit)dg_itemUnit.SelectedItem;

                    decimal cost = 0;
                    decimal price = 0;
                    if (itemUnit.UnitId == item.UnitId)
                    {
                        txt.Text = item.Factor.ToString();
                    }
 

                    int factor = int.Parse(txt.Text);
                    if (factor > item.Factor)
                    {
                        txt.Text = "0";
                        factor = 0;
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trFactorMaxError") + "(" + item.Factor + ")", animation: ToasterAnimation.FadeIn);

                    }
                    if (itemUnit.UnitId == item.UnitId)
                    {
                        txt.Text = item.Factor.ToString();
                        cost = item.Cost;
                        price = item.Price * (int)item.Factor;
                    }
                    else
                    {
                        cost = item.Cost / factor;
                        price = item.Price * factor;
                    }
                    itemUnit.Factor = factor;
                    itemUnit.Cost = cost;
                    itemUnit.SalePrice = price;
                    RefreshItemUnitDataGrid();
                }

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void tb_barcode_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var txt = sender as TextBox;
                e.Handled = true;
                int _datagridSelectedIndex = dg_itemUnit.SelectedIndex;
                var itemUnit = (ItemUnit)dg_itemUnit.SelectedItem;

                itemUnit.Barcode = txt.Text;

                RefreshItemUnitDataGrid();

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void chk_isBlocked_Changed(object sender, RoutedEventArgs e)
        {
            try
            {
                var chk = sender as CheckBox;
                e.Handled = true;

                int _datagridSelectedIndex = dg_itemUnit.SelectedIndex;
                var itemUnit = (ItemUnit)dg_itemUnit.SelectedItem;

                itemUnit.IsBlocked = true;
                RefreshItemUnitDataGrid();


            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void chk_isBlocked_Uncheck_Changed(object sender, RoutedEventArgs e)
        {
            try
            {
                var chk = sender as CheckBox;
                e.Handled = true;

                int _datagridSelectedIndex = dg_itemUnit.SelectedIndex;
                var itemUnit = (ItemUnit)dg_itemUnit.SelectedItem;

                itemUnit.IsBlocked = false;
                RefreshItemUnitDataGrid();


            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

       
    }
}
