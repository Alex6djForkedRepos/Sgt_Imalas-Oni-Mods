using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace ModOriginInfo.Patches
{
	internal class CodexEntryGenerator_Patches
	{

		[HarmonyPatch(typeof(CodexEntryGenerator), nameof(CodexEntryGenerator.GenerateBuildingDescriptionContainers))]
		public class CodexEntryGenerator_GenerateBuildingDescriptionContainers_Patch
		{
			public static void Postfix( BuildingDef def, List<ContentContainer> containers)
			{
				string modOrigin = ModAssets.GetModNameIfValid(def, 0);
				if (modOrigin.Any() && containers.Any())
				{
					containers.Last().content.Add(new CodexText(modOrigin));
				}
			}
		}
	}
}
