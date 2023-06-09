using Microsoft.Win32;
using netoaster;
using POSCA.Classes;
using POSCA.Classes.ApiClasses;
using POSCA.View.windows;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace POSCA.View.settings
{
    /// <summary>
    /// Interaction logic for uc_generalSettings.xaml
    /// </summary>
    public partial class uc_generalSettings : UserControl
    {
        CompanySettings setVName = new CompanySettings();
        CompanySettings setVAddress = new CompanySettings();
        CompanySettings setVEmail = new CompanySettings();
        CompanySettings setVArabicName = new CompanySettings(); 
        CompanySettings setVMobile = new CompanySettings();
        CompanySettings setVPhone = new CompanySettings();
        CompanySettings setVFax = new CompanySettings();
        CompanySettings setVLogo = new CompanySettings();
        CompanySettings valueModel = new CompanySettings();

        bool isImgPressed = false;
        OpenFileDialog openFileDialog = new OpenFileDialog();
        string imgFileName = "pic/no-image-icon-125x125.png";
        ImageBrush brush = new ImageBrush();
        public uc_generalSettings()
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
        private static uc_generalSettings _instance;
        public static uc_generalSettings Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_generalSettings();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        
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
                requiredControlList = new List<string> { "companyName", "companyNameAr" };
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
                //fillAccuracy();
                #region get settings Ids

                setVName = await FillCombo.getSettingBySetName("com_name");
                setVArabicName = await FillCombo.getSettingBySetName("com_nameAr");
                setVAddress = await FillCombo.getSettingBySetName("com_address");
                setVEmail = await FillCombo.getSettingBySetName("com_email");
                setVMobile = await FillCombo.getSettingBySetName("com_mobile");
                setVPhone = await FillCombo.getSettingBySetName("com_phone");
                setVFax = await FillCombo.getSettingBySetName("com_fax");
                setVLogo = await FillCombo.getSettingBySetName("com_logo");
                #endregion

                #region set values
                tb_companyName.Text = AppSettings.companyName;
                tb_companyNameAr.Text = AppSettings.companyNameAr;
                tb_companyAddress.Text = AppSettings.companyAddress;
                tb_companyEmail.Text = AppSettings.companyEmail;
                tb_companyFax.Text = AppSettings.companyFax;
                tb_companyPhone.Text = AppSettings.companyPhone;
                tb_companyMobile.Text = AppSettings.companyMobile;

                await getImg();
                #endregion
                Keyboard.Focus(tb_companyName);

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
            
            txt_title.Text = AppSettings.resourcemanager.GetString("GeneralSettings");

            txt_companyInfo.Text = AppSettings.resourcemanager.GetString("AssociationInfo");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_companyName, AppSettings.resourcemanager.GetString("AssociationNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_companyNameAr, AppSettings.resourcemanager.GetString("AssociationNameArHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_companyAddress, AppSettings.resourcemanager.GetString("trAddress"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_companyEmail, AppSettings.resourcemanager.GetString("trEmail"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_companyMobile, AppSettings.resourcemanager.GetString("trMobileHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_companyPhone, AppSettings.resourcemanager.GetString("trPhoneHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_companyFax, AppSettings.resourcemanager.GetString("trFaxHint"));
          
            txt_saveButton.Text = AppSettings.resourcemanager.GetString("trSave");
       

        }
        #region fill
        /*
        private void fillAccuracy()
        {
            var list = new[] {
                new { Text = "0"       , Value = "0" },
                new { Text = "0.0"     , Value = "1" },
                new { Text = "0.00"    , Value = "2" },
                new { Text = "0.000"   , Value = "3" },
                 };
            cb_accuracy.DisplayMemberPath = "Text";
            cb_accuracy.SelectedValuePath = "Value";
            cb_accuracy.ItemsSource = list;
        }
        */
        #endregion

        private async Task getImg()
        {
            try
            {
                if (string.IsNullOrEmpty(setVLogo.Value))
                {
                    HelpClass.clearImg(btn_image);
                }
                else
                {
                    byte[] imageBuffer = await setVLogo.downloadImage(setVLogo.Value); // read this as BLOB from your DB

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
                        string tmpPath = System.IO.Path.Combine(dir, AppSettings.TMPSettingFolder);
                        if (!Directory.Exists(tmpPath))
                            Directory.CreateDirectory(tmpPath);
                        
                        tmpPath = System.IO.Path.Combine(tmpPath, setVLogo.Value);
                        openFileDialog.FileName = tmpPath;
                    }
                    else
                        HelpClass.clearImg(btn_image);
                }
            }
            catch { }
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);

                    if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                    {
                        List<CompanySettings> vlst = new List<CompanySettings>();
                        #region name
                        if ( !tb_companyName.Text.Equals(AppSettings.companyName))
                        {
                            setVName.Value = tb_companyName.Text;
                            setVName.SettingId = setVName.SettingId;

                            vlst.Add(setVName);
                        }
                        #endregion
                        #region arabic name
                        if (!tb_companyNameAr.Text.Equals(AppSettings.companyNameAr))
                        {
                            setVArabicName.Value = tb_companyNameAr.Text;
                            setVArabicName.SettingId = setVArabicName.SettingId;

                            vlst.Add(setVArabicName);
                        }
                        #endregion
                        #region address
                        if (!tb_companyAddress.Text.Equals(AppSettings.companyAddress))
                        {
                            setVAddress.Value = tb_companyAddress.Text;
                            setVAddress.SettingId = setVAddress.SettingId;

                            vlst.Add(setVAddress);
                        }
                        #endregion
                       
                        #region email
                        if ( !tb_companyEmail.Text.Equals(AppSettings.companyEmail))
                        {
                            setVEmail.Value = tb_companyEmail.Text;
                            setVEmail.SettingId = setVEmail.SettingId;

                            vlst.Add(setVEmail);
                        }
                        #endregion
                        #region mobile
                        if (!tb_companyMobile.Text.Equals(AppSettings.companyMobile))
                        {
                            setVMobile.Value =  tb_companyMobile.Text;
                            setVMobile.SettingId = setVMobile.SettingId;

                            vlst.Add(setVMobile);
                        }
                        #endregion
                        #region phone
                        if (!tb_companyPhone.Text.Equals(AppSettings.companyPhone))
                        {
                            setVPhone.Value =  tb_companyPhone.Text;
                            setVMobile.SettingId = setVMobile.SettingId;

                            vlst.Add(setVPhone);
                        }
                        #endregion
                        #region fax
                        if (!tb_companyFax.Text.Equals(AppSettings.companyFax))
                        {
                            setVFax.Value =tb_companyFax.Text;
                            setVMobile.SettingId = setVMobile.SettingId;

                            vlst.Add(setVFax);
                        }
                        #endregion
                        #region logo
                        int sLogo = 0;
                        if (isImgPressed)
                        {

                            setVLogo.Value = sLogo.ToString();
                            setVMobile.SettingId = setVMobile.SettingId;


                            vlst.Add(setVLogo);
                        }
                        #endregion

                        int res = (int)await valueModel.SaveList(vlst);
                        if (!res.Equals(0))
                        {
                            AppSettings.companyName = tb_companyName.Text;
                            AppSettings.companyNameAr = tb_companyNameAr.Text;
                            AppSettings.companyAddress = tb_companyAddress.Text;
                            AppSettings.companyEmail = tb_companyEmail.Text;
                            AppSettings.companyMobile = tb_companyMobile.Text;
                            AppSettings.companyPhone = tb_companyPhone.Text;
                            AppSettings.companyFax = tb_companyFax.Text;
                            AppSettings.companylogoImage = setVLogo.Value;

                            if (isImgPressed)
                            {
                                string b = await setVLogo.uploadImage(imgFileName, Md5Encription.MD5Hash("Inc-m" + sLogo), setVLogo.SettingId);
                                setVLogo.Value = b;
                                AppSettings.companylogoImage = b;
                                isImgPressed = false;

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

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_image_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender != null)
                    HelpClass.StartAwait(grid_main);

                isImgPressed = true;
                openFileDialog.Filter = "Images|*.png;*.jpg;*.bmp;*.jpeg;*.jfif";
                if (openFileDialog.ShowDialog() == true)
                {
                    brush.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Relative));
                    btn_image.Background = brush;
                    imgFileName = openFileDialog.FileName;
                }
                if (sender != null)
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                if (sender != null)
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion
        #region events

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
       
        #endregion
        #region Refresh & Search
       
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new Bank();


            p_error_email.Visibility = Visibility.Collapsed;
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

      
    }
}
