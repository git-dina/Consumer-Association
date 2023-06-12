using netoaster;
using POSCA.Classes;
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
using Microsoft.Win32;
using System.IO;
using POSCA.View.windows;
using POSCA.Classes.ApiClasses;

namespace POSCA.View.locations
{
    /// <summary>
    /// Interaction logic for uc_locationsData.xaml
    /// </summary>
    public partial class uc_locationsData : UserControl
    {
        public uc_locationsData()
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
        private static uc_locationsData _instance;
        public static uc_locationsData Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_locationsData();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        //string basicsPermission = "locations_basics";
        Location location = new Location();
        IEnumerable<Location> locationsQuery;
        IEnumerable<Location> locations;
        byte tgl_locationState;
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
                requiredControlList = new List<string> { "LocationNumber", "Name", "LocationTypeId" };
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


                await FillCombo.fillLocationTypes(cb_LocationTypeId);
                Keyboard.Focus(tb_Name);
                /*
                if (FillCombo.locationsListAll is null)
                    await RefreshCustomersList();
                else
                    locations = FillCombo.locationsListAll.ToList();
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

            txt_title.Text = AppSettings.resourcemanager.GetString("Location");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_LocationNumber, AppSettings.resourcemanager.GetString("LocationNumHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_LocationTypeId, AppSettings.resourcemanager.GetString("LocationTypeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Name, AppSettings.resourcemanager.GetString("trNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_address, AppSettings.resourcemanager.GetString("trAdressHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("GeneralNotesHint"));
           
            txt_IsBlocked.Text = AppSettings.resourcemanager.GetString("IsBlocked");
            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trSave");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            dg_location.Columns[0].Header = AppSettings.resourcemanager.GetString("trName");
            dg_location.Columns[1].Header = AppSettings.resourcemanager.GetString("trType");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");

            //tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            //tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            //tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            //tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            //tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            //tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");
            //tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

            //txt_active.Text = AppSettings.resourcemanager.GetString("trActive_");
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                // if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);

                    location = new Location();
                    if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                    {
                        location.Name = tb_Name.Text;
                        location.LocationNumber = tb_LocationNumber.Text;

                        if (tgl_IsBlocked.IsChecked == true)
                            location.IsBlocked = true;
                        else
                            location.IsBlocked = false;

                        if (cb_LocationTypeId.SelectedIndex >= 0)
                            location.LocationTypeId = (int)cb_LocationTypeId.SelectedValue;
                     

                        location.Address = tb_address.Text;
                        location.Notes = tb_Notes.Text;
                        location.CreateUserId = MainWindow.userLogin.userId;

                        FillCombo.locationsList = await location.save(location);
                        if (FillCombo.locationsList == null)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                            Clear();
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
                if (location.LocationId > 0)
                {
                    if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                    {
                        location.Name = tb_Name.Text;
                        location.LocationNumber = tb_LocationNumber.Text;

                        if (tgl_IsBlocked.IsChecked == true)
                            location.IsBlocked = true;
                        else
                            location.IsBlocked = false;

                        if (cb_LocationTypeId.SelectedIndex >= 0)
                            location.LocationTypeId = (int)cb_LocationTypeId.SelectedValue;


                        location.Address = tb_address.Text;
                        location.Notes = tb_Notes.Text;
                        location.CreateUserId = MainWindow.userLogin.userId;

                        FillCombo.locationsList = await location.save(location);
                        if (FillCombo.locationsList == null)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                            await Search();
                            if (dg_location.SelectedIndex != -1)
                            {
                                location = dg_location.SelectedItem as Location;
                            }
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

                // if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (location.LocationId != 0)
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
                            FillCombo.locationsList = await location.delete(location.LocationId, MainWindow.userLogin.userId);
                            if (FillCombo.locationsList == null)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                location.LocationId = 0;
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
        //private async Task activate()
        //{//activate
        //    location.isActive = 1;
        //    var s = await location.save(location);
        //    if (s <= 0)
        //        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
        //    else
        //    {
        //        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopActive"), animation: ToasterAnimation.FadeIn);
        //        await RefreshCustomersList();
        //        await Search();
        //    }
        //}
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

                Btn_refresh_Click(new Button(), null);
                Tb_search_TextChanged(tb_search, null);


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
        private async void Dg_location_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //HelpClass.StartAwait(grid_main);
                //selection

                if (dg_location.SelectedIndex != -1)
                {
                    location = dg_location.SelectedItem as Location;
                    this.DataContext = location;


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
                await RefreshLocationsList();
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
            if (FillCombo.locationsList is null)
                await RefreshLocationsList();
            searchText = tb_search.Text.ToLower();
            locationsQuery = FillCombo.locationsList.Where(s =>
            s.LocationId.ToString().Contains(searchText) ||
            s.Name.ToLower().Contains(searchText)
             || s.Name.ToLower().Contains(searchText)
            ).ToList();
            RefreshLocationsView();
        }
        async Task<IEnumerable<Location>> RefreshLocationsList()
        {

            await FillCombo.RefreshLocations();
            locations = FillCombo.locationsList.ToList();

            return locations;
        }
        void RefreshLocationsView()
        {
            dg_location.ItemsSource = null;
            dg_location.ItemsSource = locationsQuery;
            txt_count.Text = locationsQuery.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new Location();
            dg_location.SelectedIndex = -1;
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

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
        /*
        #region Phone
        int? countryid;
        private async void Cb_areaPhone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (cb_areaPhone.SelectedValue != null)
                {
                    if (cb_areaPhone.SelectedIndex >= 0)
                    {
                        countryid = int.Parse(cb_areaPhone.SelectedValue.ToString());
                        await FillCombo.fillCountriesLocal(cb_areaPhoneLocal, (long)countryid, brd_areaPhoneLocal);
                    }
                }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void Cb_areaFax_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (cb_areaFax.SelectedValue != null)
                {
                    if (cb_areaFax.SelectedIndex >= 0)
                    {
                        countryid = int.Parse(cb_areaFax.SelectedValue.ToString());
                        await FillCombo.fillCountriesLocal(cb_areaFaxLocal, (long)countryid, brd_areaFaxLocal);
                    }
                }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        #endregion
        */
        #region Image
        /*

        string imgFileName = "pic/no-image-icon-125x125.png";
        bool isImgPressed = false;
        OpenFileDialog openFileDialog = new OpenFileDialog();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        private void Btn_image_Click(object sender, RoutedEventArgs e)
        {
            //select image
            try
            {
                HelpClass.StartAwait(grid_main);
                isImgPressed = true;
                openFileDialog.Filter = "Images|*.png;*.jpg;*.bmp;*.jpeg;*.jfif";
                if (openFileDialog.ShowDialog() == true)
                {
                    HelpClass.imageBrush = new ImageBrush();
                    HelpClass.imageBrush.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Relative));
                    btn_image.Background = HelpClass.imageBrush;
                    imgFileName = openFileDialog.FileName;
                }
                else
                { }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

private async Task getImg()
{
try
{
HelpClass.StartAwait(grid_image, "forImage");
if (string.IsNullOrEmpty(location.image))
{
HelpClass.clearImg(btn_image);
}
else
{
byte[] imageBuffer = await location.downloadImage(location.image); // read this as BLOB from your DB

var bitmapImage = new BitmapImage();
if (imageBuffer != null)
{
using (var memoryStream = new MemoryStream(imageBuffer))
{
bitmapImage.BeginInit();
bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
bitmapImage.StreamSource = memoryStream;
bitmapImage.EndInit();
}

btn_image.Background = new ImageBrush(bitmapImage);
// configure trmporary path
string dir = Directory.GetCurrentDirectory();
string tmpPath = System.IO.Path.Combine(dir, Global.TMPLocationsFolder, location.image);
openFileDialog.FileName = tmpPath;
}
else
HelpClass.clearImg(btn_image);
}
HelpClass.EndAwait(grid_image, "forImage");
}
catch
{
HelpClass.EndAwait(grid_image, "forImage");
}
}
*/

