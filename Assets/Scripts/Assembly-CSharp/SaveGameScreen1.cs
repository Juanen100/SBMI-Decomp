using System;
using UnityEngine;

public class SaveGameScreen1 : SBGUIScreen
{
	private SBGUILabel messageLabel1;

	private SBGUILabel messageLabel1_bottom;

	private SBGUILabel messageLabel2;

	private SBGUILabel messageLabel2_bottom;

	private SBGUILabel info1;

	private SBGUILabel info2;

	private SBGUILabel btnLabel1;

	private SBGUILabel btnLabel2;

	private SBGUIButton btn1;

	private SBGUIButton btn2;

	private Vector3 rewardCenter;

	protected override void Awake()
	{
		base.Awake();
		messageLabel1 = (SBGUILabel)FindChild("message1");
		messageLabel1_bottom = (SBGUILabel)FindChild("message1_bottom");
		messageLabel2 = (SBGUILabel)FindChild("message2");
		messageLabel2_bottom = (SBGUILabel)FindChild("message2_bottom");
		info1 = (SBGUILabel)FindChild("info1");
		info2 = (SBGUILabel)FindChild("info2");
		btnLabel1 = (SBGUILabel)FindChild("btnLabel1");
		btnLabel2 = (SBGUILabel)FindChild("btnLabel2");
		btn1 = (SBGUIButton)FindChild("button1");
		btn2 = (SBGUIButton)FindChild("button2");
	}

	private void Start()
	{
	}

	public void SetUp(string message1, string message2, string info1, string info2, string btnLabel1, string btnLabel2, Action action1, Action action2)
	{
		messageLabel1.SetText(message1);
		messageLabel1_bottom.SetText(message1);
		messageLabel2.SetText(message2);
		messageLabel2_bottom.SetText(message2);
		this.info1.SetText(info1);
		this.info2.SetText(info2);
		this.btnLabel1.SetText(btnLabel1);
		this.btnLabel2.SetText(btnLabel2);
		AttachActionToButton(btn1, action1);
		AttachActionToButton(btn2, action2);
	}
}
