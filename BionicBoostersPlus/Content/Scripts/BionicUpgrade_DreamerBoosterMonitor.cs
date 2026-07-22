using BionicBoostersPlus.Content.ModDb;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace BionicBoostersPlus.Content.Scripts
{
	/// <summary>
	/// Upgrade smi that gets activated by installing the booster in the bionic
	/// Links up to the booster storage on start
	/// </summary>
	internal class BionicUpgrade_DreamerBoosterMonitor : BionicUpgrade_SM<BionicUpgrade_DreamerBoosterMonitor, BionicUpgrade_DreamerBoosterMonitor.Instance>
	{
		public const float SecondsToMakeDreamjournal = 600;

		public static void FindAndAttachToInstalledBooster(
		  BionicUpgrade_DreamerBoosterMonitor.Instance smi)
		{
			smi.Initialize();
		}

		public static void DataGatheringUpdate(
		  BionicUpgrade_DreamerBoosterMonitor.Instance smi,
		  float dt)
		{
			smi.GatheringDataUpdate(dt);
		}

		public new class Instance : BaseInstance
		{
			private static GameObject dreamJournalPrefab;
			Dreamer.Instance dreamer;
			private BionicUpgrade_DreamerBooster.Instance dreamingBooster;
			public bool CanSpawnDreamJournal => this.dreamingBooster != null && this.dreamingBooster.IsReady;
			public bool IsDreaming() => IsInsideState(base.sm.dreaming);

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				dreamer = master.gameObject.GetSMI<Dreamer.Instance>();
				if (dreamJournalPrefab == null)
					dreamJournalPrefab = Assets.GetPrefab(DreamJournalConfig.ID);
			}
			public void StartDreaming()
			{
				dreamer.SetDream(Db.Get().Dreams.CommonDream);
				dreamer.StartDreaming();
			}
			public void StopDreaming()
			{
				dreamer.StopDreaming();
			}
			public void Initialize()
			{
				foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in this.gameObject.GetSMI<BionicUpgradesMonitor.Instance>().upgradeComponentSlots)
				{
					if (upgradeComponentSlot.HasUpgradeInstalled)
					{
						BionicUpgrade_DreamerBooster.Instance smi = upgradeComponentSlot.installedUpgradeComponent.GetSMI<BionicUpgrade_DreamerBooster.Instance>();
						if (smi != null && !smi.IsBeingMonitored)
						{
							this.dreamingBooster = smi;
							smi.SetMonitor(this);
							break;
						}
					}
				}
			}
			private void SpawnDreamJournal()
			{
				Vector3 position = this.dreamer.transform.position;
				++position.y;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				Util.KInstantiate(dreamJournalPrefab, position, Quaternion.identity).SetActive(true);
			}

			public void GatheringDataUpdate(float dt)
			{
				dreamingBooster.AddData(dt <= 0 ? 0 : dt / SecondsToMakeDreamjournal);
				if (!this.CanSpawnDreamJournal)
					return;
				SpawnDreamJournal();
				dreamingBooster?.SetDataProgress(0);
			}

			public override float GetCurrentWattageCost()
			{
				if (IsDreaming())
				{
					return base.Data.WattageCost;
				}
				return 0f;
			}

			public override string GetCurrentWattageCostName()
			{
				float currentWattageCost = GetCurrentWattageCost();
				if (IsDreaming())
				{
					string text = "<b>" + ((currentWattageCost >= 0f) ? "+" : "-") + "</b>";
					return string.Format(global::STRINGS.DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_ACTIVE_TEMPLATE, upgradeComponent.GetProperName(), text + GameUtil.GetFormattedWattage(currentWattageCost));
				}
				return string.Format(global::STRINGS.DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_INACTIVE_TEMPLATE, upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(upgradeComponent.PotentialWattage));
			}
		}

		public const float DataGatheringDuration = 600f;

		public State noBooster;
		public State idle;
		public State dreaming;

		public override void InitializeStates(out BaseState default_state)
		{
			base.serializable = SerializeType.ParamsOnly;
			default_state = noBooster;
			noBooster.Enter(FindAndAttachToInstalledBooster)
				.GoTo(idle);

			idle.TagTransition(GameTags.BionicBedTime, dreaming)
				.ToggleStatusItem(BB_StatusItems.DreamBooster_Idle)
				.Enter(smi => smi.StopDreaming());
			dreaming
				.TagTransition(GameTags.BionicBedTime, idle, true)
				.ToggleStatusItem(BB_StatusItems.DreamBooster_Dreaming)
				.Enter(smi => smi.StartDreaming());
		}
	}
}
