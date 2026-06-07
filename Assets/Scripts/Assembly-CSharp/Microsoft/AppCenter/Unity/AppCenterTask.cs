using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Microsoft.AppCenter.Unity
{
	public class AppCenterTask : CustomYieldInstruction
	{
		private readonly List<Action<AppCenterTask>> _continuationActions;

		protected readonly object _lockObject;

		public bool IsComplete { get; private set; }

		public override bool keepWaiting
		{
			get
			{
				return false;
			}
		}

		public AppCenterTask(AndroidJavaObject javaFuture)
		{
		}

		public AppCenterTask()
		{
		}

		public void ContinueWith(Action<AppCenterTask> continuationAction)
		{
		}

		protected virtual void CompletionAction()
		{
		}

		protected void ThrowIfCompleted()
		{
		}

		internal static AppCenterTask FromCompleted()
		{
			return null;
		}

		private void InvokeContinuationActions()
		{
		}
	}
	public class AppCenterTask<TResult> : AppCenterTask
	{
		private ManualResetEvent _completionEvent;

		private TResult _result;

		private UnityAppCenterConsumer<TResult> _consumer;

		public TResult Result
		{
			get
			{
				return default(TResult);
			}
		}

		public AppCenterTask(AndroidJavaObject javaFuture)
			: base(null)
		{
		}

		public AppCenterTask()
			: base(null)
		{
		}

		internal void SetResult(TResult result)
		{
		}

		public void ContinueWith(Action<AppCenterTask<TResult>> continuationAction)
		{
		}

		internal static AppCenterTask<TResult> FromCompleted(TResult result)
		{
			return null;
		}
	}
}
