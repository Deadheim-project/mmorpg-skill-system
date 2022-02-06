using Jotunn.Entities;
using Jotunn.Managers;
using System;

namespace ValheimLevelSystem
{
    public class ResetToken
    {
        public static void LoadAssets()
        {
            PrefabManager.OnPrefabsRegistered += AddClonedItems;
        }

        private static void AddClonedItems()
        {
            try
            {
                CustomItem CI = new CustomItem("ResetToken", "Thunderstone");
                ItemDrop itemDrop = CI.ItemDrop;
                itemDrop.m_itemData.m_shared.m_name = "Reset Token";
                itemDrop.m_itemData.m_shared.m_description = "Reset your skills points";
                itemDrop.m_itemData.m_shared.m_maxStackSize = 10;
                ItemManager.Instance.AddItem(CI);
            }
            catch (Exception ex)
            {
                Jotunn.Logger.LogError($"Error while adding cloned item: {ex.Message}");
            }
            finally
            {
                PrefabManager.OnPrefabsRegistered -= AddClonedItems;
            }
        }
    }
}
