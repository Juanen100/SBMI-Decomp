using System.IO;
using MTools;

public class CVSReader
{
	private int CELLS_BYTES_OFFSET;

	private int KEY_BYTES_OFFSET;

	private int MAX_ROWS;

	private int MAX_COLUMNS;

	private CSVTypeInfo[] typeLookUp;

	private bool isFileOpen;

	private StreamReader reader;

	public CVSReader(string filePath)
	{
	}

	public CVSReader(Stream stream)
	{
	}

	public CVSReader()
	{
	}

	public int GetRowCount()
	{
		return 0;
	}

	public int GetColCount()
	{
		return 0;
	}

	public int GetCellBytesOffset()
	{
		return 0;
	}

	public int GetKeyBytesOffset()
	{
		return 0;
	}

	public CSVTypeInfo[] GetTypeInfoTable()
	{
		return null;
	}

	~CVSReader()
	{
	}

	public void Close()
	{
	}

	public bool IsOpen()
	{
		return false;
	}

	public bool Open(string path)
	{
		return false;
	}

	public bool Open(Stream stream)
	{
		return false;
	}

	public string ReadLine()
	{
		return null;
	}

	private bool IsSkipLine(string str)
	{
		return false;
	}

	public MArray ParseLine(ref string key)
	{
		return null;
	}

	private void ParseTypeLine()
	{
	}

	private void SetTypeData(string data, int colNum)
	{
	}

	private static string TrimString(string str)
	{
		return null;
	}
}
