//using Database;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace BionicBoostersPlus.Content.ModDb
//{
//	internal class BB_Skills
//	{
//		public static Skill
//			 Adaptation_EyeProtection //Double Eyelids
//			, Adaptation_GillProtection //Mucus Glands
//			, Adaptation_Insulation //Blubber/Fat Layer
//			, Adaptation_WaterBreathingRateReduction //idk, sth about rebreathing/slowing down heartrate when diving
//			, Adaptation_SlimySkin
//			;

//		public static void Register(Skills __instance)
//		{

//			Adaptation_WaterBreathingRateReduction =
//				__instance.AddSkill(new Skill("Adaptation_WaterBreathingRateReduction",
//				ADAPTATION_WATERBREATHINGRATEREDUCTION.NAME,
//				ADAPTATION_WATERBREATHINGRATEREDUCTION.TOOLTIP,
//				0, "", "skillbadge_role_adaptation_gills",
//				Aq_SkillGroups.ADAPTATION_ID,
//				[Aq_SkillPerks.Adapt_WaterbreathingEfficiency, Db.Get().SkillPerks.IncreasedLungCapacity],
//				null
//				//, requiredDuplicantModel, dlc
//				));

//			Adaptation_EyeProtection =
//				__instance.AddSkill(new Skill("Adaptation_EyeProtection",
//				ADAPTATION_EYEPROTECTION.NAME,
//				ADAPTATION_EYEPROTECTION.TOOLTIP,
//				0, "", "skillbadge_role_adaptation_eye_protection",
//				Aq_SkillGroups.ADAPTATION_ID,
//				[Aq_SkillPerks.Adapt_EyeProtectionMinor, Aq_SkillPerks.Adapt_EyeProtectionMajor],
//				null
//				//, requiredDuplicantModel, dlc
//				));

//			Adaptation_Insulation =
//				__instance.AddSkill(new Skill("Adaptation_Insulation",
//				ADAPTATION_INSULATION.NAME,
//				ADAPTATION_INSULATION.TOOLTIP,
//				0, "", "skillbadge_role_adaptation_insulation",
//				Aq_SkillGroups.ADAPTATION_ID,
//				[Aq_SkillPerks.Adapt_ColdImmunity, Aq_SkillPerks.Adapt_FatLayer],
//				null
//				//, requiredDuplicantModel, dlc
//				));

//			Adaptation_GillProtection =
//				__instance.AddSkill(new Skill("Adaptation_GillProtection",
//				ADAPTATION_GILLPROTECTION.NAME,
//				ADAPTATION_GILLPROTECTION.TOOLTIP,
//				1, "", "skillbadge_role_adaptation_mucus",
//				Aq_SkillGroups.ADAPTATION_ID,
//				[Aq_SkillPerks.Adapt_SuitAirImmunity, Aq_SkillPerks.Adapt_ItchyGillsImmunity, Aq_SkillPerks.Adapt_GillMoisturizing, Aq_SkillPerks.Adapt_HeatImmunity],
//				[Adaptation_WaterBreathingRateReduction.Id, Adaptation_EyeProtection.Id, Adaptation_Insulation.Id]
//				//, requiredDuplicantModel, dlc
//				));
//		}
//	}
//}
