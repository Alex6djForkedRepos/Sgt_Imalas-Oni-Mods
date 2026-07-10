
using BlueprintsV2.BlueprintData;
using BlueprintsV2.BlueprintsV2.Visualizers.CustomTileRenderer;
using BlueprintsV2.Tools;
using Database;
using HarmonyLib;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TUNING;
using UnityEngine;
using UtilLibs;
using static BlueprintsV2.BlueprintData.BlueprintState;
using static STRINGS.DUPLICANTS.STATUSITEMS;

namespace BlueprintsV2.Visualizers
{

	public class TileVisual : BuildingVisual, ICleanableVisual
	{
		readonly static Dictionary<ulong, Dictionary<int, BuildingDef>> ActiveTileVisuals = [];
		public static bool HasTileAt(ulong playerId, int cell, out BuildingDef visual)
		{
			visual = null;
			return ActiveTileVisuals.TryGetValue(playerId, out var dict) && dict.TryGetValue(cell, out visual);
		}
		public static void RegisterReplacementVis(int cell, BuildingDef def)
		{
			if (!ActiveTileVisuals.ContainsKey(BlueprintState.PlayerId_ReplacementTiles))
				ActiveTileVisuals[PlayerId_ReplacementTiles] = new();

			ActiveTileVisuals[PlayerId_ReplacementTiles][cell] = def;

			CustomTileRenderer.AddTileBlock(PlayerId_ReplacementTiles, LayerMask.NameToLayer("Overlay"), def, false, SimHashes.Void, cell);
		}
		public static void UnregisterReplacementVis(int cell, BuildingDef def)
		{
			if (!ActiveTileVisuals.ContainsKey(BlueprintState.PlayerId_ReplacementTiles))
				ActiveTileVisuals[PlayerId_ReplacementTiles] = new();

			if (ActiveTileVisuals[PlayerId_ReplacementTiles].TryGetValue(cell, out var exiting))
				ActiveTileVisuals[PlayerId_ReplacementTiles].Remove(cell);

			CustomTileRenderer.RemoveTileBlock(PlayerId_ReplacementTiles, def, false, SimHashes.Void, cell);
		}

		public static void OnPlayerAdded(ulong playerId)
		{
			ActiveTileVisuals[playerId] = [];
		}
		public static void OnPlayerRemoved(ulong playerId)
		{
			ActiveTileVisuals.Remove(playerId);
		}

		public override PermittedRotations GetAllowedRotations() => BlueprintTransformationInfo.All;
		public override void ApplyRotation(Orientation rotation, bool flippedX, bool flippedY)
		{
			///tiles dont rotate
		}

		public int DirtyCell { get; private set; } = -1;

		private readonly bool hasReplacementLayer;

		public TileVisual(BuildingConfig buildingConfig, int cell, ulong playerId) : base(buildingConfig, cell, playerId)
		{
			if (!ActiveTileVisuals.ContainsKey(playerId))
				ActiveTileVisuals[playerId] = [];

			hasReplacementLayer = buildingConfig.BuildingDef.ReplacementLayer != ObjectLayer.NumLayers;
			VisualsUtilities.SetTileColor(_playerId, cell, GetVisualizerColor(cell), buildingConfig);
			this.cell = -1;
			DirtyCell = cell;
			UpdateGrid(cell);
		}

		public override void ForceRedraw()
		{
			VisualsUtilities.SetTileColor(_playerId, cell, GetVisualizerColor(cell), buildingConfig);
		}

		public override void MoveVisualizer(int cellParam, bool forceRedraw)
		{
			if (cellParam != cell || forceRedraw)
			{
				Visualizer.transform.SetPosition(Grid.CellToPosCBC(cellParam, buildingConfig.BuildingDef.SceneLayer));
				UpdateGrid(cellParam);
				VisualsUtilities.SetTileColor(_playerId, cellParam, GetVisualizerColor(cellParam), buildingConfig);
			}
		}

