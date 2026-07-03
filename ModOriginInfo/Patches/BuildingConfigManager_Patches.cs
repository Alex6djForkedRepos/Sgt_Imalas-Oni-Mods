using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;

namespace ModOriginInfo.Patches
{
	internal class BuildingConfigManager_Patches
	{

        [HarmonyPatch(typeof(BuildingConfigManager), nameof(BuildingConfigManager.RegisterBuilding))]
        public class BuildingConfigManager_RegisterBuilding_Patch
        {
			static void Postfix(BuildingConfigManager __instance,IBuildingConfig config)
			{
				if(__instance.configTable.TryGetValue(config, out var def))
					ModAssets.RegisterBuildingDef(config, def);

			}
		}
	}
}
