using BlueprintsV2.BlueprintData;
using Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UtilLibs;

namespace BlueprintsV2.BlueprintsV2.UnityUI.Components.PreviewVisualizers
{
	internal class Vis_SpritePreview : KMonoBehaviour
	{
		Image icon;
		internal Vis_SpritePreview Init()
		{
			icon = transform.Find("TileMask/TileVis").gameObject.GetComponent<Image>();
			var rect = icon.rectTransform();

			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);

			//_mask = transform.Find("TileMask").GetComponent<RectMask2D>();
			icon.gameObject.SetActive(true);
			return this;
		}
		public void SetDisplayed(Tuple<Sprite,Color> tuple)
		{
			icon.sprite = tuple.first;
			var color = tuple.second;
			color.a = 1;
			icon.color = color;
		}
	}
}
