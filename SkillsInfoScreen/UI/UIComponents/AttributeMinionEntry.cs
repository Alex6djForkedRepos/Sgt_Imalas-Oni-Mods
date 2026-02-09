using FMOD;
using Klei.AI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;
using UnityEngine.UI;
using static KSerialization.DebugLog;
using static STRINGS.UI.UISIDESCREENS.AUTOPLUMBERSIDESCREEN.BUTTONS;

namespace SkillsInfoScreen.UI.UIComponents
{
	internal class AttributeMinionEntry : KMonoBehaviour
	{
		Attribute Attribute;
		Image XP_Progressbar;
		LocText XP_Progress, XPLevelInfo, TotalLevelInfo;
		AttributeLevels Levels;
		MinionIdentity Minion;


		public void Init(MinionIdentity minion, Attribute attribute)
		{
			Minion = minion;
			Attribute = attribute;
			Levels = minion.GetComponent<AttributeLevels>();

			XP_Progressbar = transform.Find("XPBar/fill").gameObject.GetComponent<Image>();
			XP_Progress = transform.Find("XPBar/amountText").gameObject.GetComponent<LocText>();
			XPLevelInfo = transform.Find("XPBar/levelText").gameObject.GetComponent<LocText>();
			TotalLevelInfo = transform.Find("SkillPoints").gameObject.GetComponent<LocText>();
			Refresh();
		}
		public override void OnSpawn()
		{
			base.OnSpawn();
			Refresh();
		}
		public void Refresh()
		{
			if(Levels != null)
			{
				int levelVal = Levels.GetLevel(Attribute);
				//int maxLevelVal = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL;
				var level = Levels.GetAttributeLevel(Attribute.Id);
				int currentLvlXp = Mathf.RoundToInt(level.experience);
				int maxLvlXp = Mathf.RoundToInt(level.GetExperienceForNextLevel());
				float levelPercentage = level.GetPercentComplete();

				XP_Progressbar.fillAmount = levelPercentage;
				XP_Progress.SetText(STRINGS.XP_VERY_SHORT+$"{currentLvlXp}/{maxLvlXp}");
				XPLevelInfo.SetText(STRINGS.LEVEL_VERY_SHORT+ levelVal);
			}
			if(Minion != null)
			{
				AttributeInstance instance = Minion.modifiers.attributes.Get(Attribute.Id);
				int level = (int)ModAssets.GetTotalDisplayValue(instance);
				TotalLevelInfo.SetText(level.ToString());
			}
		}
	}
}
