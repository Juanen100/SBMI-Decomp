using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace com.amazon.device.iap.cpt
{
	public abstract class AmazonIapV2Impl : MonoBehaviour, IAmazonIapV2
	{
		private abstract class AmazonIapV2Base : AmazonIapV2Impl
		{
			private static readonly object startLock;

			private static bool startCalled;

			public AmazonIapV2Base()
			{
			}

			protected void Start()
			{
			}

			protected abstract void Init();

			protected abstract void RegisterCallback();

			protected abstract void RegisterEventListener();

			protected abstract void RegisterCrossPlatformTool();

			public override void UnityFireEvent(string jsonMessage)
			{
			}

			public override RequestOutput GetUserData()
			{
				return null;
			}

			private string GetUserDataJson(string jsonMessage)
			{
				return null;
			}

			protected abstract string NativeGetUserDataJson(string jsonMessage);

			public override RequestOutput Purchase(SkuInput skuInput)
			{
				return null;
			}

			private string PurchaseJson(string jsonMessage)
			{
				return null;
			}

			protected abstract string NativePurchaseJson(string jsonMessage);

			public override RequestOutput GetProductData(SkusInput skusInput)
			{
				return null;
			}

			private string GetProductDataJson(string jsonMessage)
			{
				return null;
			}

			protected abstract string NativeGetProductDataJson(string jsonMessage);

			public override RequestOutput GetPurchaseUpdates(ResetInput resetInput)
			{
				return null;
			}

			private string GetPurchaseUpdatesJson(string jsonMessage)
			{
				return null;
			}

			protected abstract string NativeGetPurchaseUpdatesJson(string jsonMessage);

			public override void NotifyFulfillment(NotifyFulfillmentInput notifyFulfillmentInput)
			{
			}

			private string NotifyFulfillmentJson(string jsonMessage)
			{
				return null;
			}

			protected abstract string NativeNotifyFulfillmentJson(string jsonMessage);

			public override void AddGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate)
			{
			}

			public override void RemoveGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate)
			{
			}

			public override void AddPurchaseResponseListener(PurchaseResponseDelegate responseDelegate)
			{
			}

			public override void RemovePurchaseResponseListener(PurchaseResponseDelegate responseDelegate)
			{
			}

			public override void AddGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate)
			{
			}

			public override void RemoveGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate)
			{
			}

			public override void AddGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate)
			{
			}

			public override void RemoveGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate)
			{
			}
		}

		private class AmazonIapV2Default : AmazonIapV2Base
		{
			protected override void Init()
			{
			}

			protected override void RegisterCallback()
			{
			}

			protected override void RegisterEventListener()
			{
			}

			protected override void RegisterCrossPlatformTool()
			{
			}

			protected override string NativeGetUserDataJson(string jsonMessage)
			{
				return null;
			}

			protected override string NativePurchaseJson(string jsonMessage)
			{
				return null;
			}

			protected override string NativeGetProductDataJson(string jsonMessage)
			{
				return null;
			}

			protected override string NativeGetPurchaseUpdatesJson(string jsonMessage)
			{
				return null;
			}

			protected override string NativeNotifyFulfillmentJson(string jsonMessage)
			{
				return null;
			}
		}

		private abstract class AmazonIapV2DelegatesBase : AmazonIapV2Base
		{
			private const string CrossPlatformTool = "XAMARIN";

			protected CallbackDelegate callbackDelegate;

			protected CallbackDelegate eventDelegate;

			protected override void Init()
			{
			}

			protected override void RegisterCallback()
			{
			}

			protected override void RegisterEventListener()
			{
			}

			protected override void RegisterCrossPlatformTool()
			{
			}

			public override void UnityFireEvent(string jsonMessage)
			{
			}

			protected abstract void NativeInit();

			protected abstract void NativeRegisterCallback(CallbackDelegate callback);

			protected abstract void NativeRegisterEventListener(CallbackDelegate callback);

			protected abstract void NativeRegisterCrossPlatformTool(string crossPlatformTool);
		}

		protected delegate void CallbackDelegate(string jsonMessage);

		private class Builder
		{
			internal static readonly IAmazonIapV2 instance;

			static Builder()
			{
			}
		}

		private class AmazonIapV2UnityAndroid : AmazonIapV2UnityBase
		{
			public new static AmazonIapV2UnityAndroid Instance
			{
				get
				{
					return null;
				}
			}

			[PreserveSig]
			private static extern string nativeRegisterCallbackGameObject(string name);

			[PreserveSig]
			private static extern string nativeInit();

			[PreserveSig]
			private static extern string nativeGetUserDataJson(string jsonMessage);

			[PreserveSig]
			private static extern string nativePurchaseJson(string jsonMessage);

			[PreserveSig]
			private static extern string nativeGetProductDataJson(string jsonMessage);

			[PreserveSig]
			private static extern string nativeGetPurchaseUpdatesJson(string jsonMessage);

			[PreserveSig]
			private static extern string nativeNotifyFulfillmentJson(string jsonMessage);

			protected override void NativeInit()
			{
			}

			protected override void RegisterCallback()
			{
			}

			protected override void RegisterEventListener()
			{
			}

			protected override void NativeRegisterCrossPlatformTool(string crossPlatformTool)
			{
			}

			protected override string NativeGetUserDataJson(string jsonMessage)
			{
				return null;
			}

			protected override string NativePurchaseJson(string jsonMessage)
			{
				return null;
			}

			protected override string NativeGetProductDataJson(string jsonMessage)
			{
				return null;
			}

			protected override string NativeGetPurchaseUpdatesJson(string jsonMessage)
			{
				return null;
			}

			protected override string NativeNotifyFulfillmentJson(string jsonMessage)
			{
				return null;
			}
		}

		private abstract class AmazonIapV2UnityBase : AmazonIapV2Base
		{
			private const string CrossPlatformTool = "UNITY";

			private static AmazonIapV2UnityBase instance;

			private static Type instanceType;

			private static bool quit;

			private static object initLock;

			static AmazonIapV2UnityBase()
			{
			}

			public static T getInstance<T>() where T : AmazonIapV2UnityBase
			{
				return null;
			}

			public void OnDestroy()
			{
			}

			private static void assertTrue(bool statement, string errorMessage)
			{
			}

			protected override void Init()
			{
			}

			protected override void RegisterCrossPlatformTool()
			{
			}

			protected abstract void NativeInit();

			protected abstract void NativeRegisterCrossPlatformTool(string crossPlatformTool);
		}

		private static AmazonLogger logger;

		private static readonly Dictionary<string, IDelegator> callbackDictionary;

		private static readonly object callbackLock;

		private static readonly Dictionary<string, List<IDelegator>> eventListeners;

		private static readonly object eventLock;

		public static IAmazonIapV2 Instance
		{
			get
			{
				return null;
			}
		}

		private AmazonIapV2Impl()
		{
		}

		public static void callback(string jsonMessage)
		{
		}

		private static void callbackCaller(Dictionary<string, object> response, string callerId)
		{
		}

		public static void FireEvent(string jsonMessage)
		{
		}

		public abstract RequestOutput GetUserData();

		public abstract RequestOutput Purchase(SkuInput skuInput);

		public abstract RequestOutput GetProductData(SkusInput skusInput);

		public abstract RequestOutput GetPurchaseUpdates(ResetInput resetInput);

		public abstract void NotifyFulfillment(NotifyFulfillmentInput notifyFulfillmentInput);

		public abstract void UnityFireEvent(string jsonMessage);

		public abstract void AddGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate);

		public abstract void RemoveGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate);

		public abstract void AddPurchaseResponseListener(PurchaseResponseDelegate responseDelegate);

		public abstract void RemovePurchaseResponseListener(PurchaseResponseDelegate responseDelegate);

		public abstract void AddGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate);

		public abstract void RemoveGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate);

		public abstract void AddGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate);

		public abstract void RemoveGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate);
	}
}
