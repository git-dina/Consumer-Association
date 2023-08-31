using Microsoft.Reporting.WebForms;
using netoaster;
using POSCA.Classes;
using POSCA.Classes.ApiClasses;
using System;
using System.Collections.Generic;
using System.IO;
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
            txt_title.Text = AppSettings.resourcemanager.GetString("ActivitiesReport");


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

            btn_search.Content = AppSettings.resourcemanager.GetString("trSearch");
            btn_clear.Content = AppSettings.resourcemanager.GetString("trClear");
            btn_export.Content = AppSettings.resourcemanager.GetString("trExport");
            btn_print.Content = AppSettings.resourcemanager.GetString("trPrint");

            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
        }
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        async Task Clear()
        {
            dg_customerActivity.ItemsSource = null;

          
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

        private async void Btn_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                long boxNumberFrom = 0;
                long boxNumberTo = 0;
                if (tgl_BoxNumber.IsChecked == true)
                {
                    if (tb_BoxNumberFrom.Text != "")
                        boxNumberFrom = long.Parse(tb_BoxNumberFrom.Text);


                    if (tb_BoxNumberTo.Text != "")
                        boxNumberTo = long.Parse(tb_BoxNumberTo.Text);
                }

                 long customerIdFrom = 0;
                long customerIdTo = 0;
                if (tgl_CustomerNumber.IsChecked == true)
                {
                    if (tb_CustomerNumberFrom.Text != "")
                        customerIdFrom = long.Parse(tb_CustomerNumberFrom.Text);
                    if (tb_CustomerNumberTo.Text != "")
                        customerIdTo = long.Parse(tb_CustomerNumberTo.Text);
                }
                string customerName = "";
                if(tgl_CustomerName.IsChecked == true)
                    customerName = tb_CustomerNameFrom.Text;

                long activityId = 0;

                if (tgl_Activities.IsChecked == true)
                if (cb_Activities.SelectedIndex != -1)
                    activityId =(long) cb_Activities.SelectedValue;

                DateTime? activityStartDateFrom = null;
                DateTime? activityStartDateTo = null;

                if (tgl_ActivitiesStartDate.IsChecked == true)
                {
                    if (dp_ActivitiesStartDateFrom.SelectedDate != null)
                        activityStartDateFrom = dp_ActivitiesStartDateFrom.SelectedDate;
                    if (dp_ActivitiesStartDateTo.SelectedDate != null)
                        activityStartDateTo = dp_ActivitiesStartDateTo.SelectedDate;
                }
                DateTime? activityEndDateFrom = null;
                DateTime? activityEndDateTo = null;
                if (tgl_ActivitiesEndDate.IsChecked == true)
                {
                    if (dp_ActivitiesEndDateFrom.SelectedDate != null)
                        activityEndDateFrom = dp_ActivitiesEndDateFrom.SelectedDate;
                    if (dp_ActivitiesEndDateTo.SelectedDate != null)
                        activityEndDateTo = dp_ActivitiesEndDateTo.SelectedDate;
                }

                 DateTime? joinDateFrom = null;
                DateTime? joinDateTo = null;
                if (tgl_JoinDate.IsChecked == true)
                {
                    if (dp_JoinDateFrom.SelectedDate != null)
                        joinDateFrom = dp_JoinDateFrom.SelectedDate;
                    if (dp_JoinDateTo.SelectedDate != null)
                        joinDateTo = dp_JoinDateTo.SelectedDate;
                }
                var res = await customerActivity.GetActivitiesReport(boxNumberFrom,boxNumberTo,customerIdFrom,customerIdTo,
                                            customerName,activityId,activityStartDateFrom,activityStartDateTo,
                                            activityEndDateFrom, activityEndDateTo, joinDateFrom, joinDateTo);

                dg_customerActivity.ItemsSource = null;
                dg_customerActivity.ItemsSource = res;
                dg_customerActivity.Items.Refresh();
                HelpClass.EndAwait(grid_main);

            }
            catch
            {
                HelpClass.EndAwait(grid_main);

            }
        }

        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

               // if (MainWindow.groupObject.HasPermissionAction(basicsPermission, MainWindow.groupObjects, "report") || SectionData.isAdminPermision())
                {
                    #region
                    Microsoft.Office.Interop.Excel.Application excel = null;
                    Microsoft.Office.Interop.Excel.Workbook wb = null;
                    object missing = Type.Missing;
                    Microsoft.Office.Interop.Excel.Worksheet ws = null;
                    Microsoft.Office.Interop.Excel.Range rng = null;

                    // collection of DataGrid Items
                    var dtExcelDataTable = dg_customerActivity.ItemsSource;

                    excel = new Microsoft.Office.Interop.Excel.Application();
                    wb = excel.Workbooks.Add();
                    ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.ActiveSheet;
                    ws.Columns.AutoFit();
                    ws.Columns.EntireColumn.ColumnWidth = 25;

                    // Header row
                    for (int Idx = 0; Idx < dg_customerActivity.Columns.Count; Idx++)
                    {
                        ws.Range["A1"].Offset[0, Idx].Value = dg_customerActivity.Columns[Idx].Header;
                    }

                    // Data Rows
                    for (int i = 0; i < dg_customerActivity.Columns.Count; i++)
                    {
                        for (int j = 0; j < dg_customerActivity.Items.Count; j++)
                        {
                            TextBlock b = dg_customerActivity.Columns[i].GetCellContent(dg_customerActivity.Items[j]) as TextBlock;
                            Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)ws.Cells[j + 2, i + 1];
                            myRange.NumberFormat = "@";
                            myRange.Value2 = b.Text;
                        }
                    }

                    excel.Visible = true;
                    wb.Activate();
                    #endregion
                }
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
            
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
