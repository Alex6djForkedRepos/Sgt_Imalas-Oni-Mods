using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rockets_TinyYetBig.Content.Scripts.Buildings
{
	internal class SkipOperationalEnergyConsumer : EnergyConsumer
	{
		//[Serialize] //regular flag is also not serialized, probably to skip first frame
		private bool _isPowered;
		public override bool IsPowered { get => _isPowered; set => _isPowered = value; }
	}
}
