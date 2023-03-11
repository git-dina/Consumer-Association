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
        #endregion
        #region Assistant Supplier
        static public AssistantSupplier assistantSupplier = new AssistantSupplier();
        static public List<AssistantSupplier> assistantSupplierList ;

        static public async Task<IEnumerable<AssistantSupplier>> RefreshAssistantSuppliers()
        {
            assistantSupplierList = await assistantSupplier.get(true);

            return assistantSupplierList;
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
        #endregion
    }
}
