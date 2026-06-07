using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class SaveGameScrollScreen : SBGUIScrollableDialog
{
	public GameObject slotPrefab;

	private SBGUILabel messageLabel1;

	private SBGUILabel messageLabel1_bottom;

	private SBGUILabel messageLabel2;

	private SBGUILabel messageLabel2_bottom;

	private SBGUIButton localBtn;

	private SBGUIButton serverBtn;

	private SBGUIButton offlineBtn;

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

	private SBGUILabel info3;

	private SBGUILabel title_offline;

	private SBGUILabel level_offline;

	private SBGUILabel money_offline;

	private SBGUILabel jelly_offline;

	private SBGUILabel patty_offline;

	private SBGUILabel timeStamp_offline;

	protected static TFPool<SBGUICreditsSlot> slotPool;

	public void Setup(string message1, string message2)
	{
	}

	public void CreateUI1(string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local, Action local, Action server)
	{
	}

	public void CreateUI(string info1, string title_server, string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string info2, string title_local, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local, string info3, string title_offline, string level_offline, string money_offline, string jelly_offline, string patty_offline, string timeStamp_offline, Action local, Action server, Action offline)
	{
	}

	[DebuggerHidden]
	private IEnumerator ScrollingCredits()
	{
		return null;
	}

	public override void Deactivate()
	{
	}

	private SBGUICreditsSlot CreateCreditsSlot(Session session, SBGUIElement anchor, Vector3 offset, string info1, string title_server, string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string info2, string title_local, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local, string info3, string title_offline, string level_offline, string money_offline, string jelly_offline, string patty_offline, string timeStamp_offline, Action local, Action server, Action offline)
	{
		return null;
	}

	public void setUpChild(SBGUICreditsSlot slot, string info1, string title_server, string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string info2, string title_local, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local, string info3, string title_offline, string level_offline, string money_offline, string jelly_offline, string patty_offline, string timeStamp_offline, Action local, Action server, Action offline)
	{
	}

	[DebuggerHidden]
	private IEnumerator serSubView1()
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator serSubView2()
	{
		return null;
	}
}
