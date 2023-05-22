using Microsoft.Win32;
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

namespace POSCA.View.sectionData.vendors
{
    /// <summary>
    /// Interaction logic for uc_supportVendors.xaml
    /// </summary>
    public partial class uc_supportVendors : UserControl
    {

        public uc_supportVendors()
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
        private static uc_supportVendors _instance;
        public static uc_supportVendors Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_supportVendors();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        AssistantSupplier assistantSupplier = new AssistantSupplier();
        IEnumerable<AssistantSupplier> assistantSuppliersQuery;
        string searchText = "";
        public static List<string> requiredControlList;

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "Name" };
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


                //FillCombo.FillDefaultPayType_cashBalanceCardMultiple(cb_payType);
                Keyboard.Focus(tb_Name);
                /*
                if (FillCombo.assistantSuppliersListAll is null)
                    await RefreshCustomersList();
                else
                    assistantSuppliers = FillCombo.assistantSuppliersListAll.ToList();
                */
                await Search();
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

            txt_title.Text = AppSettings.resourcemanager.GetString("AssistantSuppliers");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            txt_IsBlocked.Text = AppSettings.resourcemanager.GetString("IsBlocked");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Name, AppSettings.resourcemanager.GetString("trNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));
            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trSave");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            dg_assistantSupplier.Columns[0].Header = AppSettings.resourcemanager.GetString("trNo");
            dg_assistantSupplier.Columns[1].Header = AppSettings.resourcemanager.GetString("trName");
            dg_assistantSupplier.Columns[2].Header = AppSettings.resourcemanager.GetString("trNote");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");

            //tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            //tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            //tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            //tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            //tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            //tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {

                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                //{
                HelpClass.StartAwait(grid_main);

                assistantSupplier = new AssistantSupplier();
                if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                {
                    assistantSupplier.Name = tb_Name.Text;
                    if (tgl_IsBlocked.IsChecked == true)
                        assistantSupplier.IsBlocked = true;
                    else
                        assistantSupplier.IsBlocked = false;

                    assistantSupplier.Notes = tb_Notes.Text;
                    assistantSupplier.CreateUserId = MainWindow.userLogin.userId;

                    FillCombo.assistantSupplierList = await assistantSupplier.save(assistantSupplier);
                    if (FillCombo.assistantSupplierList == null)
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    else
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                        Clear();
                        await Search();
                    }
                }
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
        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {

                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (assistantSupplier.AssistantSupId > 0)
                    {
                        if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                        {

                            assistantSupplier.Name = tb_Name.Text;
                            if (tgl_IsBlocked.IsChecked == true)
                                assistantSupplier.IsBlocked = true;
                            else
                                assistantSupplier.IsBlocked = false;

                            assistantSupplier.Notes = tb_Notes.Text;
                            assistantSupplier.UpdateUserId = MainWindow.userLogin.userId;

                            FillCombo.assistantSupplierList = await assistantSupplier.save(assistantSupplier);
                            if (FillCombo.assistantSupplierList == null)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);

                                await Search();

                            }
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
                    if (assistantSupplier.AssistantSupId != 0)
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
                            FillCombo.assistantSupplierList = await assistantSupplier.delete(assistantSupplier.AssistantSupId, MainWindow.userLogin.userId);
                            if (FillCombo.assistantSupplierList == null)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                assistantSupplier.AssistantSupId = 0;
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                await Search();
                                Clear();
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
                if (tb_search.Text != "")
                {
                    //dina search
                    //suppliers = await FillCombo.supplier.searchSuppliers(tb_search.Text);
                    //RefreshSuppliersView();
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
        //private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    try
        //    {
        //        HelpClass.StartAwait(grid_main);
        //        await Search();
        //        HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
        //    }
        //}

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
        private async void Dg_assistantSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection

                if (dg_assistantSupplier.SelectedIndex != -1)
                {
                    assistantSupplier = dg_assistantSupplier.SelectedItem as AssistantSupplier;
                    this.DataContext = assistantSupplier;

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

                tb_search.Text = "";
                searchText = "";
                await RefreshAssistantList();
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
            if (FillCombo.assistantSupplierList is null)
                await RefreshAssistantList();
            searchText = tb_search.Text.ToLower();
            assistantSuppliersQuery = FillCombo.assistantSupplierList.Where(s =>
            s.Name.ToLower().Contains(searchText)
           || s.AssistantSupId.ToString().Contains(searchText)
           ).ToList();
            RefreshGroupsView();
        }
        async Task<IEnumerable<AssistantSupplier>> RefreshAssistantList()
        {
            await FillCombo.RefreshAssistantSuppliers();

            return FillCombo.assistantSupplierList;
        }
        void RefreshGroupsView()
        {
            dg_assistantSupplier.ItemsSource = assistantSuppliersQuery;
            txt_count.Text = assistantSuppliersQuery.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new AssistantSupplier();

            dg_assistantSupplier.SelectedIndex = -1;
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



        private void btn_columnSwap_Click(object sender, RoutedEventArgs e)
        {
            ColumnDefinition cd_gridMain3 = new ColumnDefinition();
            cd_gridMain3.Width = cd_gridMain1.Width;
            cd_gridMain1.Width = cd_gridMain2.Width;
            cd_gridMain2.Width = cd_gridMain3.Width;
        }

    }
}
