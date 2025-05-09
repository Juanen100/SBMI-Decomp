using System;

public interface IControlBinding
{
	Action<Session> Action { get; }

	SBGUIButton DynamicButton { set; }

	string Label { get; set; }

	string DecorateSessionActionId(uint ownerDid);

	void DynamicUpdate(Session session);
}
