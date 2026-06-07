using System.Runtime.InteropServices;

public class PathFinder2
{
	public enum PROGRESS
	{
		INACTIVE = 0,
		SEEKING = 1,
		FAILED = 2,
		DONE = 3
	}

	[StructLayout((LayoutKind)0, Size = 8)]
	private struct SearchGridElem
	{
		public short flags;

		public short gscore;

		public short hscore;

		public short parent;
	}

	private class PriorityQueue
	{
		[StructLayout((LayoutKind)0, Size = 6)]
		private struct QueueEntry
		{
			public short entryValue;

			public ushort gridIdx;

			public ushort next;
		}

		[StructLayout((LayoutKind)0, Size = 4)]
		private struct BinEntry
		{
			public byte page;

			public byte binInPage;

			public short entriesCount;
		}

		[StructLayout((LayoutKind)0, Size = 8)]
		private struct Page
		{
			public ushort[] entryStarts;
		}

		private const ushort INVALID_ENTRY = ushort.MaxValue;

		private const byte INVALID_PAGE = byte.MaxValue;

		private int binStep;

		private int maxBins;

		private int minOccupiedBin;

		private int minOccupiedEntryInBin;

		private ushort firstFreeEntry;

		private short binsPerPage;

		private byte maxPageCount;

		private byte pageCount;

		private QueueEntry[] entries;

		private BinEntry[] binStarts;

		private Page[] pages;

		public byte freePage;

		public byte freeBinInPage;

		public PriorityQueue(int maxBins, int binStep, int maxValues)
		{
		}

		private bool AddPage()
		{
			return false;
		}

		public bool Push(ushort gridIndex, short val)
		{
			return false;
		}

		public ushort Pop()
		{
			return 0;
		}

		public bool Reinsert(ushort gridIndex, short oldVal, short newVal)
		{
			return false;
		}
	}

	public const int NOBUDGET = int.MaxValue;

	private const short GRID_OPEN = 1;

	private const short GRID_CLOSED = 2;

	private static int maxRow;

	private static int maxColumn;

	private static int gridSize;

	private static byte[] costGrid;

	private SearchGridElem[] searchGrid;

	private PriorityQueue queue;

	private static int[] neightbors;

	private int goalGridIdx;

	private PROGRESS progress;

	private int goalRow;

	private int goalColumn;

	private int openCount;

	private GridPosition start;

	private GridPosition goal;

	private static bool initialized;

	public PathFinder2(Terrain terrain)
	{
	}

	private static int RowColToIndex(int row, int col)
	{
		return 0;
	}

	private void CreateSearchGrid()
	{
	}

	public void Start(GridPosition start, GridPosition goal)
	{
	}

	public PROGRESS Seek(int budget = int.MaxValue)
	{
		return default(PROGRESS);
	}

	public void BuildPath(out Path<GridPosition> path)
	{
		path = null;
	}

	public static bool IsInitialized()
	{
		return false;
	}

	public static void UpdateCost(int row, int column, byte newCost)
	{
	}
}
