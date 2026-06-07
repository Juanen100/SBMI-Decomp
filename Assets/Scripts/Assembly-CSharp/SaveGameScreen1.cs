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
	}

	private void Start()
	{
	}

	public void SetUp(string message1, string message2, string info1, string info2, string btnLabel1, string btnLabel2, Action action1, Action action2)
	{
	}
}
