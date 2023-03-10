using POSCA.Classes;
using POSCA.View.sectionData;
using POSCA.View.sectionData.vendors;
//using POSCA.View.sectionData.vendors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            //if (AppSettings.lang.Equals("en"))
            //    grid_mainWindow.FlowDirection = FlowDirection.LeftToRight;
            //else
                grid_mainWindow.FlowDirection = FlowDirection.RightToLeft;
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


                translate();
                //try
                //{
                //    tb_version.Text = AppSettings.CurrentVersion;
                //}
                //catch (Exception ex)
                //{
                //    HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
                //}
                /*

                #region loading 
                loadingList = new List<keyValueBool>();
                bool isDone = true;
                loadingList.Add(new keyValueBool { key = "loading_listObjects", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getGroupObjects", value = false });
                loadingList.Add(new keyValueBool { key = "loading_globalItemUnitsList", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefreshBranches", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefreshBranchesAllWithoutMain", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefreshByBranchandUser", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefreshCategory", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefreshUnit", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefreshVendors", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefreshCards", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getUserPersonalInfo", value = false });
                //loadingList.Add(new keyValueBool { key = "loading_getUserPath", value = false });//dina

                // loadingList.Add(new keyValueBool { key = "loading_getItemCost", value = false });//dina
                //  loadingList.Add(new keyValueBool { key = "loading_getPrintCount", value = false });
                //loadingList.Add(new keyValueBool { key = "loading_getTaxDetails", value = false });//dina
                //loadingList.Add(new keyValueBool { key = "loading_getDefaultSystemInfo", value = false });//dina
                // loadingList.Add(new keyValueBool { key = "loading_getDateForm", value = false });//dina
                //loadingList.Add(new keyValueBool { key = "loading_getRegionAndCurrency", value = false });//dina
                //loadingList.Add(new keyValueBool { key = "loading_getStorageCost", value = false });//dina
                //loadingList.Add(new keyValueBool { key = "loading_getAccurac", value = false });//dina
                loadingList.Add(new keyValueBool { key = "loading_getprintSitting", value = false });
                loadingList.Add(new keyValueBool { key = "loading_POSList", value = false });
                //loadingList.Add(new keyValueBool { key = "loading_getTableTimes", value = false });//dina
                // loadingList.Add(new keyValueBool { key = "loading_getDefaultInvoiceType", value = false });//dina
                // loadingList.Add(new keyValueBool { key = "loading_getStatusesOfPreparingOrder", value = false });//dina
                loadingList.Add(new keyValueBool { key = "loading_typesOfService", value = false });
                //loadingList.Add(new keyValueBool { key = "loading_maxDiscount", value = false });//dina
                loadingList.Add(new keyValueBool { key = "loading_itemUnitsUsersList", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getSetValues", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getRegions", value = false });
                loadingList.Add(new keyValueBool { key = "loading_activationSite", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getDefaultServerStatus", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getCountries", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getCities", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getUsers", value = false });

                loading_listObjects();
                loading_getGroupObjects();
                loading_globalItemUnitsList();
                loading_RefreshBranches();
                loading_RefreshBranchesAllWithoutMain();
                loading_RefreshByBranchandUser();
                loading_RefreshCategory();
                loading_RefreshUnit();
                loading_RefreshVendors();
                loading_RefreshCards();
                loading_getUserPersonalInfo();
                loading_getUsers();
                //loading_getUserPath();//dina

                //  loading_getItemCost();
                //  loading_getPrintCount();
                //loading_getTaxDetails();//dina
                //loading_getDefaultSystemInfo();//dina
                //loading_getDateForm();//dina
                //loading_getRegionAndCurrency();
                //loading_getStorageCost();//dina
                //loading_getAccurac();
                loading_getprintSitting();
                loading_POSList();
                //loading_getTableTimes();//dina
                // loading_getDefaultInvoiceType();//dina
                //loading_getStatusesOfPreparingOrder();//dina
                loading_typesOfService();
                //loading_maxDiscount();//dina
                loading_itemUnitsUsersList();
                loading_getSetValues();
                loading_getRegions();
                loading_getCountries();
                loading_getCities();
                loading_activationSite();
                loading_getDefaultServerStatus();

                do
                {
                    isDone = true;
                    foreach (var item in loadingList)
                    {
                        if (item.value == false)
                        {
                            isDone = false;
                            break;
                        }
                    }
                    if (!isDone)
                    {
                        //MessageBox.Show("not done");
                        //string s = "";
                        //foreach (var item in loadingList)
                        //{
                        //    s += item.name + " - " + item.value + "\n";
                        //}
                        //MessageBox.Show(s);
                        await Task.Delay(0500);
                        //MessageBox.Show("do");
                    }
                }
                while (!isDone);
                //MessageBox.Show(catchError + " and count: " + catchErrorCount);
                #endregion
                */
             

                #region Permision
                permission();
                #endregion

               

                //SelectAllText
                EventManager.RegisterClassHandler(typeof(System.Windows.Controls.TextBox), System.Windows.Controls.TextBox.GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText));
                txt_rightReserved.Text = DateTime.Now.Date.Year + " © All Right Reserved for Increase";

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
            /*
            bool loadWindow = false;
            //loadWindow = loadingDefaultPath(AppSettings.defaultPath);
            if (!HelpClass.isAdminPermision())
                foreach (Button button in FindControls.FindVisualChildren<Button>(this))
                {
                    if (button.Tag != null)
                        if (FillCombo.groupObject.HasPermission(button.Tag.ToString(), FillCombo.groupObjects))
                        {
                            button.Visibility = Visibility.Visible;
                            if (!loadWindow)
                            {
                                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                                loadWindow = true;
                            }
                        }
                        else button.Visibility = Visibility.Collapsed;
                }
            else
            if (!loadWindow)
                Btn_home_Click(btn_home, null);
            */
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
            txt_home.Text = AppSettings.resourcemanager.GetString("trHome");
            txt_sectionData.Text = AppSettings.resourcemanager.GetString("trSectionData");
            txt_vendors.Text = AppSettings.resourcemanager.GetString("Suppliers");
            txt_vendorsData.Text = AppSettings.resourcemanager.GetString("SuppliersData");
            txt_vendorsGroups.Text = AppSettings.resourcemanager.GetString("SuppliersGroups");
            txt_vendorsType.Text = AppSettings.resourcemanager.GetString("SuppliersTypes");
            txt_supportVendors.Text = AppSettings.resourcemanager.GetString("AssistantSuppliers");

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

                //foreach (var control in FindControls.FindVisualChildren<Expander>(this))
                //{

                //    var expander = control as Expander;
                //    if (expander.Tag != null && Sender.Tag != null)
                //        if (expander.Tag.ToString() != Sender.Tag.ToString())
                //            expander.IsExpanded = false;
                //}
               
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_vendorsData_Click(object sender, RoutedEventArgs e)
        {
            grid_main.Children.Clear();
            grid_main.Children.Add(uc_vendorsData.Instance);
        }

        private void Btn_vendorsGroups_Click(object sender, RoutedEventArgs e)
        {
            grid_main.Children.Clear();
            grid_main.Children.Add(uc_vendorsGroups.Instance);
        }

        private void Btn_vendorsType_Click(object sender, RoutedEventArgs e)
        {
            grid_main.Children.Clear();
            grid_main.Children.Add(uc_vendorsType.Instance);
        }

        private void Btn_supportVendors_Click(object sender, RoutedEventArgs e)
        {
            grid_main.Children.Clear();
            grid_main.Children.Add(uc_supportVendors.Instance);
        }

        private void Btn_phoneType_Click(object sender, RoutedEventArgs e)
        {
            grid_main.Children.Clear();
            grid_main.Children.Add(uc_phoneType.Instance);
        }
    }
}
