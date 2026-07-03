//using HarmonyLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Emit;
//using System.Text;
//using UnityEngine;
//using UtilLibs;

//namespace ModOriginInfo.Patches
//{
//	internal class EntityConfigManager_Patches
//	{

//        [HarmonyPatch(typeof(EntityConfigManager), nameof(EntityConfigManager.RegisterEntities))]
//        public class EntityConfigManager_RegisterEntities_Patch
//        {
//			static IMultiEntityConfig cachedConfigs;

//			public static void Prefix(IMultiEntityConfig config)
//			{
//				SgtLogger.l("Register Entities: " + config.GetType().Name);
//				cachedConfigs = config;
//			}

//			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
//			{
//				var m_CreatePrefabs = AccessTools.Method(typeof(IMultiEntityConfig), nameof(IMultiEntityConfig.CreatePrefabs));

//				foreach (var ci in orig)
//				{
//					yield return ci;
//					if (ci.Calls(m_CreatePrefabs))
//					{
//						yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(EntityConfigManager_RegisterEntities_Patch), nameof(FetchEntityList)));
//					}
//				}
//			}

//			private static List<GameObject> FetchEntityList(List<GameObject> entities)
//			{
//				foreach(var e in entities) 
//					ModAssets.RegisterMultiEntity(cachedConfigs, e);
//				return entities;
//			}
//		}
//		[HarmonyPatch(typeof(EntityConfigManager), nameof(EntityConfigManager.RegisterEntity))]
//		public class EntityConfigManager_RegisterEntity_Patch
//		{
//			static IEntityConfig cachedConfig;

//			public static void Prefix(IEntityConfig config)
//			{
//				SgtLogger.l("Register Entity: "+config.GetType().Name);	
//				cachedConfig = config;
//			}

//			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
//			{
//				var m_CreatePrefabs = AccessTools.Method(typeof(IEntityConfig), nameof(IEntityConfig.CreatePrefab));

//				foreach (var ci in orig)
//				{
//					yield return ci;
//					if (ci.Calls(m_CreatePrefabs))
//					{
//						yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(EntityConfigManager_RegisterEntities_Patch), nameof(FetchEntityList)));
//					}
//				}
//			}

//			private static GameObject FetchEntityList(GameObject entity)
//			{
//				ModAssets.RegisterEntity(cachedConfig, entity);
//				return entity;
//			}
//		}
//	}
//}
