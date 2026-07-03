using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _SgtsModUpdater.Model.ModsJsonData
{
	internal class ModsJson
	{
		public int version;
		public List<KleiMod> mods;
		public bool mod_load_in_progress;
	}
}
