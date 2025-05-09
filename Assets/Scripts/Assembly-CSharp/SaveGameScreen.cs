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

	private Vector3 saveArrowOffset = new Vector3(1.55f, 0.8f, -0.3f);

	protected override void Awake()
	{
		base.Awake();
		if (info1 == null || info2 == null || messageLabelBoundary1 == null)
		{
		}
		messageLabel3 = (SBGUILabel)FindChild("message3");
		messageLabel3_bottom = (SBGUILabel)FindChild("message3_bottom");
		messageLabel2 = (SBGUILabel)FindChild("message2");
		messageLabel2_bottom = (SBGUILabel)FindChild("message2_bottom");
		messageLabelBoundary1 = (SBGUIAtlasImage)FindChild("message_label_boundary");
		info1 = (SBGUILabel)FindChild("infoText1");
		title_server = (SBGUILabel)FindChild("title_server");
		level_server = (SBGUILabel)FindChild("level_server");
		money_server = (SBGUILabel)FindChild("money_server");
		jelly_server = (SBGUILabel)FindChild("jelly_server");
		timeStamp_server = (SBGUILabel)FindChild("timeStamp_server");
		info2 = (SBGUILabel)FindChild("infoText2");
		title_local = (SBGUILabel)FindChild("title_local");
		level_local = (SBGUILabel)FindChild("level_local");
		money_local = (SBGUILabel)FindChild("money_local");
		jelly_local = (SBGUILabel)FindChild("jelly_local");
		timeStamp_local = (SBGUILabel)FindChild("timeStamp_local");
		localBtn = (SBGUIButton)FindChild("local_button");
		serverBtn = (SBGUIButton)FindChild("server_button");
		btnName_local = (SBGUILabel)FindChild("local_label");
		btnName_server = (SBGUILabel)FindChild("server_label");
		highLight = (SBGUIAtlasImage)FindChild("highLight");
		saveGameArrow = (SBGUIAtlasImage)FindChild("saveGameArrow");
	}

	private void Start()
	{
	}

	public void SetUp(string message1, string message3, string message2, string title_server, string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string btnName_server, string title_local, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local, string btnName_local, Action server, Action local, Session session)
	{
		messageLabel3.SetText(message3);
		messageLabel3_bottom.SetText(message3);
		messageLabel2.SetText(message2);
		messageLabel2_bottom.SetText(message2);
		this.title_server.SetText(title_server);
		this.level_server.SetText(level_server);
		this.money_server.SetText(money_server);
		this.jelly_server.SetText(jelly_server);
		this.btnName_server.SetText(btnName_server);
		this.title_local.SetText(title_local);
		this.level_local.SetText(level_local);
		this.money_local.SetText(money_local);
		this.jelly_local.SetText(jelly_local);
		this.btnName_local.SetText(btnName_local);
		DateTime dateTime = Convert.ToDateTime(timeStamp_server);
		string text = "   " + dateTime.Month + "/" + dateTime.Day + "/" + dateTime.Year.ToString().Substring(Math.Max(0, dateTime.Year.ToString().Length - 2)) + "|at " + ((dateTime.Hour <= 12) ? dateTime.Hour : (dateTime.Hour - 12)) + ":" + dateTime.Minute + " " + ((dateTime.Hour <= 12) ? "AM" : "PM");
		DateTime dateTime2 = Convert.ToDateTime(timeStamp_local);
		string text2 = "  " + dateTime2.Month + "/" + dateTime2.Day + "/" + dateTime2.Year.ToString().Substring(Math.Max(0, dateTime2.Year.ToString().Length - 2)) + "|at " + ((dateTime2.Hour <= 12) ? dateTime2.Hour : (dateTime2.Hour - 12)) + ":" + dateTime2.Minute + " " + ((dateTime2.Hour <= 12) ? "AM" : "PM");
		this.timeStamp_server.SetText(text);
		this.timeStamp_local.SetText(text2);
		GameObject gameObject = GameObject.Find("local");
		GameObject gameObject2 = GameObject.Find("server");
		bool flag = false;
		if ((Convert.ToInt32(level_local) == Convert.ToInt32(level_server)) ? ((DateTime.Compare(Convert.ToDateTime(timeStamp_local), Convert.ToDateTime(timeStamp_server)) > 0) ? true : false) : ((Convert.ToInt32(level_local) > Convert.ToInt32(level_server)) ? true : false))
		{
			highLight.transform.parent = gameObject.transform;
			highLight.transform.localPosition = Vector3.zero;
			highLight.transform.localPosition = new Vector3(0f, 0f, -0.01f);
			saveGameArrow.transform.parent = gameObject.transform;
			saveGameArrow.transform.localPosition = Vector3.zero;
			saveGameArrow.transform.localPosition = saveArrowOffset;
		}
		else
		{
			highLight.transform.parent = gameObject2.transform;
			highLight.transform.localPosition = Vector3.zero;
			highLight.transform.localPosition = new Vector3(0f, 0f, -0.01f);
			saveGameArrow.transform.parent = gameObject2.transform;
			saveGameArrow.transform.localPosition = Vector3.zero;
			saveGameArrow.transform.localPosition = saveArrowOffset;
		}
		AttachActionToButton(serverBtn, server);
		AttachActionToButton(localBtn, local);
	}

	public float GetMainWindowZ()
	{
		return base.gameObject.transform.Find("window").localPosition.z;
	}
}
