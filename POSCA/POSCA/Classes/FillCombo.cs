﻿using System.Collections.Generic;
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
        
        static public async Task fillSupplierGroupsWithDefault(ComboBox combo)
        {
            if (supplierGroupList is null)
                await RefreshSupplierGroups();

           var listWithDefault = supplierGroupList.ToList();

            SupplierGroup sup = new SupplierGroup();
            sup.Name = "-";
            sup.SupplierGroupId = 0;
            listWithDefault.Insert(0, sup);

            combo.ItemsSource = listWithDefault;
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
        static public async Task fillAssistant(ComboBox combo)
        {
            if (assistantSupplierList is null)
                await RefreshAssistantSuppliers();

            combo.ItemsSource = assistantSupplierList;
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
            combo.DisplayMemberPath = "CountryName";
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

        static public async Task fillSuppliers(ComboBox combo)
        {
            if (suppliersList is null)
                await RefreshSuppliers();

            combo.ItemsSource = suppliersList;
            combo.SelectedValuePath = "SupId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        
        static public async Task fillUnBlockedSuppliers(ComboBox combo)
        {
            if (suppliersList is null)
                await RefreshSuppliers();

            combo.ItemsSource = suppliersList.Where(x => x.IsBlocked == false).ToList();
            combo.SelectedValuePath = "SupId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
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

        static public async Task fillBrandsWithDefault(ComboBox combo)
        {
            if (brandList is null)
                await RefreshBrands();

            var lst = brandList.ToList();
            Brand sup = new Brand();
            sup.Name = "-";
            sup.BrandId = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;
            combo.SelectedValuePath = "BrandId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = 0;
        }
        #endregion
        #region BarcodeType
        static public List<keyValueString> barcodeTypeList;

        static public  IEnumerable<keyValueString>RefreshBarcodeTypes()
        {
            barcodeTypeList = new List<keyValueString>() {
                new keyValueString(){key="external", value=AppSettings.resourcemanager.GetString("external") },
                new keyValueString(){key="internal", value=AppSettings.resourcemanager.GetString("internal") },
                new keyValueString(){key="isWeight", value=AppSettings.resourcemanager.GetString("IsWeight") },
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

        static public async Task fillCategorysWithDefault(ComboBox combo, long id=0)
        {
            if (categoryList is null)
                await RefreshCategorys();

            var lst = categoryList.ToList();
            lst = lst.Where(x => x.CategoryId != id).ToList();
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
            foreach (var row in unitList)
                row.Name = AppSettings.resourcemanager.GetString(row.Name);

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
        static public async Task fillUnits(DataGridComboBoxColumn combo)
        {
            if (unitList is null)
                await RefreshUnits();

            combo.ItemsSource = unitList;
            combo.SelectedValuePath = "UnitId";
            combo.DisplayMemberPath = "Name";
           // combo.SelectedIndex = -1;
        } 
        
        static public async Task fillSmallUnits(DataGridComboBoxColumn combo, string itemUnit)
        {
            if (unitList is null)
                await RefreshUnits();

            List<Unit> unitsLst = new List<Unit>();
            switch(itemUnit)
            {
                case "حبة":
                    unitsLst.Add(unitList.Where(x => x.Name == "حبة").FirstOrDefault());
                    break;
                case "باكيت":
                    unitsLst.Add(unitList.Where(x => x.Name == "حبة").FirstOrDefault());
                    unitsLst.Add(unitList.Where(x => x.Name == "باكيت").FirstOrDefault());
                    break;
                default:
                    unitsLst = unitList.ToList();
                    break;
            }
            combo.ItemsSource = unitsLst;
            combo.SelectedValuePath = "UnitId";
            combo.DisplayMemberPath = "Name";
           // combo.SelectedIndex = -1;
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

        #region itemstatus
        static public List<keyValueString> itemStatusList;
        static public IEnumerable<keyValueString> RefreshItemStatus()
        {
            itemStatusList = new List<keyValueString>() {
                new keyValueString(){key="normal", value=AppSettings.resourcemanager.GetString("Normal") },
                new keyValueString(){key="stopped", value=AppSettings.resourcemanager.GetString("Stopped") },
                new keyValueString(){key="canceled", value=AppSettings.resourcemanager.GetString("trCanceled") },
            };

            return itemStatusList;
        }

        static public void fillItemStatus(ComboBox combo)
        {
            if (itemStatusList is null)
                RefreshItemStatus();

            combo.ItemsSource = itemStatusList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = -1;
        }
        #endregion
        #region itemRecieptType
        static public List<keyValueString> itemRecieptTypeList;
        static public IEnumerable<keyValueString> RefreshItemRecieptType()
        {
            itemRecieptTypeList = new List<keyValueString>() {
                new keyValueString(){key="orders", value=AppSettings.resourcemanager.GetString("Orders") },
                new keyValueString(){key="direct", value=AppSettings.resourcemanager.GetString("Direct") },
                new keyValueString(){key="orders_direct", value=AppSettings.resourcemanager.GetString("OrdersAndDirect") },
                new keyValueString(){key="service", value=AppSettings.resourcemanager.GetString("Service") },
                new keyValueString(){key="vegetable", value=AppSettings.resourcemanager.GetString("Vegetable") },
                new keyValueString(){key="forStorage", value=AppSettings.resourcemanager.GetString("ForStorage") },
                new keyValueString(){key="forStorage_branchDirect", value=AppSettings.resourcemanager.GetString("ForStorageAndBranch") },
            };

            return itemRecieptTypeList;
        }

        static public void fillItemRecieptType(ComboBox combo)
        {
            if (itemRecieptTypeList is null)
                RefreshItemRecieptType();

            combo.ItemsSource = itemRecieptTypeList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = -1;
        }
        #endregion
        
        #region itemType
        static public List<keyValueString> itemTypeList;
        static public IEnumerable<keyValueString> RefreshItemTypes()
        {
            itemTypeList = new List<keyValueString>() {
                new keyValueString(){key="general", value=AppSettings.resourcemanager.GetString("General") },
                new keyValueString(){key="allSup", value=AppSettings.resourcemanager.GetString("FromAllSuppliers") },
                new keyValueString(){key="festivals", value=AppSettings.resourcemanager.GetString("Festivals") },
            };

            return itemTypeList;
        }

        static public void fillItemType(ComboBox combo)
        {
            if (itemTypeList is null)
                RefreshItemTypes();

            combo.ItemsSource = itemTypeList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = -1;
        }
        #endregion 
        #region itemTransType
        static public List<keyValueString> itemTransTypeList;
        static public IEnumerable<keyValueString> RefreshItemTransTypes()
        {
            itemTransTypeList = new List<keyValueString>() {
                new keyValueString(){key="new_committee", value=AppSettings.resourcemanager.GetString("NewAndCommittee") },
                new keyValueString(){key="orderPlaced", value=AppSettings.resourcemanager.GetString("IsPurchased") },
                new keyValueString(){key="recieved", value=AppSettings.resourcemanager.GetString("ItemRecieved") },
            };

            return itemTransTypeList;
        }

        static public void fillItemTransTypes(ComboBox combo)
        {
            if (itemTransTypeList is null)
                RefreshItemTransTypes();

            combo.ItemsSource = itemTransTypeList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = -1;
        }
        #endregion

        #region itemTransaction list
        static public List<keyValueString> itemTransactionsList;
        static public IEnumerable<keyValueString> RefreshItemTransactionsList()
        {
            itemTransactionsList = new List<keyValueString>() {
                new keyValueString(){key="purchaseOrders", value=AppSettings.resourcemanager.GetString("PurchaseOrders") },
                new keyValueString(){key="receipt", value=AppSettings.resourcemanager.GetString("Receipt") },
                new keyValueString(){key="returns", value=AppSettings.resourcemanager.GetString("Returns") },
                new keyValueString(){key="destructive", value=AppSettings.resourcemanager.GetString("Destructive") },
                new keyValueString(){key="offers", value=AppSettings.resourcemanager.GetString("Offers") },
                new keyValueString(){key="exchangeTransfer", value=AppSettings.resourcemanager.GetString("ExchangeAndTransfer") },
                new keyValueString(){key="settlements", value=AppSettings.resourcemanager.GetString("Settlements") },
                new keyValueString(){key="strategic", value=AppSettings.resourcemanager.GetString("StrategicItem") },
            };

            return itemTransactionsList;
        }
        #endregion
        #region Item
        static public Item item = new Item();
        static public List<Item> itemList;

      
        static public async Task<IEnumerable<Item>> RefreshItems()
        {
            itemList = await item.get(true);

            return itemList;
        }

        static public async Task fillItems(ComboBox combo)
        {
            if (itemList is null)
                await RefreshItems();

            combo.ItemsSource = itemList;
            combo.SelectedValuePath = "ItemId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        static public async Task fillItemsWithDefault(ComboBox combo)
        {
            if (itemList is null)
                await RefreshItems();

            var lst = itemList.ToList();
            Item sup = new Item();
            sup.Name = "-";
            sup.ItemId = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;
            combo.SelectedValuePath = "ItemId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        #endregion

        #region LocationType
        static public LocationType locationType = new LocationType();
        static public List<LocationType> locationTypeList;

        static public async Task<IEnumerable<LocationType>> RefreshLocationTypes()
        {
            locationTypeList = await locationType.get(true);

            return locationTypeList;
        }

        static public async Task fillLocationTypes(ComboBox combo)
        {
            if (locationTypeList is null)
                await RefreshLocationTypes();

            combo.ItemsSource = locationTypeList;
            combo.SelectedValuePath = "LocationTypeId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        static public async Task fillLocationTypesWithDefault(ComboBox combo)
        {
            if (locationTypeList is null)
                await RefreshLocationTypes();

            var lst = locationTypeList.ToList();
            LocationType sup = new LocationType();
            sup.Name = "-";
            sup.LocationTypeId = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;
            combo.SelectedValuePath = "LocationTypeId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        #endregion

        #region Location
        static public Location location = new Location();
        static public List<Location> locationsList;

        static public async Task<IEnumerable<Location>> RefreshLocations()
        {
            locationsList = await location.get(true);

            return locationsList;
        }

        static public async Task fillLocations(ComboBox combo)
        {
            if (locationsList is null)
                await RefreshLocations();

            combo.ItemsSource = locationsList;
            combo.SelectedValuePath = "LocationId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        static public async Task fillLocationsWithDefault(ComboBox combo)
        {
            if (locationsList is null)
                await RefreshLocations();

            var lst = locationsList.ToList();
            Location sup = new Location();
            sup.Name = "-";
            sup.LocationId = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;
            combo.SelectedValuePath = "LocationId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        #endregion

        #region Purchase
        static public PurchaseInvoice purchaseInvoice = new PurchaseInvoice();
        #region purchase order status
        static public List<keyValueString> PurchaseOrderStatusList;
        static public IEnumerable<keyValueString> RefreshPurchaseOrderStatus()
        {
            PurchaseOrderStatusList = new List<keyValueString>() {
                new keyValueString(){key="opened", value=AppSettings.resourcemanager.GetString("Opened") },
                new keyValueString(){key="orderPlaced", value=AppSettings.resourcemanager.GetString("PurchaseOrderPlaced") },
                new keyValueString(){key="entireReceipt", value=AppSettings.resourcemanager.GetString("EntierReceived") },
                new keyValueString(){key="receipt", value=AppSettings.resourcemanager.GetString("Received") },
            };

            return PurchaseOrderStatusList;
        }

        static public void fillPurchaseOrderStatus(ComboBox combo)
        {
            if (PurchaseOrderStatusList is null)
                RefreshPurchaseOrderStatus();

            combo.ItemsSource = PurchaseOrderStatusList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = -1;
        }
        #endregion
        #endregion

        #region company settings
        static public CompanySettings companySettings = new CompanySettings();
        static public List<CompanySettings> companySettingsList ;
        static public async Task<IEnumerable<CompanySettings>> RefreshCompanySettings()
        {
            if(companySettingsList == null)
                companySettingsList = await companySettings.Get();
            AppSettings.companyName = companySettingsList.Where(x => x.Name.Equals("com_name")).Select(x => x.Value).FirstOrDefault();
            AppSettings.companyNameAr = companySettingsList.Where(x => x.Name.Equals("com_nameAr")).Select(x => x.Value).FirstOrDefault();
            AppSettings.companyAddress = companySettingsList.Where(x => x.Name.Equals("com_address")).Select(x => x.Value).FirstOrDefault();
            AppSettings.companyEmail = companySettingsList.Where(x => x.Name.Equals("com_email")).Select(x => x.Value).FirstOrDefault();
            AppSettings.companyMobile = companySettingsList.Where(x => x.Name.Equals("com_mobile")).Select(x => x.Value).FirstOrDefault();
            AppSettings.companyPhone = companySettingsList.Where(x => x.Name.Equals("com_phone")).Select(x => x.Value).FirstOrDefault();
            AppSettings.companyFax = companySettingsList.Where(x => x.Name.Equals("com_fax")).Select(x => x.Value).FirstOrDefault();
            AppSettings.companylogoImage = companySettingsList.Where(x => x.Name.Equals("com_logo")).Select(x => x.Value).FirstOrDefault();

            return companySettingsList;
        }

        public static async Task<CompanySettings> getSettingBySetName(string setName)
        {
            CompanySettings set = new CompanySettings();
            long setId = 0;

            if (FillCombo.companySettingsList is null)
                await FillCombo.RefreshCompanySettings();

            set = FillCombo.companySettingsList.Where(s => s.Name == setName).FirstOrDefault<CompanySettings>();
            setId = set.SettingId;

            return set;
        }
        #endregion

        #region receipts
        #region receipts status
        static public List<keyValueString> ReceiptStatusList;
        static public IEnumerable<keyValueString> RefreshReceiptStatusList()
        {
            ReceiptStatusList = new List<keyValueString>() {
                new keyValueString(){key="new", value=AppSettings.resourcemanager.GetString("trNew") },
                new keyValueString(){key="notCarriedOver", value=AppSettings.resourcemanager.GetString("NotCarriedOver") },
                new keyValueString(){key="accountingTransfer", value=AppSettings.resourcemanager.GetString("AccountingTransfer") },
                new keyValueString(){key="locationTransfer", value=AppSettings.resourcemanager.GetString("LocationTransfer") },
            };

            return ReceiptStatusList;
        }

        static public void fillReceiptStatus(ComboBox combo)
        {
            if (ReceiptStatusList is null)
                RefreshReceiptStatusList();

            combo.ItemsSource = ReceiptStatusList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = 0;
        }
        #endregion

        #region receipts Types
        static public List<keyValueString> ReceiptsTypesList;
        static public IEnumerable<keyValueString> RefreshReceiptsTypes()
        {
            ReceiptsTypesList = new List<keyValueString>() {
                new keyValueString(){key="purchaseOrders", value=AppSettings.resourcemanager.GetString("PurchaseOrders") },
                new keyValueString(){key="direct", value=AppSettings.resourcemanager.GetString("Direct") },
                new keyValueString(){key="vegetable", value=AppSettings.resourcemanager.GetString("Vegetable") },
                new keyValueString(){key="service", value=AppSettings.resourcemanager.GetString("Service") },
                new keyValueString(){key="free", value=AppSettings.resourcemanager.GetString("trFree") },
                new keyValueString(){key="freeVegetables", value=AppSettings.resourcemanager.GetString("FreeVegetables") },
                new keyValueString(){key="customFree", value=AppSettings.resourcemanager.GetString("CustomFree") },
            };

            return ReceiptsTypesList;
        }

        static public void fillReceiptsTypes(ComboBox combo)
        {
       
            if (ReceiptsTypesList is null)
                RefreshReceiptsTypes();

            combo.ItemsSource = ReceiptsTypesList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";

        }
        static public void fillReceiptsTypesWithDefault(ComboBox combo)
        {
       
            if (ReceiptsTypesList is null)
                RefreshReceiptsTypes();

            var lst = ReceiptsTypesList.ToList();
            keyValueString sup = new keyValueString();
            sup.key = "";
            sup.value = "-";
            lst.Insert(0, sup);

            combo.ItemsSource = lst;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";

        }
        #endregion

        #region CustomFreeTypes
        static public List<keyValueString> CustomFreeTypesList;
        static public IEnumerable<keyValueString> RefreshCustomFreeTypesList()
        {
            CustomFreeTypesList = new List<keyValueString>() {
                new keyValueString(){key="priceDifferences", value=AppSettings.resourcemanager.GetString("PriceDifferences") },
                new keyValueString(){key="rent", value=AppSettings.resourcemanager.GetString("Rent") },
                new keyValueString(){key="support", value=AppSettings.resourcemanager.GetString("Support") },
            };

            return CustomFreeTypesList;
        }

        static public void fillCustomFreeTypes(ComboBox combo)
        {
            if (CustomFreeTypesList is null)
                RefreshCustomFreeTypesList();

            combo.ItemsSource = CustomFreeTypesList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = 0;
        }
        #endregion
        #endregion

        #region returns
        #region receipts status
        static public List<keyValueString> ReturnStatusList;
        static public IEnumerable<keyValueString> RefreshReturnStatusList()
        {
            ReturnStatusList = new List<keyValueString>() {
                new keyValueString(){key="new", value=AppSettings.resourcemanager.GetString("trNew") },
                new keyValueString(){key="notCarriedOver", value=AppSettings.resourcemanager.GetString("NotCarriedOver") },
                new keyValueString(){key="accountingTransfer", value=AppSettings.resourcemanager.GetString("AccountingTransfer") },
                new keyValueString(){key="locationTransfer", value=AppSettings.resourcemanager.GetString("LocationTransfer") },
            };

            return ReturnStatusList;
        }

        static public void fillReturnStatus(ComboBox combo)
        {
            if (ReturnStatusList is null)
                RefreshReturnStatusList();

            combo.ItemsSource = ReturnStatusList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = 0;
        }
        #endregion
        #region ReturnTypes
        //static public List<keyValueString> ReturnTypesList;
        //static public IEnumerable<keyValueString> RefreshReturnTypesList()
        //{
        //    ReturnTypesList = new List<keyValueString>() {
        //        new keyValueString(){key="normalReturn", value=AppSettings.resourcemanager.GetString("NormalReturn") },
        //        new keyValueString(){key="vegetablesReturn", value=AppSettings.resourcemanager.GetString("vegetablesReturnOnly") },
        //    };

        //    return ReturnTypesList;
        //}

        //static public void fillReturnTypes(ComboBox combo)
        //{
        //    if (ReturnTypesList is null)
        //        RefreshReturnTypesList();

        //    combo.ItemsSource = ReturnTypesList;
        //    combo.SelectedValuePath = "key";
        //    combo.DisplayMemberPath = "value";
        //    combo.SelectedIndex = 0;
        //}
        #endregion
        #endregion

        #region Promotion
        static public Promotion Promotion = new Promotion();
        #region PromotionType
        static public List<keyValueString> PromotionTypesList;
        static public IEnumerable<keyValueString> RefreshPromotionTypesList()
        {
            PromotionTypesList = new List<keyValueString>() {
                new keyValueString(){key="quantity", value=AppSettings.resourcemanager.GetString("QuantityOffer") },
                new keyValueString(){key="percentage", value=AppSettings.resourcemanager.GetString("PercentageOffer") },
            };

            return PromotionTypesList;
        }

        static public void fillPromotionTypes(ComboBox combo)
        {
            if (PromotionTypesList is null)
                RefreshPromotionTypesList();

            combo.ItemsSource = PromotionTypesList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = 0;
        }
        #endregion 
        
        #region PromotionNature
        static public List<keyValueString> PromotionNatureList;
        static public IEnumerable<keyValueString> RefreshPromotionNatureList()
        {
            PromotionNatureList = new List<keyValueString>() {
                new keyValueString(){key="continual", value=AppSettings.resourcemanager.GetString("ContinualOffer") },
                new keyValueString(){key="temporary", value=AppSettings.resourcemanager.GetString("TemporaryOffer") },
            };

            return PromotionNatureList;
        }

        static public void fillPromotionNatures(ComboBox combo)
        {
            if (PromotionNatureList is null)
                RefreshPromotionNatureList();

            combo.ItemsSource = PromotionNatureList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = 0;
        }
        #endregion
        #endregion

        #region Sales

        #region PaymentMethods
        //static public List<keyValueString> PaymentMethodsList;

        //static public IEnumerable<keyValueString> RefreshPaymentMethodsList()
        //{
        //    PaymentMethodsList = new List<keyValueString>() {
        //        new keyValueString(){key="cash", value=AppSettings.resourcemanager.GetString("trCash") },
        //        new keyValueString(){key="k-net", value=AppSettings.resourcemanager.GetString("trKNET") },
        //        new keyValueString(){key="visa", value=AppSettings.resourcemanager.GetString("trVisa") },
        //        new keyValueString(){key="master-card", value=AppSettings.resourcemanager.GetString("trMasterCard") },
        //        new keyValueString(){key="amerkan-express", value=AppSettings.resourcemanager.GetString("trAmerkanExpress") },
        //        new keyValueString(){key="return", value=AppSettings.resourcemanager.GetString("trReturned") },
        //        new keyValueString(){key="customer-card", value=AppSettings.resourcemanager.GetString("trCustomerCard") },
        //        new keyValueString(){key="top-students", value=AppSettings.resourcemanager.GetString("trTopStudents") },
        //        new keyValueString(){key="quran-gift", value=AppSettings.resourcemanager.GetString("trHolyQuranGift") },
        //    };

        //    return PaymentMethodsList;
        //}
        #endregion
        #endregion

        #region Area
        static public Area area = new Area();
        static public List<Area> areaList;

        static public async Task<IEnumerable<Area>> RefreshAreas()
        {
            areaList = await area.get(true);

            return areaList;
        }

        static public async Task fillAreas(ComboBox combo)
        {
            if (areaList is null)
                await RefreshAreas();

            combo.ItemsSource = areaList;
            combo.SelectedValuePath = "AreaId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }

        static public async Task fillAreasWithDefault(ComboBox combo)
        {
            if (areaList is null)
                await RefreshAreas();

            var lst = areaList.ToList();
            Area sup = new Area();
            sup.Name = "-";
            sup.AreaId = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;
            combo.SelectedValuePath = "AreaId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = 0;
        }
        #endregion

        #region Job
        static public Job job = new Job();
        static public List<Job> jobList;

        static public async Task<IEnumerable<Job>> RefreshJobs()
        {
            jobList = await job.get(true);

            return jobList;
        }

        static public async Task fillJobs(ComboBox combo)
        {
            if (jobList is null)
                await RefreshJobs();

            combo.ItemsSource = jobList;
            combo.SelectedValuePath = "JobId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }

        static public async Task fillJobsWithDefault(ComboBox combo)
        {
            if (jobList is null)
                await RefreshJobs();

            var lst = jobList.ToList();
            Job sup = new Job();
            sup.Name = "-";
            sup.JobId = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;
            combo.SelectedValuePath = "JobId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = 0;
        }
        #endregion
        #region KinshipTies
        static public KinshipTies kinshipTies = new KinshipTies();
        static public List<KinshipTies> kinshipTiesList;

        static public async Task<IEnumerable<KinshipTies>> RefreshKinshipTiess()
        {
            kinshipTiesList = await kinshipTies.get(true);

            return kinshipTiesList;
        }

        static public async Task fillKinshipTiess(ComboBox combo)
        {
            if (kinshipTiesList is null)
                await RefreshKinshipTiess();

            combo.ItemsSource = kinshipTiesList;
            combo.SelectedValuePath = "KinshipId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }

        static public async Task fillKinshipTiessWithDefault(ComboBox combo)
        {
            if (kinshipTiesList is null)
                await RefreshKinshipTiess();

            var lst = kinshipTiesList.ToList();
            KinshipTies sup = new KinshipTies();
            sup.Name = "-";
            sup.KinshipId = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;
            combo.SelectedValuePath = "KinshipId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = 0;
        }
        #endregion

        #region HirarachyStructure
        static public HirarachyStructure hirarachyStructure = new HirarachyStructure();
        static public List<HirarachyStructure> hirarachyStructureList;

        static public async Task<IEnumerable<HirarachyStructure>> RefreshHirarachyStructures()
        {
            hirarachyStructureList = await hirarachyStructure.get(true);

            return hirarachyStructureList;
        }

        static public async Task fillHirarachyStructures(ComboBox combo)
        {
            if (hirarachyStructureList is null)
                await RefreshHirarachyStructures();

            combo.ItemsSource = hirarachyStructureList;
            combo.SelectedValuePath = "Id";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }

        static public async Task fillHirarachyStructuresWithDefault(ComboBox combo)
        {
            if (hirarachyStructureList is null)
                await RefreshHirarachyStructures();

            var lst = hirarachyStructureList.ToList();
            HirarachyStructure sup = new HirarachyStructure();
            sup.Name = "-";
            sup.Id = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;
            combo.SelectedValuePath = "Id";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = 0;
        }
        #endregion
        #region CustomerBank
        static public CustomerBank customerBank = new CustomerBank();
        static public List<CustomerBank> customerBankList;

        static public async Task<IEnumerable<CustomerBank>> RefreshCustomerBanks()
        {
            customerBankList = await customerBank.get(true);

            return customerBankList;
        }

        static public async Task fillCustomerBanks(ComboBox combo)
        {
            if (customerBankList is null)
                await RefreshCustomerBanks();

            combo.ItemsSource = customerBankList;
            combo.SelectedValuePath = "BankId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }

        static public async Task fillCustomerBanksWithDefault(ComboBox combo)
        {
            if (customerBankList is null)
                await RefreshCustomerBanks();

            var lst = customerBankList.ToList();
            CustomerBank sup = new CustomerBank();
            sup.Name = "-";
            sup.BankId = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;
            combo.SelectedValuePath = "BankId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = 0;
        }
        #endregion
        #region GenderList
        static public List<keyValueString> GenderList;
        static public IEnumerable<keyValueString> RefreshGenderList()
        {
            GenderList = new List<keyValueString>() {
                new keyValueString(){key="male", value=AppSettings.resourcemanager.GetString("Male") },
                new keyValueString(){key="female", value=AppSettings.resourcemanager.GetString("Female") },
            };

            return GenderList;
        }

        static public void fillGender(ComboBox combo)
        {
            if (GenderList is null)
                RefreshGenderList();

            combo.ItemsSource = GenderList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = 0;
        }
        #endregion

        #region MemberNature
        static public List<keyValueString> MemberNatureList;
        static public IEnumerable<keyValueString> RefreshMemberNatureList()
        {
            MemberNatureList = new List<keyValueString>() {
                new keyValueString(){key="worker", value=AppSettings.resourcemanager.GetString("Worker") },
                new keyValueString(){key="not-working", value=AppSettings.resourcemanager.GetString("NotWorking") },
            };

            return MemberNatureList;
        }

        static public void fillMemberNature(ComboBox combo)
        {
            if (MemberNatureList is null)
                RefreshMemberNatureList();

            combo.ItemsSource = MemberNatureList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = 0;
        }
        #endregion 
        #region Martial Status
        static public List<keyValueString> MartialStatusList;
        static public IEnumerable<keyValueString> RefreshMartialStatusList()
        {
            MartialStatusList = new List<keyValueString>() {
                new keyValueString(){key="single", value=AppSettings.resourcemanager.GetString("Single") },
                new keyValueString(){key="married", value=AppSettings.resourcemanager.GetString("Married") },
                new keyValueString(){key="married-support", value=AppSettings.resourcemanager.GetString("MarriedAndSupports") },
                new keyValueString(){key="divorced", value=AppSettings.resourcemanager.GetString("Divorced") },
                new keyValueString(){key="divorced-support", value=AppSettings.resourcemanager.GetString("DivorcedAndSupports") },
                new keyValueString(){key="widower", value=AppSettings.resourcemanager.GetString("Widower") },
                new keyValueString(){key="widower-support", value=AppSettings.resourcemanager.GetString("WidowerAndSupports") },
            };

            return MartialStatusList;
        }

        static public void fillMartialStatus(ComboBox combo)
        {
            if (MartialStatusList is null)
                RefreshMartialStatusList();

            var lst = MartialStatusList.ToList();
            var sup = new keyValueString();
            sup.value = "-";
            sup.key = "";
            lst.Insert(0, sup);

            combo.ItemsSource = MartialStatusList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = 0;
        }
        #endregion
        #region Customer

        #region Activity type
        static public List<ActivityType> activityTypeList;
        static public ActivityType activityType = new ActivityType();

        static public async Task<IEnumerable<ActivityType>> RefreshActivityTypes()
        {
            activityTypeList = await activityType.get(true);

            return activityTypeList;
        }

        static public async Task fillActivityTypesWithDefault(ComboBox combo,int id=0)
        {
            if (activityTypeList is null)
                await RefreshActivityTypes();

            var lst = activityTypeList.ToList();
            lst = lst.Where(x => x.Id != id && x.IsFinal == false).ToList();
            ActivityType sup = new ActivityType();
            sup.Name = "-";
            sup.Id = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;

            combo.SelectedValuePath = "Id";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        static public async Task fillFinalActivityTypes(ComboBox combo)
        {
            if (activityTypeList is null)
                await RefreshActivityTypes();

            var lst = activityTypeList.Where(x => x.IsBlocked == false && x.IsFinal == true).ToList();
            combo.ItemsSource = lst;

            combo.SelectedValuePath = "Id";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }
        #endregion
        
        #region Activity 
        static public List<Activity> activitiesList;
        static public Activity activity = new Activity();

        static public async Task<IEnumerable<Activity>> RefreshActivities()
        {
            activitiesList = await activity.get(true);

            return activitiesList;
        }

        static public async Task fillActivitiesWithDefault(ComboBox combo)
        {
            if (activitiesList is null)
                await RefreshActivities();

            var lst = activitiesList.ToList();
 
            Activity sup = new Activity();
            sup.Description = "-";
            sup.ActivityId = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;

            combo.SelectedValuePath = "ActivityId";
            combo.DisplayMemberPath = "Description";
            combo.SelectedIndex = -1;
        }
        static public async Task fillActivities(ComboBox combo)
        {
            if (activitiesList is null)
                await RefreshActivities();

            var lst = activitiesList.Where(x => x.IsBlocked == false).ToList();
            combo.ItemsSource = lst;

            combo.SelectedValuePath = "ActivityId";
            combo.DisplayMemberPath = "Description";
            combo.SelectedIndex = -1;
        }
        #endregion
        static public Customer customer = new Customer();
        static public List<Customer> customerList;

        static public async Task<IEnumerable<Customer>> RefreshCustomers()
        {
            customerList = await customer.get(true);

            return customerList;
        }

        static public async Task fillCustomers(ComboBox combo)
        {
            if (customerList is null)
                await RefreshCustomers();

            combo.ItemsSource = customerList;
            combo.SelectedValuePath = "CustomerId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = -1;
        }

        static public async Task fillCustomersWithDefault(ComboBox combo)
        {
            if (customerList is null)
                await RefreshCustomers();

            var lst = customerList.ToList();
            Customer sup = new Customer();
            sup.Name = "-";
            sup.CustomerId = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;
            combo.SelectedValuePath = "CustomerId";
            combo.DisplayMemberPath = "Name";
            combo.SelectedIndex = 0;
        }


        #endregion

        #region change box type
        static public List<keyValueString> ChangeBoxTypeList;
        static public IEnumerable<keyValueString> RefreshChangeBoxTypeList()
        {
            ChangeBoxTypeList = new List<keyValueString>() {
                new keyValueString(){key="change", value=AppSettings.resourcemanager.GetString("ChangeBoxNumber") },
                new keyValueString(){key="exchange", value=AppSettings.resourcemanager.GetString("SwitchWithAnotherContributor") },
                new keyValueString(){key="emptying", value=AppSettings.resourcemanager.GetString("ClearBoxNumber") },
               
            };

            return ChangeBoxTypeList;
        }

        static public void fillChangeBoxType(ComboBox combo)
        {
            if (ChangeBoxTypeList is null)
                RefreshChangeBoxTypeList();

            combo.ItemsSource = ChangeBoxTypeList;
            combo.SelectedValuePath = "key";
            combo.DisplayMemberPath = "value";
            combo.SelectedIndex = 0;
        }
        #endregion

        #region Sales
        static public List<PaymentType> paymentTypeList;
        static public SalesInvoice sales = new SalesInvoice();

        static public async Task<IEnumerable<PaymentType>> RefreshPaymentTypes()
        {
            paymentTypeList = await sales.GetPaymentTypes();

            return paymentTypeList;
        }

        static public async Task fillPaymentTypesWithDefault(ComboBox combo)
        {
            if (paymentTypeList is null)
                await RefreshPaymentTypes();

            var lst = paymentTypeList.ToList();

            PaymentType sup = new PaymentType();
            sup.PaymentTypeName = "-";
            sup.PaymentTypeId = 0;
            lst.Insert(0, sup);

            combo.ItemsSource = lst;

            combo.SelectedValuePath = "PaymentTypeId";
            combo.DisplayMemberPath = "PaymentTypeName";
            combo.SelectedIndex = -1;
        }
        static public async Task fillPaymentTypes(ComboBox combo)
        {
            if (paymentTypeList is null)
                await RefreshPaymentTypes();

            combo.ItemsSource = paymentTypeList;

            combo.SelectedValuePath = "PaymentTypeId";
            combo.DisplayMemberPath = "PaymentTypeName";
            combo.SelectedIndex = -1;
        }
        #endregion
        #region users
        static public User user = new User();
        static public List<User> userList;

        static public async Task<IEnumerable<User>> RefreshUsers()
        {
            userList = await user.Get(true);

            return userList;
        }

        static public async Task fillUsers(ComboBox combo)
        {
            if (userList is null)
                await RefreshUsers();

            combo.ItemsSource = userList;
            combo.SelectedValuePath = "UserId";
            combo.DisplayMemberPath = "LoginName";
            combo.SelectedIndex = -1;
        }
        #endregion

        #region Permissions
        static public List<AppObject> appObjectsList;
        static public Permissions permission = new Permissions();

        static public async Task<IEnumerable<AppObject>> RefreshAppObjects()
        {
            appObjectsList = await permission.GetAppObjects();

            return appObjectsList;
        }

        static public List<Role> rolesList;
        static public async Task<IEnumerable<Role>> RefreshRoles()
        {
            rolesList = await permission.GetRoles(true);

            return rolesList;
        }
        static public async Task fillRoles(ComboBox combo)
        {
            if (rolesList is null)
                await RefreshRoles();

            combo.ItemsSource = rolesList;
            combo.SelectedValuePath = "RoleID";
            if(AppSettings.lang =="ar")
                combo.DisplayMemberPath = "NameAr";
            else
                combo.DisplayMemberPath = "NameEn";

            combo.SelectedIndex = -1;
        }
        #endregion
    }
}
