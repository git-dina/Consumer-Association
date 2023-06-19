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
    /// Interaction logic for wd_addPurchaseItems.xaml
    /// </summary>
    public partial class wd_addPromotionItems : Window
    {

        public wd_addPromotionItems()
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
        public List<Item> items { get; set; }
        public Item item { get; set; }
        public List<long?> locationsId { get; set; }
        public string itemsFor { get; set; }
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

                RefreshItemsView();

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

            dg_item.Columns[0].Header = AppSettings.resourcemanager.GetString("ItemNumber");
            dg_item.Columns[1].Header = AppSettings.resourcemanager.GetString("SupplierNumber");
            dg_item.Columns[2].Header = AppSettings.resourcemanager.GetString("trName");
            dg_item.Columns[3].Header = AppSettings.resourcemanager.GetString("Factor");
            dg_item.Columns[4].Header = AppSettings.resourcemanager.GetString("trPrice");
            dg_item.Columns[5].Header = AppSettings.resourcemanager.GetString("Cost");
            dg_item.Columns[6].Header = AppSettings.resourcemanager.GetString("Category");

        }
        void RefreshItemsView()
        {
            dg_item.ItemsSource = items;
            dg_item.Items.Refresh();
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

                if (tb_search.Text != "")
                {
                    items = await FillCombo.item.GetPromotionItemByCodeOrName(tb_search.Text, locationsId);
                    RefreshItemsView();
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


        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (dg_item.SelectedIndex > -1)
                {
                    item = dg_item.SelectedItem as Item;
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

       
        private void Dg_item_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                /*
                purchaseInvoice = dg_item.SelectedItem as PurchaseInvoice;
                isOk = true;
                this.Close();
                */
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void dg_item_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            item = dg_item.SelectedItem as Item;
            isOk = true;
            this.Close();
        }
    }
}