        #endregion

       

        private async void Btn_addLocationType_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_userControl w = new wd_userControl();

                w.grid_uc.Children.Add(uc_locationType.Instance);
                w.ShowDialog();
                uc_locationType.Instance.UserControl_Unloaded(uc_locationType.Instance, null);
                await FillCombo.RefreshLocationTypes();
                await FillCombo.fillLocationTypes(cb_LocationTypeId);
                //if (w.isOk)
                //{

                //}
                Window.GetWindow(this).Opacity = 1;

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Tb_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (location.LocationId == 0)
                tb_Name.Text = tb_Name.Text;
        }

        /*
         #region report
         //report  parameters
         ReportCls reportclass = new ReportCls();
         LocalReport rep = new LocalReport();
         //  SaveFileDialog saveFileDialog = new SaveFileDialog();

         // end report parameters
         public void BuildReport()
         {

             List<ReportParameter> paramarr = new List<ReportParameter>();

             string addpath;
             bool isArabic = ReportCls.checkLang();
             if (isArabic)
             {
                 addpath = @"\Reports\SectionData\persons\Ar\ArLocations.rdlc";
             }
             else
             {
                 addpath = @"\Reports\SectionData\persons\En\EnLocations.rdlc";
             }
             string searchval = "";
             string stateval = "";
             //filter   
             stateval = tgl_isActive.IsChecked == true ? AppSettings.resourcemanagerreport.GetString("trActive_")
               : AppSettings.resourcemanagerreport.GetString("trNotActive");
             paramarr.Add(new ReportParameter("stateval", stateval));
             paramarr.Add(new ReportParameter("trActiveState", AppSettings.resourcemanagerreport.GetString("trState")));
             paramarr.Add(new ReportParameter("trSearch", AppSettings.resourcemanagerreport.GetString("trSearch")));
             searchval = tb_search.Text;
             paramarr.Add(new ReportParameter("searchVal", searchval));
             //end filter
             string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

             clsReports.LocationReport(locationsQuery, rep, reppath, paramarr);
             clsReports.setReportLanguage(paramarr);
             clsReports.Header(paramarr);

             rep.SetParameters(paramarr);

             rep.Refresh();

         }
         private void Btn_pdf_Click(object sender, RoutedEventArgs e)
         {//pdf
             try
             {

                 HelpClass.StartAwait(grid_main);

                 if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                 {
                     #region
                     BuildReport();

                     saveFileDialog.Filter = "PDF|*.pdf;";

                     if (saveFileDialog.ShowDialog() == true)
                     {
                         string filepath = saveFileDialog.FileName;
                         LocalReportExtensions.ExportToPDF(rep, filepath);
                     }
                     #endregion
                 }
                 else
                     Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

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
             try
             {

                 HelpClass.StartAwait(grid_main);
                 if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                 {

                     #region
                     BuildReport();
                     LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, AppSettings.rep_print_count == null ? short.Parse("1") : short.Parse(AppSettings.rep_print_count));
                     #endregion
                 }
                 else
                     Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


                 HelpClass.EndAwait(grid_main);
             }
             catch (Exception ex)
             {

                 HelpClass.EndAwait(grid_main);
                 HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
             }

         }

         private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
         {
             try
             {

                 HelpClass.StartAwait(grid_main);

                 if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                 {
                     #region
                     BuildReport();
                     this.Dispatcher.Invoke(() =>
                     {
                         saveFileDialog.Filter = "EXCEL|*.xls;";
                         if (saveFileDialog.ShowDialog() == true)
                         {
                             string filepath = saveFileDialog.FileName;
                             LocalReportExtensions.ExportToExcel(rep, filepath);
                         }
                     });
                     #endregion
                 }
                 else
                     Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


                 HelpClass.EndAwait(grid_main);
             }
             catch (Exception ex)
             {

                 HelpClass.EndAwait(grid_main);

                 HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
             }
         }

         private void Btn_preview_Click(object sender, RoutedEventArgs e)
         {
             try
             {

                 HelpClass.StartAwait(grid_main);
                 if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                 {
                     #region
                     Window.GetWindow(this).Opacity = 0.2;

                     string pdfpath = "";
                     //
                     pdfpath = @"\Thumb\report\temp.pdf";
                     pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                     BuildReport();

                     LocalReportExtensions.ExportToPDF(rep, pdfpath);
                     wd_previewPdf w = new wd_previewPdf();
                     w.pdfPath = pdfpath;
                     if (!string.IsNullOrEmpty(w.pdfPath))
                     {
                         w.ShowDialog();
                         w.wb_pdfWebViewer.Dispose();


                     }
                     Window.GetWindow(this).Opacity = 1;
                     #endregion
                 }
                 else
                     Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


                 HelpClass.EndAwait(grid_main);
             }
             catch (Exception ex)
             {

                 Window.GetWindow(this).Opacity = 1;
                 HelpClass.EndAwait(grid_main);
                 HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
             }

         }
         private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
         {//pie
             try
             {
                 HelpClass.StartAwait(grid_main);

                 if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                 {
                     #region
                     Window.GetWindow(this).Opacity = 0.2;
                     win_lvc win = new win_lvc(locationsQuery, 1, false);
                     win.ShowDialog();
                     Window.GetWindow(this).Opacity = 1;
                     #endregion
                 }
                 else
                     Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

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
         */

       
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

    }
}
