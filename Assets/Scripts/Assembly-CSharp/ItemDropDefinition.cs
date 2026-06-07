using UnityEngine;

public class ItemDropDefinition
{
	private int did;

	private IDisplayController displayController;

	private Vector2 cleanupScreenDestination;

	private bool forceTapToCollect;

	public IDisplayController DisplayController
	{
		get
		{
			return null;
		}
	}

	public Vector2 CleanupScreenDestination
	{
		get
		{
			return default(Vector2);
		}
		set
		{
		}
	}

	public int Did
	{
		get
		{
			return 0;
		}
	}

	public bool ForceTapToCollect
	{
		get
		{
			return false;
		}
	}

	public ItemDropDefinition(int did, IDisplayController displayController, Vector2 cleanupScreenDestination, bool forceTapToCollect)
	{
	}
}
