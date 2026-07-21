using BionicBoostersPlus.Content.Scripts;
using Epic.OnlineServices.UserInfo;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UtilLibs;

namespace BionicBoostersPlus.Patches
{
	class SelectedRecipeQueueScreen_Patches
	{

		[HarmonyPatch(typeof(SelectedRecipeQueueScreen), nameof(SelectedRecipeQueueScreen.GetResultDescriptions))]
		public class SelectedRecipeQueueScreen_GetResultDescriptions_Patch
		{
			public static void Postfix(SelectedRecipeQueueScreen __instance, List<SelectedRecipeQueueScreen.DescriptorWithSprite> __result, ComplexRecipe recipe)
			{
				if (__instance.target is not ComplexFabricatorRandomOutput randomOutput)
					return;

				if (!__result.Any() || !recipe.results.Any())
					return;

				var chipsEntry = __result.Last();

				var product = recipe.results.Last();

				var oldDesc = __result.Last().descriptor;

				var text = oldDesc.text;
				var tt = oldDesc.tooltipText;

				string rangeTT = string.Format("{0} - {1}", randomOutput.MinAmount(recipe), randomOutput.MaxAmount(recipe));

				tt = tt.Replace(product.amount.ToString(), rangeTT);
				text = text.Replace(product.amount.ToString(), rangeTT);

				__result.Remove(chipsEntry);
				__result.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(
					new Descriptor(text, tt, oldDesc.type, oldDesc.onlyForSimpleInfoScreen),
					chipsEntry.tintedSprite,
					chipsEntry.showFilterRow));

			}
		}
	}
}
