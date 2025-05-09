using System;
using System.Collections;
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

	protected static TFPool<SBGUICreditsSlot> slotPool = new TFPool<SBGUICreditsSlot>();

	public void Setup(string message1, string message2)
	{
		if (info1 == null || info2 == null || info3 == null)
		{
		}
		messageLabel1 = (SBGUILabel)FindChild("message1");
		messageLabel1_bottom = (SBGUILabel)FindChild("message1_bottom");
		messageLabel2 = (SBGUILabel)FindChild("message2");
		messageLabel2_bottom = (SBGUILabel)FindChild("message2_bottom");
		messageLabel1.SetText(message1);
		messageLabel1_bottom.SetText(message1);
		messageLabel2.SetText(message2);
		messageLabel2_bottom.SetText(message2);
	}

	public void CreateUI1(string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local, Action local, Action server)
	{
	}

	public void CreateUI(string info1, string title_server, string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string info2, string title_local, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local, string info3, string title_offline, string level_offline, string money_offline, string jelly_offline, string patty_offline, string timeStamp_offline, Action local, Action server, Action offline)
	{
		SBGUICreditsSlot component = slotPrefab.GetComponent<SBGUICreditsSlot>();
		SBGUIImage sBGUIImage = (SBGUIImage)component.FindChild("slot_boundary");
		Vector2 vector = sBGUIImage.Size * 0.01f;
		Rect scrollSize = new Rect(0f, 0f, vector.x, vector.y);
		region.ResetScroll(scrollSize);
		region.ResetToMinScroll();
		CreateCreditsSlot(session, region.Marker, Vector3.zero, info1, title_server, level_server, money_server, jelly_server, patty_server, timeStamp_server, info2, title_local, level_local, money_local, jelly_local, patty_local, timeStamp_local, info3, title_offline, level_offline, money_offline, jelly_offline, patty_offline, timeStamp_offline, local, server, offline);
	}

	private IEnumerator ScrollingCredits()
	{
		yield return null;
		bool keepScrolling = true;
		while (keepScrolling)
		{
			if (region.WasRecentlyTouched)
			{
				keepScrolling = false;
			}
			region.momentum.TrackForSmoothing(region.subViewMarker.tform.position + new Vector3(0f, 0.005f, 0f));
			region.momentum.CalculateSmoothVelocity();
			yield return null;
		}
	}

	public override void Deactivate()
	{
		StopCoroutine("ScrollingCredits");
		slotPool.Clear(delegate(SBGUICreditsSlot slot)
		{
			slot.Deactivate();
		});
		base.Deactivate();
	}

	private SBGUICreditsSlot CreateCreditsSlot(Session session, SBGUIElement anchor, Vector3 offset, string info1, string title_server, string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string info2, string title_local, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local, string info3, string title_offline, string level_offline, string money_offline, string jelly_offline, string patty_offline, string timeStamp_offline, Action local, Action server, Action offline)
	{
		SBGUICreditsSlot sBGUICreditsSlot = ((title_offline == null) ? slotPool.Create(SBGUICreditsSlot.MakeCreditsSlot1) : slotPool.Create(SBGUICreditsSlot.MakeCreditsSlot2));
		setUpChild(sBGUICreditsSlot, title_server, level_server, money_server, jelly_server, patty_server, timeStamp_server, info1, title_local, level_local, money_local, jelly_local, patty_local, timeStamp_local, info2, title_offline, level_offline, money_offline, jelly_offline, patty_offline, timeStamp_offline, info3, local, server, offline);
		sBGUICreditsSlot.SetActive(true);
		sBGUICreditsSlot.Setup(session, this, anchor, offset);
		return sBGUICreditsSlot;
	}

	public void setUpChild(SBGUICreditsSlot slot, string info1, string title_server, string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string info2, string title_local, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local, string info3, string title_offline, string level_offline, string money_offline, string jelly_offline, string patty_offline, string timeStamp_offline, Action local, Action server, Action offline)
	{
		this.info1 = (SBGUILabel)slot.FindChild("infoText1");
		this.title_server = (SBGUILabel)slot.FindChild("title_server");
		this.level_server = (SBGUILabel)slot.FindChild("level_server");
		this.money_server = (SBGUILabel)slot.FindChild("money_server");
		this.jelly_server = (SBGUILabel)slot.FindChild("jelly_server");
		this.patty_server = (SBGUILabel)slot.FindChild("patty_server");
		this.timeStamp_server = (SBGUILabel)slot.FindChild("timeStamp_server");
		this.info2 = (SBGUILabel)slot.FindChild("infoText2");
		this.title_local = (SBGUILabel)slot.FindChild("title_local");
		this.level_local = (SBGUILabel)slot.FindChild("level_local");
		this.money_local = (SBGUILabel)slot.FindChild("money_local");
		this.jelly_local = (SBGUILabel)slot.FindChild("jelly_local");
		this.patty_local = (SBGUILabel)slot.FindChild("patty_local");
		this.timeStamp_local = (SBGUILabel)slot.FindChild("timeStamp_local");
		this.title_server.SetText(title_server);
		this.level_server.SetText(level_server);
		this.money_server.SetText(money_server);
		this.jelly_server.SetText(jelly_server);
		this.patty_server.SetText(patty_server);
		this.timeStamp_server.SetText(timeStamp_server);
		this.title_local.SetText(title_local);
		this.level_local.SetText(level_local);
		this.money_local.SetText(money_local);
		this.jelly_local.SetText(jelly_local);
		this.patty_local.SetText(patty_local);
		this.timeStamp_local.SetText(timeStamp_local);
		localBtn = (SBGUIButton)slot.FindChild("local_button");
		serverBtn = (SBGUIButton)slot.FindChild("server_button");
		AttachActionToButton(localBtn, local);
		AttachActionToButton(serverBtn, server);
		if (title_offline != null)
		{
			this.info3 = (SBGUILabel)slot.FindChild("infoText3");
			this.title_offline = (SBGUILabel)slot.FindChild("title_offline");
			this.level_offline = (SBGUILabel)slot.FindChild("level_offline");
			this.money_offline = (SBGUILabel)slot.FindChild("money_offline");
			this.jelly_offline = (SBGUILabel)slot.FindChild("jelly_offline");
			this.patty_offline = (SBGUILabel)slot.FindChild("patty_offline");
			this.timeStamp_offline = (SBGUILabel)slot.FindChild("timeStamp_offline");
			this.title_offline.SetText(title_offline);
			this.level_offline.SetText(level_offline);
			this.money_offline.SetText(money_offline);
			this.jelly_offline.SetText(jelly_offline);
			this.patty_offline.SetText(patty_offline);
			this.timeStamp_offline.SetText(timeStamp_offline);
			offlineBtn = (SBGUIButton)slot.FindChild("offline_button");
			AttachActionToButton(offlineBtn, offline);
			StartCoroutine(serSubView2());
		}
		else
		{
			StartCoroutine(serSubView1());
		}
	}

	private IEnumerator serSubView1()
	{
		yield return new WaitForSeconds(0.2f);
		GameObject go = GameObject.Find("CreditsSlot1");
		GUISubView subView = go.transform.parent.parent.GetComponent<GUISubView>();
		subView.transform.localPosition = new Vector3(0f, -1.1f, 21f);
		Vector3 pos = go.transform.parent.transform.localPosition;
		go.transform.parent.transform.localPosition = new Vector3(0f, 1.3f, 0f) + pos;
	}

	private IEnumerator serSubView2()
	{
		yield return new WaitForSeconds(0.2f);
		GameObject go = GameObject.Find("CreditsSlot2");
		GUISubView subView = go.transform.parent.parent.GetComponent<GUISubView>();
		subView.transform.localPosition = new Vector3(0f, -1.1f, 21f);
		Vector3 pos = go.transform.parent.transform.localPosition;
		go.transform.parent.transform.localPosition = new Vector3(0f, 1.4f, 0f) + pos;
	}
}
