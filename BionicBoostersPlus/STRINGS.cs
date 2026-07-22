using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STRINGS.UI;

namespace BionicBoostersPlus
{
	internal class STRINGS
	{
		public class MISC
		{
			public class STATUSITEMS
			{
				public class DREAMBOOSTERJOURNALSTORAGEFULL
				{
					public static LocString NAME = (LocString)"Dowsing Complete";
					public static LocString TOOLTIP = (LocString)$"This {PRE_KEYWORD}Dreaming Booster{PST_KEYWORD} has sufficient dream data stored to create a new dream journal.\n\nIt must be installed in a Bionic Duplicant in order to function";
				}

				public class DREAMBOOSTERJOURNALSTORAGE
				{
					public static LocString NAME = (LocString)"Stored Dreamdata: {0}";
					public static LocString TOOLTIP = (LocString)$"{PRE_KEYWORD}Dreaming Boosters{PST_KEYWORD} retain dream data gathered by Bionic Duplicants\n\nWhen sufficient dream data is collected and this booster is installed in a Bionic Duplicant, a new dream journal will be generated";
				}
			}
		}
		public class DUPLICANTS
		{
			public class STATUSITEMS
			{

				public class DREAMBOOSTER_IDLE
				{
					public static LocString NAME = FormatAsLink("Dreaming Booster", "BB_BOOSTER_DREAM") + " equipped";

					public static LocString TOOLTIP = "This Duplicant can now produce " + FormatAsLink("Dream Journals", "DREAMJOURNAL") + " when defragmenting";
				}
				public class DREAMBOOSTER_DREAMING
				{
					public static LocString NAME = "Dreaming (of electric sheep)";

					public static LocString TOOLTIP = "This Duplicant is adventuring through their own subconscious\n\nBionic Dreams are caused by equpping a " + FormatAsLink("Dreaming Booster", "BB_BOOSTER_DREAM") + "\n\n" + FormatAsLink("Dream Journal", "DREAMJOURNAL") + " will be ready in {time}";
				}
			}
		}
		public class ITEMS
		{
			public class BIONIC_BOOSTERS
			{
				public class BB_OVERCLOCKED
				{
					public static LocString OC_PREFIX = "Fused {0}";
					public static LocString OC_SUFFIX ="This booster has been fabricated by combining multiple boosters and is considered unstable.";
				}
				public class BB_BOOSTER_DREAM
				{
					public static LocString NAME = FormatAsLink("Dreaming Booster", "BB_BOOSTER_DREAM");
					public static LocString DESC = "Grants a Bionic Duplicant the ability to dream of electric sheep.";
					public static LocString EFFECT = "Dreaming while Defragmenting";
				}
			}
		}
		public class BUILDINGS
		{
			public class PREFABS
			{
				public class BBP_BOOSTERRECYCLER
				{
					public static LocString NAME = FormatAsLink("Booster 'Recycling' Station", "BBP_BOOSTERRECYCLER");
					public static LocString DESC = "Stepping up the booster 'Refinement'";
					public static LocString EFFECT = "Crushes down bionic boosters back into microchips.\n\nMicrochip recovery is not fully guaranteed!";
					public static LocString RECIPE_FORMAT = "Crush down {0} to recover some of its Microchips.\nMicrochip recovery rate will fluctuate.";
				}
				public class BBP_MK3BOOSTERMAKER
				{
					public static LocString NAME = FormatAsLink("Experimental Laser Soldering Station", "BBP_MK3BOOSTERMAKER");
					public static LocString DESC = "Lasers make everything more hightech right?";
					public static LocString EFFECT = "Allows the fabrication of experimental booster modules that can bring your bionics to new levels of performance, at a cost.";
				}
			}
		}
		public class UI
		{
			public class ROLES_SCREEN
			{
				public class PERKS
				{
					public class BB_BIONICDREAM
					{
						public static LocString DESCRIPTION = $"Ability to dream while defragmenting.";
					}
				}
			}
		}
	}
}
