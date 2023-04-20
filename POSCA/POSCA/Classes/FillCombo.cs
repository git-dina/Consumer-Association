using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Text;
using System;
using POSCA.Classes.ApiClasses;

namespace POSCA.Classes
{
    public class FillCombo
    {

        //static public string deliveryPermission = "setUserSetting_delivery";
        //static public string administrativeMessagesPermission = "setUserSetting_administrativeMessages";
        //static public string administrativePosTransfersPermission = "setUserSetting_administrativePosTransfers";

        #region supplier groups
        static public SupplierGroup supplierGroup = new SupplierGroup();
        static public List<SupplierGroup> supplierGroupList ;

        static public async Task<IEnumerable<SupplierGroup>> RefreshSupplierGroups()
        {
            supplierGroupList = await supplierGroup.get(true);

            return supplierGroupList;
        }

        static public async Task fillSupplierGroups(ComboBox combo)
        {
            if (supplierGroupList is null)
                await RefreshSupplierGroups();

            combo.ItemsSource = supplierGroupList;
            combo.SelectedValuePath = "SupplierGroupId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        #endregion
        #region Assistant Supplier
        static public AssistantSupplier assistantSupplier = new AssistantSupplier();
        static public List<AssistantSupplier> assistantSupplierList ;
        static public List<AssistantSupplier> assistantSupplierListWithDefault ;

        static public async Task<IEnumerable<AssistantSupplier>> RefreshAssistantSuppliers()
        {
            assistantSupplierList = await assistantSupplier.get(true);

            return assistantSupplierList;
        }

        static public async Task fillAssistantWithDefault(ComboBox combo)
        {
            if (assistantSupplierList is null)
                await RefreshAssistantSuppliers();

            assistantSupplierListWithDefault = assistantSupplierList.ToList();

            AssistantSupplier sup = new AssistantSupplier();
            sup.Name = "-";
            sup.AssistantSupId = 0;
            assistantSupplierListWithDefault.Insert(0, sup);

            combo.ItemsSource = assistantSupplierListWithDefault;
            combo.SelectedValuePath = "AssistantSupId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        #endregion

        #region supplier types
        static public SupplierType supplierType = new SupplierType();
        static public List<SupplierType> supplierTypeList;

        static public async Task<IEnumerable<SupplierType>> RefreshSupplierTypes()
        {
            supplierTypeList = await supplierType.get(true);

            return supplierTypeList;
        }

        static public async Task fillSupplierTypes(ComboBox combo)
        {
            if (supplierTypeList is null)
                await RefreshSupplierTypes();

            combo.ItemsSource = supplierTypeList;
            combo.SelectedValuePath = "SupplierTypeId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        #endregion
        #region SupplierDocType
        static public SupplierDocType supplierDocType = new SupplierDocType();
        static public List<SupplierDocType> supplierDocTypeList;

        static public async Task<IEnumerable<SupplierDocType>> RefreshSupplierDocTypes()
        {
            supplierDocTypeList = await supplierDocType.get(true);

            return supplierDocTypeList;
        }

        static public async Task fillSupplierDocTypes(ComboBox combo)
        {
            if (supplierDocTypeList is null)
                await RefreshSupplierDocTypes();

            combo.ItemsSource = supplierDocTypeList;
            combo.SelectedValuePath = "TypeId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        #endregion
        #region Bank
        static public Bank bank = new Bank();
        static public List<Bank> bankList;

        static public async Task<IEnumerable<Bank>> RefreshBanks()
        {
            bankList = await bank.get(true);

            return bankList;
        }

        static public async Task fillBanks(ComboBox combo)
        {
            if (bankList is null)
                await RefreshBanks();

            combo.ItemsSource = bankList;
            combo.SelectedValuePath = "BankId";
            combo.DisplayMemberPath = "BankName";
            combo.SelectedIndex = -1;
        }
        static public async Task fillBanksWithDefault(ComboBox combo)
        {
            if (bankList is null)
                await RefreshBanks();

            var lst = bankList.ToList();
            Bank sup = new Bank();
            sup.BankName = "-";
            sup.BankId = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;
            combo.SelectedValuePath = "BankId";
            combo.DisplayMemberPath = "BankName";
            combo.SelectedIndex = -1;
        }
        #endregion
        #region Country
        static public Country country = new Country();
        static public List<Country> countryList;

        static public async Task<IEnumerable<Country>> RefreshCountrys()
        {
            countryList = await country.get(true);

            return countryList;
        }

        static public async Task fillCountrys(ComboBox combo)
        {
            if (countryList is null)
                await RefreshCountrys();

            combo.ItemsSource = countryList;
            combo.SelectedValuePath = "CountryId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        #endregion
        #region phoneTypes
        static public PhoneType phoneType = new PhoneType();
        static public List<PhoneType> phoneTypeList;

        static public async Task<IEnumerable<PhoneType>> RefreshPhoneTypes()
        {
            phoneTypeList = await phoneType.get(true);

            return phoneTypeList;
        }

        static public async Task fillPhoneTypes(ComboBox combo)
        {
            if (phoneTypeList is null)
                await RefreshPhoneTypes();

            combo.ItemsSource = phoneTypeList;
            combo.SelectedValuePath = "PhoneTypeId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        #endregion

        #region suppliers
        static public Supplier supplier = new Supplier();
        static public List<Supplier> suppliersList;

        static public async Task<IEnumerable<Supplier>> RefreshSuppliers()
        {
            suppliersList = await supplier.get(true);

            return suppliersList;
        }
        #endregion

        #region Branch
        static public Branch branch = new Branch();
        static public List<Branch> branchList = new List<Branch>();

        static public async Task<IEnumerable<Branch>> RefreshBranches()
        {
            branchList = await branch.get(true);

            return branchList;
        }
        #endregion

        #region Brand
        static public Brand brand = new Brand();
        static public List<Brand> brandList;

        static public async Task<IEnumerable<Brand>> RefreshBrands()
        {
            brandList = await brand.get(true);

            return brandList;
        }

        static public async Task fillBrands(ComboBox combo)
        {
            if (brandList is null)
                await RefreshBrands();

            combo.ItemsSource = brandList;
            combo.SelectedValuePath = "BrandId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        #endregion
        #region BarcodeType
        static public List<keyValueString> barcodeTypeList;

        static public  IEnumerable<keyValueString>RefreshBarcodeTypes()
        {
            barcodeTypeList = new List<keyValueString>() {
                new keyValueString(){key="external", value=AppSettings.resourcemanager.GetString("external") },
                new keyValueString(){key="internal", value=AppSettings.resourcemanager.GetString("internal") },
                new keyValueString(){key="wait", value=AppSettings.resourcemanager.GetString("wait") },
            };

            return barcodeTypeList;
        }

        static public  void fillBarcodeTypes(ComboBox combo)
        {
            if (barcodeTypeList is null)
                RefreshBarcodeTypes();

            combo.ItemsSource = barcodeTypeList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = -1;
        }
        #endregion
        #region categorys
        static public Category category = new Category();
        static public List<Category> categoryList;

        static public async Task<IEnumerable<Category>> RefreshCategorys()
        {
            categoryList = await category.get(true);

            return categoryList;
        }

        static public async Task fillCategorysWithDefault(ComboBox combo)
        {
            if (categoryList is null)
                await RefreshCategorys();

            var lst = categoryList.ToList();
            Category sup = new Category();
            sup.Name = "-";
            sup.CategoryId = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;

            combo.SelectedValuePath = "CategoryId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        static public async Task fillCategorys(ComboBox combo)
        {
            if (categoryList is null)
                await RefreshCategorys();

            combo.ItemsSource = categoryList;

            combo.SelectedValuePath = "CategoryId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        #endregion
        #region Unit
        static public Unit unit = new Unit();
        static public List<Unit> unitList;

        static public async Task<IEnumerable<Unit>> RefreshUnits()
        {
            unitList = await unit.get(true);

            return unitList;
        }

        static public async Task fillUnits(ComboBox combo)
        {
            if (unitList is null)
                await RefreshUnits();

            combo.ItemsSource = unitList;
            combo.SelectedValuePath = "UnitId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        static public async Task fillUnitsWithDefault(ComboBox combo)
        {
            if (unitList is null)
                await RefreshUnits();

            var lst = unitList.ToList();
            Unit sup = new Unit();
            sup.Name = "-";
            sup.UnitId = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;
            combo.SelectedValuePath = "UnitId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        #endregion
    }
}
