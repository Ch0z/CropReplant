﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CropReplant
{
    public static class PickableExt
    {
        public static Dictionary<string, string> pickablePlants = new Dictionary<string, string>
        {
            { "Carrot", "sapling_carrot" },
            { "Turnip", "sapling_turnip" },
            { "Onion", "sapling_onion" },
            { "CarrotSeeds", "sapling_seedcarrot" },
            { "TurnipSeeds", "sapling_seedturnip" },
            { "OnionSeeds", "sapling_seedonion" },
            { "Barley", "sapling_barley" },
            { "Flax", "sapling_flax" },
            { "MushroomMagecap", "sapling_magecap" },
            { "MushroomJotunPuffs", "sapling_jotunpuffs" },
        };

        public static Dictionary<string, string> seedMap = new Dictionary<string, string>
        {
            { "Pickable_Carrot", "sapling_carrot" },
            { "Pickable_Turnip", "sapling_turnip" },
            { "Pickable_Onion", "sapling_onion" },
            { "Pickable_SeedCarrot", "sapling_seedcarrot" },
            { "Pickable_SeedTurnip", "sapling_seedturnip" },
            { "Pickable_SeedOnion", "sapling_seedonion" },
            { "Pickable_Barley", "sapling_barley" },
            { "Pickable_Flax", "sapling_flax" },
            { "Pickable_Mushroom_Magecap", "sapling_magecap" },
            { "Pickable_Mushroom_JotunPuffs", "sapling_jotunpuffs" },
        };


        public static readonly string[] replantableCrops = seedMap.Keys.ToArray();

        public static readonly string[] seeds = seedMap.Values.ToArray();

        private static readonly string[] seedSame = { "same" };

        public static readonly string[] seedsOldStyle = seedSame.Concat(seeds).ToArray();

        private static readonly int totalSeedOptions = seedsOldStyle.Length;
        private static int seedCycle = 0;
        public static string seedName = "same";

        public static void NextSeed()
        {
            if (seedCycle < totalSeedOptions - 1)
                seedCycle++;
            else
                seedCycle = 0;

            seedName = seedsOldStyle[seedCycle];
        }

        public static bool Replantable(this Pickable pickable)
        {
            return System.Array.Exists(replantableCrops, s => pickable.name.StartsWith(s));
        }

        public static void Replant(this Pickable pickable, Player player)
        {
        if (!pickable.m_picked)
            {
                Piece piece;
                GameObject prefab;
                if (CRConfig.oldStyle) 
                {
                    string currentSeedName;
                    currentSeedName = seedName;
                    if (currentSeedName == "same")
                        currentSeedName = seedMap.FirstOrDefault(s => pickable.name.StartsWith(s.Key)).Value;

                    prefab = ZNetScene.instance.GetPrefab(currentSeedName);
                    piece = prefab.GetComponent<Piece>();
                }
                else
                {
                    prefab = player.m_rightItem?.m_shared?.m_buildPieces?.GetSelectedPrefab();

                    piece = null;

                    if (prefab.name == "cultivate_v2")
                    {
                        bool keyExists = pickablePlants.ContainsKey(pickable.m_itemPrefab.name);
                        if (keyExists)
                        {
                            prefab = ZNetScene.instance.GetPrefab(pickablePlants[pickable.m_itemPrefab.name]);
                        }
                        else
                        {
                            return;
                        }
                    }

                    if (prefab != null)
                    {
                        if (System.Array.Exists(seeds, s => prefab?.name == s))
                        {
                            piece = prefab.GetComponent<Piece>();
                        }
                    }
                    else return;
                }

                bool hasResources = player.HaveRequirements(piece, Player.RequirementMode.CanBuild);

                if (hasResources)
                {
                    pickable.Interact(player, repeat: false, false);
                    UnityEngine.Object.Instantiate(prefab, pickable.transform.position, Quaternion.identity);
                    player.ConsumeResources(piece.m_resources, 1, -1, 1);
                    player.UseItemInHand();
                }
                else if (!CRConfig.blockHarvestNoResources)
                {
                    pickable.Interact(player, repeat: false, false);
                }
            }
        }
        public static List<Pickable> FindPickableOfKindInRadius(this Pickable pickable, float distance)
        {
            List<Pickable> pickableList = GameObject.FindObjectsOfType<Pickable>()
                .Where(p => (p.name == pickable.name &&
                Vector3.Distance(pickable.transform.position, p.transform.position) <= distance))
                .ToList();
            pickableList.Remove(pickable);
            return pickableList;
        }
    }
}
