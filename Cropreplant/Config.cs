using BepInEx.Configuration;
using Jotunn.Configs;
using Jotunn.Managers;
using UnityEngine;

namespace CropReplant
{
    public static class CRConfig
    {
        public static ConfigEntry<float> c_range;
        public static ConfigEntry<bool> c_multipick;
        public static ConfigEntry<bool> c_displayGrowth;
        public static ConfigEntry<bool> c_useDurability;
        public static ConfigEntry<bool> c_blockHarvestNoResources;
        public static ConfigEntry<bool> c_oldStyle;

        public static float range => c_range.Value;
        public static bool multipick => c_multipick.Value;
        public static bool displayGrowth => c_displayGrowth.Value;
        public static bool useDurability => c_useDurability.Value;
        public static bool blockHarvestNoResources => c_blockHarvestNoResources.Value;
        public static bool oldStyle => c_oldStyle.Value;

        public static ConfigEntry<bool> c_useCustomReplantKey;
        public static ConfigEntry<KeyCode> c_customReplantKey;
        public static ConfigEntry<KeyCode> c_nextSeedKey;

        public static bool useCustomReplantKey => c_useCustomReplantKey.Value;
        public static KeyCode customReplantKey => c_customReplantKey.Value;
        public static KeyCode nextSeedKey => c_nextSeedKey.Value;
        
        public static ButtonConfig replantButton;
        public static KeyHintConfig replantHint;

        public static ButtonConfig nextSeedButton;
        public static KeyHintConfig nextKeyHint;

        public static void Bind(CropReplant cr)
        {
            c_range = cr.Config.Bind("General", "MultipickRange", 2f, "Radius to pick crops in");
            c_multipick = cr.Config.Bind("General", "MultipickEnable", true, "Enable picking crops in radius");

            c_displayGrowth = cr.Config.Bind("General", "DisplayGrowth", true, "Enable growth % tooltip on plant.");

            c_useDurability = cr.Config.Bind("General", "UseDurability", true, "Enable cultivator durability depletion when replanting.");

            c_blockHarvestNoResources = cr.Config.Bind("General", "blockHarvestNoResources", false, "Block harvest if no resource to replant.");

            c_oldStyle = cr.Config.Bind("General", "oldStyle", false, "0.1.6 style replanting");

            c_useCustomReplantKey = cr.Config.Bind("General", "UseCustomReplantKey", false, "Use a custom keybind instead of game's alternative attack bind.");

            c_customReplantKey = cr.Config.Bind("General", "customReplantKey", KeyCode.H, "Custom keybind to use instead of of game's alternative attack bind.");
            c_nextSeedKey = cr.Config.Bind("General", "nextSeedKey", KeyCode.J, "Hotkey for scrolling through replant options");
            replantButton = new ButtonConfig
            {
                Name = "ReplantKey",
                Config = c_customReplantKey,
                HintToken = "Replant"
            };
            KeyHintConfig keyHintReplant = new KeyHintConfig
            {
                ButtonConfigs = new ButtonConfig[1] { replantButton }
            };
            nextSeedButton = new ButtonConfig
            {
                Name = "nextSeedKey",
                Config = c_nextSeedKey,
                HintToken = "Next Seed Key"
            };
            KeyHintConfig KeyHintNextSeed = new KeyHintConfig
            {
                ButtonConfigs = new ButtonConfig[1] { nextSeedButton }
            };
            KeyHintManager.Instance.AddKeyHint(keyHintReplant);
            KeyHintManager.Instance.AddKeyHint(KeyHintNextSeed);
            InputManager.Instance.AddButton("com.github.johndowson.CropReplant", replantButton);
            InputManager.Instance.AddButton("com.github.johndowson.CropReplant", nextSeedButton);
        }
    }

}