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
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "CustomerId", "TransactionDate", "TransactionStocksCount", "ApprovalNumber", "MeetingDate", "CheckNumber", "CheckDate" };
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


                //await Search();
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
      
            txt_title.Text = AppSettings.resourcemanager.GetString("FamilyCard");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            txt_search.Text = AppSettings.resourcemanager.GetString("trSearch");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerId, AppSettings.resourcemanager.GetString("CustomerNoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerName, AppSettings.resourcemanager.GetString("CustomerNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CustomerStatus, AppSettings.resourcemanager.GetString("CustomerStatusHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_BoxNumber, AppSettings.resourcemanager.GetString("BoxNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_CivilNum, AppSettings.resourcemanager.GetString("CivilNoHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_ReleaseDate, AppSettings.resourcemanager.GetString("ReleaseDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_AutomatedNumber, AppSettings.resourcemanager.GetString("AutomtedNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            txt_escortInformation.Text = AppSettings.resourcemanager.GetString("EscortsInformation");
            dg_escort.Columns[0].Header = AppSettings.resourcemanager.GetString("CivilNo");
            dg_escort.Columns[1].Header = AppSettings.resourcemanager.GetString("IsCustomer");
            dg_escort.Columns[2].Header = AppSettings.resourcemanager.GetString("BoxNumber");
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

            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trSave");

            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
        
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                /*
                    {
                        HelpClass.StartAwait(grid_main);

                        familyCard = new FamilyCard();
                        if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                        {

                            familyCard.TransactionDate = dp_TransactionDate.SelectedDate;
                            familyCard.Notes = tb_Notes.Text;

                            familyCard.CreateUserId = MainWindow.userLogin.userId;

                            FillCombo.familyCardList = await familyCard.save(familyCard);
                            if (FillCombo.familyCardList == null)
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
                */
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
                    /*

                    if (HelpClass.validate(requiredControlList, this))
                    {
                        familyCard.TransactionType = "add";
                        familyCard.CustomerId = long.Parse(tb_CustomerId.Text);
                        familyCard.BoxNumber = customer.BoxNumber;
                        familyCard.TransactionDate = dp_TransactionDate.SelectedDate;
                        familyCard.TransactionStocksCount = int.Parse(tb_TransactionStocksCount.Text);
                        familyCard.StocksCount = int.Parse(tb_StocksCount.Text);
                        familyCard.StocksPrice = decimal.Parse(tb_StocksPrice.Text);
                        familyCard.TotalPrice = decimal.Parse(tb_TotalPrice.Text);
                        familyCard.ApprovalNumber = tb_ApprovalNumber.Text;
                        familyCard.MeetingDate = dp_MeetingDate.SelectedDate;
                        familyCard.CheckNumber = tb_CheckNumber.Text;
                        familyCard.CheckDate = dp_CheckDate.SelectedDate;
                        familyCard.Notes = tb_Notes.Text;

                        familyCard.UpdateUserId = MainWindow.userLogin.userId;

                        var res = await familyCard.AddStocks(familyCard);
                        if (res == 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                            Clear();
                            //await Search();

                        }
                    }
                    else
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("saveNotDoneEmptyFields"), animation: ToasterAnimation.FadeIn);
                    }

                    */
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
                    /*
                    if (familyCard.FamilyCardId != 0)
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
                            FillCombo.familyCardList = await familyCard.delete(familyCard.FamilyCardId, MainWindow.userLogin.userId);
                            if (FillCombo.familyCardList == null)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                familyCard.FamilyCardId = 0;
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                await Search();
                                await Clear();
                            }
                        }

                    }
                    */
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
            /*
            familyCards = await familyCard.SearchTransactions("add", tb_search.Text);
            RefreshFamilyCardsView();
            */
        }
        //async Task<IEnumerable<FamilyCard>> RefreshFamilyCardsList()
        //{

        //    await familyCard.get(true);

        //    return FillCombo.familyCardList;

        //    return familyCards;
        //}
        void RefreshFamilyCardsView()
        {
            dg_familyCard.ItemsSource = familyCards;
            txt_count.Text = familyCards.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        private void Clear()
        {
            familyCard = new FamilyCard();
            this.DataContext = new FamilyCard();
            dg_familyCard.SelectedIndex = -1;
            /*
            #region clear inputs
            tb_CustomerId.Text = "";
            tb_CustomerName.Text = "";
            dp_JoinDate.SelectedDate = null;
            tb_StocksCount.Text = "";
            tb_TotalPrice.Text = "";
            txt_JoinDay.Text = "";
            txt_JoinMonth.Text = "";
            txt_JoinYear.Text = "";
            #endregion
            */
            inputEditable();
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
            /*
            tb_CustomerId.Text = familyCard.CustomerId.ToString();
            dp_TransactionDate.SelectedDate = familyCard.TransactionDate;
            tb_CustomerName.Text = familyCard.CustomerName;
            dp_JoinDate.SelectedDate = familyCard.JoinDate;
            tb_StocksCount.Text = familyCard.StocksCount.ToString();
            tb_TotalPrice.Text = HelpClass.DecTostring(familyCard.StocksCount * familyCard.StocksPrice);
            if (familyCard.JoinDate != null)
            {
                DateTime zeroTime = new DateTime(1, 1, 1);
                TimeSpan span1 = DateTime.Now - (DateTime)familyCard.JoinDate;
                txt_JoinYear.Text = ((zeroTime + span1).Year - 1).ToString();
                txt_JoinMonth.Text = ((zeroTime + span1).Month - 1).ToString();
                txt_JoinDay.Text = ((zeroTime + span1).Day).ToString();
            }
            */

            inputEditable();
        }

        private void inputEditable()
        {
            /*
            if (familyCard.TransactionId == 0)
            {
                tb_CustomerId.IsEnabled = true;
                tb_TransactionStocksCount.IsEnabled = true;
                tb_ApprovalNumber.IsEnabled = true;
                tb_CheckNumber.IsEnabled = true;
                tb_Notes.IsEnabled = true;
                dp_CheckDate.IsEnabled = true;
                dp_MeetingDate.IsEnabled = true;
                btn_update.IsEnabled = true;
            }
            else
            {
                tb_CustomerId.IsEnabled = false;
                tb_TransactionStocksCount.IsEnabled = false;
                tb_ApprovalNumber.IsEnabled = false;
                tb_CheckNumber.IsEnabled = false;
                tb_Notes.IsEnabled = false;
                dp_CheckDate.IsEnabled = false;
                dp_MeetingDate.IsEnabled = false;
                btn_update.IsEnabled = false;
            }
            */
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            swapToData();
        }

        #endregion

        private void Btn_archiving_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_updateIBAN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_personalDocuments_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_pastProfits_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void tb_CustomerId_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.Key == Key.Return && tb_CustomerId.Text != "")
                {
                    customer = null;
                    HelpClass.StartAwait(grid_main);

                   customer = await FillCombo.customer.GetById(long.Parse(tb_CustomerId.Text));

                    if (customer != null)
                    {
                        if (customer.CustomerStatus != "continouse")
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("CustomerNotContinouse"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            tb_CustomerName.Text = customer.Name;
                            tb_CustomerStatus.Text = AppSettings.resourcemanager.GetString(customer.CustomerStatus);
                            tb_BoxNumber.Text = customer.BoxNumber.ToString();
                            tb_CivilNum.Text = customer.CivilNum.ToString();
                            tb_AutomatedNumber.Text = customer.customerAddress.AutomtedNumber.ToString();

                        }
                    }
                    else
                    {
                        tb_CustomerId.Text = "";
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

      
    }
}
