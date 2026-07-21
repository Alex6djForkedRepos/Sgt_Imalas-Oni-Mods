using BionicBoostersPlus.Content.ModDb;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BionicBoostersPlus.Content.Scripts
{
	class Db_Patches
	{

		[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
		public class Db_Initialize_Patch
		{
			public static void Postfix(Db __instance)
			{
				BB_Db.Init(__instance);
			}
		}
	}
}
