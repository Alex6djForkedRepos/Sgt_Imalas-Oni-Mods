using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UtilLibs.UIcmp;

namespace UtilLibs.UI.FUI
{
	public class FItemPickerArray : KMonoBehaviour
	{
		class FItemPickerEntry : KMonoBehaviour
		{
			Image SymbolDisplay;
			FToggleButton button;
			string TargetSelection;
			Action<string> onIconSelect = null;

			Sprite toDisplay = null;

			public override void OnPrefabInit()
			{
				base.OnPrefabInit();
				SymbolDisplay = transform.Find("Image").gameObject.GetComponent<Image>();
				button = gameObject.AddOrGet<FToggleButton>();
				button.OnClick += ColorEntryClicked;
				if (toDisplay != null)
					SymbolDisplay.sprite = toDisplay;
				Refresh(string.Empty);
			}
			public void Init(string id, Sprite icon, Action<string> onColorSelect)
			{
				TargetSelection = id;
				onIconSelect = onColorSelect;
				SymbolDisplay?.sprite = icon;
				toDisplay = icon;
			}

			public void Refresh(string selected)
			{
				if (button == null)
					return;

				button.SetIsSelected(selected == TargetSelection);
			}
			void ColorEntryClicked()
			{
				onIconSelect?.Invoke(TargetSelection);
			}
		}

		FItemPickerEntry Prefab;
		Dictionary<string, FItemPickerEntry> PalletteEntries = [];
		public string SelectedEntry = string.Empty;
		public event Action<string> OnSelectionChanged;
		Dictionary<string, Sprite> _values = [];

		public void Init(Dictionary<string, Sprite> vals)
		{
			_values = vals;
			InitPallette();
		}

		public void SetSelected(string selected)
		{
			OnSelectItem(selected);
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			InitPallette();
		}
		public override void OnSpawn()
		{
			base.OnSpawn();
			RefreshPallette();
		}

		void InitPallette()
		{
			Prefab = transform.Find("Item").gameObject.AddComponent<FItemPickerEntry>();
			Prefab.gameObject.SetActive(false);

			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				var child = transform.GetChild(i);
				if (child.gameObject != Prefab.gameObject)
					Destroy(child.gameObject);
			}

			foreach (var vals in _values)
			{
				var entry = Util.KInstantiateUI<FItemPickerEntry>(Prefab.gameObject, gameObject);
				entry.Init(vals.Key, vals.Value, OnSelectItem);
				entry.gameObject.SetActive(true);
				PalletteEntries[vals.Key] = entry;
			}
			if (_values != null && _values.Keys.Any())
				SelectedEntry = _values.Keys.First();
		}
		void RefreshPallette()
		{
			foreach (var entry in PalletteEntries)
				entry.Value.Refresh(SelectedEntry);
		}
		void OnSelectItem(string item)
		{
			if (item != SelectedEntry)
			{
				SelectedEntry = item;
				OnSelectionChanged?.Invoke(SelectedEntry);
				RefreshPallette();
			}
		}
	}
}
