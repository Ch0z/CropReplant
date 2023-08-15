using HarmonyLib;
using Jotunn;
using UnityEngine.UIElements;

namespace CropReplant.PickablePatches
{
#pragma warning disable IDE0051 // Remove unused private members
    [HarmonyPatch(typeof(Pickable), "GetHoverText")]
    static class PickableGetHoverText_Patch
    {
        static string Postfix(string __result, Pickable __instance)
        {
            if (!__instance.m_picked)
            {
                var player = Player.m_localPlayer;
                if (player.CultivatorRequirement() && __instance.Replantable())
                {
                    string seedNameLocalized = PickableExt.seedName == "same"
                        ? Localization.instance.Localize("$same")
                        : Localization.instance.Localize("$piece_" + PickableExt.seedName);
                    return __result +
                        $"\n[<color=yellow><b>{CRConfig.customReplantKey}</b></color>] " + Localization.instance.Localize("$replant_with") + " " + seedNameLocalized +
                        $"\n[<color=yellow><b>{CRConfig.nextSeedKey}</b></color>] " + Localization.instance.Localize("$choose_different");
                }
            }
            return __result;
        }
    }
}
