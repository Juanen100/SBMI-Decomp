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
		return null;
	}

	public bool AccountResolveRequired()
	{
		return false;
	}

	public void AccountResolved()
	{
	}

	public void ResetAuth()
	{
	}

	public void FindAndMigrateLoginID()
	{
	}

	private void MigrateLocalData(string kffPlayerID, string soaringUserID, SoaringLoginType loginType)
	{
	}

	public override void OnAuthorize(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
	}

	public override void OnInitializing(bool success, SoaringError error, SoaringDictionary data)
	{
	}

	public void OnFindLoginID(SoaringContext context)
	{
	}

	public override void OnPlayerConflict(SoaringPlayerResolver resolver, SoaringPlayerResolver.SoaringPlayerData platform_player, SoaringPlayerResolver.SoaringPlayerData last_player, SoaringPlayerResolver.SoaringPlayerData device_player, SoaringContext context)
	{
	}

	public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	public override void OnRetrieveCampaign(bool success, SoaringError error, SoaringArray cpns, SoaringContext context)
	{
	}

	public override void OnRecievedEvent(SoaringEvents manager, SoaringEvent soaringEv)
	{
	}
}
