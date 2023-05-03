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

        public List<ItemAllowedTransaction> ItemAllowedTransactions { get; set; }
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
            txt_title.Text = AppSettings.resourcemanager.GetString("Generalization");
            txt_itemAllowedTransaction.Text = AppSettings.resourcemanager.GetString("Generalization");

            //dg_itemAllowedTransaction.Columns[0].Header = AppSettings.resourcemanager.GetString("Year");
            //dg_itemAllowedTransaction.Columns[1].Header = AppSettings.resourcemanager.GetString("GeneralizationNumber");

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

        }


        private void setItemAllowedTransactionData()
        {
            dg_itemAllowedTransaction.ItemsSource = ItemAllowedTransactions;
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
                ItemAllowedTransactions = (List<ItemAllowedTransaction>)dg_itemAllowedTransaction.ItemsSource;

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

        }

        private void tgl_selectAll_Unchecked(object sender, RoutedEventArgs e)
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
