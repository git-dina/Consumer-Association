using POSCA.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace POSCA.View.windows
{
    /// <summary>
    /// Interaction logic for wd_supplierAllowedOperations.xaml
    /// </summary>
    public partial class wd_supplierAllowedOperations : Window
    {

        public wd_supplierAllowedOperations()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name); }
        }
        BrushConverter bc = new BrushConverter();



        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public bool IsAllowedPO;
        public bool IsAllowedReceipt;
        public bool IsAllowedDirectReturn;
        public bool IsAllowedReturnDiscount;
        public bool IsAllowCashingChecks;
        public bool isOk { get; set; }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load

            try
            {


                HelpClass.StartAwait(grid_main);

                #region translate

                //if (AppSettings.lang.Equals("en"))
                //{
                //    grid_main.FlowDirection = FlowDirection.LeftToRight;
                //}
                //else
                //{
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                //}

                translate();
                #endregion

                setAllowedOperations();


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
            txt_title.Text = AppSettings.resourcemanager.GetString("AllowedOperations");
            txt_IsAllowedPO.Text = AppSettings.resourcemanager.GetString("IsAllowedPO");
            txt_IsAllowedReceipt.Text = AppSettings.resourcemanager.GetString("IsAllowedReceipt");
            txt_IsAllowedDirectReturn.Text = AppSettings.resourcemanager.GetString("IsAllowedDirectReturn");
            txt_IsAllowedReturnDiscount.Text = AppSettings.resourcemanager.GetString("IsAllowedReturnDiscount");
            txt_IsAllowCashingChecks.Text = AppSettings.resourcemanager.GetString("IsAllowCashingChecks");


            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

        }

       private void setAllowedOperations()
        {
            if (IsAllowCashingChecks == true)
                tgl_IsAllowCashingChecks.IsChecked = true;
            else
                tgl_IsAllowCashingChecks.IsChecked = false;

            if (IsAllowedDirectReturn == true)
                tgl_IsAllowedDirectReturn.IsChecked = true;
            else
                tgl_IsAllowedDirectReturn.IsChecked = false;

            if (IsAllowedPO == true)
                tgl_IsAllowedPO.IsChecked = true;
            else
                tgl_IsAllowedPO.IsChecked = false;

            if (IsAllowedReceipt== true)
                tgl_IsAllowedReceipt.IsChecked = true;
            else
                tgl_IsAllowedReceipt.IsChecked = false;

            if (IsAllowedReturnDiscount == true)
                tgl_IsAllowedReturnDiscount.IsChecked = true;
            else
                tgl_IsAllowedReturnDiscount.IsChecked = false;
        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    //Btn_save_Click(null, null);
                }
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name); }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
               
                isOk = false;
                this.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (tgl_IsAllowCashingChecks.IsChecked == true)
                    IsAllowCashingChecks = true;
                else
                    IsAllowCashingChecks = false;

                if (tgl_IsAllowedDirectReturn.IsChecked == true)
                    IsAllowedDirectReturn = true;
                else
                    IsAllowedDirectReturn = false;

                if (tgl_IsAllowedPO.IsChecked == true)
                    IsAllowedPO = true;
                else
                    IsAllowedPO = false;

                if (tgl_IsAllowedReceipt.IsChecked == true)
                    IsAllowedReceipt = true;
                else
                    IsAllowedReceipt = false;

                if (tgl_IsAllowedReturnDiscount.IsChecked == true)
                    IsAllowedReturnDiscount = true;
                else
                    IsAllowedReturnDiscount = false;

                isOk = true;
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
