﻿using BepInEx;
using HarmonyLib;
using System.Linq;
using System.Collections.Generic;
using Jotunn.Entities;
using Jotunn.Managers;

namespace CropReplant
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    public class CropReplant : BaseUnityPlugin
    {
        public const string PluginGUID = "com.github.johndowson.CropReplant";
        public const string PluginName = "CropReplant";
        public const string PluginVersion = "2.2.1";
        private CustomLocalization Localization;

        private static readonly Harmony harmony = new(typeof(CropReplant).GetCustomAttributes(typeof(BepInPlugin), false)
            .Cast<BepInPlugin>()
            .First()
            .GUID);

#pragma warning disable IDE0051 // Remove unused private members

        private void Awake()
        {
            CRConfig.Bind(this);
            harmony.PatchAll();
            Localization = new CustomLocalization();
            LocalizationManager.Instance.AddLocalization(Localization);
            Localization.AddTranslation("English", new Dictionary<string, string>
            {
                {"replant_with", "Replant with"},
                {"same", "the same crop"},
                {"choose_different", "Choose different seed"}
            });
            Localization.AddTranslation("French", new Dictionary<string, string>
            {
                {"replant_with", "Replanter avec"},
                {"same", "la même graine"},
                {"choose_different", "Choisir une autre graine"}
            });
        }

        private void OnDestroy()
        {
            harmony.UnpatchSelf();
        }
    }
}