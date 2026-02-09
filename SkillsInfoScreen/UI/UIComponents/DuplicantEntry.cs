using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

namespace SkillsInfoScreen.UI.UIComponents
{
	internal class DuplicantEntry : KMonoBehaviour
	{
		MinionIdentity Minion;
		MinionResume Resume;
		Image MinionImage;
		GameObject TraitPrefab, TraitContainer;
		LocText MinionName;
		Image XP_Progressbar;
		LocText XP_Progress, SkillpointsInfo;
		GameObject AttributePrefab, SpacerPrefab;
		Dictionary<string, AttributeMinionEntry> Attributes = [];
		Dictionary<string, GameObject> Traits = [];

		public void Init(MinionIdentity minion)
		{
			ModAssets.LoadColors();

			Minion = minion;
			Resume = minion.GetComponent<MinionResume>();

			XP_Progressbar = transform.Find("XP/XPBar/fill").gameObject.GetComponent<Image>();
			XP_Progress = transform.Find("XP/XPBar/amountText").gameObject.GetComponent<LocText>();
			SkillpointsInfo = transform.Find("XP/SkillPoints").gameObject.GetComponent<LocText>();


			AttributePrefab = transform.Find("AttributeInfo").gameObject;
			AttributePrefab.SetActive(false);
			SpacerPrefab = transform.Find("Spacer").gameObject;
			MinionName = transform.Find("NameTraitContainer/DupeName").gameObject.GetComponent<LocText>();
			MinionName.SetText(minion.GetProperName());
			MinionImage = transform.Find("IconContainer/Icon").gameObject.GetComponent<Image>();
			MinionImage.sprite = Db.Get().Personalities.Get(minion.personalityResourceId).GetMiniIcon();

			TraitContainer = transform.Find("NameTraitContainer").gameObject;
			TraitPrefab = transform.Find("NameTraitContainer/TraitPrefab").gameObject;
			TraitPrefab.SetActive(false);

			InitAttributes();
			InitTraits();
		}

		internal void Refresh()
		{
			foreach (AttributeMinionEntry att in Attributes.Values)
				att.Refresh();
			RefreshTraits();
			RefreshXP();
		}
		void RefreshXP()
		{
			var totalSkillPoints = Resume.TotalSkillPointsGained;
			var availableSkillPoints = Resume.AvailableSkillpoints;
			SkillpointsInfo.SetText($"{availableSkillPoints}/{totalSkillPoints} " + global::STRINGS.UI.SKILLS_SCREEN.SORT_BY_SKILL_AVAILABLE);


			float previousExperienceBar = MinionResume.CalculatePreviousExperienceBar(totalSkillPoints);
			float nextExperienceBar = MinionResume.CalculateNextExperienceBar(totalSkillPoints);
			float currentXPPercentage = (Resume.TotalExperienceGained - previousExperienceBar) / (nextExperienceBar - previousExperienceBar);
			this.XP_Progress.SetText($"{Mathf.RoundToInt(Resume.TotalExperienceGained - previousExperienceBar).ToString()} / {Mathf.RoundToInt(nextExperienceBar - previousExperienceBar).ToString()}");
			this.XP_Progressbar.fillAmount = currentXPPercentage;
		}

		void RefreshTraits()
		{
			foreach (var traitGO in Traits.Values)
				traitGO.SetActive(false);
			//too cluttered ui
			return;

			var minionTraits = Minion.GetComponent<Traits>().TraitList;
			minionTraits.RemoveAll(t => t.Id.Contains("BaseTrait") || DUPLICANTSTATS.JOYTRAITS.Any(j => j.id == t.Id) || DUPLICANTSTATS.STRESSTRAITS.Any(s => s.id == t.Id) || t.Id == "StressShocker");
			bool tooManyTraits = minionTraits.Count > 4;
			if (tooManyTraits)
			{
				AddOrGetTraitContainer(TooMany, minionTraits.Count + " " + global::STRINGS.UI.CHARACTERCONTAINER_TRAITS_TITLE, null);
			}
			else
			{
				foreach (var trait in Minion.GetComponent<Traits>().TraitList)
				{
					AddOrGetTraitContainer(trait.Id, trait.GetName(), trait.PositiveTrait);
				}
			}
		}
		const string TooMany = "TooManyTraitsEntry";

		void AddOrGetTraitContainer(string traitID, string traitName, bool? goodTrait)
		{
			if (!Traits.TryGetValue(traitID, out var TraitGO))
			{
				TraitGO = Util.KInstantiateUI(TraitPrefab, TraitContainer, true);
				Traits.Add(traitID, TraitGO);
			}
			TraitGO.GetComponentInChildren<LocText>().SetText(traitName);
			TraitGO.SetActive(true);
			if (goodTrait.HasValue)
				TraitGO.GetComponent<Image>().color = goodTrait.Value ? ModAssets.Good : ModAssets.Bad;
			else
				TraitGO.GetComponent<Image>().color = Color.gray;

		}

		void InitTraits()
		{

		}

		void InitAttributes()
		{
			var attributeDb = Db.Get().Attributes;
			var stats = DUPLICANTSTATS.ALL_ATTRIBUTES.OrderBy(id => global::STRINGS.UI.StripLinkFormatting(attributeDb.TryGet(id)?.Name ?? "unknown"));

			foreach (var attributeId in stats)
			{
				if (attributeId == "SpaceNavigation" && !DlcManager.IsExpansion1Active())
					continue;

				var attribute = attributeDb.TryGet(attributeId);

				var attributeEntryGO = Util.KInstantiateUI(AttributePrefab, gameObject);
				attributeEntryGO.SetActive(true);
				var entry = attributeEntryGO.AddOrGet<AttributeMinionEntry>();
				entry.Init(Minion, attribute);
				Attributes[attributeId] = entry;

				if (attributeId != stats.Last())
					Util.KInstantiateUI(SpacerPrefab, gameObject);
			}
		}
	}
}
