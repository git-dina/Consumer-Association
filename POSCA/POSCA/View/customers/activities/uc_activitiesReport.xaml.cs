using POSCA.Classes;
using POSCA.Classes.ApiClasses;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POSCA.View.customers.activities
{
    /// <summary>
    /// Interaction logic for uc_activitiesReport.xaml
    /// </summary>
    public partial class uc_activitiesReport : UserControl
    {
        public uc_activitiesReport()
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
        private static uc_activitiesReport _instance;
        public static uc_activitiesReport Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_activitiesReport();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        CustomerActivity customerActivity = new CustomerActivity();
        IEnumerable<CustomerActivity> activitiesQuery;

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
                
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }

                translate();
                //await Search();
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
            txt_BoxNumber.Text = AppSettings.resourcemanager.GetString("BoxNumber");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_BoxNumberFrom, AppSettings.resourcemanager.GetString("FromHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_BoxNumberTo, AppSettings.resourcemanager.GetString("ToHint"));
            txt_CustomerNumber.Text = AppSettings.resourcemanager.GetString("CustomerNo");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerNumberFrom, AppSettings.resourcemanager.GetString("FromHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerNumberTo, AppSettings.resourcemanager.GetString("ToHint"));
            txt_CustomerName.Text = AppSettings.resourcemanager.GetString("CustomerName");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerNameFrom, AppSettings.resourcemanager.GetString("trNameHint"));
             txt_Activities.Text = AppSettings.resourcemanager.GetString("TheActivity");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_Activities, AppSettings.resourcemanager.GetString("ActivityHint"));
            txt_ActivitiesStartDate.Text = AppSettings.resourcemanager.GetString("ActivityStartDate");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_ActivitiesStartDateFrom, AppSettings.resourcemanager.GetString("FromHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_ActivitiesStartDateTo, AppSettings.resourcemanager.GetString("ToHint"));
            txt_ActivitiesEndDate.Text = AppSettings.resourcemanager.GetString("ActivityEndDate");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_ActivitiesEndDateFrom, AppSettings.resourcemanager.GetString("FromHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_ActivitiesEndDateTo, AppSettings.resourcemanager.GetString("ToHint"));
            txt_JoinDate.Text = AppSettings.resourcemanager.GetString("JoinDate");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_JoinDateFrom, AppSettings.resourcemanager.GetString("FromHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_JoinDateTo, AppSettings.resourcemanager.GetString("ToHint"));

            dg_customerActivity.Columns[0].Header = AppSettings.resourcemanager.GetString("BoxNumber");
            dg_customerActivity.Columns[1].Header = AppSettings.resourcemanager.GetString("CustomerNo");
            dg_customerActivity.Columns[2].Header = AppSettings.resourcemanager.GetString("CivilNo");
            dg_customerActivity.Columns[3].Header = AppSettings.resourcemanager.GetString("CustomerName");
            dg_customerActivity.Columns[4].Header = AppSettings.resourcemanager.GetString("TheActivity");
            dg_customerActivity.Columns[5].Header = AppSettings.resourcemanager.GetString("ActivityValueBeforeDisc");
            dg_customerActivity.Columns[6].Header = AppSettings.resourcemanager.GetString("ActivityValueAfterDisc");
            dg_customerActivity.Columns[7].Header = AppSettings.resourcemanager.GetString("UsedNumber");
            dg_customerActivity.Columns[8].Header = AppSettings.resourcemanager.GetString("FamilyCard");
            dg_customerActivity.Columns[9].Header = AppSettings.resourcemanager.GetString("ActivityStartDate");
            dg_customerActivity.Columns[10].Header = AppSettings.resourcemanager.GetString("ActivityEndDate");
            dg_customerActivity.Columns[11].Header = AppSettings.resourcemanager.GetString("RegistrationDate");
            dg_customerActivity.Columns[12].Header = AppSettings.resourcemanager.GetString("trNotes");
            btn_search.ToolTip = AppSettings.resourcemanager.GetString("trSearch");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            btn_export.ToolTip = AppSettings.resourcemanager.GetString("trExport");
            btn_print.ToolTip = AppSettings.resourcemanager.GetString("trPrint");

            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
        }
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        async Task Clear()
        {
            customerActivity = new CustomerActivity();
            this.DataContext = new CustomerActivity();
            dg_customerActivity.SelectedIndex = -1;

          
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
                //HelpClass.validate(requiredControlList, this);
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
                //HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }




        #endregion

        private void Btn_search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_export_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
