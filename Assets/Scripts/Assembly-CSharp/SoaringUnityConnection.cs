using UnityEngine;

public class SoaringUnityConnection : SoaringConnection
{
	private WWW mConnection;

	public override float Progress
	{
		get
		{
			return 0f;
		}
	}

	public override string ContentAsText
	{
		get
		{
			return null;
		}
	}

	public override byte[] Content
	{
		get
		{
			return null;
		}
	}

	public override bool HasError
	{
		get
		{
			return false;
		}
	}

	public override bool IsValid
	{
		get
		{
			return false;
		}
	}

	public override bool Create(SCWebQueue.SCData properties)
	{
		return false;
	}

	public override bool SaveData()
	{
		return false;
	}

	public override bool IsDone()
	{
		return false;
	}
}
