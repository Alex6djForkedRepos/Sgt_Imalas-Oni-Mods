using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STRINGS.UI;

namespace MugConversionRecipes
{
	internal class STRINGS
	{
		public class BUILDINGS
		{
			public class PREFABS
			{
				public class MCR_RECOMBINATOR
				{
					public static LocString NAME = UI.FormatAsLink("Artifact Recombinator",nameof(MCR_RECOMBINATOR));
					public static LocString DESC = "Any technology advanced enough is indistinguishable from magic.";
					public static LocString EFFECT = "Dismantels and recombinates artifacts at a molecular level, allowing to turn them into different artifacts.";

					public static LocString RECIPE_DESC = "Recombines an artifact, creating a {0}.";

				}
			}
		}
	}
}
