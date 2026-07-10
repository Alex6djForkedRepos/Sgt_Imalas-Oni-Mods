using System;
using System.Collections.Generic;
using System.Text;
using ONI_Together_API;

namespace BlueprintsV2.BlueprintsV2.BlueprintData.OniTogether_Integration
{
	internal class MP_Helpers
	{
		public static bool MPInstalledAndActive()
		{
			return MP_Mod_Info.MultiplayerModPresent && SessionInfoAPI.InSession;
		}
	}
}
