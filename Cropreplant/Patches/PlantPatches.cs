using HarmonyLib;
using System;
using System.Reflection;

namespace CropReplant.Patches
{
#pragma warning disable IDE0051 // Remove unused private members
    [HarmonyPatch(typeof(Plant), "GetHoverText")]
    static class PlantGetHoverText_Patch
    {
		private static string GetColour(double percentage)
		{
			string colour = "#e74c3c";//Red
			if (percentage >= 25.0 && percentage <= 50.0)
			{
				colour = "#e67e22";//Orange
			}
			if (percentage >= 50.0 && percentage <= 75.0)
			{
				colour = "#f1c40f";//Yellow
			}
			if (percentage >= 75.0)
			{
				colour = "#27ae60";//Green
			}
            return colour;
		}
        static string Postfix(string __result, Plant __instance)
        {
			if (CRConfig.displayGrowth) 
			{
				bool is_healthy = __instance.GetStatus() == Plant.Status.Healthy;
				if (is_healthy)
				{
					DateTime d = new DateTime(__instance.m_nview.GetZDO().GetLong("plantTime", ZNet.instance.GetTime().Ticks));
                    var timeSincePlanted = (ZNet.instance.GetTime() - d).TotalSeconds;
					var growTime = __instance.GetGrowTime();
					var percentGrow = (int)(timeSincePlanted / growTime * 100);
					string colour = GetColour(percentGrow);
					if (percentGrow < 100)
					{
						__result += $"\n<color={colour}>{percentGrow}% - {TimeSpan.FromSeconds(growTime - timeSincePlanted):hh\\:mm\\:ss}</color>";
					}
					if (percentGrow >= 100)
					{
                        __result += $"\n<color={colour}>100%</color>";
					}
				}
            }
            return __result;
        }
    }
}
