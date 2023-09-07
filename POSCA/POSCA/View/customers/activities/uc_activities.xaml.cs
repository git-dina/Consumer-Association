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
    public partial class uc_activities : UserControl
    {

        public uc_activities()
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
        private static uc_activities _instance;
        public static uc_activities Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_activities();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        Activity activity = new Activity();
        IEnumerable<Activity> activitiesQuery;
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
                requiredControlList = new List<string> { "Description" , "TypeId", "BasicValue", "ValueAfterDiscount",
                                                    "RegestrtionCount","StartDate","EndDate"};
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


                Keyboard.Focus(tb_Description);
                await FillCombo.fillFinalActivityTypes(cb_TypeId);
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

            txt_title.Text = AppSettings.resourcemanager.GetString("Activity");
            txt_search.Text = AppSettings.resourcemanager.GetString("trSearch");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ActivityId, AppSettings.resourcemanager.GetString("trNoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_TypeId, AppSettings.resourcemanager.GetString("ActivityTypeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Description, AppSettings.resourcemanager.GetString("trDescriptionHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_BasicValue, AppSettings.resourcemanager.GetString("ActivityBasicValueHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ValueAfterDiscount, AppSettings.resourcemanager.GetString("ValueAfterDiscountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_MaximumBenefit, AppSettings.resourcemanager.GetString("MaximumBenefitFromTheActivityHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_RegestrtionCount, AppSettings.resourcemanager.GetString("RegistrationTotalNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_RemainCount, AppSettings.resourcemanager.GetString("AvailableNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_StartDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_EndDate, AppSettings.resourcemanager.GetString("trEndDateHint"));

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            txt_IsBlocked.Text = AppSettings.resourcemanager.GetString("IsBlocked");
            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trSave");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            dg_activity.Columns[0].Header = AppSettings.resourcemanager.GetString("trNo");
            dg_activity.Columns[1].Header = AppSettings.resourcemanager.GetString("trName");
            dg_activity.Columns[2].Header = AppSettings.resourcemanager.GetString("trValue");
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

                    activity = new Activity();
                    if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                    {

                        activity.Description = tb_Description.Text;
                        activity.TypeId =(int) cb_TypeId.SelectedValue;
                        activity.BasicValue = decimal.Parse(tb_BasicValue.Text);
                        activity.ValueAfterDiscount = decimal.Parse(tb_ValueAfterDiscount.Text);
                        activity.MaximumBenefit = int.Parse(tb_MaximumBenefit.Text);
                        activity.RegestrtionCount = int.Parse(tb_RegestrtionCount.Text);
                        activity.StartDate = dp_StartDate.SelectedDate;
                        activity.EndDate = dp_EndDate.SelectedDate;
                        activity.Notes = tb_Notes.Text;
                        if (tgl_IsBlocked.IsChecked == true)
                            activity.IsBlocked = true;
                        else
                            activity.IsBlocked = false;
                        activity.CreateUserId = MainWindow.userLogin.UserId;

                        FillCombo.activitiesList = await activity.save(activity);
                        if (FillCombo.activitiesList == null)
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
                    if (activity.ActivityId > 0)
                    {
                        if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                        {

                            activity.Description = tb_Description.Text;
                            activity.TypeId = (int)cb_TypeId.SelectedValue;
                            activity.BasicValue = decimal.Parse(tb_BasicValue.Text);
                            activity.ValueAfterDiscount = decimal.Parse(tb_ValueAfterDiscount.Text);
                            activity.MaximumBenefit = int.Parse(tb_MaximumBenefit.Text);
                            activity.RegestrtionCount = int.Parse(tb_RegestrtionCount.Text);
                            activity.StartDate = dp_StartDate.SelectedDate;
                            activity.EndDate = dp_EndDate.SelectedDate;
                            activity.Notes = tb_Notes.Text;
                            if (tgl_IsBlocked.IsChecked == true)
                                activity.IsBlocked = true;
                            else
                                activity.IsBlocked = false;
                            activity.UpdateUserId = MainWindow.userLogin.UserId;

                            FillCombo.activitiesList = await activity.save(activity);
                            if (FillCombo.activitiesList == null)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                                await Search();

                            }
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
                    if (activity.ActivityId != 0)
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
                            FillCombo.activitiesList = await activity.delete(activity.ActivityId, MainWindow.userLogin.UserId);
                            if (FillCombo.activitiesList == null)
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
                    activity = dg_activity.SelectedItem as Activity;
                    this.DataContext = activity;
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

                //tb_search.Text = "";
                //searchText = "";
                await RefreshActivitiesList();
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
            //search
            if (FillCombo.activitiesList is null)
                await RefreshActivitiesList();
            searchText = tb_search.Text.ToLower();
            activitiesQuery = FillCombo.activitiesList.Where(s =>
            s.Description.ToLower().Contains(searchText)
            ).ToList();
            RefreshActivitiesView();
        }
        async Task<IEnumerable<Activity>> RefreshActivitiesList()
        {
            await FillCombo.RefreshActivities();

            return FillCombo.activitiesList;
        }
        void RefreshActivitiesView()
        {
            dg_activity.ItemsSource = activitiesQuery;
            txt_count.Text = activitiesQuery.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        async Task Clear()
        {
            activity = new Activity();
            this.DataContext = new Activity();
            dg_activity.SelectedIndex = -1;

            var maxId = await FillCombo.activity.getMaxActivityId();
            tb_ActivityId.Text = maxId;
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
        }


        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            swapToData();
        }

        #endregion

        private void dp_StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dp_StartDate.SelectedDate != null && dp_EndDate.SelectedDate != null)
                    if (dp_StartDate.SelectedDate.Value.Date > dp_EndDate.SelectedDate.Value.Date)
                        dp_EndDate.SelectedDate = dp_StartDate.SelectedDate.Value.Date;

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void dp_EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dp_StartDate.SelectedDate != null && dp_EndDate.SelectedDate != null)
                    if (dp_StartDate.SelectedDate.Value.Date > dp_EndDate.SelectedDate.Value.Date)
                        dp_EndDate.SelectedDate = dp_StartDate.SelectedDate.Value.Date;

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
