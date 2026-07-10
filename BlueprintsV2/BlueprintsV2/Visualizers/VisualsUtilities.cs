
using BlueprintsV2.BlueprintData;
using BlueprintsV2.BlueprintsV2.Visualizers.CustomTileRenderer;
using UnityEngine;

namespace BlueprintsV2.Visualizers
{
	public static class VisualsUtilities
	{
		public static void SetTileColor(ulong playerId, int cell, Color color, BuildingConfig buildingConfig)
		{
			BlueprintState.ColoredCells[playerId][cell] = new CellColorPayload(color, buildingConfig.BuildingDef.TileLayer, buildingConfig.BuildingDef.ReplacementLayer);
			CustomTileRenderer.RefreshCell(playerId, cell, buildingConfig.BuildingDef.TileLayer, buildingConfig.BuildingDef.ReplacementLayer);
		}
	}
}
