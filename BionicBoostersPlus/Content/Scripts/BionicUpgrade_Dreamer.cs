using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using monitor = BionicUpgrade_ExplorerBoosterMonitor;

namespace BionicBoostersPlus.Content.Scripts
{
	internal class BionicUpgrade_Dreamer : GameStateMachine<BionicUpgrade_Dreamer, BionicUpgrade_Dreamer.Instance, IStateMachineTarget, BionicUpgrade_Dreamer.Def>
	{
		public class Def : BaseDef
		{
		}

		[HarmonyPatch(typeof(monitor), nameof(monitor.InitializeStates))]
		private static class BionicUpgrade_ExplorerBoosterMonitor_InitializeStates
		{
			private static void Postfix(monitor __instance)
			{

				__instance.Inactive
					.EventHandlerTransition(GameHashes.TagsChanged, __instance.Active, (smi, data) =>
					{
						var @event = ((Boxed<TagChangedEventData>)data).value;
						return @event.tag == GameTags.BionicBedTime && @event.added == true
							&& monitor.ShouldBeActive(smi);
					})
					.DefaultState(Standby);

				Standby
					.EnterTransition(Finished, monitor.Not(monitor.IsThereGeysersToDiscover))
					.EventHandlerTransition(GeyserRevealed, smi => Game.Instance, Finished, IsThereNoMoreGeysersToDiscoverInMyWorld)
					.EventTransition(GameHashes.MinionMigration, smi => Game.Instance, Finished,
						monitor.And(ShouldBeInActive, monitor.Not(monitor.IsThereGeysersToDiscover)));

				Finished
					.ToggleStatusItem(
						name: STRINGS.DUPLICANTS.STATUSITEMS.BIONICEXPLORERBOOSTER_FINISHED.NAME,
						tooltip: STRINGS.DUPLICANTS.STATUSITEMS.BIONICEXPLORERBOOSTER_FINISHED.TOOLTIP,
						icon_type: StatusItem.IconType.Exclamation,
						notification_type: NotificationType.Bad)
					.EventTransition(GameHashes.MinionMigration, smi => Game.Instance, Standby,
						monitor.And(ShouldBeInActive, monitor.IsThereGeysersToDiscover));

				__instance.Active
					.TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null)
					.EventHandlerTransition(GameHashes.TagsChanged, __instance.Inactive, (smi, data) =>
					{
						var @event = ((Boxed<TagChangedEventData>)data).value;
						return @event.tag == GameTags.BionicBedTime && @event.added == false
							&& !monitor.IsInBedTimeChore(smi);
					});

				// если один бионик нашел гейзер, заставим остальных биоников перепроверить остались ли ещё гейзеры
				__instance.Active.gatheringData
					.EventHandlerTransition(GeyserRevealed, smi => Game.Instance, Finished, IsThereNoMoreGeysersToDiscoverInMyWorld);

				__instance.Active.discover
					.Enter(smi => GameScheduler.Instance.Schedule("", 1f, ScheduleGeyserDiscoveredCB, smi));
			}

			private static void ScheduleGeyserDiscoveredCB(object data)
			{
				var smi = data as monitor.Instance;
				if (!smi.IsNullOrStopped())
					Game.Instance.BoxingTrigger((int)GeyserRevealed, smi.GetMyParentWorldId());
			}

			private static bool ShouldBeInActive(monitor.Instance smi) => !(monitor.IsOnline(smi) && monitor.IsInBedTimeChore(smi));

			private static bool IsThereNoMoreGeysersToDiscoverInMyWorld(monitor.Instance smi, object data)
			{
				int worldId = ((Boxed<int>)data).value;
				return (smi.GetMyParentWorldId() == worldId) && !monitor.IsThereGeysersToDiscover(smi);
			}
		}


		public new class Instance : GameInstance
		{
			public bool IsBeingMonitored => monitor != null;

			public bool IsReady => Progress == 1f;

			public float Progress => base.sm.Progress.Get(this);

			public Instance(IStateMachineTarget master, Def def)
				: base(master, def)
			{
			}

			public void SetMonitor(BionicUpgrade_DreamerMonitor.Instance monitor)
			{
				this.monitor = monitor;
			}

			public void AddData(float dataProgressDelta)
			{
				float dataProgress = Mathf.Clamp(Progress + dataProgressDelta, 0f, 1f);
				SetDataProgress(dataProgress);
			}

			public void SetDataProgress(float dataProgress)
			{
				Mathf.Clamp(dataProgress, 0f, 1f);
				base.sm.Progress.Set(dataProgress, this);
			}
		}

		public const float DataGatheringDuration = 600f;

		public FloatParameter Progress;

		public State not_ready;

		public State ready;

		public override void InitializeStates(out BaseState default_state)
		{
			base.serializable = SerializeType.ParamsOnly;
			default_state = not_ready;
			not_ready.ParamTransition(Progress, ready, GameStateMachine<BionicUpgrade_Dreamer, Instance, IStateMachineTarget, Def>.IsGTEOne).ToggleStatusItem(Db.Get().MiscStatusItems.BionicExplorerBooster);
			ready.ParamTransition(Progress, not_ready, GameStateMachine<BionicUpgrade_Dreamer, Instance, IStateMachineTarget, Def>.IsLTOne).ToggleStatusItem(Db.Get().MiscStatusItems.BionicExplorerBoosterReady);
		}
	}
}
