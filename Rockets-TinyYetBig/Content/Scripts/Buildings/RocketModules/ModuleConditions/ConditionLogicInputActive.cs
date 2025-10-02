﻿using KSerialization;
using Rockets_TinyYetBig.Content.Scripts.Buildings.RocketModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Rockets_TinyYetBig.Content.Scripts
{
	internal class ConditionLogicInputActive : ProcessCondition
	{
		RocketLogicLaunchCondition handler;
		public ConditionLogicInputActive(RocketLogicLaunchCondition _handler)
		{
			handler = _handler;
		}
		public override Status EvaluateCondition()
		{
			if (handler.logicPorts.GetInputValue(LogicOperationalController.PORT_ID) == 1)
			{
				return Status.Ready;
			}
			return Status.Failure;
		}
		public override bool ShowInUI() => handler.worldModule != null;
		public override string GetStatusMessage(ProcessCondition.Status status) => handler.nameable.savedName;

		public override string GetStatusTooltip(ProcessCondition.Status status) =>
			(status == ProcessCondition.Status.Ready)
			? string.Format(STRINGS.BUILDINGS.PREFABS.RTB_ROCKETLOGICLAUNCHCONDITIONSETTER.STATUSITEM_TOOLTIP_ACTIVE, handler.nameable.savedName)
			: string.Format(STRINGS.BUILDINGS.PREFABS.RTB_ROCKETLOGICLAUNCHCONDITIONSETTER.STATUSITEM_TOOLTIP_INACTIVE, handler.nameable.savedName);

	}
}
