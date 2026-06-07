using UnityEngine;

namespace Helpshift
{
	public class HelpshiftAndroidInboxMessage : HelpshiftInboxMessage
	{
		private AndroidJavaObject inboxMessageJavaInstance;

		private static AndroidJavaClass inboxInterfaceClass;

		private string identifier;

		private string title;

		private string titleColor;

		private string body;

		private string bodyColor;

		private string backgroundColor;

		private long createdAt;

		private long expiryTimeStamp;

		private bool readStatus;

		private bool seenStatus;

		private int actionsCount;

		private HelpshiftAndroidInboxMessage()
		{
		}

		public string GetIdentifier()
		{
			return null;
		}

		public string GetCoverImageFilePath()
		{
			return null;
		}

		public string GetIconImageFilePath()
		{
			return null;
		}

		public string GetTitle()
		{
			return null;
		}

		public string GetTitleColor()
		{
			return null;
		}

		public string GetBody()
		{
			return null;
		}

		public string GetBodyColor()
		{
			return null;
		}

		public string GetBackgroundColor()
		{
			return null;
		}

		public double GetCreatedAt()
		{
			return 0.0;
		}

		public double GetExpiryTimeStamp()
		{
			return 0.0;
		}

		public bool HasExpiryTimestamp()
		{
			return false;
		}

		public bool GetReadStatus()
		{
			return false;
		}

		public bool GetSeenStatus()
		{
			return false;
		}

		public int GetCountOfActions()
		{
			return 0;
		}

		public string GetActionTitle(int index)
		{
			return null;
		}

		public string GetActionTitleColor(int index)
		{
			return null;
		}

		public bool IsActionGoalCompletion(int index)
		{
			return false;
		}

		public void ExecuteAction(int index, object activity)
		{
		}

		public string GetActionData(int index)
		{
			return null;
		}

		public HelpshiftInboxMessageActionType GetActionType(int index)
		{
			return default(HelpshiftInboxMessageActionType);
		}

		public static HelpshiftAndroidInboxMessage createInboxMessage(AndroidJavaObject message)
		{
			return null;
		}

		public override string ToString()
		{
			return null;
		}
	}
}
