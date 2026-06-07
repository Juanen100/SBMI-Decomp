using System;
using DeltaDNA.Android;
using UnityEngine;

namespace DeltaDNA
{
	public class AndroidNotifications : MonoBehaviour
	{
		private DDNANotifications ddnaNotifications;

		private bool? notificationsPresent;

		public event Action<string> OnDidLaunchWithPushNotification
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<string> OnDidReceivePushNotification
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<string> OnDidRegisterForPushNotifications
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<string> OnDidFailToRegisterForPushNotifications
		{
			add
			{
			}
			remove
			{
			}
		}

		private void Awake()
		{
		}

		public void RegisterForPushNotifications(bool secondary = false)
		{
		}

		public void UnregisterForPushNotifications()
		{
		}

		private bool AreNotificationsPresent()
		{
			return false;
		}

		public void DidReceivePushNotification(string notification)
		{
		}

		public void DidRegisterForPushNotifications(string registrationId)
		{
		}

		public void DidFailToRegisterForPushNotifications(string error)
		{
		}
	}
}
