using HarmonyLib;
using UnityEngine;
using System.Reflection;
using static ItemDrop;

namespace CropReplant
{
    public static class PlayerExt
    {
        public static MethodInfo FindHoverObject = typeof(Player).GetMethod("FindHoverObject", AccessTools.allDeclared);
        public static bool CultivatorRequirement(this Player player)
        {
            return CRConfig.oldStyle
                ? player.m_inventory.HaveItem("$item_cultivator")
                : player.m_rightItem?.m_shared.m_name == "$item_cultivator";
        }
        public static void UseItemInHand(this Player player)
        {
            if (CRConfig.useDurability & !CRConfig.oldStyle)
            {
                ItemData item = ((Humanoid)player).m_inventory.GetItem("$item_cultivator", -1, false);
                item.m_durability -= item.m_shared.m_useDurabilityDrain;
            }
        }
    }
}