		public void Clean()
		{
			if (!Grid.IsValidBuildingCell(DirtyCell))
			{
				return;
			}

			if (DirtyCell != -1 && Grid.IsValidBuildingCell(DirtyCell))
			{

				if (ActiveTileVisuals[_playerId].TryGetValue(DirtyCell, out var vis) && vis == this.BuildingDef)
				{
					CustomTileRenderer.RemoveTileBlock(_playerId, buildingConfig.BuildingDef, false, SimHashes.Void, DirtyCell);
					ActiveTileVisuals[_playerId].Remove(DirtyCell);
				}
				CustomTileRenderer.RefreshCell(GetPlayerId(), DirtyCell, buildingConfig.BuildingDef.TileLayer, buildingConfig.BuildingDef.ReplacementLayer);
				//if (buildingConfig.BuildingDef.isKAnimTile)
				//{
				//	GameObject tileLayerObject = Grid.Objects[DirtyCell, (int)buildingConfig.BuildingDef.TileLayer];
				//	if (tileLayerObject == null || !tileLayerObject.TryGetComponent<Constructable>(out _))
				//	{
				//		CustomTileRenderer.RemoveTileBlock(GetPlayerId(), buildingConfig.BuildingDef, false, SimHashes.Void, DirtyCell);
				//	}

				//	GameObject replacementLayerObject = hasReplacementLayer ? Grid.Objects[DirtyCell, (int)buildingConfig.BuildingDef.ReplacementLayer] : null;
				//	if (replacementLayerObject == null || replacementLayerObject == Visualizer)
				//	{
				//		CustomTileRenderer.RemoveTileBlock(GetPlayerId(), buildingConfig.BuildingDef, true, SimHashes.Void, DirtyCell);
				//	}
				//}
				//if (Grid.Objects[DirtyCell, (int)buildingConfig.BuildingDef.TileLayer] == Visualizer)
				//{
				//	Grid.Objects[DirtyCell, (int)buildingConfig.BuildingDef.TileLayer] = null;
				//}
				//if (hasReplacementLayer && Grid.Objects[DirtyCell, (int)buildingConfig.BuildingDef.ReplacementLayer] == Visualizer)
				//{
				//	Grid.Objects[DirtyCell, (int)buildingConfig.BuildingDef.ReplacementLayer] = null;
				//}
			}
			DirtyCell = -1;

		}
		private bool CanReplace(int cell)
		{
			CellOffset[] placementOffsets = buildingConfig.BuildingDef.PlacementOffsets;

			for (int index = 0; index < placementOffsets.Length; ++index)
			{
				CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(placementOffsets[index], buildingConfig.Orientation);
				int offsetCell = Grid.OffsetCell(cell, rotatedCellOffset);

				if (!Grid.IsValidBuildingCell(cell) || Grid.Objects[offsetCell, (int)buildingConfig.BuildingDef.ObjectLayer] == null || Grid.Objects[offsetCell, (int)buildingConfig.BuildingDef.ReplacementLayer] != null)
				{
					return false;
				}
			}

			return true;
		}
		private void UpdateGrid(int cell)
		{
			Clean();
			if (Grid.IsValidBuildingCell(cell) && buildingConfig.BuildingDef.isKAnimTile && buildingConfig.BuildingDef.BlockTileAtlas != null)
			{
				if (ActiveTileVisuals[_playerId].ContainsKey(cell))
				{
					SgtLogger.warning(_playerId + ": there is already a tilevisual in " + cell);
				}
				bool replacing = hasReplacementLayer && CanReplace(cell);
				CustomTileRenderer.AddTileBlock(_playerId, LayerMask.NameToLayer("Overlay"), buildingConfig.BuildingDef, false, SimHashes.Void, cell);
				ActiveTileVisuals[_playerId][cell] = this.BuildingDef;
				//if (!BlueprintState.InstantBuild && BlueprintState.ForceBuild && CanRebuildWithMaterial(cell, out _))
				//{
				//	VisualsUtilities.SetVisualizerColor(_playerId, cell, ModAssets.BLUEPRINTS_COLOR_VALIDPLACEMENT, Visualizer, buildingConfig);
				//}
				//else if (!ValidCell(cell))
				//{
				//	VisualsUtilities.SetVisualizerColor(_playerId, cell, ModAssets.BLUEPRINTS_COLOR_INVALIDPLACEMENT, Visualizer, buildingConfig);
				//}
				CustomTileRenderer.RefreshCell(_playerId, cell, buildingConfig.BuildingDef.TileLayer, buildingConfig.BuildingDef.ReplacementLayer);

				DirtyCell = cell;

				//bool visualizerSeated = false;

				//if (Grid.Objects[cell, (int)buildingConfig.BuildingDef.TileLayer] == null)
				//{
				//	Grid.Objects[cell, (int)buildingConfig.BuildingDef.TileLayer] = Visualizer;
				//	visualizerSeated = true;
				//}
				//if (buildingConfig.BuildingDef.isKAnimTile)
				//{
				//	GameObject tileLayerObject = Grid.Objects[DirtyCell, (int)buildingConfig.BuildingDef.TileLayer];
				//	GameObject replacementLayerObject = hasReplacementLayer ? Grid.Objects[DirtyCell, (int)buildingConfig.BuildingDef.ReplacementLayer] : null;

				//	if (tileLayerObject == null || tileLayerObject.GetComponent<Constructable>() == null && replacementLayerObject == null)
				//	{
				//		if (buildingConfig.BuildingDef.BlockTileAtlas != null)
				//		{
				//			if (!BlueprintState.InstantBuild && BlueprintState.ForceBuild && CanRebuildWithMaterial(cell, out _))
				//			{
				//				VisualsUtilities.SetVisualizerColor(cell, ModAssets.BLUEPRINTS_COLOR_VALIDPLACEMENT, Visualizer, buildingConfig);
				//			}

				//			if (!ValidCell(cell))
				//			{
				//				VisualsUtilities.SetVisualizerColor(cell, ModAssets.BLUEPRINTS_COLOR_INVALIDPLACEMENT, Visualizer, buildingConfig);
				//			}

				//			if (Grid.Objects[DirtyCell, (int)buildingConfig.BuildingDef.ReplacementLayer] == null)
				//			{
				//				CustomTileRenderer.AddTileBlock(GetPlayerId(), LayerMask.NameToLayer("Overlay"), buildingConfig.BuildingDef, replacing, SimHashes.Void, cell);
				//				if (replacing && !visualizerSeated && Grid.Objects[DirtyCell, (int)buildingConfig.BuildingDef.ReplacementLayer] == null)
				//				{
				//					Grid.Objects[cell, (int)buildingConfig.BuildingDef.ReplacementLayer] = Visualizer;
				//				}
				//			}

				//			TileVisualizer.RefreshCell(cell, buildingConfig.BuildingDef.TileLayer, buildingConfig.BuildingDef.ReplacementLayer);

				//		}
				//	}
				//}

				//DirtyCell = cell;
			}
		}
	}

}
