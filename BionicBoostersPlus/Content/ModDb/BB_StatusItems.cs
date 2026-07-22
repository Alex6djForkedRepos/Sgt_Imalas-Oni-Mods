using System;
using System.Collections.Generic;
using System.Text;

namespace BionicBoostersPlus.Content.ModDb
{
	internal class BB_StatusItems
	{
		public static StatusItem DreamBooster_Idle;
		public static StatusItem DreamBooster_Dreaming;

		public static StatusItem DreamBoosterJournalStorage;
		public static StatusItem DreamBoosterJournalStorageFull;

		public static void InitStatusitems(Db db)
		{
			var dsi = Db.Get().DuplicantStatusItems;
			var msi = Db.Get().MiscStatusItems;

			DreamBooster_Idle = dsi.CreateStatusItem("DreamBooster_Idle", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
			DreamBooster_Dreaming = dsi.CreateStatusItem("DreamBooster_Dreaming", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);

			DreamBoosterJournalStorage = msi.CreateStatusItem("DreamBoosterJournalStorage", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
			DreamBoosterJournalStorageFull = msi.CreateStatusItem("DreamBoosterJournalStorageFull", "MISC", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID);
			
		}
	}
}
