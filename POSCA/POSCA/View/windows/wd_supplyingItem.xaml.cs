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

namespace POSCA.View.windows
{
    /// <summary>
    /// Interaction logic for wd_supplyingItem.xaml
    /// </summary>
    public partial class wd_supplyingItem : Window
    {

        public wd_supplyingItem()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name); }
        }

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            isOk = false;
            this.Close();
        }

        public string itemStatus { get; set; }
        public string itemRecieptType { get; set; }
        public string itemType { get; set; }
        public string itemTransactionType { get; set; }
        public decimal? packageWeight { get; set; }
        public long? packageUnit { get; set; }
        public bool isOk { get; set; }
        public static List<string> requiredControlList;

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load

            try
            {


                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "ItemStatus", "ItemReceiptType", "ItemType",
                                            "ItemTransactionType" ,"PackageWeight","PackageUnit"};

                #region translate

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
                #endregion

                FillCombo.fillItemStatus(cb_ItemStatus);
                FillCombo.fillItemRecieptType(cb_ItemReceiptType);
                FillCombo.fillItemType(cb_ItemType);
                FillCombo.fillItemTransTypes(cb_ItemTransactionType);
                await FillCombo.fillUnits(cb_PackageUnit);

                setSupplyingItemData();
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
            txt_title.Text = AppSettings.resourcemanager.GetString("ExtraInformation");
           txt_itemTransaction.Text = AppSettings.resourcemanager.GetString("ItemTransaction");
           txt_packageInformation.Text = AppSettings.resourcemanager.GetString("PackageInformation");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_ItemStatus, AppSettings.resourcemanager.GetString("ItemStatusHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_ItemReceiptType, AppSettings.resourcemanager.GetString("ItemReceiptTypeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_ItemType, AppSettings.resourcemanager.GetString("ItemTypeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_ItemTransactionType, AppSettings.resourcemanager.GetString("ItemTransactionTypeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_PackageWeight, AppSettings.resourcemanager.GetString("PackageWeightHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_PackageUnit, AppSettings.resourcemanager.GetString("PackageUnitHint"));

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

        }

        private void setSupplyingItemData()
        {
            cb_ItemReceiptType.SelectedValue = itemRecieptType;
            cb_ItemStatus.SelectedValue = itemStatus;
            cb_ItemType.SelectedValue = itemType;
            //if (itemTransactionType == null || itemTransactionType.Equals("")) 
            //    cb_ItemTransactionType.SelectedValue = "new_committee";
            //else
                cb_ItemTransactionType.SelectedValue = itemTransactionType;

            cb_PackageUnit.SelectedValue = packageUnit;
            tb_PackageWeight.Text = packageWeight.ToString();
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


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
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
        #region events
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
        private  void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                itemRecieptType = cb_ItemReceiptType.SelectedValue.ToString();
                itemStatus = cb_ItemStatus.SelectedValue.ToString();
                itemType = cb_ItemType.SelectedValue.ToString();
                itemTransactionType = cb_ItemTransactionType.SelectedValue.ToString();
                if (!tb_PackageWeight.Text.Equals(""))
                    packageWeight = decimal.Parse(tb_PackageWeight.Text);
                if(cb_PackageUnit.SelectedIndex != -1)
                    packageUnit = int.Parse(cb_PackageUnit.SelectedValue.ToString());
               

                isOk = true;
                this.Close();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
