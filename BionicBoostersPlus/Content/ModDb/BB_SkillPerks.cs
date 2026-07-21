using Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace BionicBoostersPlus.Content.ModDb
{
	internal class BB_SkillPerks
	{
		public static SkillPerk
			BB_BionicDream,
			BB_BionicCarryCapacityOC
			
			;
		public static void Register(SkillPerks __instance)
		{
			BB_BionicDream = __instance.Add(new SimpleSkillPerk(nameof(BB_BionicDream), STRINGS.UI.ROLES_SCREEN.PERKS.BB_BIONICDREAM.DESCRIPTION));
			BB_BionicCarryCapacityOC = __instance.Add(new SkillAttributePerk(nameof(BB_BionicCarryCapacityOC), Db.Get().Attributes.CarryAmount.Id, 500f, BB_Boosters.GetOCName(BionicUpgradeComponentConfig.Booster_Carry1), true));

		}
	}
}
