using System;
using UnityEngine;

namespace DeltaDNA
{
	public class IosNotifications : MonoBehaviour
	{
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

		public void RegisterForPushNotifications()
		{
		}

		public void UnregisterForPushNotifications()
		{
		}

		public void DidReceivePushNotification(string notification)
		{
		}

		public void DidRegisterForPushNotifications(string deviceToken)
		{
		}

		public void DidFailToRegisterForPushNotifications(string error)
		{
		}
	}
}
