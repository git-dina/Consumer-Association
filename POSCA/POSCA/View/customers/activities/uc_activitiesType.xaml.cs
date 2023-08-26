using netoaster;
using POSCA.Classes;
using POSCA.Classes.ApiClasses;
using POSCA.View.windows;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace POSCA.View.customers.activities
{
    /// <summary>
    /// Interaction logic for uc_activitiesType.xaml
    /// </summary>
    public partial class uc_activitiesType : UserControl
    {
        public uc_activitiesType()
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
        private static uc_activitiesType _instance;
        public static uc_activitiesType Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_activitiesType();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        ActivityType activityType = new ActivityType();
        IEnumerable<ActivityType> activityTypesQuery;
        IEnumerable<ActivityType> ActivityTypes;
        string searchText = "";
        public static List<string> requiredControlList;

        public void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        //List<ActivityType> categories;
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "Name" };
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();


                Keyboard.Focus(tb_Name);

                //await FillCombo.fillActivityTypesWithDefault(cb_ParentTypeId);
                await Clear();
                await Search();

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



            txt_title.Text = AppSettings.resourcemanager.GetString("ActivitiesTypes");

            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Id, AppSettings.resourcemanager.GetString("trNoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Name, AppSettings.resourcemanager.GetString("trNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_ParentTypeId, AppSettings.resourcemanager.GetString("ParentActivityHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("GeneralNotesHint"));

            txt_IsFinal.Text = AppSettings.resourcemanager.GetString("FinalType");
            txt_AllContributors.Text = AppSettings.resourcemanager.GetString("AllContributors");
            txt_OnlyFamilyCardHolder.Text = AppSettings.resourcemanager.GetString("OnlyFamilyCardHolder");
            txt_IsBlocked.Text = AppSettings.resourcemanager.GetString("IsBlocked");
            txt_OnlyOneActivity.Text = AppSettings.resourcemanager.GetString("RegisterInOneActivity");
            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trSave");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");

        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                // if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);

                    activityType = new ActivityType();
                    if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                    {
                        activityType.Name = tb_Name.Text;

                        if (cb_ParentTypeId.SelectedIndex > 0)
                            activityType.ParentTypeId = (int)cb_ParentTypeId.SelectedValue;
                        if (tgl_AllContributors.IsChecked == true)
                            activityType.AllContributors = true;
                        else
                            activityType.AllContributors = false;

                         if (tgl_IsFinal.IsChecked == true)
                            activityType.IsFinal = true;
                        else
                            activityType.IsFinal = false;

                         if (tgl_OnlyFamilyCardHolder.IsChecked == true)
                            activityType.OnlyFamilyCardHolder = true;
                        else
                            activityType.OnlyFamilyCardHolder = false;

                         if (tgl_OnlyOneActivity.IsChecked == true)
                            activityType.OnlyOneActivity = true;
                        else
                            activityType.OnlyOneActivity = false;

                        if (tgl_IsBlocked.IsChecked == true)
                            activityType.IsBlocked = true;
                        else
                            activityType.IsBlocked = false;

                        activityType.Notes = tb_Notes.Text;

                        FillCombo.activityTypeList = await activityType.save(activityType);

                        if (FillCombo.activityTypeList == null)
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
                //{
                HelpClass.StartAwait(grid_main);
                if (activityType.Id > 0)
                {
                    if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                    {
                        activityType.Name = tb_Name.Text;
                        if (cb_ParentTypeId.SelectedIndex > 0)
                            activityType.ParentTypeId = (int)cb_ParentTypeId.SelectedValue;
                        if (tgl_AllContributors.IsChecked == true)
                            activityType.AllContributors = true;
                        else
                            activityType.AllContributors = false;

                        if (tgl_IsFinal.IsChecked == true)
                            activityType.IsFinal = true;
                        else
                            activityType.IsFinal = false;

                        if (tgl_OnlyFamilyCardHolder.IsChecked == true)
                            activityType.OnlyFamilyCardHolder = true;
                        else
                            activityType.OnlyFamilyCardHolder = false;

                        if (tgl_OnlyOneActivity.IsChecked == true)
                            activityType.OnlyOneActivity = true;
                        else
                            activityType.OnlyOneActivity = false;

                        if (tgl_IsBlocked.IsChecked == true)
                            activityType.IsBlocked = true;
                        else
                            activityType.IsBlocked = false;

                        activityType.Notes = tb_Notes.Text;

                        FillCombo.activityTypeList = await activityType.save(activityType);

                        if (FillCombo.activityTypeList == null)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);

                            await Clear();
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
                //}
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
                HelpClass.StartAwait(grid_main);
                if (activityType.Id != 0)
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
                        FillCombo.activityTypeList = await activityType.delete(activityType.Id, MainWindow.userLogin.userId);
                        if (FillCombo.activityTypeList == null)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            activityType.Id = 0;
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                            await Search();
                            await Clear();
                        }
                    }

                }
                HelpClass.EndAwait(grid_main);
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

        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Clear();
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
                searchText = "";
                await RefreshActivityTypesList();
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
            if (FillCombo.activityTypeList is null)
                await RefreshActivityTypesList();
            /*
            searchText = tb_search.Text.ToLower();
            //ActivityTypesQuery = FillCombo.ActivityTypeList.Where(s =>
            ActivityTypesQuery = ActivityTypes.Where(s =>
            s.ActivityTypeId.ToString().Contains(searchText) ||
            s.Name.ToLower().Contains(searchText)
            ).ToList();
            */
            activityTypesQuery = FillCombo.activityTypeList.ToList();
            RefreshActivityTypesView();
        }
        async Task<IEnumerable<ActivityType>> RefreshActivityTypesList()
        {

            await FillCombo.RefreshActivityTypes();
            ActivityTypes = FillCombo.activityTypeList.ToList();

            return ActivityTypes;
        }
        void RefreshActivityTypesView()
        {
            tv_activityTypes.Items.Clear();
            if (activityTypesQuery.Where(x => x.ParentTypeId is null).Count() > 0)
            {
                buildTreeViewList(activityTypesQuery.Where(x => x.ParentTypeId is null).ToList(), tv_activityTypes);

            }
            else
            {
                buildTreeViewList(activityTypesQuery.Where(x => x.ParentTypeId is null).ToList(), tv_activityTypes);

            }


        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        async Task Clear()
        {
            this.DataContext = new ActivityType();
            //dg_ActivityType.SelectedIndex = -1;
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            await FillCombo.fillActivityTypesWithDefault(cb_ParentTypeId);
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
        #region TreeView

        void buildTreeViewList(List<ActivityType> _categories, TreeView treeViewItemParent)
        {
            foreach (var item in _categories)
            {
                TreeViewItem treeViewItem = new TreeViewItem();
                treeViewItem.Tag = item.Id.ToString();
                treeViewItem.Header = item.Name;
                treeViewItem.FontSize = 16;
                treeViewItem.Foreground = Application.Current.Resources["textColor"] as SolidColorBrush; ;
                treeViewItem.Selected += TreeViewItem_Selected;

                treeViewItemParent.Items.Add(treeViewItem);
                if (activityTypesQuery.Where(x => x.ParentTypeId == item.Id).ToList().Count() > 0)
                {
                    buildTreeViewList(activityTypesQuery.Where(x => x.ParentTypeId == item.Id).ToList(), treeViewItem);
                }


            }
        }
        void buildTreeViewList(List<ActivityType> _categories, TreeViewItem treeViewItemParent)
        {
            foreach (var item in _categories)
            {
                TreeViewItem treeViewItem = new TreeViewItem();
                treeViewItem.Tag = item.Id.ToString();
                treeViewItem.Header = item.Name;
                treeViewItem.FontSize = 16;
                treeViewItem.Foreground = Application.Current.Resources["textColor"] as SolidColorBrush; ;
                treeViewItem.Selected += TreeViewItem_Selected;

                treeViewItemParent.Items.Add(treeViewItem);
                if (activityTypesQuery.Where(x => x.ParentTypeId == item.Id).ToList().Count() > 0)
                {
                    buildTreeViewList(activityTypesQuery.Where(x => x.ParentTypeId == x.Id).ToList(), treeViewItem);
                }
            }
        }
        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {

            TreeViewItem treeViewItem = sender as TreeViewItem;
            if (treeViewItem.IsSelected)
            {
                unExpandTreeViewItem();
                setSelectedStyleTreeViewItem();
                activityType = FillCombo.activityTypeList.Where(x => x.Id == long.Parse(treeViewItem.Tag.ToString())).FirstOrDefault();
                this.DataContext = activityType;

            }
            treeViewItem.IsExpanded = true;
        }
        void unExpandTreeViewItem()
        {
            List<TreeViewItem> list = FindControls.FindVisualChildren<TreeViewItem>(this).ToList();
            foreach (var item in list)
            {
                if (!item.IsSelected)
                    item.IsExpanded = false;
            }
        }
        void setSelectedStyleTreeViewItem()
        {
            List<TreeViewItem> list = FindControls.FindVisualChildren<TreeViewItem>(this).ToList();
            foreach (var item in list)
            {
                if (!item.IsSelected)
                {
                    item.Foreground = Application.Current.Resources["textColor"] as SolidColorBrush;
                    item.FontWeight = FontWeights.Regular;
                }
                else
                {
                    item.Foreground = Application.Current.Resources["SecondColor"] as SolidColorBrush;
                    item.FontWeight = FontWeights.SemiBold;
                }
            }
        }

        #endregion

        private void btn_columnSwap_Click(object sender, RoutedEventArgs e)
        {
            ColumnDefinition cd_gridMain3 = new ColumnDefinition();
            cd_gridMain3.Width = cd_gridMain1.Width;
            cd_gridMain1.Width = cd_gridMain2.Width;
            cd_gridMain2.Width = cd_gridMain3.Width;
        }

        private void cb_ActivityTypeParentId_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                var tb = cb_ParentTypeId.Template.FindName("PART_EditableTextBox", cb_ParentTypeId) as TextBox;
                tb.FontFamily = Application.Current.Resources["Font-cairo-regular"] as FontFamily;
                cb_ParentTypeId.ItemsSource = FillCombo.activityTypeList.Where(p => p.Name.ToLower().Contains(tb.Text.ToLower())).ToList();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
