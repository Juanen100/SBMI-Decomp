#define ASSERTS_ON
public class PathFinder2
{
	public enum PROGRESS
	{
		INACTIVE = 0,
		SEEKING = 1,
		FAILED = 2,
		DONE = 3
	}

	private struct SearchGridElem
	{
		public short flags;

		public short gscore;

		public short hscore;

		public short parent;
	}

	private class PriorityQueue
	{
		private struct QueueEntry
		{
			public short entryValue;

			public ushort gridIdx;

			public ushort next;
		}

		private struct BinEntry
		{
			public byte page;

			public byte binInPage;

			public short entriesCount;
		}

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
			this.binStep = binStep;
			entries = new QueueEntry[maxValues];
			binStarts = new BinEntry[maxBins];
			this.maxBins = maxBins;
			binsPerPage = 10;
			maxPageCount = (byte)(maxBins / binsPerPage);
			pageCount = 0;
			pages = new Page[maxPageCount];
			freePage = byte.MaxValue;
			for (int i = 0; i < maxBins; i++)
			{
				binStarts[i].page = byte.MaxValue;
			}
			for (int j = 0; j < maxValues; j++)
			{
				entries[j].gridIdx = ushort.MaxValue;
				entries[j].next = (ushort)(j + 1);
			}
			entries[maxValues - 1].next = ushort.MaxValue;
			minOccupiedBin = int.MaxValue;
			minOccupiedEntryInBin = 0;
		}

		private bool AddPage()
		{
			if (pageCount >= maxPageCount)
			{
				return false;
			}
			int num = binStep * binsPerPage;
			pages[pageCount].entryStarts = new ushort[num];
			for (int i = 0; i < num; i++)
			{
				pages[pageCount].entryStarts[i] = ushort.MaxValue;
			}
			int num2 = 0;
			for (int j = 0; j < binsPerPage - 1; j++)
			{
				pages[pageCount].entryStarts[num2] = (ushort)((pageCount << 8) | (j + 1));
				num2 += binStep;
			}
			pages[pageCount].entryStarts[num2] = (ushort)((freePage << 8) | freeBinInPage);
			freePage = pageCount;
			freeBinInPage = 0;
			pageCount++;
			return true;
		}

		public bool Push(ushort gridIndex, short val)
		{
			int num = val / binStep;
			if (num >= maxBins)
			{
				return false;
			}
			ushort num2 = firstFreeEntry;
			firstFreeEntry = entries[firstFreeEntry].next;
			TFUtils.Assert(firstFreeEntry != ushort.MaxValue, "no free list entries in priorityQueue");
			if (binStarts[num].entriesCount == 0)
			{
				if (freePage == byte.MaxValue && !AddPage())
				{
					return false;
				}
				binStarts[num].page = freePage;
				binStarts[num].binInPage = freeBinInPage;
				ushort num3 = pages[freePage].entryStarts[freeBinInPage * binStep];
				pages[freePage].entryStarts[freeBinInPage * binStep] = ushort.MaxValue;
				freePage = (byte)(num3 >> 8);
				freeBinInPage = (byte)(num3 & 0xFF);
			}
			binStarts[num].entriesCount++;
			int num4 = val - num * binStep;
			TFUtils.Assert(num4 >= 0 && num4 < binStep, "wrong indexInbin in priorityQueue");
			int page = binStarts[num].page;
			int num5 = binStep * binStarts[num].binInPage + num4;
			entries[num2].entryValue = val;
			entries[num2].gridIdx = gridIndex;
			ushort next = pages[page].entryStarts[num5];
			entries[num2].next = next;
			pages[page].entryStarts[num5] = num2;
			if (num < minOccupiedBin)
			{
				minOccupiedBin = num;
				minOccupiedEntryInBin = num4;
			}
			else if (num == minOccupiedBin && num4 < minOccupiedEntryInBin)
			{
				minOccupiedEntryInBin = num4;
			}
			return true;
		}

