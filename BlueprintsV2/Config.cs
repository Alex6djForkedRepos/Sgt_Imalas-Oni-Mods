﻿using PeterHan.PLib.Options;
using System;

namespace BlueprintsV2
{
	public enum DefaultSelections
	{
		[Option("STRINGS.BLUEPRINTS_CONFIG.DEFAULTMENUSELECTION.DEFAULTMENUSELECTION_ALL")]
		All,
		[Option("STRINGS.BLUEPRINTS_CONFIG.DEFAULTMENUSELECTION.DEFAULTMENUSELECTION_NONE")]
		None
	}

	[Serializable]
	[RestartRequired]
	[ConfigFile(SharedConfigLocation: true)]
	public class Config : SingletonOptions<Config>
	{
		[Option("STRINGS.BLUEPRINTS_CONFIG.DEFAULTMENUSELECTION.TITLE", "STRINGS.BLUEPRINTS_CONFIG.DEFAULTMENUSELECTION.TOOLTIP")]
		public DefaultSelections DefaultMenuSelections { get; set; } = DefaultSelections.All;

		[Option("STRINGS.BLUEPRINTS_CONFIG.REQUIRECONSTRUCTABLE.TITLE", "STRINGS.BLUEPRINTS_CONFIG.REQUIRECONSTRUCTABLE.TOOLTIP")]
		public bool RequireConstructable { get; set; } = true;

		[Option("STRINGS.BLUEPRINTS_CONFIG.FXTIME.TITLE", "STRINGS.BLUEPRINTS_CONFIG.FXTIME.TOOLTIP")]
		public float FXTime { get; set; } = 4;

		[Option("STRINGS.BLUEPRINTS_CONFIG.CREATEBLUEPRINTTOOLSYNC.TITLE", "STRINGS.BLUEPRINTS_CONFIG.CREATEBLUEPRINTTOOLSYNC.TOOLTIP")]
		public bool CreateBlueprintToolSync { get; set; } = true;

		[Option("STRINGS.BLUEPRINTS_CONFIG.SNAPSHOTTOOLSYNC.TITLE", "STRINGS.BLUEPRINTS_CONFIG.SNAPSHOTTOOLSYNC.TOOLTIP")]
		public bool SnapshotToolSync { get; set; } = true;
	}
}
