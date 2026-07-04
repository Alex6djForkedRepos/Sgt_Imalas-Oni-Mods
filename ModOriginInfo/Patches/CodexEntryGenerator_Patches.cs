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
			public static void Postfix(BuildingDef def, List<ContentContainer> containers)
			{
				string modOrigin = ModAssets.GetModNameIfValid(def, 0);
				if (modOrigin.Any() && containers.Any())
				{
					containers.Last().content.Add(new CodexText(modOrigin));
				}
			}
		}


		[HarmonyPatch(typeof(CodexEntryGenerator_Elements), nameof(CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers))]
		public class CodexEntryGenerator_Elements_GenerateMadeAndUsedContainers_Patch
		{
			public static void Prefix(Tag tag, List<ContentContainer> containers)
			{
				if (ModAssets.IsModded(tag, out _))
				{
					string modOrigin = ModAssets.GetModNameIfValid(tag, 0);
					if (modOrigin.Any() && containers.Any())
					{
						containers.Last().content.Add(new CodexText(modOrigin));
					}
				}
			}
		}
	}
}
