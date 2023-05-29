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
    /// Interaction logic for wd_purchaseInv.xaml
    /// </summary>
    public partial class wd_purchaseInv : Window
    {

        public wd_purchaseInv()
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

        public string invoiceType { get; set; }
        public bool? isApproved { get; set; }

       public PurchaseInvoice purchaseInvoice = new PurchaseInvoice();
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
            txt_title.Text = AppSettings.resourcemanager.GetString("trOrders");
           
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_LocationId, AppSettings.resourcemanager.GetString("trBranchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_searchInvNumber, AppSettings.resourcemanager.GetString("SupplyingOrderNumHint"));

            dg_purchaseInvoice.Columns[0].Header = AppSettings.resourcemanager.GetString("OrderNum");
            dg_purchaseInvoice.Columns[1].Header = AppSettings.resourcemanager.GetString("Location");
            dg_purchaseInvoice.Columns[2].Header = AppSettings.resourcemanager.GetString("Supplier");
            dg_purchaseInvoice.Columns[3].Header = AppSettings.resourcemanager.GetString("DocumentDate");

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
                if(cb_LocationId.SelectedValue != null && tb_searchInvNumber.Text != "")
                    await searchInvoices((long)cb_LocationId.SelectedValue,tb_searchInvNumber.Text,invoiceType,isApproved);
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
        private async Task searchInvoices(long locationId, string invNumber, string invoiceType,bool? isApproved)
        {
            var invoices = await purchaseInvoice.searchOrders(locationId, invNumber,invoiceType,isApproved);
            dg_purchaseInvoice.ItemsSource = invoices;
            dg_purchaseInvoice.Items.Refresh();
        }


        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // HelpClass.StartAwait(grid_main);
                if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                {



                    isOk = true;
                    this.Close();
                }
                // HelpClass.EndAwait(grid_main);
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

        private void dg_purchaseInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                purchaseInvoice = dg_purchaseInvoice.SelectedItem as PurchaseInvoice;
                isOk = true;
                this.Close();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Dg_purchaseInvoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //purchaseInvoice = dg_purchaseInvoice.SelectedItem as PurchaseInvoice;
                //isOk = true;
                //this.Close();
            }
            catch (Exception ex)
            {
               // HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
