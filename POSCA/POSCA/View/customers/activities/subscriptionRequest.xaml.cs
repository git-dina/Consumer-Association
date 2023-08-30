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

namespace POSCA.View.customers.activities
{
    /// <summary>
    /// Interaction logic for uc_customerBank.xaml
    /// </summary>
    public partial class uc_subscriptionRequest : UserControl
    {

        public uc_subscriptionRequest()
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
        private static uc_subscriptionRequest _instance;
        public static uc_subscriptionRequest Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_subscriptionRequest();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        CustomerActivity customerActivity = new CustomerActivity();
        IEnumerable<CustomerActivity> customerActivitiesQuery;
        Customer customer;
        string searchText = "";
        public static List<string> requiredControlList;

        public void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "BoxNumber" , "ActivityId", "ActivityCount" };
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();
                swapToData();


                Keyboard.Focus(tb_BoxNumber);
                await FillCombo.fillActivities(cb_ActivityId);
                await Search();
                await Clear();
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

            txt_title.Text = AppSettings.resourcemanager.GetString("SubscriptionRequest");
            txt_search.Text = AppSettings.resourcemanager.GetString("trSearch");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_RequestId, AppSettings.resourcemanager.GetString("RequestNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_BoxNumber, AppSettings.resourcemanager.GetString("BoxNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerName, AppSettings.resourcemanager.GetString("CustomerNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CivilNum, AppSettings.resourcemanager.GetString("CivilNumHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerStatus, AppSettings.resourcemanager.GetString("CustomerStatusHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_FamilyCardHolder, AppSettings.resourcemanager.GetString("FamilyCardHolder"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_ActivityId, AppSettings.resourcemanager.GetString("TheActivity"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_BasicValue, AppSettings.resourcemanager.GetString("ActivityBasicValueHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ValueAfterDiscount, AppSettings.resourcemanager.GetString("ValueAfterDiscountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_MaximumBenefit, AppSettings.resourcemanager.GetString("MaximumBenefitFromTheActivityHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ActivityCount, AppSettings.resourcemanager.GetString("RequiredCountHint"));
        
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trSave");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            dg_activity.Columns[0].Header = AppSettings.resourcemanager.GetString("BoxNumber");
            dg_activity.Columns[1].Header = AppSettings.resourcemanager.GetString("CustomerNo");
            dg_activity.Columns[2].Header = AppSettings.resourcemanager.GetString("CivilNo");
            dg_activity.Columns[3].Header = AppSettings.resourcemanager.GetString("CustomerName");
            dg_activity.Columns[4].Header = AppSettings.resourcemanager.GetString("TheActivity");
            dg_activity.Columns[5].Header = AppSettings.resourcemanager.GetString("ActivityValueAfterDisc");
            dg_activity.Columns[6].Header = AppSettings.resourcemanager.GetString("UsedNumber");
            dg_activity.Columns[7].Header = AppSettings.resourcemanager.GetString("ActivityStartDate");
            dg_activity.Columns[8].Header = AppSettings.resourcemanager.GetString("ActivityEndDate");
            dg_activity.Columns[9].Header = AppSettings.resourcemanager.GetString("RegistrationDate");

            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");


            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);

                    customerActivity = new CustomerActivity();
                    if (HelpClass.validate(requiredControlList, this) )
                    {
                        if (int.Parse(tb_ActivityCount.Text) > 0)
                        {
                            customerActivity.CustomerId = customer.CustomerId;
                            customerActivity.ActivityId = (long)cb_ActivityId.SelectedValue;
                            customerActivity.Count = int.Parse(tb_ActivityCount.Text);

                            customerActivity.Notes = tb_Notes.Text;

                            customerActivity.CreateUserId = MainWindow.userLogin.userId;

                            var res = await customerActivity.Save(customerActivity);
                            if (res.RequestId == 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                                await Clear();
                                await Search();
                            }
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMustBeMoreThanZero"), animation: ToasterAnimation.FadeIn);
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
        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (customerActivity.RequestId > 0)
                    {
                        if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                        {
                            if (int.Parse(tb_ActivityCount.Text) > 0)
                            {
                                customerActivity.Count = int.Parse(tb_ActivityCount.Text);

                                customerActivity.Notes = tb_Notes.Text;
                                customerActivity.UpdateUserId = MainWindow.userLogin.userId;

                                var res = await customerActivity.Save(customerActivity);
                                if (res.RequestId == 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                                    await Search();

                                }
                            }
                            else
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMustBeMoreThanZero"), animation: ToasterAnimation.FadeIn);
                        }                     
                        else
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("saveNotDoneEmptyFields"), animation: ToasterAnimation.FadeIn);
                        }
                       
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);

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
                    if (customerActivity.ActivityId != 0)
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
                            var res = await customerActivity.delete(customerActivity.RequestId, MainWindow.userLogin.userId);
                            if (res == 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                await Search();
                                await Clear();
                            }
                        }

                    }
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
        private async void Dg_activity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection

