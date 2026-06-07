using System;
using UnityEngine;

public class SaveGameScreen : SBGUIScreen
{
	private SBGUILabel messageLabel1;

	private SBGUILabel messageLabel1_bottom;

	private SBGUILabel messageLabel3;

	private SBGUILabel messageLabel3_bottom;

	private SBGUILabel messageLabel2;

	private SBGUILabel messageLabel2_bottom;

	private SBGUIAtlasImage messageLabelBoundary1;

	private SBGUIButton localBtn;

	private SBGUIButton serverBtn;

	private SBGUILabel btnName_local;

	private SBGUILabel btnName_server;

	private SBGUILabel info1;

	private SBGUILabel title_server;

	private SBGUILabel level_server;

	private SBGUILabel money_server;

	private SBGUILabel jelly_server;

	private SBGUILabel patty_server;

	private SBGUILabel timeStamp_server;

	private SBGUILabel info2;

	private SBGUILabel title_local;

	private SBGUILabel level_local;

	private SBGUILabel money_local;

	private SBGUILabel jelly_local;

	private SBGUILabel patty_local;

	private SBGUILabel timeStamp_local;

	private SBGUIAtlasImage pattySprite;

	private SBGUIAtlasImage highLight;

	private SBGUIAtlasImage saveGameArrow;

	private Vector3 rewardCenter;

	private Vector3 saveArrowOffset;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	public void SetUp(string message1, string message3, string message2, string title_server, string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string btnName_server, string title_local, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local, string btnName_local, Action server, Action local, Session session)
	{
	}

	public float GetMainWindowZ()
	{
		return 0f;
	}
}