		public ushort Pop()
		{
			if (binStarts[minOccupiedBin].page == byte.MaxValue)
			{
				int num = minOccupiedBin + 1;
				while (binStarts[num].page == byte.MaxValue)
				{
					num++;
					TFUtils.Assert(num < maxBins, "bad binIndex in priorityQueue while searching for min bin");
				}
				minOccupiedBin = num;
				minOccupiedEntryInBin = 0;
			}
			TFUtils.Assert(binStarts[minOccupiedBin].entriesCount > 0, "no entries in priorityQueue for Pop");
			byte page = binStarts[minOccupiedBin].page;
			byte binInPage = binStarts[minOccupiedBin].binInPage;
			int num2 = binInPage * binStep;
			while (pages[page].entryStarts[num2 + minOccupiedEntryInBin] == ushort.MaxValue)
			{
				minOccupiedEntryInBin++;
				TFUtils.Assert(num2 + minOccupiedEntryInBin < binStep * binsPerPage, "invalid loop while searching for minOccupiedEntryBin in priorityQueue");
			}
			TFUtils.Assert(minOccupiedEntryInBin < binStep, "bad minOccupiedEntryInBin in priorityQueue while searching for min index in bin");
			ushort num3 = pages[page].entryStarts[num2 + minOccupiedEntryInBin];
			ushort next = entries[num3].next;
			ushort gridIdx = entries[num3].gridIdx;
			pages[page].entryStarts[num2 + minOccupiedEntryInBin] = next;
			entries[num3].next = firstFreeEntry;
			firstFreeEntry = num3;
			binStarts[minOccupiedBin].entriesCount--;
			if (binStarts[minOccupiedBin].entriesCount == 0)
			{
				pages[page].entryStarts[num2] = (ushort)((freePage << 8) | freeBinInPage);
				freePage = page;
				freeBinInPage = binInPage;
				binStarts[minOccupiedBin].page = byte.MaxValue;
			}
			return gridIdx;
		}

		public bool Reinsert(ushort gridIndex, short oldVal, short newVal)
		{
			int num = oldVal / binStep;
			if (num >= maxBins)
			{
				return false;
			}
			byte page = binStarts[num].page;
			byte binInPage = binStarts[num].binInPage;
			int num2 = binInPage * binStep;
			int num3 = oldVal - num * binStep;
			ushort num4 = ushort.MaxValue;
			ushort num5 = pages[page].entryStarts[num2 + num3];
			while (entries[num5].gridIdx != gridIndex)
			{
				num4 = num5;
				num5 = entries[num5].next;
			}
			if (num4 == ushort.MaxValue)
			{
				pages[page].entryStarts[num2 + num3] = entries[num5].next;
			}
			else
			{
				entries[num4].next = entries[num5].next;
			}
			entries[num5].next = firstFreeEntry;
			firstFreeEntry = num5;
			binStarts[num].entriesCount--;
			if (binStarts[num].entriesCount == 0)
			{
				pages[page].entryStarts[num2] = (ushort)((freePage << 8) | freeBinInPage);
				freePage = page;
				freeBinInPage = binInPage;
				binStarts[num].page = byte.MaxValue;
			}
			return Push(gridIndex, newVal);
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

	private static int[] neightbors = new int[4];

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
		if (!initialized)
		{
			maxRow = terrain.GridHeight + 2;
			maxColumn = terrain.GridWidth + 2;
			gridSize = maxRow * maxColumn;
			costGrid = new byte[gridSize];
			for (int i = 0; i < maxRow - 1; i++)
			{
				for (int j = 0; j < maxColumn - 1; j++)
				{
					byte terrainCost = terrain.GetTerrainCost(i, j);
					int num = RowColToIndex(i, j);
					costGrid[num] = terrainCost;
				}
			}
			int num2 = (maxRow - 1) * maxColumn;
			for (int k = 0; k < maxColumn; k++)
			{
				costGrid[k] = byte.MaxValue;
				costGrid[k + num2] = byte.MaxValue;
			}
			int num3 = maxColumn;
			for (int l = 1; l < maxRow - 1; l++)
			{
				costGrid[num3] = byte.MaxValue;
				costGrid[num3 + maxColumn - 1] = byte.MaxValue;
				num3 += maxColumn;
			}
			neightbors[0] = -maxRow;
			neightbors[1] = 1;
			neightbors[2] = maxRow;
			neightbors[3] = -1;
			initialized = true;
		}
		CreateSearchGrid();
	}

	private static int RowColToIndex(int row, int col)
	{
		return (row + 1) * maxColumn + (col + 1);
	}

	private void CreateSearchGrid()
	{
		searchGrid = new SearchGridElem[gridSize];
		int num = (maxRow - 1) * maxColumn;
		for (int i = 0; i < maxColumn; i++)
		{
			searchGrid[i].flags = 2;
			searchGrid[i + num].flags = 2;
		}
		int num2 = maxColumn;
		for (int j = 1; j < maxRow - 1; j++)
		{
			searchGrid[num2].flags = 2;
			searchGrid[num2 + maxColumn - 1].flags = 2;
			num2 += maxColumn;
		}
	}

