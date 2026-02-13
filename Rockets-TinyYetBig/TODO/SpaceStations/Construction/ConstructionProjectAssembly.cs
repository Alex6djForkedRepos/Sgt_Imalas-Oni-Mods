using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rockets_TinyYetBig.SpaceStations.Construction
{
	public struct ConstructionProjectAssembly
	{
		public ConstructionProjectAssembly(List<PartProject> parts)
		{
			Parts = [.. parts];
		}

		public string ProjectName = "Space Project";
		public string ProjectDescription = "Space Project Desc.";


		public string RequiredScienceUnlockId = string.Empty;


		public Tag RequiredPrebuilt = null;
		public List<PartProject> Parts = new List<PartProject>();

		public Sprite PreviewSprite = null;
		public Action<SpaceConstructable> OnConstructionFinishedAction = null;
	}
}
