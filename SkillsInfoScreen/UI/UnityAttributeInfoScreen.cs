using Klei.AI;
using SkillsInfoScreen.UI.UIComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateClasses;
using TUNING;
using UnityEngine;
using UtilLibs;
using UtilLibs.UI.FUI;
using UtilLibs.UIcmp;
using static GameTags;
using static STRINGS.BUILDINGS.PREFABS.DOOR.CONTROL_STATE;

namespace SkillsInfoScreen.UI
{
	internal class UnityAttributeInfoScreen : KScreen
	{
		public UnityAttributeInfoScreen() : base()
		{
			ConsumeMouseScroll = true;
		}

		public static UnityAttributeInfoScreen Instance = null;

		GameObject ItemContainer;
		WorldHeaderEntry WorldHeaderPrefab;
		DuplicantEntry DuplicantEntryPrefab;
		FToggle SplitByAsteroids;
		FOrderByParamToggle SortByName, SortByXP;
		GameObject SortByContainer, SortByAttributePrefab, SortBySpacerPrefab;
		Dictionary<string, FOrderByParamToggle> SortByAttributes = [];
		FButton Close;

		Dictionary<string, Klei.AI.Attribute> Columns = [];
		Dictionary<int, WorldHeaderEntry> WorldHeaders = [];
		Dictionary<int, DuplicantEntry> MinionEntries = [];

		static Dictionary<string, float> MaxSkillLevels;

		public static void DestroyInstance() { Instance = null; }

		public static void InitScreen(GameObject parent)
		{
			if (Instance == null)
			{
				Instance = Util.KInstantiateUI<UnityAttributeInfoScreen>(ModAssets.ScreenGO, parent, true);
				Instance.Init();
				Instance.Show(false);
			}
		}
		public override void OnShow(bool show)
		{
			base.OnShow(show);
			if (show)
			{
				transform.SetAsLastSibling();
				Refresh();
			}
		}
		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Init();
		}
		bool init;
		void Init()
		{
			if (init)
				return;
			init = true;

			ItemContainer = transform.Find("ScrollArea/Content").gameObject;
			WorldHeaderPrefab = transform.Find("ScrollArea/Content/WorldPrefab").gameObject.AddOrGet<WorldHeaderEntry>();
			WorldHeaderPrefab.gameObject.SetActive(false);
			DuplicantEntryPrefab = transform.Find("ScrollArea/Content/DupePrefab").gameObject.AddOrGet<DuplicantEntry>();
			DuplicantEntryPrefab.gameObject.SetActive(false);
			SplitByAsteroids = transform.Find("Info/ShowAsteroids/Checkbox").gameObject.AddOrGet<FToggle>();
			SortByName = transform.Find("Info/Filter_Name").gameObject.AddOrGet<FOrderByParamToggle>();
			SortByXP = transform.Find("Info/Filter_XP").gameObject.AddOrGet<FOrderByParamToggle>();
			SortByContainer = transform.Find("Info").gameObject;
			SortByAttributePrefab = transform.Find("Info/Attribute_Filter").gameObject;
			SortByAttributePrefab.gameObject.SetActive(false);
			SortBySpacerPrefab = transform.Find("Info/spacer").gameObject;

			Close = transform.Find("TopBar/CloseButton").gameObject.AddOrGet<FButton>();
			Close.OnClick += () => ManagementMenu.Instance.CloseAll();
			Close.PlayClickSound = false;

			transform.Find("TopBar").gameObject.AddOrGet<DraggablePanel>().Target = transform;

			transform.Find("ResizeKnob").gameObject.AddOrGet<ResizeDragKnob>().Target = transform;

			InitAttributeFilters();
		}

		void Refresh()
		{
			RefreshAsteroidHeaders();
			RefreshMinionEntries();
		}
		void RefreshAsteroidHeaders()
		{
			if (DlcManager.IsPureVanilla())
				return;
			foreach (var entry in WorldHeaders.Values)
			{
				entry.gameObject.SetActive(false);
			}
			foreach (var worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.isDiscovered)
					continue;
				if (!Components.LiveMinionIdentities.Any(id => id.GetMyWorldId() == worldContainer.id))
					continue;
				int id = worldContainer.id;
				var header = AddOrGetWorldHeader(id);
				header.Refresh();
			}
		}
		void RefreshMinionEntries()
		{
			foreach (var entry in MinionEntries.Values)
			{
				entry.gameObject.SetActive(false);
			}
			var minions =
				Components.LiveMinionIdentities
				.OrderBy(minion => minion.GetMyWorldId())
				.ThenByDescending(minion => minion.GetProperName());


			foreach (MinionIdentity minion in minions)
			{
				var entry = AddOrGetMinionEntry(minion);
				entry.Refresh();
				if (DlcManager.IsExpansion1Active() && WorldHeaders.TryGetValue(minion.GetMyWorldId(), out var header))
				{
					entry.transform.SetSiblingIndex(header.transform.GetSiblingIndex() + 1);
				}
			}

		}

		void InitAttributeFilters()
		{
			var attributeDb = Db.Get().Attributes;
			MaxSkillLevels = [];

			var stats = DUPLICANTSTATS.ALL_ATTRIBUTES.OrderBy(id => global::STRINGS.UI.StripLinkFormatting(attributeDb.TryGet(id)?.Name ?? "unknown"));

			foreach (var attributeId in stats)
			{
				if (attributeId == "SpaceNavigation" && !DlcManager.IsExpansion1Active())
					continue;
				MaxSkillLevels[attributeId] = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL;

				var attribute = attributeDb.TryGet(attributeId);
				Columns[attributeId] = attribute;

				var filterGO = Util.KInstantiateUI(SortByAttributePrefab, SortByContainer);
				filterGO.SetActive(true);
				filterGO.GetComponentInChildren<LocText>().SetText(attribute.Name);
				var toggle = filterGO.AddOrGet<FOrderByParamToggle>();
				SortByAttributes[attributeId] = toggle;

				if (attributeId != stats.Last())
					Util.KInstantiateUI(SortBySpacerPrefab, SortByContainer, true);
				SgtLogger.l("Adding column for " + attributeId);
			}
		}

		DuplicantEntry AddOrGetMinionEntry(MinionIdentity minion)
		{
			if (!MinionEntries.TryGetValue(minion.GetInstanceID(), out var value))
			{
				value = Util.KInstantiateUI<DuplicantEntry>(DuplicantEntryPrefab.gameObject, ItemContainer, true);
				value.Init(minion);
				MinionEntries.Add(minion.GetInstanceID(), value);
			}
			value.gameObject.SetActive(true);
			return value;
		}
		WorldHeaderEntry AddOrGetWorldHeader(int worldId)
		{
			if (!WorldHeaders.TryGetValue(worldId, out var value))
			{
				value = Util.KInstantiateUI<WorldHeaderEntry>(WorldHeaderPrefab.gameObject, ItemContainer, true);
				value.Init(ClusterManager.Instance.GetWorld(worldId));
				WorldHeaders.Add(worldId, value);
			}
			value.gameObject.SetActive(true);
			return value;
		}


		public override void Show(bool show = true)
		{
			base.Show(show);
		}

		public override void OnKeyDown(KButtonEvent e)
		{
			if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
			{
				this.Show(false);
			}
			base.OnKeyDown(e);
		}
	}
}
