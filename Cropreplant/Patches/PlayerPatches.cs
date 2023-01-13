using HarmonyLib;
using UnityEngine;

namespace CropReplant.PlayerPatches
{
#pragma warning disable IDE0051 // Remove unused private members
    [HarmonyPatch(typeof(Player), "Update")]
    static class PlayerUpdate_Patch
    {
        static void Postfix(Player __instance)
        {

            bool keyReplantDown = (CRConfig.useCustomReplantKey || CRConfig.oldStyle)
                ? Input.GetKeyDown(CRConfig.customReplantKey)
                : (ZInput.GetButtonDown("Remove") || ZInput.GetButtonDown("JoyRemove"));

            if (CRConfig.oldStyle)
            {
                bool keyNextSeedDown = Input.GetKeyDown(CRConfig.nextSeedKey);
                if (keyNextSeedDown)
                {
                    PickableExt.NextSeed();
                }
            }

            if (keyReplantDown)
            {
                if (__instance.CultivatorRequirement())
                {
                    __instance.FindHoverObject(out var hover, out var _);
                    Pickable pickable = hover?.GetComponentInParent<Pickable>();
                    if (!(pickable != null) || !pickable.Replantable())
                    {
                        return;
                    }
                    pickable.Replant(__instance);
                    if (!CRConfig.multipick)
                    {
                        return;
                    }
                    foreach (Pickable item in pickable.FindPickableOfKindInRadius(CRConfig.range))
                    {
                        item.Replant(__instance);
                    }
                }
            }
        }
    }
}