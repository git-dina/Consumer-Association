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

namespace POSCA.View.customers.customerTransaction
{
    /// <summary>
    /// Interaction logic for uc_retreatOfCustomer.xaml
    /// </summary>
    public partial class uc_retreatOfCustomer : UserControl
    {

        public uc_retreatOfCustomer()
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
        private static uc_retreatOfCustomer _instance;
        public static uc_retreatOfCustomer Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_retreatOfCustomer();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        CustomerTransaction customerTransaction = new CustomerTransaction();
        Customer customer = new Customer();
        List<CustomerTransaction> customerTransactions;
        string searchText = "";
        public static List<string> requiredControlList;

        public void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "CustomerId", "TransactionDate", "StocksCount", "ApprovalNumber", "MeetingDate", "CheckNumber", "CheckDate" };
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


                Keyboard.Focus(dp_TransactionDate);


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

            txt_title.Text = AppSettings.resourcemanager.GetString("AddStockes");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            txt_search.Text = AppSettings.resourcemanager.GetString("trSearch");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_TransactionDate, AppSettings.resourcemanager.GetString("TransactionDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerId, AppSettings.resourcemanager.GetString("CustomerNoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_JoinDate, AppSettings.resourcemanager.GetString("JoinDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerName, AppSettings.resourcemanager.GetString("CustomerNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_TransactionStocksCount, AppSettings.resourcemanager.GetString("TransactionStocksCountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_StocksCount, AppSettings.resourcemanager.GetString("StocksCountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_StocksPrice, AppSettings.resourcemanager.GetString("StocksPriceHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_TotalPrice, AppSettings.resourcemanager.GetString("TotalPriceHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ApprovalNumber, AppSettings.resourcemanager.GetString("BoardApprovalNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_MeetingDate, AppSettings.resourcemanager.GetString("SessionDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CheckNumber, AppSettings.resourcemanager.GetString("CheckNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_CheckDate, AppSettings.resourcemanager.GetString("CheckDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            txt_durationJoin.Text = AppSettings.resourcemanager.GetString("DurationJoin");

            txt_JoinDayTitle.Text = AppSettings.resourcemanager.GetString("trDay");
            txt_JoinMonthTitle.Text = AppSettings.resourcemanager.GetString("trMonth");
            txt_JoinYearTitle.Text = AppSettings.resourcemanager.GetString("trYear");


            dg_customerTransaction.Columns[0].Header = AppSettings.resourcemanager.GetString("TransactionDate");
            dg_customerTransaction.Columns[1].Header = AppSettings.resourcemanager.GetString("CustomerNo");
            dg_customerTransaction.Columns[2].Header = AppSettings.resourcemanager.GetString("CustomerName");
            dg_customerTransaction.Columns[3].Header = AppSettings.resourcemanager.GetString("StocksCount");
            dg_customerTransaction.Columns[4].Header = AppSettings.resourcemanager.GetString("StocksPrice");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");

            //txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trSave");
            //txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                /*
                  {
                      HelpClass.StartAwait(grid_main);

                      customerTransaction = new CustomerTransaction();
                      if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                      {

                          customerTransaction.Name = tb_Name.Text;
                          customerTransaction.Notes = tb_Notes.Text;

                          customerTransaction.CreateUserId = MainWindow.userLogin.userId;

                          FillCombo.customerTransactionList = await customerTransaction.save(customerTransaction);
                          if (FillCombo.customerTransactionList == null)
                              Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                          else
                          {
                              Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                              await Clear();
                              await Search();
                          }
                      }
                      else
                      {
                          Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("saveNotDoneEmptyFields"), animation: ToasterAnimation.FadeIn);
                      }
                      HelpClass.EndAwait(grid_main);
                  }
                  */
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


                    if (HelpClass.validate(requiredControlList, this))
                    {
                        customerTransaction.TransactionType = "add";
                        customerTransaction.CustomerId = long.Parse(tb_CustomerId.Text);
                        customerTransaction.TransactionDate = dp_TransactionDate.SelectedDate;
                        customerTransaction.StocksCount = int.Parse(tb_StocksCount.Text);
                        customerTransaction.StocksPrice = decimal.Parse(tb_StocksPrice.Text);
                        customerTransaction.TotalPrice = decimal.Parse(tb_TotalPrice.Text);
                        customerTransaction.ApprovalNumber = tb_ApprovalNumber.Text;
                        customerTransaction.MeetingDate = dp_MeetingDate.SelectedDate;
                        customerTransaction.CheckNumber = tb_CheckNumber.Text;
                        customerTransaction.CheckDate = dp_CheckDate.SelectedDate;
                        customerTransaction.Notes = tb_Notes.Text;

                        customerTransaction.UpdateUserId = MainWindow.userLogin.userId;

                        var res = await customerTransaction.RetreatTransaction(customerTransaction);
                        if (res == 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                            Clear();
                            //await Search();

                        }
                    }
                    else
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("saveNotDoneEmptyFields"), animation: ToasterAnimation.FadeIn);
                    }


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
                    /*
                    if (customerTransaction.CustomerTransactionId != 0)
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
                            FillCombo.customerTransactionList = await customerTransaction.delete(customerTransaction.CustomerTransactionId, MainWindow.userLogin.userId);
                            if (FillCombo.customerTransactionList == null)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                customerTransaction.CustomerTransactionId = 0;
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                await Search();
                                await Clear();
                            }
                        }

                    }
                    */
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
                //if (tb_search.Text != "")
                //{
                //dina search
                //suppliers = await FillCombo.supplier.searchSuppliers(tb_search.Text);
                //RefreshSuppliersView();

                await Search();


                //}
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

        private async void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                await Clear();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void Dg_customerTransaction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection

                if (dg_customerTransaction.SelectedIndex != -1)
                {
                    customerTransaction = dg_customerTransaction.SelectedItem as CustomerTransaction;
                    this.DataContext = customerTransaction;
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

                // await RefreshCustomerTransactionsList();
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
            customerTransactions = await customerTransaction.SearchTransactions("retreat",tb_search.Text);
            RefreshCustomerTransactionsView();
        }
        //async Task<IEnumerable<CustomerTransaction>> RefreshCustomerTransactionsList()
        //{

        //    await customerTransaction.get(true);

        //    return FillCombo.customerTransactionList;

        //    return customerTransactions;
        //}
        void RefreshCustomerTransactionsView()
        {
            dg_customerTransaction.ItemsSource = customerTransactions;
            txt_count.Text = customerTransactions.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        async Task Clear()
        {
            this.DataContext = new CustomerTransaction();
            dg_customerTransaction.SelectedIndex = -1;

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

            if (tb_search.Text != "")
                Btn_search_Click(null, null);
            else
            {
                customerTransactions = new List<CustomerTransaction>();
                RefreshCustomerTransactionsView();
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

        private void Btn_archiving_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_updateIBAN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_personalDocuments_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_pastProfits_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void tb_CustomerId_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return && tb_CustomerId.Text != "")
                {
                    if (FillCombo.customerList != null)
                    {
                        customer = FillCombo.customerList.Where(x => x.CustomerId == long.Parse(tb_CustomerId.Text)).FirstOrDefault();
                    }

                    if (customer == null)
                        customer = await FillCombo.customer.GetById(long.Parse(tb_CustomerId.Text));

                    if (customer != null)
                    {
                        if (customer.CustomerStatus != "continouse")
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CustomerNotContinouse"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            tb_CustomerName.Text = customer.Name;
                            dp_JoinDate.SelectedDate = customer.JoinDate;
                            tb_StocksCount.Text = customer.AllStocksCount.ToString();
                            txt_JoinDay.Text = customer.JoinDay.ToString();
                            txt_JoinMonth.Text = customer.JoinMonth.ToString();
                            txt_JoinYear.Text = customer.JoinYear.ToString();

                        }
                    }
                    else
                    {
                        tb_CustomerId.Text = "";
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("NumberNotTrue"), animation: ToasterAnimation.FadeIn);
                    }

                }
            }
            catch { }
        }
    }
}
