﻿using Microsoft.Win32;
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

namespace POSCA.View.customers
{
    /// <summary>
    /// Interaction logic for uc_familyCard.xaml
    /// </summary>
    public partial class uc_familyCard : UserControl
    {

        public uc_familyCard()
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
        private static uc_familyCard _instance;
        public static uc_familyCard Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_familyCard();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        FamilyCard familyCard = new FamilyCard();
        Customer customer;
        List<FamilyCard> familyCards;
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
                requiredControlList = new List<string> { "CustomerId", "ReleaseDate"};
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


                Keyboard.Focus(tb_CustomerId);

                await fillKinshipTiess();

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
      
            txt_title.Text = AppSettings.resourcemanager.GetString("FamilyCard");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            txt_search.Text = AppSettings.resourcemanager.GetString("trSearch");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_FamilyCardId, AppSettings.resourcemanager.GetString("FamilyCardNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerId, AppSettings.resourcemanager.GetString("CustomerNoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerName, AppSettings.resourcemanager.GetString("CustomerNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerStatus, AppSettings.resourcemanager.GetString("CustomerStatusHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_BoxNumber, AppSettings.resourcemanager.GetString("BoxNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CivilNum, AppSettings.resourcemanager.GetString("CivilNoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_ReleaseDate, AppSettings.resourcemanager.GetString("ReleaseDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_AutomatedNumber, AppSettings.resourcemanager.GetString("AutomtedNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            txt_IsStopped.Text = AppSettings.resourcemanager.GetString("StoppingCard");
            txt_escortInformation.Text = AppSettings.resourcemanager.GetString("EscortsInformation");
            dg_escort.Columns[0].Header = AppSettings.resourcemanager.GetString("IsCustomer");
            dg_escort.Columns[1].Header = AppSettings.resourcemanager.GetString("BoxNumber");
            dg_escort.Columns[2].Header = AppSettings.resourcemanager.GetString("CivilNo");
            dg_escort.Columns[3].Header = AppSettings.resourcemanager.GetString("trName");
            dg_escort.Columns[4].Header = AppSettings.resourcemanager.GetString("Relationship");
            dg_escort.Columns[5].Header = AppSettings.resourcemanager.GetString("AddedDate");

            dg_familyCard.Columns[0].Header = AppSettings.resourcemanager.GetString("CustomerNo");
            dg_familyCard.Columns[1].Header = AppSettings.resourcemanager.GetString("BoxNumber");
            dg_familyCard.Columns[2].Header = AppSettings.resourcemanager.GetString("CustomerName");
            dg_familyCard.Columns[3].Header = AppSettings.resourcemanager.GetString("CivilNo");
            dg_familyCard.Columns[4].Header = AppSettings.resourcemanager.GetString("CurrentCustomerPur");
            dg_familyCard.Columns[5].Header = AppSettings.resourcemanager.GetString("SharesCount");
          
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");

            txt_addButton.Text = AppSettings.resourcemanager.GetString("trSave");
            txt_cardActiveButton.Text = AppSettings.resourcemanager.GetString("CardActivities");

            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
        
        }
        private async Task fillKinshipTiess()
        {
            if (FillCombo.kinshipTiesList is null)
                await FillCombo.RefreshKinshipTiess();

            cb_KinshipId.DisplayMemberPath = "Name";
            cb_KinshipId.SelectedValuePath = "KinshipId";
            cb_KinshipId.ItemsSource = FillCombo.kinshipTiesList;
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
           
                    {
                    HelpClass.StartAwait(grid_main);
                   

                    if (HelpClass.validate(requiredControlList, this))
                    {
                        foreach(var row in familyCard.Escorts)
                        {
                            row.CustomerId = long.Parse(tb_CustomerId.Text);

                            if((row.IsCustomer.Equals(true) && (row.BoxNumber == null || row.BoxNumber.Equals(""))  ) 
                                || (row.IsCustomer.Equals(false) && (row.CivilNum == null || row.CivilNum.Equals("") || row.EscortName == null || row.EscortName.Equals("")  )   )
                               || row.AddedDate == null )
                                {

                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("MissingEscortsData"), animation: ToasterAnimation.FadeIn);
                                HelpClass.EndAwait(grid_main); 
                                return;
                            }
                            if(row.CivilNum == null || row.CivilNum.Length < 12)
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CivilNumLengthAlert"), animation: ToasterAnimation.FadeIn);
                                HelpClass.EndAwait(grid_main);
                                return;
                            }
                        }
                        familyCard.CustomerId = long.Parse(tb_CustomerId.Text);
                        familyCard.ReleaseDate = dp_ReleaseDate.SelectedDate;
                        if (tgl_IsStopped.IsChecked == true)
                            familyCard.IsStopped = true;
                        else
                            familyCard.IsStopped = false;
                        familyCard.Notes = tb_Notes.Text;
                        familyCard.CreateUserId = MainWindow.userLogin.UserId;
                        familyCard.UpdateUserId = MainWindow.userLogin.UserId;

                        var res = await familyCard.save(familyCard);
                        if (res.FamilyCardId == 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                            //Clear();

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
        
        

        #endregion
        #region events

        private async void Btn_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Search();
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
        private async void Dg_familyCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection

                if (dg_familyCard.SelectedIndex != -1)
                {
                    familyCard = dg_familyCard.SelectedItem as FamilyCard;

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

                // await RefreshFamilyCardsList();
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
         
            familyCards = await customer.SearchFamilyCardsCustomers( tb_search.Text);
            RefreshFamilyCardsView();
      
        }

        void RefreshFamilyCardsView()
        {
            dg_familyCard.ItemsSource = familyCards;
            txt_count.Text = familyCards.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        private async Task Clear()
        {
            familyCard = new FamilyCard();
            this.DataContext = new FamilyCard();
            dg_familyCard.SelectedIndex = -1;

            var maxCardId = await familyCard.GetMaxFamilyCardId();
            tb_FamilyCardId.Text = maxCardId.ToString();

            #region clear inputs
            tb_CustomerId.Text = "";
           tb_CustomerName.Text = "";
            tb_CustomerStatus.Text = "";
            tb_CivilNum.Text = "";
            tb_AutomatedNumber.Text = "";
            tb_BoxNumber.Text = "";
            #endregion
            RefreshEscortDataGrid();
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

            //if (tb_search.Text != "")
            //    Btn_search_Click(null, null);
            //else
            //{
            //    familyCards = new List<FamilyCard>();
            //    RefreshFamilyCardsView();
            //}

        }
        void swapToData()
        {
            cd_gridMain1.Width = new GridLength(0, GridUnitType.Star);
            cd_gridMain2.Width = new GridLength(1, GridUnitType.Star);

            this.DataContext = familyCard;
          
            //tb_CustomerId.Text = familyCard.CustomerId.ToString();
            //dp_ReleaseDate.SelectedDate = familyCard.ReleaseDate;
            //tb_CustomerName.Text = familyCard.CustomerName;
            //tb_BoxNumber.Text = familyCard.BoxNumber.ToString();
            //tb_CustomerStatus.Text = familyCard.CustomerStatus;
            //tb_CivilNum.Text = familyCard.CivilNum;
            //tb_AutomatedNumber.Text = familyCard.AutomatedNumber;

            //if (familyCard.IsStopped == true)
            //    tgl_IsStopped.IsChecked = true;
            //else
            //    tgl_IsStopped.IsChecked = false;

            RefreshEscortDataGrid();



        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            swapToData();
        }

        #endregion



        private async void tb_CustomerId_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.Key == Key.Return && tb_CustomerId.Text != "")
                {
                    familyCard = new FamilyCard();
                    HelpClass.StartAwait(grid_main);

                    familyCard = await FillCombo.customer.GetFamilyCardById(long.Parse(tb_CustomerId.Text));

                    if (familyCard != null)
                    {
                        if (familyCard.CustomerStatus.Trim() != "continouse")
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CustomerNotContinouse"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            tb_CustomerName.Text = familyCard.CustomerName;
                            tb_CustomerStatus.Text = AppSettings.resourcemanager.GetString(familyCard.CustomerStatus);
                            tb_BoxNumber.Text = familyCard.BoxNumber.ToString();
                            tb_CivilNum.Text = familyCard.CivilNum.ToString();
                           tb_AutomatedNumber.Text = familyCard.AutomatedNumber.ToString();

                        }
                    }
                    else
                    {
                        await Clear();
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("NumberNotTrue"), animation: ToasterAnimation.FadeIn);
                    }
                    this.DataContext = familyCard;
                    RefreshEscortDataGrid();
                    HelpClass.EndAwait(grid_main);

                }
   
            }
            catch
            {
                HelpClass.EndAwait(grid_main);
            }
        }
        private async void tb_BoxNumber_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.Key == Key.Return && tb_BoxNumber.Text != "")
                {
                    familyCard = new FamilyCard();
                    HelpClass.StartAwait(grid_main);

                    familyCard = await FillCombo.customer.GetFamilyCardByBoxNumber(long.Parse(tb_BoxNumber.Text));

                    if (familyCard != null)
                    {
                        if (familyCard.CustomerStatus.Trim() != "continouse")
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CustomerNotContinouse"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            tb_CustomerId.Text = familyCard.CustomerId.ToString();
                            tb_CustomerName.Text = familyCard.CustomerName;
                            tb_CustomerStatus.Text = AppSettings.resourcemanager.GetString(familyCard.CustomerStatus);
                            //tb_BoxNumber.Text = familyCard.BoxNumber.ToString();
                            tb_CivilNum.Text = familyCard.CivilNum.ToString();
                            tb_AutomatedNumber.Text = familyCard.AutomatedNumber.ToString();

                        }
                    }
                    else
                    {
                        await Clear();
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("NumberNotTrue"), animation: ToasterAnimation.FadeIn);
                    }
                    this.DataContext = familyCard;
                    RefreshEscortDataGrid();
                    HelpClass.EndAwait(grid_main);

                }

            }
            catch
            {
                HelpClass.EndAwait(grid_main);
            }
        }
        private void Btn_cardActive_Click(object sender, RoutedEventArgs e)
        {

        }

        #region escorts
        private void Btn_addEscort_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_addEscort.IsEnabled = false;
                dg_escort.IsEnabled = false;
                if (familyCard.Escorts == null)
                    familyCard.Escorts = new List<Escort>();
                familyCard.Escorts.Add(new Escort());
                //familyCard.Escorts[0].IsCustomer = true;
                RefreshEscortDataGrid();
            }
            catch (Exception ex)
            {
                dg_escort.IsEnabled = true;
                btn_addEscort.IsEnabled = true;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void deleteEscortRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {

                        btn_addEscort.IsEnabled = false;
                        dg_escort.IsEnabled = false;
                        Escort row = (Escort)dg_escort.SelectedItems[0];
                        familyCard.Escorts.Remove(row);
                        RefreshEscortDataGrid();
                    }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                dg_escort.IsEnabled = true;
                btn_addEscort.IsEnabled = true;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public void RefreshEscortDataGrid()
        {
            try
            {
                dg_escort.CancelEdit();
                dg_escort.ItemsSource = null;
                dg_escort.ItemsSource = familyCard.Escorts;
                dg_escort.Items.Refresh();

                dg_escort.IsEnabled = true;
                btn_addEscort.IsEnabled = true;
            }
            catch (Exception ex)
            {
                dg_escort.IsEnabled = true;
                btn_addEscort.IsEnabled = true;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
        }

        #endregion


        private async void Dgc_BoxNumber_KeyDown(object sender, KeyEventArgs e)        
        {
            try
            {
                TextBox tb = sender as TextBox;

                if (e.Key == Key.Return && tb.Text != "")
                {
                    customer = null;
                    HelpClass.StartAwait(grid_main);

                    customer = await FillCombo.customer.GetByBoxNumber(long.Parse(tb.Text));

                    if (customer != null)
                    {
                        if (customer.CustomerStatus != "continouse")
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CustomerNotContinouse"), animation: ToasterAnimation.FadeIn);
                            tb.Text = "";
                        }
                        else if (customer.BoxNumber == long.Parse( tb_BoxNumber.Text))
                        {
                            tb.Text = "";
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("SecondDifferentFromFirst"), animation: ToasterAnimation.FadeIn);

                        }
                        else
                        {
                            var escort = (Escort)dg_escort.SelectedItem;

                            escort.BoxNumber = customer.BoxNumber;
                            escort.EscortName = customer.Name;
                            escort.CivilNum = customer.CivilNum;
                            RefreshEscortDataGrid();

                        }
                    }
                    else
                    {
                        tb.Text = "";
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

        
        private void Dgc_IsCustomer_Checking(object sender, RoutedEventArgs e)
        {
            try
            {
                //dg_escort.CancelEdit();
                //dg_escort.ItemsSource = familyCard.Escorts;
                //dg_escort.Items.Refresh();
                CheckBox checkBox = sender as CheckBox;
                Escort row = (Escort)dg_escort.SelectedItem;
                row.IsCustomer = checkBox.IsChecked;
                //dg_escort.IsEnabled = true;
                //btn_addEscort.IsEnabled = true;
            }
            catch (Exception ex)
            {
                //dg_escort.IsEnabled = true;
                //btn_addEscort.IsEnabled = true;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
        }

        private void col_IsCustomer_Unselected(object sender, RoutedEventArgs e)
        {

        }

        private void cb_KinshipId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var cmb = sender as ComboBox;
                e.Handled = true;

                if (cmb.IsMouseOver)
                {
                    if (dg_escort.SelectedIndex != -1 && cmb != null && cmb.SelectedValue != null)
                    {
                        int _datagridSelectedIndex = dg_escort.SelectedIndex;
                        var escort = (Escort)dg_escort.SelectedItem;

                        int kinshipId = int.Parse(cmb.SelectedValue.ToString());
                        escort.KinshipId = kinshipId;
                       
                        RefreshEscortDataGrid();

                    }
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void dgc_CivilNum_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var txt = sender as TextBox;
                e.Handled = true;
                int _datagridSelectedIndex = dg_escort.SelectedIndex;
                var escort = (Escort)dg_escort.SelectedItem;

                escort.CivilNum = txt.Text;

                RefreshEscortDataGrid();

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void dgc_EscortName_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var txt = sender as TextBox;
                e.Handled = true;
                int _datagridSelectedIndex = dg_escort.SelectedIndex;
                var escort = (Escort)dg_escort.SelectedItem;

                escort.EscortName = txt.Text;

                RefreshEscortDataGrid();

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

       
    }
}