	public void Start(GridPosition start, GridPosition goal)
	{
		this.goal = goal;
		goalRow = goal.row + 1;
		goalColumn = goal.col + 1;
		int num = start.row + 1;
		int num2 = start.col + 1;
		goalGridIdx = goalRow * maxRow + goalColumn;
		int num3 = num * maxRow + num2;
		searchGrid[num3].flags |= 1;
		searchGrid[num3].gscore = 0;
		int num4 = goalRow - num;
		int num5 = goalColumn - num2;
		if (num4 < 0)
		{
			num4 = -num4;
		}
		if (num5 < 0)
		{
			num5 = -num5;
		}
		searchGrid[num3].hscore = (short)(num4 + num5);
		searchGrid[num3].parent = -1;
		int num6 = maxRow + maxColumn;
		int num7 = num6 * 100;
		int num8 = 20;
		queue = new PriorityQueue(num7 / num8, num8, num6 * 8);
		openCount = 1;
		if (queue.Push((ushort)num3, (short)(searchGrid[num3].gscore + searchGrid[num3].hscore)))
		{
			progress = PROGRESS.SEEKING;
		}
		else
		{
			progress = PROGRESS.FAILED;
		}
	}

	public PROGRESS Seek(int budget = int.MaxValue)
	{
		if (progress != PROGRESS.SEEKING)
		{
			return progress;
		}
		int num = 1;
		while (openCount > 0)
		{
			if (num > budget)
			{
				return progress;
			}
			num++;
			int num2 = queue.Pop();
			if (goalGridIdx == num2)
			{
				progress = PROGRESS.DONE;
				return progress;
			}
			searchGrid[num2].flags &= -2;
			searchGrid[num2].flags |= 2;
			openCount--;
			for (int i = 0; i < 4; i++)
			{
				int num3 = num2 + neightbors[i];
				short flags = searchGrid[num3].flags;
				if ((flags & 2) != 0)
				{
					continue;
				}
				int num4 = searchGrid[num2].gscore + costGrid[num3];
				if ((flags & 1) != 0)
				{
					if (num4 < searchGrid[num3].gscore)
					{
						if (!queue.Reinsert((ushort)num3, (short)(searchGrid[num3].gscore + searchGrid[num3].hscore), (short)(num4 + searchGrid[num3].hscore)))
						{
							progress = PROGRESS.FAILED;
							return progress;
						}
						searchGrid[num3].gscore = (short)num4;
						searchGrid[num3].parent = (short)num2;
					}
					continue;
				}
				searchGrid[num3].gscore = (short)num4;
				int num5 = num3 / maxRow;
				int num6 = num3 - num5 * maxRow;
				int num7 = goalRow - num5;
				int num8 = goalColumn - num6;
				int num9 = num7 >> 31;
				num7 ^= num9;
				num7 += num9 & 1;
				int num10 = num8 >> 31;
				num8 ^= num10;
				num8 += num10 & 1;
				searchGrid[num3].hscore = (short)(num7 + num8);
				searchGrid[num3].flags |= 1;
				searchGrid[num3].parent = (short)num2;
				openCount++;
				if (!queue.Push((ushort)num3, (short)(searchGrid[num3].gscore + searchGrid[num3].hscore)))
				{
					progress = PROGRESS.FAILED;
					return progress;
				}
			}
		}
		progress = PROGRESS.FAILED;
		return progress;
	}

	public void BuildPath(out Path<GridPosition> path)
	{
		path = new Path<GridPosition>();
		GridPosition position = goal;
		path.Add(position);
		int parent = searchGrid[goalGridIdx].parent;
		int num = 1;
		int num2 = 2 * (maxRow + maxColumn);
		while (parent > 0)
		{
			int num3 = parent / maxRow;
			int num4 = parent - num3 * maxRow;
			GridPosition position2 = new GridPosition(num3 - 1, num4 - 1);
			path.Add(position2);
			TFUtils.Assert(num++ < num2, "path too long");
			parent = searchGrid[parent].parent;
		}
	}

	public static bool IsInitialized()
	{
		return initialized;
	}

	public static void UpdateCost(int row, int column, byte newCost)
	{
		int num = RowColToIndex(row, column);
		costGrid[num] = newCost;
	}
}
