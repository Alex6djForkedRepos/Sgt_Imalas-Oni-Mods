using Klei.AI;
using KMod;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UtilLibs;

namespace ModOriginInfo
{
	internal class ModAssets
	{
		/// <summary>
		/// Mod ids that get skipped in the research screen (they add their own origin tooltips)
		/// </summary>
		public static HashSet<string> SkippableResearchModIds = ["Rocketry Expanded", "RonivansLegacy_ChemicalProcessing"];
		/// <summary>
		/// if you have a mod that does this as well, msg me or reflect for this method below to add your own id to the list
		/// </summary>
		/// <param name="modID"></param>
		public static void AddSkippableResearchModId(string modID) => SkippableResearchModIds.Add(modID);

		public static bool IsModdedResearchItem(string id) => IsModdedInternal(id, out var modId) && !SkippableResearchModIds.Contains(modId);

		public static bool IsModded(KPrefabID id) => id != null && IsModdedInternal(id.PrefabTag, out _);
		public static bool IsModded(string id, out string modId) => IsModdedInternal(id, out modId);
		public static bool IsModded(Tag id, out string modId) => IsModdedInternal(id, out modId);
		static bool IsModdedInternal(Tag tag, out string modId) => ModdedPrefabs.TryGetValue(tag, out modId);
		static Dictionary<Tag, string> ModdedPrefabs = [];

		internal static void RegisterBuildingDef(IBuildingConfig cfg, BuildingDef def)
		{
			var defType = cfg.GetType();

			if (AssemblyToModIdMap.TryGetValue(defType.Assembly, out string modID))
			{
				ModdedPrefabs[def.PrefabID] = modID;
				//SgtLogger.l("Modded Building: " + def.PrefabID + " from " + modID);
			}
			//else
			//	SgtLogger.l("Vanilla Building: "+def.PrefabID);
		}

		internal static void RegisterEntity(IEntityConfig def, GameObject prefab)
		{

		}
		internal static void RegisterMultiEntity(IMultiEntityConfig def, GameObject prefab)
		{

		}

		static Dictionary<Assembly, string> AssemblyToModIdMap = [];

		internal static void MapAssembliesToMods()
		{
			foreach (var mod in Global.Instance.modManager.mods)
			{
				if (!mod.IsEnabledForActiveDlc() || mod.loaded_mod_data == null || mod.loaded_mod_data.dlls == null)
					continue;

				foreach (var dll in mod.loaded_mod_data.dlls)
				{
					AssemblyToModIdMap.Add(dll, mod.staticID);
				}
			}
		}


		static Color modLabelColor = UIUtils.rgb(52, 79, 103),
					 modLabelResearchColor = UIUtils.rgb(123, 177, 255);
		static Dictionary<string, string> modIdToNamesMap = [];

		internal static string GetModNameIfValid(KPrefabID def, int newLineCount) => def != null ? GetModNameIfValid(def.PrefabTag, newLineCount) : string.Empty;
		internal static string GetModNameIfValid(BuildingDef def, int newLineCount) => def != null ? GetModNameIfValid(def.PrefabID, newLineCount) : string.Empty;
		internal static string GetModNameIfValid(Tag tag, int newLineCount) => GetModNameIfValid(tag.ToString(), newLineCount);
		internal static string GetResearchModNameIfValid(string tag, int newLineCount) => GetModNameIfValid(tag, newLineCount, modLabelResearchColor);
		internal static string GetModNameIfValid(string tag, int newLineCount, Color? colorOverride = null)
		{
			if (!colorOverride.HasValue)
				colorOverride = modLabelColor;
			var result = string.Empty;

			if (!IsModded(tag, out string modId))
				return result;

			if (!modIdToNamesMap.TryGetValue(modId, out string modName))
			{
				var mod = Global.Instance.modManager.mods.FirstOrDefault(mod => mod.IsEnabledForActiveDlc() && mod.staticID == modId);
				//var randomizedColor = UIUtils.HSVShift(modLabel, new System.Random(modId.GetHashCode()).Next(100));
				if (mod != null)
				{
					modName = UIUtils.StripAllFormatting(mod.label.title);
				}
				else
					modName = modId;
				modName = UIUtils.ItalicText(modName);

				modIdToNamesMap[modId] = modName;
			}

			return new string('\n', newLineCount) + UIUtils.EmboldenText(string.Format("{0}\n{1}", Text, UIUtils.ColorText(modName, colorOverride.Value)));
		}

		static readonly string Text = new TranslatableTextBuilder("Modded Building, added by:")
				.Add("zh", "模组建筑，添加自：")
				.Add("de", "Mod-Gebäude, hinzugefügt von:")
				.Add("fr", "Bâtiment du mod, ajouté par :")
				.Add("kr", "모드 건물, 추가자:")
				.Add("ru", "Добавлено следующим модом: ")
				.Translate();

	}
}
