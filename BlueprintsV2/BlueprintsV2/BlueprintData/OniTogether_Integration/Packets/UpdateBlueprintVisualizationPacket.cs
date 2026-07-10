using BlueprintsV2.BlueprintData;
using ONI_Together.Networking.Packets.Architecture;
using ONI_Together_API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueprintsV2.BlueprintsV2.BlueprintData.OniTogether_Integration.Packets
{
	internal class UpdateBlueprintVisualizationPacket : IPacket
	{
		public UpdateBlueprintVisualizationPacket()
		{
			SenderId = SessionInfoAPI.LocalUserID;
		}
		ulong SenderId;


		public void Deserialize(BinaryReader reader)
		{
			SenderId = reader.ReadUInt64();
		}
		public void Serialize(BinaryWriter writer)
		{
			writer.Write(SenderId);
		}
		public void OnDispatched()
		{
			BlueprintState.MoveDisplayBlueprintForPlayer(SenderId);
		}
	}
}
