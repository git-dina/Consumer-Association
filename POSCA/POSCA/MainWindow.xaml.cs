﻿using POSCA.Classes;
using POSCA.Classes.ApiClasses;
using POSCA.View.catalog;
using POSCA.View.customers;
using POSCA.View.customers.activities;
using POSCA.View.customers.customerSectionData;
using POSCA.View.customers.customerTransaction;
using POSCA.View.customerTransactions.customerTransactionTransaction;
using POSCA.View.locations;
using POSCA.View.promotion;
using POSCA.View.purchases;
using POSCA.View.receipts;
using POSCA.View.sales;
using POSCA.View.sectionData;
using POSCA.View.sectionData.vendors;
using POSCA.View.settings;
using POSCA.View.usersManagement;
using POSCA.View.windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace POSCA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static User userLogin = new User();
        internal static Pos posLogin;
        internal static Branch branchLogin;

        public static DispatcherTimer timer;

        static public MainWindow mainWindow;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
            mainWindow = this;
            windowFlowDirection();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
        }
        void windowFlowDirection()
        {
            #region translate
            if (AppSettings.lang.Equals("en"))
            {
                AppSettings.resourcemanager = new ResourceManager("POSCA.en_file", Assembly.GetExecutingAssembly());
                grid_mainWindow.FlowDirection = FlowDirection.LeftToRight;
            }
            else
            {
                AppSettings.resourcemanager = new ResourceManager("POSCA.ar_file", Assembly.GetExecutingAssembly());

                grid_mainWindow.FlowDirection = FlowDirection.RightToLeft;
            }
            #endregion
        }
        public async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_mainWindow, "mainWindow_loaded");
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timer.Start();


                if (AppSettings.lang.Equals("en"))
                {
                    AppSettings.resourcemanager = new ResourceManager("POSCA.en_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                    txt_lang.Text = "AR";

                }
                else
                {
                    AppSettings.resourcemanager = new ResourceManager("POSCA.ar_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                    txt_lang.Text = "EN";

                }
                translate();

                //should be moved to login page
                await FillCombo.RefreshCompanySettings();

                txt_userName.Text = userLogin.UserName;
                if (userLogin.RoleId != null)
                {
                    if (AppSettings.lang == "ar")
                        txt_userJob.Text = userLogin.userRole.NameAr;
                    else
                        txt_userJob.Text = userLogin.userRole.NameEn;
                }
                else
                    txt_userJob.Text = AppSettings.resourcemanager.GetString("Admin");
                #region Permision
                permission();
                #endregion

               

                //SelectAllText
                EventManager.RegisterClassHandler(typeof(System.Windows.Controls.TextBox), System.Windows.Controls.TextBox.GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText));
                txt_rightReserved.Text = DateTime.Now.Date.Year + " © All Right Reserved for ";

                HelpClass.EndAwait(grid_mainWindow, "mainWindow_loaded");
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_mainWindow, "mainWindow_loaded");
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (timer != null)
                timer.Stop();
        }

        void SelectAllText(object sender, RoutedEventArgs e)
        {
            try
            {
                var textBox = sender as System.Windows.Controls.TextBox;
                if (textBox != null)
                    if (!textBox.IsReadOnly)
                        textBox.SelectAll();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        void permission()
        {
            
            if (!HelpClass.isAdminPermision())
                foreach (Button button in FindControls.FindVisualChildren<Button>(this).Where(x=>x.Tag != null))
                {
                    if (button.Tag != null)
                        if (MainWindow.userLogin.userRole.Permissions
                            .Where(x => x.AppObject == button.Tag.ToString()
                        && x.ViewObject == true).Count() != 0)
                            button.Visibility = Visibility.Visible;
                        else
                            button.Visibility = Visibility.Collapsed;
                }

            bool isChanged = false;
            do
            {
                isChanged = false;
                // to hide empty expander
                foreach (Expander expander in FindControls.FindVisualChildren<Expander>(this).Where(x=> x.IsVisible))
                {
                    if (expander.Content.GetType().Name == "StackPanel")
                    {
                        var stackPanel = expander.Content as StackPanel;
                        if (stackPanel.Tag.ToString() == "expanderContent")
                        { 
                            // to solve proplem: stack has two Type Button and Expander
                            try
                            {
                                bool containVisible = false;
                                foreach (Button item2 in stackPanel.Children.Cast<Object>().Where(x => x.GetType().Name == "Button"))
                                {
                                    if (item2.IsVisible)
                                        containVisible = true;
                                }
                                foreach (Expander item2 in stackPanel.Children.Cast<Object>().Where(x => x.GetType().Name == "Expander"))
                                {
                                    if (item2.IsVisible)
                                        containVisible = true;
                                }

                                if (!containVisible)
                                {
                                    expander.Visibility = Visibility.Collapsed;
                                    isChanged = true;
                                }
                            }
                            catch
                            {

                            }
                            
                        }
                    }
                }
            }
            // return if we collapsed any thing 
            while (isChanged);

            // load window
            if (btn_salesInvoice.IsVisible)
                Btn_salesInvoice_Click(btn_salesInvoice, null);
        }
        void timer_Tick(object sender, EventArgs e)
        {
            try
            {

                txtTime.Text = DateTime.Now.ToShortTimeString();
                txtDate.Text = DateTime.Now.ToShortDateString();


            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void BTN_Close_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_mainWindow);

                {
                    //await close();

                    //HelpClass.deleteDirectoryFiles(Global.TMPFolder);

                    Application.Current.Shutdown();
                }


                HelpClass.EndAwait(grid_mainWindow);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_mainWindow);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void BTN_Minimize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowState = System.Windows.WindowState.Minimized;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /*
        void colorTextRefreash(TextBlock txt)
        {
            txt_home.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_catalog.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_storage.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_purchases.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_sales.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_kitchen.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_delivery.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_accounts.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_reports.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_sectiondata.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_settings.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));

            txt.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF"));
        }
        void fn_ColorIconRefreash(Path p)
        {
            path_iconSettings.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconSectionData.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconReports.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconAccounts.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconSales.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconKitchen.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconDelivery.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconPurchases.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconStorage.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconCatalog.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconHome.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));

            p.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF"));
        }
        */
        public void translate()
        {
             txt_dashboard.Text = AppSettings.resourcemanager.GetString("trDashBoard");
            txt_salesInvoice.Text = AppSettings.resourcemanager.GetString("trSales");

            #region Home
            txt_mainSection.Text = AppSettings.resourcemanager.GetString("trHome");
            txt_catalog.Text = AppSettings.resourcemanager.GetString("trCatalog");
            txt_category.Text = AppSettings.resourcemanager.GetString("Categories");
            txt_item.Text = AppSettings.resourcemanager.GetString("Items");
            //txt_unit.Text = AppSettings.resourcemanager.GetString("Units");

            txt_purchases.Text = AppSettings.resourcemanager.GetString("trPurchase");
            txt_purchaseOrder.Text = AppSettings.resourcemanager.GetString("ProcurementRequests");
            txt_purchaseInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaseOrders");
           
            txt_receipts.Text = AppSettings.resourcemanager.GetString("Receipts");
            txt_receiptInvoice.Text = AppSettings.resourcemanager.GetString("Receipt");
            txt_returnReceiptInv.Text = AppSettings.resourcemanager.GetString("Returns");


            txt_promotion.Text = AppSettings.resourcemanager.GetString("Offers");
            txt_promotionInvoice.Text = AppSettings.resourcemanager.GetString("PromotionalOffers");
            txt_contributorsPromotion.Text = AppSettings.resourcemanager.GetString("ContributorsPromotions");

            txt_sectionData.Text = AppSettings.resourcemanager.GetString("trSectionData");
            txt_phoneType.Text = AppSettings.resourcemanager.GetString("PhonesTypes");
            txt_bank.Text = AppSettings.resourcemanager.GetString("trBanks");
            txt_country.Text = AppSettings.resourcemanager.GetString("Countries");
            txt_brand.Text = AppSettings.resourcemanager.GetString("Brands");

            txt_locations.Text = AppSettings.resourcemanager.GetString("Locations");
            txt_locationsData.Text = AppSettings.resourcemanager.GetString("LocationsData");
            txt_locationType.Text = AppSettings.resourcemanager.GetString("LocationsType");
            
            txt_vendors.Text = AppSettings.resourcemanager.GetString("Suppliers");
            txt_vendorsData.Text = AppSettings.resourcemanager.GetString("SuppliersData");
            txt_vendorsGroups.Text = AppSettings.resourcemanager.GetString("SuppliersGroups");
            txt_vendorsType.Text = AppSettings.resourcemanager.GetString("SuppliersTypes");
            txt_supportVendors.Text = AppSettings.resourcemanager.GetString("AssistantSuppliers");
            txt_supplierDocType.Text = AppSettings.resourcemanager.GetString("SupplierDocTypes");

            #endregion

            #region customers
            txt_customers.Text = AppSettings.resourcemanager.GetString("Customers");
            txt_familyCard.Text = AppSettings.resourcemanager.GetString("FamilyCard");
            txt_fundChange.Text = AppSettings.resourcemanager.GetString("FundNumberChange");

            txt_customerSectionData.Text = AppSettings.resourcemanager.GetString("trBasicData");
            txt_customerArea.Text = AppSettings.resourcemanager.GetString("trAreas");
            txt_job.Text = AppSettings.resourcemanager.GetString("trJobs");
            txt_kinshipTies.Text = AppSettings.resourcemanager.GetString("trkinshipTies");
            txt_customerBank.Text = AppSettings.resourcemanager.GetString("CustomerBanks");
            txt_hirarachyStructure.Text = AppSettings.resourcemanager.GetString("AdminstrativeStructure");
            txt_customerData.Text = AppSettings.resourcemanager.GetString("customersData");

            txt_customerTransaction.Text = AppSettings.resourcemanager.GetString("CustomersTransactions");
            txt_addStocks.Text = AppSettings.resourcemanager.GetString("AddStockes");
            txt_reduceStocks.Text = AppSettings.resourcemanager.GetString("ReduceStocks");
            txt_transformStocks.Text = AppSettings.resourcemanager.GetString("TransformStocks");
            txt_retreatOfCustomer.Text = AppSettings.resourcemanager.GetString("Retreat");
            txt_deathOfCustomer.Text = AppSettings.resourcemanager.GetString("Death");


            txt_customerActivities.Text = AppSettings.resourcemanager.GetString("ContributorActivities");
            txt_activitiesType.Text = AppSettings.resourcemanager.GetString("ActivitiesTypes");
            txt_activities.Text = AppSettings.resourcemanager.GetString("Activities");
            txt_subscriptionRequest.Text = AppSettings.resourcemanager.GetString("SubscriptionRequest");
            txt_activitiesReport.Text = AppSettings.resourcemanager.GetString("ActivitiesReport");
            #endregion

            txt_settings.Text = AppSettings.resourcemanager.GetString("Settings"); 
          txt_generalSettings.Text = AppSettings.resourcemanager.GetString("GeneralSettings");

            #region usersManagement
            txt_usersManagement.Text = AppSettings.resourcemanager.GetString("UsersManagement");
            txt_user.Text = AppSettings.resourcemanager.GetString("trUsers");
            txt_permissions.Text = AppSettings.resourcemanager.GetString("trPermissions");
            #endregion
            /*
            tt_menu.Content = AppSettings.resourcemanager.GetString("trMenu");
            tt_home.Content = AppSettings.resourcemanager.GetString("trHome");
            tt_catalog.Content = AppSettings.resourcemanager.GetString("trCatalog");
            txt_catalog.Text = AppSettings.resourcemanager.GetString("trCatalog");
            tt_storage.Content = AppSettings.resourcemanager.GetString("trStore");
            txt_storage.Text = AppSettings.resourcemanager.GetString("trStore");
            tt_purchase.Content = AppSettings.resourcemanager.GetString("trPurchases");
            txt_purchases.Text = AppSettings.resourcemanager.GetString("trPurchases");
            tt_kitchen.Content = AppSettings.resourcemanager.GetString("trKitchen");
            txt_kitchen.Text = AppSettings.resourcemanager.GetString("trKitchen");
            tt_delivery.Content = AppSettings.resourcemanager.GetString("trDelivery");
            txt_delivery.Text = AppSettings.resourcemanager.GetString("trDelivery");
            tt_sales.Content = AppSettings.resourcemanager.GetString("trSales");
            txt_sales.Text = AppSettings.resourcemanager.GetString("trSales");
            tt_accounts.Content = AppSettings.resourcemanager.GetString("trAccounting");
            txt_accounts.Text = AppSettings.resourcemanager.GetString("trAccounting");
            tt_reports.Content = AppSettings.resourcemanager.GetString("trReports");
            txt_reports.Text = AppSettings.resourcemanager.GetString("trReports");
            tt_sectionData.Content = AppSettings.resourcemanager.GetString("trSectionData");
            txt_sectiondata.Text = AppSettings.resourcemanager.GetString("trSectionData");
            tt_settings.Content = AppSettings.resourcemanager.GetString("trSettings");
            txt_settings.Text = AppSettings.resourcemanager.GetString("trSettings");
            txt_cashTitle.Text = AppSettings.resourcemanager.GetString("trBalance");

            mi_refreshLoading.Header = AppSettings.resourcemanager.GetString("trRefresh");
            mi_changePassword.Header = AppSettings.resourcemanager.GetString("trChangePassword");
            mi_aboutUs.Header = AppSettings.resourcemanager.GetString("trAboutUs");
            BTN_logOut.Header = AppSettings.resourcemanager.GetString("trLogOut");

            txt_notifications.Text = AppSettings.resourcemanager.GetString("trNotifications");
            txt_noNoti.Text = AppSettings.resourcemanager.GetString("trNoNotifications");
            btn_showAll.Content = AppSettings.resourcemanager.GetString("trShowAll");


            BTN_Close.ToolTip = AppSettings.resourcemanager.GetString("trClose");
            BTN_Minimize.ToolTip = AppSettings.resourcemanager.GetString("minimize");
            btn_deliveryWaitConfirmUser.ToolTip = AppSettings.resourcemanager.GetString("trOrdersWaitConfirmUser");
            btn_transfers.ToolTip = AppSettings.resourcemanager.GetString("trDailyClosing");
            BTN_notifications.ToolTip = AppSettings.resourcemanager.GetString("trNotification");


            tb_versionTitle.Text = AppSettings.resourcemanager.GetString("Version") + ": ";
            */

        }
        private void Btn_home_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var control in FindControls.FindVisualChildren<Expander>(this))
                {

                    var expander = control as Expander;
                    if (expander.Tag != null )
                            expander.IsExpanded = false;
                }

                /*
                colorTextRefreash(txt_home);
                FN_pathVisible(path_openHome);
                fn_ColorIconRefreash(path_iconHome);
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_home.Instance);
                if (isHome)
                {
                    uc_home.Instance.timerAnimation();
                    isHome = false;
                }
                Button button = sender as Button;
                MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
                */
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        private void Btn_userImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window.GetWindow(this).Opacity = 0.2;
                //wd_userInfo w = new wd_userInfo();
                //w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
            }
            catch (Exception ex)
            {
                Window.GetWindow(this).Opacity = 1;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_message_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            try
            {
                var Sender = sender as Expander;
                foreach (var control in FindControls.FindVisualChildren<Expander>(this).ToList())
                {

                    var expander = control as Expander;
                    if (expander.Tag != null && Sender.Tag != null)
                        //if (expander.Tag.ToString() != Sender.Tag.ToString() )
                        if (!(expander.Tag.ToString().Contains(Sender.Tag.ToString()) 
                            || Sender.Tag.ToString().Contains(expander.Tag.ToString())))
                        {
                            expander.Expanded -= Expander_Expanded;
                            expander.IsExpanded = false;
                            expander.Expanded += Expander_Expanded;
                        }
                }

                try
                {
                    if (!menuState)
                    {
                        Storyboard sb = this.FindResource("Storyboard1") as Storyboard;
                        sb.Begin();
                        txt_companyName.Visibility = Visibility.Visible;
                        txt_dashboard.Visibility = Visibility.Visible;
                        txt_salesInvoice.Visibility = Visibility.Visible;
                        menuState = true;
                    }
                }
                catch (Exception ex)
                {
                    HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void secondMenuTitleActivate(string active)
        {

            try
            {
                var list = FindControls.FindVisualChildren<TextBlock>(this)
                    .Where(x => x.Tag != null && x.Tag.ToString().Contains("secondMenuTitle"))
                    .ToList();
                foreach (var control in list)
                {
                    if (control.Tag.ToString().Contains(active))
                        control.Foreground = Application.Current.Resources["White"] as SolidColorBrush;
                    else
                        control.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#A8AEC6"));

                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

            if (active == "dashboard")
                path_iconDashboard.Fill = Application.Current.Resources["White"] as SolidColorBrush;
            else
                path_iconDashboard.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#A8AEC6"));

            if (active == "salesInvoice")
                path_iconSalesInvoice.Fill = Application.Current.Resources["White"] as SolidColorBrush;
            else
                path_iconSalesInvoice.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#A8AEC6"));


        }
        private void Btn_dashboard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                //grid_main.Children.Add(uc_vendorsData.Instance);
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_vendorsData_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_vendorsData.Instance);
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }


        }

        private void Btn_vendorsGroups_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_vendorsGroups.Instance);
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_vendorsType_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_vendorsType.Instance);
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_supportVendors_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_supportVendors.Instance);
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_phoneType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
            grid_main.Children.Add(uc_phoneType.Instance);
            
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_bank_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
            grid_main.Children.Add(uc_bank.Instance);
           
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_country_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
            grid_main.Children.Add(uc_country.Instance);
           
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_supplierDocType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
            grid_main.Children.Add(uc_supplierDocType.Instance);
          
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_category_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
            grid_main.Children.Add(uc_category.Instance);
            
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        private void Btn_item_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
            grid_main.Children.Add(uc_item.Instance);
            
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        //private void Btn_unit_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        grid_main.Children.Clear();
        //    grid_main.Children.Add(uc_unit.Instance);
            
        //        Button button = sender as Button;
        //        secondMenuTitleActivate(button.Tag.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
        //    }
        //}

        private void Btn_purchaseInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
            grid_main.Children.Add(uc_purchaseInvoice.Instance);
           
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_purchaseOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
            grid_main.Children.Add(uc_purchaseOrder.Instance);
           
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_receiptInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
            grid_main.Children.Add(uc_receiptInvoice.Instance);
           
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_returnReceiptInv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
            grid_main.Children.Add(uc_returnReceiptInv.Instance);
            
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_salesInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_salesInvoice.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_promotionInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
            grid_main.Children.Add(uc_promotionInvoice.Instance);
           
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_contributorsPromotion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_contributorsPromotion.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_brand_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
            grid_main.Children.Add(uc_brand.Instance);
            
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_locationsData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
            grid_main.Children.Add(uc_locationsData.Instance);
           
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_locationType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
            grid_main.Children.Add(uc_locationType.Instance);
            
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_customerArea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_area.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_job_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_job.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_kinshipTies_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_kinshipTies.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_hirarachyStructure_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_hirarachyStructure.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_customerBank_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_customerBank.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void btn_activitiesType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_activitiesType.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        private void Btn_customerData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_customerData.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_familyCard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_familyCard.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_fundChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_fundChange.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_addStocks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_addStocks.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_reduceStocks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_reduceStocks.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void Btn_transformStocks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_transformStocks.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void Btn_retreatOfCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_retreatOfCustomer.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }

        private void Btn_deathOfCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_deathOfCustomer.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
       

        private void btn_activities_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_activities.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void btn_subscriptionRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_subscriptionRequest.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }   

        private void btn_activitiesReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_activitiesReport.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_generalSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
            grid_main.Children.Add(uc_generalSettings.Instance);
           
                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_user_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_user.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
         private void Btn_permissions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_permissions.Instance);

                Button button = sender as Button;
                secondMenuTitleActivate(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_lang_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.lang.Equals("en"))
                AppSettings.lang = "ar";
            else
                AppSettings.lang = "en";


            //update languge in main window
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;

            if (parentWindow != null)
            {
                //access property of the MainWindow class that exposes the access rights...
                if (AppSettings.lang.Equals("en"))
                {
                    AppSettings.resourcemanager = new ResourceManager("POSCA.en_file", Assembly.GetExecutingAssembly());
                    parentWindow.grid_mainWindow.FlowDirection = FlowDirection.LeftToRight;
                    txt_lang.Text = "AR";

                }
                else
                {
                    AppSettings.resourcemanager = new ResourceManager("POSCA.ar_file", Assembly.GetExecutingAssembly());
                    parentWindow.grid_mainWindow.FlowDirection = FlowDirection.RightToLeft;
                    txt_lang.Text = "EN";
                }
                parentWindow.translate();

            }
        }
        private void Btn_lockApp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Visibility = Visibility.Collapsed;
                wd_pauseScreen w = new wd_pauseScreen();
                w.ShowDialog();
                Window.GetWindow(this).Visibility = Visibility.Visible;

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                Window.GetWindow(this).Visibility = Visibility.Visible;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        bool menuState = false;
        private void BTN_Menu_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (!menuState)
                {
                    Storyboard sb = this.FindResource("Storyboard1") as Storyboard;
                    sb.Begin();
                    txt_companyName.Visibility = Visibility.Visible;
                    txt_dashboard.Visibility = Visibility.Visible;
                    txt_salesInvoice.Visibility = Visibility.Visible;
                    menuState = true;
                }
                else
                {
                    try
                    {
                        foreach (var control in FindControls.FindVisualChildren<Expander>(this).ToList())
                        {
                            var expander = control as Expander;
                            expander.IsExpanded = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }
                    txt_companyName.Visibility = Visibility.Collapsed;
                    txt_dashboard.Visibility = Visibility.Collapsed;
                    txt_salesInvoice.Visibility = Visibility.Collapsed;
                    Storyboard sb = this.FindResource("Storyboard2") as Storyboard;
                    sb.Begin();
                    menuState = false;
                }
             

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        
    }
}
