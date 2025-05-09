using UnityEngine;

public class SBAuth : SoaringDelegate
{
	public bool SoaringAuthorizing;

	private SoaringPlayerResolver soaringPlayerResolver;

	public static SoaringDictionary campaigns;

	public SBAuth(RuntimePlatform platform)
	{
	}

	public SoaringPlayerResolver AccountResolver()
	{
		return soaringPlayerResolver;
	}

	public bool AccountResolveRequired()
	{
		return soaringPlayerResolver != null;
	}

	public void AccountResolved()
	{
		soaringPlayerResolver = null;
	}

	public void ResetAuth()
	{
		campaigns = null;
		Soaring.LogOut();
	}

	public void FindAndMigrateLoginID()
	{
		SoaringContext soaringContext = new SoaringContext();
		soaringContext.ContextResponder = OnFindLoginID;
		SoaringPlayerResolver.FindLoginID(soaringContext);
	}

	private void MigrateLocalData(string kffPlayerID, string soaringUserID, SoaringLoginType loginType)
	{
		bool flag = false;
		if (!string.IsNullOrEmpty(soaringUserID))
		{
			flag = !Player.CheckSoaringPathExists(soaringUserID);
		}
		if (flag)
		{
			string text = null;
			if (!string.IsNullOrEmpty(kffPlayerID))
			{
				text = kffPlayerID;
			}
			else
			{
				text = ((loginType != SoaringLoginType.Soaring && loginType != SoaringLoginType.Device) ? SoaringPlatform.PlatformUserID : SoaringPlatform.DeviceID);
			}
			if (SoaringInternal.instance.Versions != null)
			{
				SoaringInternal.instance.Versions.ClearAllContent();
				TFUtils.RefreshSAFiles();
			}
			SoaringDebug.Log("Migration ID: " + text + " : " + loginType);
			Player.MigratePlayerData(soaringUserID, text);
		}
	}

	public override void OnAuthorize(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
		if (error != null)
		{
			success = false;
		}
		if (success)
		{
			MigrateLocalData(null, player.UserTag, player.LoginType);
			bool flag = false;
			if (player.Name == null)
			{
				flag = true;
			}
			else if (player.LoginType == SoaringLoginType.Soaring || player.LoginType == SoaringLoginType.Device)
			{
				if (player.Name != SoaringPlatform.DeviceID)
				{
					flag = true;
				}
			}
			else if (player.Name != SoaringPlatform.PlatformUserAlias)
			{
				flag = true;
			}
			Soaring.RetreiveUserProfile();
			SoaringContext soaringContext = new SoaringContext();
			soaringContext.Responder = this;
			Soaring.RequestCampaign(soaringContext);
			if (player.LoginType != SoaringLoginType.Soaring && player.LoginType != SoaringLoginType.Device)
			{
				GameObject gameObject = GameObject.Find("SBGameCenterManager");
				SBGameCenterManager component = gameObject.GetComponent<SBGameCenterManager>();
				component.PlayerAuthenticated();
			}
		}
		else
		{
			SoaringDebug.Log("Authorization Failed: " + error, LogType.Error);
			SoaringAuthorizing = false;
		}
	}

	public override void OnInitializing(bool success, SoaringError error, SoaringDictionary data)
	{
		if (!success)
		{
			string text = string.Empty;
			if (error != null)
			{
				text = error;
			}
			SoaringDebug.Log("Failed to initialize soaring: " + text, LogType.Error);
		}
		SBMISoaring.OnInitializeSoaring();
		FindAndMigrateLoginID();
	}

	public void OnFindLoginID(SoaringContext context)
	{
		SoaringDebug.Log("OnFindLoginID: " + context.ToJsonString(), LogType.Error);
		string playerID = context.soaringValue("id");
		SoaringLoginType type = (SoaringLoginType)(int)context.soaringValue("type");
		context.Responder = this;
		context.ContextResponder = null;
		campaigns = null;
		SBMISoaring.FinalizeMigration(playerID, type, context);
	}

	public override void OnPlayerConflict(SoaringPlayerResolver resolver, SoaringPlayerResolver.SoaringPlayerData platform_player, SoaringPlayerResolver.SoaringPlayerData last_player, SoaringPlayerResolver.SoaringPlayerData device_player, SoaringContext context)
	{
		soaringPlayerResolver = resolver;
		SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = platform_player;
		if (soaringPlayerData == null)
		{
			soaringPlayerData = device_player;
		}
		resolver.HandleLoginConflict(soaringPlayerData);
	}

	public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		if (data == null)
		{
			Debug.LogWarning(GetType().Name + ": OnComponentFinished: " + module);
		}
		else
		{
			Debug.LogWarning(GetType().Name + ": OnComponentFinished: " + module + "\n" + data.ToJsonString());
		}
		if (!(module == "finalizeMigration"))
		{
			return;
		}
		if (!success || error != null || data == null)
		{
			SoaringInternal.instance.TriggerOfflineMode(true);
		}
		else
		{
			bool flag = data.soaringValue("found");
			SoaringLoginType loginType = (SoaringLoginType)(int)context.soaringValue("type");
			string text = null;
			string text2 = null;
			if (flag)
			{
				text = data.soaringValue("kffUserId");
				text2 = data.soaringValue("tag");
				MigrateLocalData(text, text2, loginType);
			}
		}
		Soaring.Player.Load();
	}

	public override void OnRetrieveCampaign(bool success, SoaringError error, SoaringArray cpns, SoaringContext context)
	{
		SoaringAuthorizing = false;
		if (cpns == null)
		{
			campaigns = null;
			return;
		}
		int num = cpns.count();
		if (num == 0)
		{
			campaigns = null;
			return;
		}
		campaigns = new SoaringDictionary(num);
		for (int i = 0; i < num; i++)
		{
			SoaringCampaign soaringCampaign = (SoaringCampaign)cpns.objectAtIndex(i);
			string text = soaringCampaign.Group;
			if (string.IsNullOrEmpty(text))
			{
				text = "none";
			}
			campaigns.addValue(text, soaringCampaign.Description);
		}
	}

	public override void OnRecievedEvent(SoaringEvents manager, SoaringEvent soaringEv)
	{
		if (soaringEv != null)
		{
			Session session_ref = SessionDriver.session_ref;
			if (session_ref != null)
			{
				SoaringDebug.Log(soaringEv.Name + ": Adding Event");
				session_ref.soaringEvents.addObject(soaringEv);
			}
		}
	}
}
