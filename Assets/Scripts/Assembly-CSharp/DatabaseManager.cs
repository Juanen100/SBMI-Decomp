using System.IO;
using MTools;
using UnityEngine;

public class DatabaseManager
{
	private class SheetInfo : Object
	{
		public int indexInDatabase;

		public int keyBytesOffset;

		public int cellBytesOffset;

		public int numCol;

		public int numRow;

		public string fileName;

		public CSVTypeInfo[] typeInfo;
	}

	private bool bInitialized;

	private const int cVersion = 1;

	private SheetInfo[] sheetTypeInfo;

	private MDictionary dictionarySheets;

	private static DatabaseManager sInstance;

	public bool HasData
	{
		get
		{
			return false;
		}
	}

	public static DatabaseManager Instance
	{
		get
		{
			return null;
		}
	}

	public int SheetCount
	{
		get
		{
			return 0;
		}
	}

	private void Initialize()
	{
	}

	public int GetNumRows(string sheetName)
	{
		return 0;
	}

	public bool HasRow(string sheetName, string rowName)
	{
		return false;
	}

	public bool HasRow(int sheetIDX, string rowName)
	{
		return false;
	}

	public MArray GetEntireRow(string sheetName, string rowName)
	{
		return null;
	}

	public string[] GetHeaderRow(string sheetName)
	{
		return null;
	}

	public int GetSheetIndex(string sheetName)
	{
		return 0;
	}

	public int GetRowIndex(int sheetID, string rowID)
	{
		return 0;
	}

	public MArray GetSheetKeys(string sheetName)
	{
		return null;
	}

	public int GetColumnIndexInSheet(int sheetIdx, string columnName)
	{
		return 0;
	}

	public int GetColumnIndexInSheet(string sheetName, string columnName)
	{
		return 0;
	}

	public int GetIntCell(string sheetName, string rowName, string columnName)
	{
		return 0;
	}

	public int GetIntCell(string sheetName, string rowName, int columnName)
	{
		return 0;
	}

	public string GetStringCell(string sheetName, string rowName, string columnName)
	{
		return null;
	}

	public float GetFloatCell(string sheetName, string rowName, string columnName)
	{
		return 0f;
	}

	public int GetIntCell(int sheetID, int rowID, string columnName)
	{
		return 0;
	}

	public string GetStringCell(int sheetID, int rowID, string columnName)
	{
		return null;
	}

	public float GetFloatCell(int sheetID, int rowID, string columnName)
	{
		return 0f;
	}

	public int GetIntCell(int sheetID, int rowID, int columnID)
	{
		return 0;
	}

	public string GetStringCell(int sheetID, int rowID, int columnID)
	{
		return null;
	}

	public float GetFloatCell(int sheetID, int rowID, int columnID)
	{
		return 0f;
	}

	private MObject GetCell(string sheetName, string rowName, string columnName, bool failOk = false)
	{
		return null;
	}

	private MObject GetCell(string sheetName, string rowName, int columnIndex, bool failOk = false)
	{
		return null;
	}

	private MObject GetCell(int sheetID, int rowID, string columnName, bool failOk = false)
	{
		return null;
	}

	private MObject GetCell(int sheetID, int rowID, int columnID, bool failOk = false)
	{
		return null;
	}

	public void SaveBinaryInstruction(string fileName)
	{
	}

	public void LoadBinaryInstruction(string fileName)
	{
	}

	public void SaveBinaryData(string fileName)
	{
	}

	public bool LoadDatabaseFromInstruction(string instructionFileName, string dbDataFileName)
	{
		return false;
	}

	private MArray ReadAllKeysFromBinaryData(MBinaryReader reader, int numKeys)
	{
		return null;
	}

	private MArray ReadAllValuesFromBinaryData(MBinaryReader reader, int numColumns, int sheetID)
	{
		return null;
	}

	private MDictionary ReadSheetDictionaryFromBinaryData(MBinaryReader reader, int sheetID)
	{
		return null;
	}

	private void WriteAllKeysToBinaryData(MBinaryWriter writer, MArray keys)
	{
	}

	private void WriterAllValuesToBinaryData(MBinaryWriter writer, MArray rowData, int sheetID)
	{
	}

	private void WriteDictionaryToBinaryData(MBinaryWriter writer, MDictionary dictionary, int sheetID)
	{
	}

	public bool LoadDatabaseFromCSV(string fileName = "Database_LookUp.csv")
	{
		return false;
	}

	public Stream LoadFileData(string fileName)
	{
		return null;
	}

	private MDictionary LoadDataSheet(string fileName, int sheetNum)
	{
		return null;
	}
}
