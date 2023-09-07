using Microsoft.Win32;
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
    /// Interaction logic for wd_itemAllowedOperations.xaml
    /// </summary>
    public partial class wd_itemAllowedOperations : Window
    {

        public wd_itemAllowedOperations()
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
        List<ItemAllowedTransaction> listItemAllowedTransaction = new List<ItemAllowedTransaction>();
        List<ItemLocation> listItemLocations = new List<ItemLocation>();

        public List<ItemAllowedTransaction> itemAllowedTransactions { get; set; }
        public List<ItemLocation> itemLocations { get; set; }
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

                setItemAllowedTransactionData();
                await setItemLocationsData();

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
          
            txt_title.Text = AppSettings.resourcemanager.GetString("ItemTransactions");
            txt_itemAllowedTransaction.Text = AppSettings.resourcemanager.GetString("ItemTransactions");
            txt_selectAll.Text = AppSettings.resourcemanager.GetString("SelectAll");

            txt_itemLocation.Text = AppSettings.resourcemanager.GetString("ItemLocations");
            txt_connectAllLocation.Text = AppSettings.resourcemanager.GetString("ConnectAllLocations");
            txt_connectAllMarketsAndBranches.Text = AppSettings.resourcemanager.GetString("ConnectAllMarketsAndBranches");
         
            //dg_itemAllowedTransaction.Columns[0].Header = AppSettings.resourcemanager.GetString("Year");
            //dg_itemAllowedTransaction.Columns[1].Header = AppSettings.resourcemanager.GetString("GeneralizationNumber");

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

        }
        private void setItemAllowedTransactionData()
        {
            if (FillCombo.itemTransactionsList == null)
                FillCombo.RefreshItemTransactionsList();

            foreach(var row in FillCombo.itemTransactionsList)
            {
                var isAllowed = false;
                long id = 0;
                if (itemAllowedTransactions != null)
                {
                    var selected = itemAllowedTransactions.Where(x => x.Transaction == row.key).FirstOrDefault();

                    if (selected != null)
                    {
                        isAllowed = true;
                        id = selected.Id;
                    }
                }
                listItemAllowedTransaction.Add(new ItemAllowedTransaction()
                {
                    Id = id,
                    Transaction = row.key,
                    TransactionText = row.value,
                    IsAllowed = isAllowed,
                }); ;

            }
            dg_itemAllowedTransaction.ItemsSource = listItemAllowedTransaction;

        }

       private async Task setItemLocationsData()
        {
            if (FillCombo.locationsList == null)
                await FillCombo.RefreshLocations();

            foreach(var row in FillCombo.locationsList)
            {
                var isAllowed = false;
                long itemLocationId = 0;
                if (itemLocations != null)
                {
                    var selected = itemLocations.Where(x => x.LocationId == row.LocationId).FirstOrDefault();

                    if (selected != null)
                    {
                        isAllowed = true;
                        itemLocationId = selected.ItemLocationId;
                    }
                }
                listItemLocations.Add(new ItemLocation()
                {
                    ItemLocationId = itemLocationId,
                    LocationId = row.LocationId,
                    LocationName = row.Name,
                    IsAllowed = isAllowed,
                });

            }
            dg_itemLocation.ItemsSource = listItemLocations;

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

        #endregion

        #region validate - clearValidate - textChange - lostFocus - . . . . 
        /*
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
        */
        #endregion
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

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // HelpClass.StartAwait(grid_main);
                listItemAllowedTransaction = (List<ItemAllowedTransaction>)dg_itemAllowedTransaction.ItemsSource;

                itemAllowedTransactions = new List<ItemAllowedTransaction>();
                foreach(var row in listItemAllowedTransaction)
                {
                    if (row.IsAllowed)
                    {
                        row.CreateUserId = MainWindow.userLogin.UserId;
                        itemAllowedTransactions.Add(row);
                    }
                }

                listItemLocations = (List<ItemLocation>)dg_itemLocation.ItemsSource;

                itemLocations = new List<ItemLocation>();
                foreach (var row in listItemLocations)
                {
                    if (row.IsAllowed)
                    {
                        row.CreateUserId = MainWindow.userLogin.UserId;
                        itemLocations.Add(row);
                    }
                }

                isOk = true;
                this.Close();
                // HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void tgl_selectAll_Checked(object sender, RoutedEventArgs e)
        {
            listItemAllowedTransaction = (List<ItemAllowedTransaction>)dg_itemAllowedTransaction.ItemsSource;

            foreach (var row in listItemAllowedTransaction)
            {
                row.IsAllowed = true;
            }
            dg_itemAllowedTransaction.ItemsSource = listItemAllowedTransaction;
            dg_itemAllowedTransaction.Items.Refresh();
        }


        private void tgl_connectAllLocation_Checked(object sender, RoutedEventArgs e)
        {
            tgl_connectAllMarketsAndBranches.IsChecked = false;
            foreach (var row in listItemLocations)
            {
                if (row.LocationName.Contains("موقع"))
                    row.IsAllowed = true;
                else
                    row.IsAllowed = false;
            }
            dg_itemLocation.ItemsSource = listItemLocations;
            dg_itemLocation.Items.Refresh();
        }

        private void tgl_connectAllLocation_Unchecked(object sender, RoutedEventArgs e)
        {
            
        }

        private void tgl_connectAllMarketsAndBranches_Checked(object sender, RoutedEventArgs e)
        {
            tgl_connectAllLocation.IsChecked = false;
            foreach (var row in listItemLocations)
            {
                if (row.LocationName.Contains("سوق") || row.LocationName.Contains("فرع"))
                    row.IsAllowed = true;
                else
                    row.IsAllowed = false;
            }
            dg_itemLocation.ItemsSource = listItemLocations;
            dg_itemLocation.Items.Refresh();
        }

        private void tgl_connectAllMarketsAndBranches_Unchecked(object sender, RoutedEventArgs e)
        {

        }
        /*
private void Btn_addItemAllowedTransaction_Click(object sender, RoutedEventArgs e)
{
try
{
btn_addItemAllowedTransaction.IsEnabled = false;
dg_itemAllowedTransaction.IsEnabled = false;
listItemAllowedTransaction.Add(new ItemAllowedTransaction());
RefreshItemAllowedTransactionDataGrid();
}
catch (Exception ex)
{
btn_addItemAllowedTransaction.IsEnabled = true;
dg_itemAllowedTransaction.IsEnabled = true;
HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
}
}
void deleteItemAllowedTransactionRowinDatagrid(object sender, RoutedEventArgs e)
{
try
{
HelpClass.StartAwait(grid_main);

for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
if (vis is DataGridRow)
{

btn_addItemAllowedTransaction.IsEnabled = false;
dg_itemAllowedTransaction.IsEnabled = false;
ItemAllowedTransaction row = (ItemAllowedTransaction)dg_itemAllowedTransaction.SelectedItems[0];
listItemAllowedTransaction.Remove(row);
RefreshItemAllowedTransactionDataGrid();
}

HelpClass.EndAwait(grid_main);
}
catch (Exception ex)
{
btn_addItemAllowedTransaction.IsEnabled = true;
dg_itemAllowedTransaction.IsEnabled = true;
HelpClass.EndAwait(grid_main);
HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
}
}
public void RefreshItemAllowedTransactionDataGrid()
{
try
{
dg_itemAllowedTransaction.CancelEdit();
dg_itemAllowedTransaction.ItemsSource = listItemAllowedTransaction;
dg_itemAllowedTransaction.Items.Refresh();

dg_itemAllowedTransaction.IsEnabled = true;
btn_addItemAllowedTransaction.IsEnabled = true;
}
catch (Exception ex)
{
dg_itemAllowedTransaction.IsEnabled = true;
btn_addItemAllowedTransaction.IsEnabled = true;
HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
}
}
*/
    }
}
