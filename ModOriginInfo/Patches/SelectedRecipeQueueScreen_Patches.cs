using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UtilLibs;

namespace ModOriginInfo.Patches
{
	internal class SelectedRecipeQueueScreen_Patches
	{

		[HarmonyPatch(typeof(SelectedRecipeQueueScreen), nameof(SelectedRecipeQueueScreen.SetRecipeCategory))]
		public class SelectedRecipeQueueScreen_SetRecipeCategory_Patch
		{
			public static void Postfix(SelectedRecipeQueueScreen __instance, string recipeCategoryID)
			{
				if (ModAssets.IsModded(recipeCategoryID, out _)) {
					SgtLogger.l(recipeCategoryID);
					__instance.recipeMainDescription.text += ModAssets.GetModNameIfValid(recipeCategoryID, 2);
				}
			}
		}
	}
}