                if (dg_activity.SelectedIndex != -1)
                {
                    customerActivity = dg_activity.SelectedItem as CustomerActivity;
                    this.DataContext = customerActivity;
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
      
        #endregion
        #region Refresh & Search
        async Task Search()
        {
            //search

            customerActivitiesQuery = await customerActivity.SearchActivities(tb_search.Text);
            RefreshActivitiesView();

        }

        void RefreshActivitiesView()
        {
            dg_activity.ItemsSource = customerActivitiesQuery;
            txt_count.Text = customerActivitiesQuery.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        async Task Clear()
        {
            customerActivity = new CustomerActivity();
            customer = new Customer();
            this.DataContext = new CustomerActivity();
            dg_activity.SelectedIndex = -1;

            var maxId = await customerActivity.getMaxRequestId();
            tb_RequestId.Text = maxId;

            #region clear inputs
            tb_BoxNumber.Text = "";
            tb_CustomerName.Text = "";
            tb_CivilNum.Text = "";
            tb_CustomerStatus.Text = "";
            tb_FamilyCardHolder.Text = "";
            tb_CivilNum.Text = "";
            tb_CivilNum.Text = "";
            tb_BasicValue.Text = "";
            tb_ValueAfterDiscount.Text = "";
            tb_MaximumBenefit.Text = "";
            tb_ActivityCount.Text = "";

            #endregion
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

        }
        void swapToData()
        {
            cd_gridMain1.Width = new GridLength(0, GridUnitType.Star);
            cd_gridMain2.Width = new GridLength(1, GridUnitType.Star);

            if (customerActivity.CustomerId != null)
            {
                customer.CustomerId = (long)customerActivity.CustomerId;
                tb_BoxNumber.Text = customerActivity.BoxNumber.ToString();
                tb_CustomerName.Text = customerActivity.CustomerName.ToString(); ;
                tb_CivilNum.Text = customerActivity.CivilNum.ToString();
                tb_CustomerStatus.Text = AppSettings.resourcemanager.GetString(customerActivity.CustomerStatus);
                tb_FamilyCardHolder.Text = customerActivity.FamilyCardHolder == true ? AppSettings.resourcemanager.GetString("FamilyCardHolder") :
                                            AppSettings.resourcemanager.GetString("HasNoFamilyCard");
                tb_CivilNum.Text = customerActivity.CivilNum;
                tb_ActivityCount.Text = customerActivity.Count.ToString();
                tb_BasicValue.Text = HelpClass.DecTostring(customerActivity.BasicValue);
                tb_ValueAfterDiscount.Text = HelpClass.DecTostring(customerActivity.ValueAfterDiscount);
                tb_MaximumBenefit.Text = customerActivity.MaximumBenefit.ToString();
            }
        }


        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            swapToData();
        }

        #endregion
 

        private async void tb_BoxNumber_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return && tb_BoxNumber.Text != "")
                {

                    customer = null;
                    HelpClass.StartAwait(grid_main);
                    customer = await FillCombo.customer.GetByBoxNumber(long.Parse(tb_BoxNumber.Text));


                    if (customer != null)
                    {
                        if (customer.CustomerStatus != "continouse")
                        {
                            tb_BoxNumber.Text = "";
                            Keyboard.Focus(tb_BoxNumber);
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CustomerNotContinouse"), animation: ToasterAnimation.FadeIn);
                        }
                        else
                        {
                            tb_CustomerName.Text = customer.Name;
                            tb_CivilNum.Text = customer.CivilNum.ToString();
                            tb_CustomerStatus.Text = AppSettings.resourcemanager.GetString(customer.CustomerStatus);
                            tb_FamilyCardHolder.Text = customer.FamilyCardHolder == true? AppSettings.resourcemanager.GetString("FamilyCardHolder") :
                                                        AppSettings.resourcemanager.GetString("HasNoFamilyCard");
                            tb_CivilNum.Text = customer.CivilNum;

                            App.MoveToNextUIElement(e);
                        }
                    }
                    else
                    {
                        tb_BoxNumber.Text = "";
                        tb_CustomerName.Text ="";
                        tb_CivilNum.Text = "";
                        tb_CustomerStatus.Text = "";
                        tb_FamilyCardHolder.Text ="";
                        tb_CivilNum.Text = "";

                        Keyboard.Focus(tb_BoxNumber);

                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("NumberNotTrue"), animation: ToasterAnimation.FadeIn);
                    }
                    HelpClass.EndAwait(grid_main);

                }

            }
            catch
            {
                HelpClass.EndAwait(grid_main);
            }
        }

        private async void tb_Count_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return && tb_ActivityCount.Text != "" && cb_ActivityId.SelectedIndex != -1)
                {

                    HelpClass.StartAwait(grid_main);
                    var act = FillCombo.activitiesList.Where(x => x.ActivityId == (long)cb_ActivityId.SelectedValue).FirstOrDefault();
                    var usedCount = await customerActivity.GetUserUsedCount((long)cb_ActivityId.SelectedValue, customer.CustomerId);
                    if(usedCount + int.Parse(tb_ActivityCount.Text) > act.MaximumBenefit)
                    {

                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CountNotAllowed"), animation: ToasterAnimation.FadeIn);
                        tb_ActivityCount.Text = "";
                        Keyboard.Focus(tb_ActivityCount);
                    }
                    else if (act.RemainCount < int.Parse(tb_ActivityCount.Text))
                    {

                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CountNotAllowed"), animation: ToasterAnimation.FadeIn);
                        tb_ActivityCount.Text = "";
                        Keyboard.Focus(tb_ActivityCount);
                    }
                    else
                        App.MoveToNextUIElement(e);
                       
                    HelpClass.EndAwait(grid_main);

                }

            }
            catch
            {
                HelpClass.EndAwait(grid_main);
            }
        }

        private async void cb_ActivityId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(cb_ActivityId.SelectedIndex != -1)
                {
                    HelpClass.StartAwait(grid_main);
                    var ac = FillCombo.activitiesList.Where(x => x.ActivityId == (long)cb_ActivityId.SelectedValue).FirstOrDefault();
                    var usedCount = await customerActivity.GetUserUsedCount((long)cb_ActivityId.SelectedValue, customer.CustomerId);
                    tb_BasicValue.Text = HelpClass.DecTostring(ac.BasicValue);
                    tb_ValueAfterDiscount.Text = HelpClass.DecTostring(ac.ValueAfterDiscount);
                    tb_MaximumBenefit.Text = ac.MaximumBenefit.ToString();

                    if(tb_ActivityCount.Text != "" && (int.Parse(tb_ActivityCount.Text) > ac.RemainCount || usedCount > ac.MaximumBenefit ))
                    {
                        tb_ActivityCount.Text = "";
                    }
                    HelpClass.EndAwait(grid_main);
                }
                else
                {
                    tb_BasicValue.Text = "";
                    tb_ValueAfterDiscount.Text ="";
                    tb_MaximumBenefit.Text = "";
                }
            }
            catch
            {
                HelpClass.EndAwait(grid_main);

            }
        }
    }
}
