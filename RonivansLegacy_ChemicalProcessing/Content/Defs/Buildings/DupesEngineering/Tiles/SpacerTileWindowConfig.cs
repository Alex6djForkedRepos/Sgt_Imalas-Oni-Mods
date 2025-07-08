﻿using RonivansLegacy_ChemicalProcessing.Content.ModDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;
using UtilLibs;

namespace RonivansLegacy_ChemicalProcessing.Content.Defs.Buildings.DupesEngineering.Tiles
{
    class SpacerTileWindowConfig : IBuildingConfig
	{
		public static string ID = "SpacerTileWindow";

		public override BuildingDef CreateBuildingDef()
		{
			string kanim = "floor_spacer_glass_kanim";
			float[] mass = [95, 5];
			string[] cost = [GameTags.Transparent.ToString(),GameTags.RefinedMetal.ToString()];

			BuildingDef def = BuildingTemplates.CreateBuildingDef(ID, 1, 1, kanim, 100, 5f, mass, cost, 1600f, BuildLocationRule.Tile, BUILDINGS.DECOR.BONUS.TIER1, NOISE_POLLUTION.NONE);
			BuildingTemplates.CreateFoundationTileDef(def);
			def.Floodable = false;
			def.Overheatable = false;
			def.Entombable = false;
			def.UseStructureTemperature = false;
			def.AudioCategory = "Glass";
			def.AudioSize = "small";
			def.BaseTimeUntilRepair = -1f;
			def.SceneLayer = Grid.SceneLayer.GlassTile;
			def.isKAnimTile = true;
			def.BlockTileIsTransparent = true;

			def.BlockTileAtlas = Assets.GetTextureAtlas("tiles_glass");
			def.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_glass_place");
			def.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
			def.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_glass_tops_decor_info");
			def.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_glass_tops_decor_place_info");

			def.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
			def.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_bunker_tops_decor_info");
			def.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_bunker_tops_decor_place_info");
			def.DragBuild = true;

			AssetUtils.AddCustomTileAtlas(def, "tiles_spacer_glass", false, "tiles_solid");
			AssetUtils.AddCustomTileTops(def, "tiles_spacer_glass_tops", false, "tiles_bunker_tops_decor_info");

			def.AddSearchTerms((string)global::STRINGS.SEARCH_TERMS.TILE);
			def.AddSearchTerms((string)global::STRINGS.SEARCH_TERMS.GLASS);

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), tag);

			var simCellOccupier = go.AddOrGet<SimCellOccupier>();
			simCellOccupier.notifyOnMelt = true;
			simCellOccupier.setTransparent = true;
			simCellOccupier.doReplaceElement = true;
			simCellOccupier.movementSpeedMultiplier = 1.25f; //== DUPLICANTSTATS.MOVEMENT_MODIFIERS.BONUS_2;

			go.AddOrGet<TileTemperature>();
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID =Hash.SDBMLower("tiles_spacer_glass_tops"); 
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
			go.AddOrGet<TileTemperature>();

			go.GetComponent<KPrefabID>().AddTag(GameTags.Window);
		}
		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			var prefab = go.GetComponent<KPrefabID>();
			prefab.AddTag(GameTags.FloorTiles, false);
		}
		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.AddOrGet<KAnimGridTileVisualizer>();
		}
	}
}
