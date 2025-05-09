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
			this.stepName = stepName;
			this.eventData = eventData;
		}
	}

	private float lastTime;

	private Queue<LogInfo> logRequests = new Queue<LogInfo>();

	public void Initialize(ref Dictionary<string, object> commonData)
	{
		lastTime = Time.time;
		LogStep("UnityInitialized", ref commonData);
	}

	public void LogStep(string stepName, ref Dictionary<string, object> eventData)
	{
		logRequests.Enqueue(new LogInfo(stepName, eventData));
	}

	public void Update()
	{
		while (logRequests.Count > 0)
		{
			float num = Time.time - lastTime;
			lastTime = Time.time;
			LogInfo logInfo = logRequests.Dequeue();
			logInfo.eventData["value"] = (int)num;
			logInfo.eventData["subtype1"] = "LoadFunnel";
			TFAnalytics.LogEvent(logInfo.stepName, logInfo.eventData);
		}
	}
}
