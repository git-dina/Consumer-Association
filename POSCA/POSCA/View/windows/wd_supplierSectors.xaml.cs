using netoaster;
using POSCA.Classes;
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
using System.Windows.Threading;

namespace POSCA.View.windows
{
    /// <summary>
    /// Interaction logic for wd_supplierSectors.xaml
    /// </summary>
    public partial class wd_supplierSectors : Window
    {

        public wd_supplierSectors()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name); }
        }
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            isOk = false;
            this.Close();
        }
        List<SupplierSector> listSupplierSector = new List<SupplierSector>();
        List<SupplierSectorSpecify> listSupplierSectorSpecify = new List<SupplierSectorSpecify>();
        List<Branch> listBranch = new List<Branch>();
        List<SupplierSector> listSupplierSector1 = new List<SupplierSector>();
        //public static DispatcherTimer timer;

        public List<SupplierSector> SupplierSectors { get; set; }
        public List<SupplierSectorSpecify> SupplierSectorSpecifys { get; set; }
        public bool isOk { get; set; }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load

            try
            {


                HelpClass.StartAwait(grid_main);

                //timer = new DispatcherTimer();
                //timer.Interval = TimeSpan.FromSeconds(1);
                //timer.Tick += timer_Tick;
                //timer.Start();


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


                setContactData();


                #region SupplierSector

                listSupplierSector = new List<SupplierSector>();
                listSupplierSector.Add(new SupplierSector()
                {
                    SupSectorId = 1,
                    SupSectorName = "SupSectorName1",
                    Notes = "Notes1",
                    FreePercentageMarkets = 1,
                    FreePercentageBranchs = 2,
                    FreePercentageStores = 3,
                    DiscountPercentageMarkets = 4,
                    DiscountPercentageBranchs = 5,
                    DiscountPercentageStores = 6
                });
                listSupplierSector.Add(new SupplierSector() { SupSectorId = 2, SupSectorName = "SupSectorName2", Notes = "Notes2" });
                dg_supplierSector.ItemsSource = listSupplierSector;
                #endregion


                #region SupplierSectorSpecify

                #region fill combo SupSectorId
                listSupplierSector1 = new List<SupplierSector>();
                listSupplierSector1.Add(new SupplierSector() { SupSectorId = 1, SupSectorName = "SupSectorName1" });
                listSupplierSector1.Add(new SupplierSector() { SupSectorId = 2, SupSectorName = "SupSectorName2" });
                cb_SupSectorId.DisplayMemberPath = "SupSectorName";
                cb_SupSectorId.SelectedValuePath = "SupSectorId";
                cb_SupSectorId.ItemsSource = listSupplierSector1;
                #endregion

                #region fill combo Branch
                listBranch = new List<Branch>();
                listBranch.Add(new Branch() { BranchId = 1, Name = "Branch1" });
                listBranch.Add(new Branch() { BranchId = 2, Name = "Branch2" });
                cb_BranchId.DisplayMemberPath = "Name";
                cb_BranchId.SelectedValuePath = "BranchId";
                cb_BranchId.ItemsSource = listBranch;
                #endregion

                listSupplierSectorSpecify = new List<SupplierSectorSpecify>();
                listSupplierSectorSpecify.Add(new SupplierSectorSpecify() { SupSectorId = 1, BranchId = 1, FreePercentage = 1, DiscountPercentage = 2, Notes = "Notes1" });
                listSupplierSectorSpecify.Add(new SupplierSectorSpecify() { SupSectorId = 2, BranchId = 2, FreePercentage = 3, DiscountPercentage = 4, Notes = "Notes2" });
                dg_supplierSectorSpecify.ItemsSource = listSupplierSectorSpecify;

                #endregion

                this.DataContext = this;
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
            txt_title.Text = AppSettings.resourcemanager.GetString("ContactData");
            //txt_phonesData.Text = AppSettings.resourcemanager.GetString("Phones");
        }

        private void setContactData()
        {
            dg_supplierSector.ItemsSource = SupplierSectors;
            dg_supplierSectorSpecify.ItemsSource = SupplierSectorSpecifys;
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
        public static List<string> requiredControlList;
        #region events
    
        private async void Dg_supplierSector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                /*
                if (dg_supplier.SelectedIndex != -1)
                {
                    supplier = dg_supplier.SelectedItem as Supplier;
                    this.DataContext = supplier;
                    if (supplier != null)
                    {
                        #region image
                        bool isModified = HelpClass.chkImgChng(supplier.image, (DateTime)supplier.updateDate, Global.TMPSuppliersFolder);
                        if (isModified)
                            getImg();
                        else
                            HelpClass.getLocalImg("Supplier", supplier.image, btn_image);
                        #endregion
                        //getImg();
                        #region delete
                        if (supplier.canDelete)
                            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
                        else
                        {
                            if (supplier.isActive == 0)
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trActive");
                            else
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trInActive");
                        }
                        #endregion
                        HelpClass.getMobile(supplier.mobile, cb_areaMobile, tb_mobile);
                        HelpClass.getPhone(supplier.phone, cb_areaPhone, cb_areaPhoneLocal, tb_phone);
                        HelpClass.getPhone(supplier.fax, cb_areaFax, cb_areaFaxLocal, tb_fax);
                    }
                }
                HelpClass.clearValidate(requiredControlList, this);
                */
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void Dg_supplierSectorSpecify_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                /*
                if (dg_supplier.SelectedIndex != -1)
                {
                    supplier = dg_supplier.SelectedItem as Supplier;
                    this.DataContext = supplier;
                    if (supplier != null)
                    {
                        #region image
                        bool isModified = HelpClass.chkImgChng(supplier.image, (DateTime)supplier.updateDate, Global.TMPSuppliersFolder);
                        if (isModified)
                            getImg();
                        else
                            HelpClass.getLocalImg("Supplier", supplier.image, btn_image);
                        #endregion
                        //getImg();
                        #region delete
                        if (supplier.canDelete)
                            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
                        else
                        {
                            if (supplier.isActive == 0)
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trActive");
                            else
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trInActive");
                        }
                        #endregion
                        HelpClass.getMobile(supplier.mobile, cb_areaMobile, tb_mobile);
                        HelpClass.getPhone(supplier.phone, cb_areaPhone, cb_areaPhoneLocal, tb_phone);
                        HelpClass.getPhone(supplier.fax, cb_areaFax, cb_areaFaxLocal, tb_fax);
                    }
                }
                HelpClass.clearValidate(requiredControlList, this);
                */
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //if (timer != null)
                //    timer.Stop();
                e.Cancel = true;
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

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // HelpClass.StartAwait(grid_main);




                // HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private  void Btn_addSupplierSector_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_addSupplierSector.IsEnabled = false;
                dg_supplierSector.IsEnabled = false;
                listSupplierSector.Add(new SupplierSector());
                RefreshSupplierSectorDataGrid();
            }
            catch (Exception ex)
            {
                dg_supplierSector.IsEnabled = true;
                btn_addSupplierSector.IsEnabled = true;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void deleteSupplierSectorRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {

                        btn_addSupplierSector.IsEnabled = false;
                        dg_supplierSector.IsEnabled = false;
                        SupplierSector row = (SupplierSector)dg_supplierSector.SelectedItems[0];
                        listSupplierSector.Remove(row);
                        RefreshSupplierSectorDataGrid();
                    }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                dg_supplierSector.IsEnabled = true;
                btn_addSupplierSector.IsEnabled = true;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public  void RefreshSupplierSectorDataGrid()
        {
            try
            {
                dg_supplierSector.CancelEdit();
                dg_supplierSector.ItemsSource = listSupplierSector;
                dg_supplierSector.Items.Refresh();

                dg_supplierSector.IsEnabled = true;
                btn_addSupplierSector.IsEnabled = true;
            }
            catch (Exception ex)
            {
                dg_supplierSector.IsEnabled = true;
                btn_addSupplierSector.IsEnabled = true;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name,false);
            }
        }

        private void Btn_addSupplierSectorSpecify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_addSupplierSectorSpecify.IsEnabled = false;
                dg_supplierSectorSpecify.IsEnabled = false;
                listSupplierSectorSpecify.Add(new SupplierSectorSpecify());
                RefreshSupplierSectorSpecifyDataGrid();
            }
            catch (Exception ex)
            {
                dg_supplierSectorSpecify.IsEnabled = true;
                btn_addSupplierSectorSpecify.IsEnabled = true;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void deleteSupplierSectorSpecifyRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {

                        btn_addSupplierSectorSpecify.IsEnabled = false;
                        dg_supplierSectorSpecify.IsEnabled = false;
                        SupplierSectorSpecify row = (SupplierSectorSpecify)dg_supplierSectorSpecify.SelectedItems[0];
                        listSupplierSectorSpecify.Remove(row);
                        RefreshSupplierSectorSpecifyDataGrid();
                    }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                dg_supplierSectorSpecify.IsEnabled = true;
                btn_addSupplierSectorSpecify.IsEnabled = true;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public void RefreshSupplierSectorSpecifyDataGrid()
        {
            try
            {
                dg_supplierSectorSpecify.CancelEdit();
                dg_supplierSectorSpecify.ItemsSource = listSupplierSectorSpecify;
                dg_supplierSectorSpecify.Items.Refresh();

                dg_supplierSectorSpecify.IsEnabled = true;
                btn_addSupplierSectorSpecify.IsEnabled = true;
            }
            catch (Exception ex)
            {
                dg_supplierSectorSpecify.IsEnabled = true;
                btn_addSupplierSectorSpecify.IsEnabled = true;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
        }

      

      

      

    
    }
}
