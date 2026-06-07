namespace Helpshift
{
	public interface HelpshiftInboxMessage
	{
		string GetIdentifier();

		string GetCoverImageFilePath();

		string GetIconImageFilePath();

		string GetTitle();

		string GetTitleColor();

		string GetBody();

		string GetBodyColor();

		string GetBackgroundColor();

		double GetCreatedAt();

		double GetExpiryTimeStamp();

		bool HasExpiryTimestamp();

		bool GetReadStatus();

		bool GetSeenStatus();

		int GetCountOfActions();

		string GetActionTitle(int index);

		string GetActionTitleColor(int index);

		bool IsActionGoalCompletion(int index);

		void ExecuteAction(int index, object activity);

		HelpshiftInboxMessageActionType GetActionType(int index);

		string GetActionData(int index);
	}
}
