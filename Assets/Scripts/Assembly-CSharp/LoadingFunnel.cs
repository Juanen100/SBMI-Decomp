using System.Collections.Generic;
using UnityEngine;

public class LoadingFunnel : MonoBehaviour
{
	private struct LogInfo
	{
		public string stepName;

		public Dictionary<string, object> eventData;

		public LogInfo(string stepName, Dictionary<string, object> eventData)
		{
			this.stepName = null;
			this.eventData = null;
		}
	}

	private Queue<LogInfo> logRequests;

	public void Initialize(ref Dictionary<string, object> commonData)
	{
	}

	public void LogStep(string stepName, ref Dictionary<string, object> eventData)
	{
	}
}
