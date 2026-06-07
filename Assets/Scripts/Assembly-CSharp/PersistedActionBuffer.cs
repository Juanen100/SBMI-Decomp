using System.Collections.Generic;

public class PersistedActionBuffer
{
	public abstract class PersistedAction
	{
		public delegate PersistedAction ConstructFromDict(Dictionary<string, object> dict);

		public static Dictionary<string, ConstructFromDict> TypeRegistry;

		private static int counter;

		public string type;

		public Identity target;

		private ulong time;

		public string tag;

		static PersistedAction()
		{
		}

		public PersistedAction(string type, Identity target)
		{
		}

		protected static string NextTag(ulong timestamp)
		{
			return null;
		}

		public ulong GetTime()
		{
			return 0uL;
		}

		public static PersistedAction FromDict(Dictionary<string, object> data)
		{
			return null;
		}

		public virtual void AddEnvelope(ulong time)
		{
		}

		public virtual void AddEnvelope(ulong time, string tag)
		{
		}

		public virtual Dictionary<string, object> ToDict()
		{
			return null;
		}

		public string DebugToString()
		{
			return null;
		}

		public abstract void Apply(Game game, ulong utcNow);

		public virtual void Confirm(Dictionary<string, object> gameState)
		{
		}

		public abstract void Process(Game game);
	}

	private List<PersistedAction> unconfirmed;

	private const int BUFFER_SOFT_LIMIT = 1;

	public static string ACTION_LIST_FILE;

	private object unconfirmedLock;

	private string unconfirmedFile;

	public PersistedActionBuffer(Player p, List<Dictionary<string, object>> actionList)
	{
	}

	public static List<Dictionary<string, object>> LoadActionList(Player p)
	{
		return null;
	}

	public void Record(PersistedAction action)
	{
	}

	public List<PersistedAction> GetAllUnackedActions()
	{
		return null;
	}

	public void DestroyCache()
	{
	}

	public void Flush()
	{
	}

	private void LoadFileToList(string fileName, List<PersistedAction> list)
	{
	}

	private void LoadActionsIntoList(List<Dictionary<string, object>> src, List<PersistedAction> dst)
	{
	}

	private void RecordActionToFile(PersistedAction action, string fileName)
	{
	}
}
