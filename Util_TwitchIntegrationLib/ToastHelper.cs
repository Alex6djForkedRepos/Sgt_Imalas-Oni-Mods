using JetBrains.Annotations;
using ONITwitchLib;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchColony.Api;
using UnityEngine;

namespace Util_TwitchIntegrationLib
{
	public static class ToastHelper
	{
		public static void InstantiateToast(string title, string body)
		{
			if (TwitchModInfo.TwitchIsPresent)
			{
				ToastManager.InstantiateToast(title, body);
			}
			else if (TwitchColonyApi.IsAvailable)
			{
				TwitchColonyApi.ShowBanner($"<size=150%>{title}</size>\n\n" + body);
			}
		}
		public static void InstantiateToastWithPosTarget(string title, string body, Vector3 pos, float orthographicSize = -1)
		{
			if (TwitchModInfo.TwitchIsPresent)
			{
				if(orthographicSize > 0)
					ToastManager.InstantiateToastWithPosTarget(title, body, pos, orthographicSize);
				else
					ToastManager.InstantiateToastWithPosTarget(title, body, pos);

			}
			else if (TwitchColonyApi.IsAvailable)
			{
				TwitchColonyApi.ShowBanner($"<size=150%>{title}</size>\n\n" + body);
			}
		}
		public static void InstantiateToastWithGoTarget(string title, string body, GameObject target)
		{
			if (TwitchModInfo.TwitchIsPresent)
			{
				ToastManager.InstantiateToastWithGoTarget(title, body, target);
			}
			else if (TwitchColonyApi.IsAvailable)
			{
				TwitchColonyApi.ShowBanner($"<size=150%>{title}</size>\n\n" + body);
			}
		}
	}
}
