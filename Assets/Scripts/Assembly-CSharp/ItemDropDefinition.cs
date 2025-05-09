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
			return displayController;
		}
	}

	public Vector2 CleanupScreenDestination
	{
		get
		{
			return cleanupScreenDestination;
		}
		set
		{
			cleanupScreenDestination = value;
		}
	}

	public int Did
	{
		get
		{
			return did;
		}
	}

	public bool ForceTapToCollect
	{
		get
		{
			return forceTapToCollect;
		}
	}

	public ItemDropDefinition(int did, IDisplayController displayController, Vector2 cleanupScreenDestination, bool forceTapToCollect)
	{
		this.did = did;
		this.displayController = displayController;
		this.cleanupScreenDestination = cleanupScreenDestination;
		this.forceTapToCollect = forceTapToCollect;
	}
}
