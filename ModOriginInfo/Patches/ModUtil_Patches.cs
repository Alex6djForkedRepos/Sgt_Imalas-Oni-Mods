using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static STRINGS.UI;

namespace ModOriginInfo.Patches
{
	internal class ModUtil_Patches
	{

        [HarmonyPatch(typeof(ModUtil), nameof(ModUtil.CreateSubstance))]
        public class ModUtil_CreateSubstance_Patch
        {
            public static void Postfix(string name)
			{
				var assembly = Assembly.GetCallingAssembly();
				ModAssets.RegisterElementId(assembly, name);
			}
        }
	}
}
