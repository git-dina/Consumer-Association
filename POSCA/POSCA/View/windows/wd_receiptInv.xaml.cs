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
    /// Interaction logic for wd_receiptInv.xaml
    /// </summary>
    public partial class wd_receiptInv : Window
    {

        public wd_receiptInv()
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

        public bool isOk { get; set; }
        public string invType { get; set; }


        public Receipt receipt = new Receipt();
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load

            try
            {


                HelpClass.StartAwait(grid_main);



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

                await FillCombo.fillLocations(cb_LocationId);
                FillCombo.fillReceiptsTypes(cb_ReceiptType);
                if (invType == "return")
                {
                    col_receiptType.Visibility = Visibility.Collapsed;
                    brd_receiptType.Visibility = Visibility.Collapsed;
                }
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
            txt_title.Text = AppSettings.resourcemanager.GetString("Receipts");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_LocationId, AppSettings.resourcemanager.GetString("trBranchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_searchInvNumber, AppSettings.resourcemanager.GetString("OrderNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_ReceiptType, AppSettings.resourcemanager.GetString("ReceiptTypeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_FromDate, AppSettings.resourcemanager.GetString("trFrom"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_ToDate, AppSettings.resourcemanager.GetString("trTo"));
          
            dg_receipt.Columns[0].Header = AppSettings.resourcemanager.GetString("ReceiptType");
            dg_receipt.Columns[1].Header = AppSettings.resourcemanager.GetString("OrderNum");
            dg_receipt.Columns[2].Header = AppSettings.resourcemanager.GetString("Location");
            dg_receipt.Columns[3].Header = AppSettings.resourcemanager.GetString("DocumentDate");

            btn_search.Content = AppSettings.resourcemanager.GetString("trSearch");
            btn_save.Content = AppSettings.resourcemanager.GetString("trSelect");
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
        private async void Btn_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cb_LocationId.SelectedValue != null)
                {
                    string receiptType = "";
                    if (cb_ReceiptType.SelectedValue != null)
                        receiptType = cb_ReceiptType.SelectedValue.ToString();

                    await searchInvoices((long)cb_LocationId.SelectedValue, tb_searchInvNumber.Text, invType,
                                       receiptType, dp_FromDate.SelectedDate, dp_ToDate.SelectedDate);
                }
            }
            catch
            {

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
        private async Task searchInvoices(long locationId, string invNumber,string invType,string receiptType=null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
              
                HelpClass.StartAwait(grid_main);
                var invoices = await receipt.searchOrders(locationId, invNumber, invType, receiptType, fromDate, toDate);
                dg_receipt.ItemsSource = invoices;
                dg_receipt.Items.Refresh();
                HelpClass.EndAwait(grid_main);
              
            }
            catch
            {
                HelpClass.EndAwait(grid_main);
            }
        }


        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (dg_receipt.SelectedIndex > -1)
                {
                    receipt = dg_receipt.SelectedItem as Receipt;
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

        #endregion

        #region validate - clearValidate - textChange - lostFocus - . . . . 

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

        private void dg_receipt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                receipt = dg_receipt.SelectedItem as Receipt;
                isOk = true;
                this.Close();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Dg_receipt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //receipt = dg_receipt.SelectedItem as Receipt;
                //isOk = true;
                //this.Close();
            }
            catch (Exception ex)
            {
                // HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void dp_FromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dp_FromDate.SelectedDate != null && dp_ToDate.SelectedDate != null)
                    if (dp_FromDate.SelectedDate.Value.Date > dp_ToDate.SelectedDate.Value.Date)
                        dp_ToDate.SelectedDate = dp_FromDate.SelectedDate.Value.Date;

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
