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
            combo.DisplayMemberPath = "Name";
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
    }
}
