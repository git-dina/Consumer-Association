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


namespace POSCA.View.catalog
{
    /// <summary>
    /// Interaction logic for uc_category.xaml
    /// </summary>
    public partial class uc_category : UserControl
    {
        public uc_category()
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
        private static uc_category _instance;
        public static uc_category Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_category();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        Category category = new Category();
        IEnumerable<Category> categorysQuery;
        IEnumerable<Category> categorys;
        string searchText = "";
        public static List<string> requiredControlList;

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        //List<Category> categories;
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "Name"};
                if (AppSettings.lang.Equals("en"))
                {
                    AppSettings.resourcemanager = new ResourceManager("POSCA.en_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    AppSettings.resourcemanager = new ResourceManager("POSCA.ar_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();


                //categorys = new List<Category>()
                //{
                //    new Category(){CategoryId = 1, Name= "root", CategoryParentId = null, CategoryParentName = "" },
                //    new Category(){CategoryId = 2, Name= "son1-1", CategoryParentId = 1, CategoryParentName = "root" },
                //    new Category(){CategoryId = 3, Name= "son1-2", CategoryParentId = 1, CategoryParentName = "root"},
                //    new Category(){CategoryId = 4, Name= "son2-1", CategoryParentId = 2, CategoryParentName = "son1-1" },
                //    new Category(){CategoryId = 5, Name= "son2-2", CategoryParentId = 3, CategoryParentName = "son1-2" },
                //};
                //tv_categorys.Items.Clear();
                //categorysQuery = categorys.ToList();
                //buildTreeViewList(categorysQuery.Where(x => x.CategoryParentId is null).ToList(), tv_categorys);


                Keyboard.Focus(tb_Name);

                await FillCombo.fillCategorysWithDefault(cb_CategoryParentId);
                Clear();
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

          

            txt_title.Text = AppSettings.resourcemanager.GetString("Category");

            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Code, AppSettings.resourcemanager.GetString("trNoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Name, AppSettings.resourcemanager.GetString("trNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_CategoryParentId, AppSettings.resourcemanager.GetString("ParentCategoryHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_ProfitPercentage, AppSettings.resourcemanager.GetString("ProfitPercentageHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_FreePercentage, AppSettings.resourcemanager.GetString("FreePercentagHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_DiscountPercentage, AppSettings.resourcemanager.GetString("DiscountPercentageHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("GeneralNotesHint"));

            txt_CanContainItems.Text = AppSettings.resourcemanager.GetString("ContainItems");
            txt_IsBlocked.Text = AppSettings.resourcemanager.GetString("IsBlocked");
            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trUpdate");
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

                    category = new Category();
                    if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                    {
                        category.Name = tb_Name.Text;
                        if (cb_CategoryParentId.SelectedIndex > 0)
                            category.CategoryParentId = (long)cb_CategoryParentId.SelectedValue;

                        if(tb_ProfitPercentage.Text != "")
                            category.ProfitPercentage = decimal.Parse( tb_ProfitPercentage.Text);
                        if (tb_WholesalePercentage.Text != "")
                            category.WholesalePercentage = decimal.Parse(tb_WholesalePercentage.Text);
                        if (tb_DiscountPercentage.Text != "")
                            category.DiscountPercentage = decimal.Parse(tb_DiscountPercentage.Text);
                        if (tb_FreePercentage.Text != "")
                            category.FreePercentage = decimal.Parse( tb_FreePercentage.Text);

                        if (tgl_CanContainItems.IsChecked == true)
                            category.CanContainItems = true;
                        else
                            category.CanContainItems = false;

                        if (tgl_IsBlocked.IsChecked == true)
                            category.IsBlocked = true;
                        else
                            category.IsBlocked = false;

                        category.Notes = tb_Notes.Text;

                        FillCombo.categoryList = await category.save(category);

                        if (FillCombo.categoryList == null)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                            Clear();
                            await Search();
                        }
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
                if (category.CategoryId > 0)
                {
                    if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                    {
                        category.Name = tb_Name.Text;
                        if (cb_CategoryParentId.SelectedIndex > 0)
                            category.CategoryParentId = (long)cb_CategoryParentId.SelectedValue;

                        if (tb_ProfitPercentage.Text != "")
                            category.ProfitPercentage = decimal.Parse(tb_ProfitPercentage.Text);
                        if (tb_WholesalePercentage.Text != "")
                            category.WholesalePercentage = decimal.Parse(tb_WholesalePercentage.Text);
                        if (tb_DiscountPercentage.Text != "")
                            category.DiscountPercentage = decimal.Parse(tb_DiscountPercentage.Text);
                        if (tb_FreePercentage.Text != "")
                            category.FreePercentage = decimal.Parse(tb_FreePercentage.Text);

                        if (tgl_CanContainItems.IsChecked == true)
                            category.CanContainItems = true;
                        else
                            category.CanContainItems = false;

                        if (tgl_IsBlocked.IsChecked == true)
                            category.IsBlocked = true;
                        else
                            category.IsBlocked = false;

                        category.Notes = tb_Notes.Text;

                        FillCombo.categoryList = await category.save(category);

                        if (FillCombo.categoryList == null)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                            Clear();
                            await Search();
                        }
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
                if (category.CategoryId != 0)
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
                        FillCombo.categoryList = await category.delete(category.CategoryId, MainWindow.userLogin.userId);
                        if (FillCombo.categoryList == null)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            category.CategoryId = 0;
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                            await Search();
                            Clear();
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
        /*
        private async void Dg_category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //HelpClass.StartAwait(grid_main);
                //selection

                if (dg_category.SelectedIndex != -1)
                {
                    category = dg_category.SelectedItem as Category;
                    this.DataContext = category;

                    tb_DiscountPercentage.Text = HelpClass.DecTostring(category.DiscountPercentage);

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
        private void Dg_category_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                MessageBox.Show("Mouse Double Click on datagrid");
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        */
        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {

                HelpClass.StartAwait(grid_main);

                //tb_search.Text = "";
                searchText = "";
                await RefreshCategorysList();
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
            if (FillCombo.categoryList is null)
                await RefreshCategorysList();
            /*
            searchText = tb_search.Text.ToLower();
            //categorysQuery = FillCombo.categoryList.Where(s =>
            categorysQuery = categorys.Where(s =>
            s.CategoryId.ToString().Contains(searchText) ||
            s.Name.ToLower().Contains(searchText)
            ).ToList();
            */
            categorysQuery = FillCombo.categoryList.ToList();
            RefreshCategorysView();
        }
        async Task<IEnumerable<Category>> RefreshCategorysList()
        {

            await FillCombo.RefreshCategorys();
            categorys = FillCombo.categoryList.ToList();

            return categorys;
        }
        void RefreshCategorysView()
        {
            //dg_category.ItemsSource = null;
            //dg_category.ItemsSource = categorysQuery;
            //txt_count.Text = categorysQuery.Count().ToString();

            tv_categorys.Items.Clear();
            if (categorysQuery.Where(x => x.CategoryParentId is null).Count()>0)
            {
                buildTreeViewList(categorysQuery.Where(x => x.CategoryParentId is null).ToList(), tv_categorys);

            }
            else
            {
                buildTreeViewList(categorysQuery.Where(x => x.CategoryParentId is null).ToList(), tv_categorys);

            }


        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        async Task Clear()
        {
            this.DataContext = new Category();
            //dg_category.SelectedIndex = -1;
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            await FillCombo.fillCategorysWithDefault(cb_CategoryParentId);
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
       
        void buildTreeViewList(List<Category> _categories, TreeView treeViewItemParent )
        {
            foreach (var item in _categories)
            {
                TreeViewItem treeViewItem = new TreeViewItem();
                treeViewItem.Tag = item.CategoryId.ToString();
                treeViewItem.Header = item.Name;
                treeViewItem.FontSize = 18;
                treeViewItem.Foreground = Application.Current.Resources["textColor"] as SolidColorBrush; ;
                treeViewItem.Selected += TreeViewItem_Selected;

                treeViewItemParent.Items.Add(treeViewItem);
                if(categorysQuery.Where(x => x.CategoryParentId == item.CategoryId).ToList().Count() >0)
                {
                    buildTreeViewList(categorysQuery.Where(x=> x.CategoryParentId == item.CategoryId ).ToList()  , treeViewItem);
                }
                 
                
            }
        }
        void buildTreeViewList(List<Category> _categories, TreeViewItem treeViewItemParent)
        {
            foreach (var item in _categories)
            {
                TreeViewItem treeViewItem = new TreeViewItem();
                treeViewItem.Tag = item.CategoryId.ToString();
                treeViewItem.Header = item.Name;
                treeViewItem.FontSize = 18;
                treeViewItem.Foreground = Application.Current.Resources["textColor"] as SolidColorBrush; ;
                treeViewItem.Selected += TreeViewItem_Selected;

                treeViewItemParent.Items.Add(treeViewItem);
                if (categorysQuery.Where(x => x.CategoryParentId == item.CategoryId).ToList().Count() > 0)
                {
                    buildTreeViewList(categorysQuery.Where(x => x.CategoryParentId == item.CategoryId).ToList(), treeViewItem);
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
                category = FillCombo.categoryList.Where(x => x.CategoryId ==long.Parse( treeViewItem.Tag.ToString())).FirstOrDefault();
                this.DataContext = category;
                //MessageBox.Show($"Category Id is: {treeViewItem.Tag}, Category Name: {treeViewItem.Header}");

            }
            treeViewItem.IsExpanded = true;
        }
        void unExpandTreeViewItem()
        {
            List<TreeViewItem> list  = FindControls.FindVisualChildren<TreeViewItem>(this).ToList();
            foreach (var item in list)
            {
                if(!item.IsSelected)
                item.IsExpanded = false;
            }
        }
        void setSelectedStyleTreeViewItem()
        {
            List<TreeViewItem> list  = FindControls.FindVisualChildren<TreeViewItem>(this).ToList();
            foreach (var item in list)
            {
                if(!item.IsSelected)
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

    }
}
