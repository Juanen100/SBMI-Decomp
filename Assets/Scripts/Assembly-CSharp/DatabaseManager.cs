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

	private const int cVersion = 1;

	private bool bInitialized;

	private SheetInfo[] sheetTypeInfo;

	private MDictionary dictionarySheets;

	private static DatabaseManager sInstance;

	public bool HasData
	{
		get
		{
			return dictionarySheets != null && dictionarySheets.count() > 0;
		}
	}

	public static DatabaseManager Instance
	{
		get
		{
			if (sInstance == null)
			{
				sInstance = new DatabaseManager();
				sInstance.Initialize();
			}
			return sInstance;
		}
	}

	public int SheetCount
	{
		get
		{
			if (!bInitialized)
			{
				return 0;
			}
			if (sheetTypeInfo == null)
			{
				return 0;
			}
			return sheetTypeInfo.Length;
		}
	}

	private void Initialize()
	{
		if (!bInitialized)
		{
			bInitialized = true;
		}
	}

	public int GetNumRows(string sheetName)
	{
		int sheetIndex = GetSheetIndex(sheetName);
		return sheetTypeInfo[sheetIndex].numRow;
	}

	public bool HasRow(string sheetName, string rowName)
	{
		MDictionary mDictionary = dictionarySheets.objectWithKey(sheetName) as MDictionary;
		if (mDictionary == null)
		{
			return false;
		}
		return mDictionary.containsKey(rowName);
	}

	public bool HasRow(int sheetIDX, string rowName)
	{
		MDictionary mDictionary = dictionarySheets.objectAtIndex(sheetIDX) as MDictionary;
		if (mDictionary == null)
		{
			return false;
		}
		return mDictionary.containsKey(rowName);
	}

	public MArray GetEntireRow(string sheetName, string rowName)
	{
		MDictionary mDictionary = dictionarySheets.objectWithKey(sheetName) as MDictionary;
		if (mDictionary == null)
		{
			Debug.Log("DatabaseManager: failed to fetch data sheet named " + sheetName);
			return null;
		}
		return mDictionary.objectWithKey(rowName) as MArray;
	}

	public string[] GetHeaderRow(string sheetName)
	{
		int sheetIndex = GetSheetIndex(sheetName);
		SheetInfo sheetInfo = sheetTypeInfo[sheetIndex];
		string[] array = new string[sheetInfo.numCol];
		for (int i = 0; i < sheetInfo.numCol; i++)
		{
			array[i] = sheetInfo.typeInfo[i].colName;
		}
		return array;
	}

	public int GetSheetIndex(string sheetName)
	{
		if (dictionarySheets == null)
		{
			return -1;
		}
		return dictionarySheets.indexOfObjectWithKey(sheetName);
	}

	public int GetRowIndex(int sheetID, string rowID)
	{
		if (sheetID < 0 || rowID == null)
		{
			return -1;
		}
		MDictionary mDictionary = dictionarySheets.objectAtIndex(sheetID) as MDictionary;
		return mDictionary.indexOfObjectWithKey(rowID);
	}

	public MArray GetSheetKeys(string sheetName)
	{
		MDictionary mDictionary = dictionarySheets.objectWithKey(sheetName) as MDictionary;
		if (mDictionary == null)
		{
			return new MArray();
		}
		return mDictionary.allKeys();
	}

	public int GetColumnIndexInSheet(int sheetIdx, string columnName)
	{
		string value = columnName.ToLower();
		SheetInfo sheetInfo = sheetTypeInfo[sheetIdx];
		for (int i = 0; i < sheetInfo.numCol; i++)
		{
			if (sheetInfo.typeInfo[i].colName.Equals(value))
			{
				return i;
			}
		}
		return -1;
	}

	public int GetColumnIndexInSheet(string sheetName, string columnName)
	{
		int sheetIndex = GetSheetIndex(sheetName);
		return GetColumnIndexInSheet(sheetIndex, columnName);
	}

	public int GetIntCell(string sheetName, string rowName, string columnName)
	{
		MObject cell = GetCell(sheetName, rowName, columnName);
		return cell.valueAsInt();
	}

	public int GetIntCell(string sheetName, string rowName, int columnName)
	{
		MObject cell = GetCell(sheetName, rowName, columnName);
		return cell.valueAsInt();
	}

	public string GetStringCell(string sheetName, string rowName, string columnName)
	{
		MObject cell = GetCell(sheetName, rowName, columnName, true);
		if (cell == null)
		{
			return null;
		}
		return cell.valueAsString();
	}

	public float GetFloatCell(string sheetName, string rowName, string columnName)
	{
		MObject cell = GetCell(sheetName, rowName, columnName);
		return cell.valueAsFloat();
	}

	public int GetIntCell(int sheetID, int rowID, string columnName)
	{
		MObject cell = GetCell(sheetID, rowID, columnName);
		return cell.valueAsInt();
	}

	public string GetStringCell(int sheetID, int rowID, string columnName)
	{
		MObject cell = GetCell(sheetID, rowID, columnName, true);
		if (cell == null)
		{
			return null;
		}
		return cell.valueAsString();
	}

	public float GetFloatCell(int sheetID, int rowID, string columnName)
	{
		MObject cell = GetCell(sheetID, rowID, columnName);
		return cell.valueAsFloat();
	}

	public int GetIntCell(int sheetID, int rowID, int columnID)
	{
		MObject cell = GetCell(sheetID, rowID, columnID);
		return cell.valueAsInt();
	}

	public string GetStringCell(int sheetID, int rowID, int columnID)
	{
		MObject cell = GetCell(sheetID, rowID, columnID, true);
		if (cell == null)
		{
			return null;
		}
		return cell.valueAsString();
	}

	public float GetFloatCell(int sheetID, int rowID, int columnID)
	{
		MObject cell = GetCell(sheetID, rowID, columnID);
		return cell.valueAsFloat();
	}

	private MObject GetCell(string sheetName, string rowName, string columnName, bool failOk = false)
	{
		MArray entireRow = GetEntireRow(sheetName, rowName);
		if (entireRow == null)
		{
			if (!failOk)
			{
				Debug.LogError("DatabaseManager: failed to find row: " + rowName + " in sheet: " + sheetName);
			}
			return null;
		}
		int columnIndexInSheet = GetColumnIndexInSheet(sheetName, columnName);
		if (columnIndexInSheet < 0)
		{
			if (!failOk)
			{
				Debug.LogError("DatabaseManager: failed to find column: " + columnName + " in sheet: " + sheetName);
			}
			return null;
		}
		return entireRow.objectAtIndex(columnIndexInSheet) as MObject;
	}

	private MObject GetCell(string sheetName, string rowName, int columnIndex, bool failOk = false)
	{
		MArray entireRow = GetEntireRow(sheetName, rowName);
		if (entireRow == null)
		{
			if (!failOk)
			{
				Debug.LogError("DatabaseManager: failed to find row: " + rowName + " in sheet: " + sheetName);
			}
			return null;
		}
		if (columnIndex < 0)
		{
			if (!failOk)
			{
				Debug.LogError("DatabaseManager: failed to find column: " + columnIndex);
			}
			return null;
		}
		return entireRow.objectAtIndex(columnIndex) as MObject;
	}

	private MObject GetCell(int sheetID, int rowID, string columnName, bool failOk = false)
	{
		if (sheetID < 0 || rowID < 0)
		{
			if (!failOk)
			{
				Debug.LogError("DatabaseManager: failed to find row: " + columnName + " in sheet: " + sheetID);
			}
			return null;
		}
		MDictionary mDictionary = dictionarySheets.objectAtIndex(sheetID) as MDictionary;
		MArray mArray = mDictionary.objectAtIndex(rowID) as MArray;
		int columnIndexInSheet = GetColumnIndexInSheet(sheetID, columnName);
		if (columnIndexInSheet < 0)
		{
			if (!failOk)
			{
				Debug.LogError("DatabaseManager: failed to find column: " + columnName + " in sheet: " + sheetID);
			}
			return null;
		}
		return mArray.objectAtIndex(columnIndexInSheet) as MObject;
	}

	private MObject GetCell(int sheetID, int rowID, int columnID, bool failOk = false)
	{
		if (sheetID < 0 || rowID < 0 || columnID < 0)
		{
			if (!failOk)
			{
				Debug.LogError("DatabaseManager: failed to find row: " + columnID + " in sheet: " + sheetID);
			}
			return null;
		}
		MDictionary mDictionary = dictionarySheets.objectAtIndex(sheetID) as MDictionary;
		MArray mArray = mDictionary.objectAtIndex(rowID) as MArray;
		return mArray.objectAtIndex(columnID) as MObject;
	}

	public void SaveBinaryInstruction(string fileName)
	{
		string filePath = ResourceUtils.GetFilePath(fileName);
		if (filePath == null)
		{
			Debug.Log("DatabaseManager: Failed to get path to " + fileName);
		}
		MBinaryWriter mBinaryWriter = new MBinaryWriter();
		mBinaryWriter.Open(filePath, true, true);
		int length = sheetTypeInfo.GetLength(0);
		mBinaryWriter.Write(1);
		mBinaryWriter.Write(length);
		for (int i = 0; i < length; i++)
		{
			SheetInfo sheetInfo = sheetTypeInfo[i];
			mBinaryWriter.Write(sheetInfo.indexInDatabase);
			mBinaryWriter.Write(sheetInfo.keyBytesOffset);
			mBinaryWriter.Write(sheetInfo.cellBytesOffset);
			mBinaryWriter.Write(sheetInfo.numRow);
			mBinaryWriter.Write(sheetInfo.numCol);
			mBinaryWriter.Write(sheetInfo.fileName);
			for (int j = 0; j < sheetInfo.numCol; j++)
			{
				mBinaryWriter.Write(sheetInfo.typeInfo[j].colName);
				mBinaryWriter.Write((int)sheetInfo.typeInfo[j].id);
			}
		}
		mBinaryWriter.Close();
	}

	public void LoadBinaryInstruction(string fileName)
	{
		MBinaryReader fileStream = ResourceUtils.GetFileStream(fileName);
		if (fileStream == null)
		{
			Debug.Log("DatabaseManager.LoadBinaryInstructions: Failed to get path to " + fileName);
		}
		int num = fileStream.ReadInt();
		if (1 > num)
		{
			return;
		}
		int num2 = fileStream.ReadInt();
		sheetTypeInfo = new SheetInfo[num2];
		for (int i = 0; i < num2; i++)
		{
			SheetInfo sheetInfo = new SheetInfo();
			sheetInfo.indexInDatabase = fileStream.ReadInt();
			sheetInfo.keyBytesOffset = fileStream.ReadInt();
			sheetInfo.cellBytesOffset = fileStream.ReadInt();
			sheetInfo.numRow = fileStream.ReadInt();
			sheetInfo.numCol = fileStream.ReadInt();
			sheetInfo.fileName = fileStream.ReadString();
			sheetInfo.typeInfo = new CSVTypeInfo[sheetInfo.numCol];
			for (int j = 0; j < sheetInfo.numCol; j++)
			{
				sheetInfo.typeInfo[j].colName = fileStream.ReadString();
				sheetInfo.typeInfo[j].id = (TypeID)fileStream.ReadInt();
			}
			sheetTypeInfo[i] = sheetInfo;
		}
		fileStream.Close();
		fileStream = null;
	}

	public void SaveBinaryData(string fileName)
	{
		string writePath = ResourceUtils.GetWritePath(fileName, null, 1);
		if (writePath == null)
		{
			Debug.Log("DatabaseManager: Failed to get path to " + fileName);
		}
		MBinaryWriter mBinaryWriter = new MBinaryWriter(writePath);
		MArray mArray = dictionarySheets.allKeys();
		MArray mArray2 = dictionarySheets.allValues();
		for (int i = 0; i < mArray.count(); i++)
		{
			mBinaryWriter.Write(i);
			string val = mArray.objectAtIndex(i) as string;
			mBinaryWriter.Write(val);
			MDictionary dictionary = mArray2.objectAtIndex(i) as MDictionary;
			WriteDictionaryToBinaryData(mBinaryWriter, dictionary, i);
		}
		mBinaryWriter.Close();
	}

	public bool LoadDatabaseFromInstruction(string instructionFileName, string dbDataFileName)
	{
		LoadBinaryInstruction(instructionFileName);
		MBinaryReader fileStream = ResourceUtils.GetFileStream(dbDataFileName);
		if (fileStream == null)
		{
			Debug.Log("DatabaseManager.LoadDatabase: Failed to get path to " + dbDataFileName);
		}
		dictionarySheets = new MDictionary(sheetTypeInfo.Length);
		for (int i = 0; i < sheetTypeInfo.Length; i++)
		{
			fileStream.ReadInt();
			string key = fileStream.ReadString();
			MDictionary val = ReadSheetDictionaryFromBinaryData(fileStream, i);
			dictionarySheets.addValue(val, key);
		}
		return sheetTypeInfo.Length > 0;
	}

	private MArray ReadAllKeysFromBinaryData(MBinaryReader reader, int numKeys)
	{
		MArray mArray = new MArray(numKeys);
		for (int i = 0; i < numKeys; i++)
		{
			string obj = reader.ReadString();
			mArray.addObject(obj);
		}
		return mArray;
	}

	private MArray ReadAllValuesFromBinaryData(MBinaryReader reader, int numColumns, int sheetID)
	{
		MArray mArray = new MArray(numColumns);
		for (int i = 0; i < numColumns; i++)
		{
			MObject mObject = new MObject();
			switch (sheetTypeInfo[sheetID].typeInfo[i].id)
			{
			case TypeID.TYPE_INT:
				mObject.setValueAsInt(reader.ReadInt());
				break;
			case TypeID.TYPE_FLOAT:
				mObject.setValueAsFloat(reader.ReadFloat());
				break;
			case TypeID.TYPE_STRING:
				mObject.setValueAsString(reader.ReadString());
				break;
			}
			mArray.addObject(mObject);
		}
		return mArray;
	}

	private MDictionary ReadSheetDictionaryFromBinaryData(MBinaryReader reader, int sheetID)
	{
		int numRow = sheetTypeInfo[sheetID].numRow;
		MArray keys = ReadAllKeysFromBinaryData(reader, numRow);
		MArray mArray = new MArray(numRow);
		for (int i = 0; i < numRow; i++)
		{
			MArray obj = ReadAllValuesFromBinaryData(reader, sheetTypeInfo[sheetID].numCol, sheetID);
			mArray.addObject(obj);
		}
		return new MDictionary(keys, mArray);
	}

	private void WriteAllKeysToBinaryData(MBinaryWriter writer, MArray keys)
	{
		for (int i = 0; i < keys.count(); i++)
		{
			string val = keys.objectAtIndex(i) as string;
			writer.Write(val);
		}
	}

	private void WriterAllValuesToBinaryData(MBinaryWriter writer, MArray rowData, int sheetID)
	{
		for (int i = 0; i < rowData.count(); i++)
		{
			MObject mObject = rowData.objectAtIndex(i) as MObject;
			switch (sheetTypeInfo[sheetID].typeInfo[i].id)
			{
			case TypeID.TYPE_INT:
				writer.Write(mObject.valueAsInt());
				break;
			case TypeID.TYPE_FLOAT:
				writer.Write(mObject.valueAsFloat());
				break;
			case TypeID.TYPE_STRING:
				writer.Write(mObject.valueAsString());
				break;
			}
		}
	}

	private void WriteDictionaryToBinaryData(MBinaryWriter writer, MDictionary dictionary, int sheetID)
	{
		WriteAllKeysToBinaryData(writer, dictionary.allKeys());
		MArray mArray = dictionary.allValues();
		for (int i = 0; i < mArray.count(); i++)
		{
			MArray rowData = mArray.objectAtIndex(i) as MArray;
			WriterAllValuesToBinaryData(writer, rowData, sheetID);
		}
	}

	public bool LoadDatabaseFromCSV(string fileName = "Database_LookUp.csv")
	{
		dictionarySheets = LoadDataSheet(fileName, -1);
		MArray mArray = dictionarySheets.allKeys();
		MArray mArray2 = dictionarySheets.allValues();
		sheetTypeInfo = new SheetInfo[mArray.count()];
		for (int i = 0; i < mArray2.count(); i++)
		{
			MArray mArray3 = mArray2.objectAtIndex(i) as MArray;
			MObject mObject = mArray3.objectAtIndex(1) as MObject;
			string fileName2 = mObject.valueAsString();
			MDictionary val = LoadDataSheet(fileName2, i);
			string key = mArray.objectAtIndex(i) as string;
			dictionarySheets.setValue(val, key);
		}
		return mArray.count() > 0;
	}

	public Stream LoadFileData(string fileName)
	{
		Debug.Log("Database Loading: " + fileName);
		Stream stream = null;
		stream = ResourceUtils.GetRawFileStream("Contents/Spreadsheets/" + fileName, null, null, 1);
		if (stream == null)
		{
			stream = ResourceUtils.GetRawFileStream("Spreadsheets/" + fileName, null, null, 6);
		}
		return stream;
	}

	private unsafe MDictionary LoadDataSheet(string fileName, int sheetNum)
	{
		Stream stream = LoadFileData(fileName);
		if (stream == null)
		{
			Debug.LogError("DatabaseManager: Failed to get path to " + fileName);
			return new MDictionary(0);
		}
		CVSReader cVSReader = new CVSReader();
		if (!cVSReader.Open(stream))
		{
			Debug.Log("DatabaseManager: file can't open.");
		}
		cVSReader.ReadLine();
		MDictionary mDictionary = new MDictionary(cVSReader.GetRowCount());
		string key = null;
		for (MArray mArray = cVSReader.ParseLine(ref key); mArray != null; mArray = cVSReader.ParseLine(ref key))
		{
			mDictionary.addValue(mArray, key);
			key = null;
		}
		if (sheetNum != -1)
		{
			SheetInfo sheetInfo = new SheetInfo();
			sheetInfo.indexInDatabase = sheetNum;
			sheetInfo.keyBytesOffset = cVSReader.GetKeyBytesOffset();
			sheetInfo.cellBytesOffset = cVSReader.GetCellBytesOffset();
			sheetInfo.numRow = mDictionary.count();
			sheetInfo.numCol = cVSReader.GetColCount();
			sheetInfo.fileName = fileName;
			sheetInfo.typeInfo = cVSReader.GetTypeInfoTable();
			int numCol = sheetInfo.numCol;
			for (int i = 0; i < numCol; i++)
			{
				fixed (char* colName = sheetInfo.typeInfo[i].colName)
				{
					char* ptr = colName;
					char c = *ptr;
					while (true)
					{
						switch (c)
						{
						case '\0':
							break;
						case 'A':
						case 'B':
						case 'C':
						case 'D':
						case 'E':
						case 'F':
						case 'G':
						case 'H':
						case 'I':
						case 'J':
						case 'K':
						case 'L':
						case 'M':
						case 'N':
						case 'O':
						case 'P':
						case 'Q':
						case 'R':
						case 'S':
						case 'T':
						case 'U':
						case 'V':
						case 'W':
						case 'X':
						case 'Y':
						case 'Z':
							*ptr = (char)(c + 32);
							goto IL_013a;
						default:
							goto IL_013a;
						}
						break;
						IL_013a:
						ptr++;
						c = *ptr;
					}
				}
			}
			sheetTypeInfo[sheetNum] = sheetInfo;
		}
		return mDictionary;
	}
}
