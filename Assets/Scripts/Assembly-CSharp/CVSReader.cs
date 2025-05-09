using System;
using System.IO;
using MTools;
using UnityEngine;

public class CVSReader
{
	private int CELLS_BYTES_OFFSET;

	private int KEY_BYTES_OFFSET;

	private int MAX_ROWS = -1;

	private int MAX_COLUMNS = -1;

	private CSVTypeInfo[] typeLookUp;

	private bool isFileOpen;

	private StreamReader reader;

	public CVSReader(string filePath)
	{
		Open(filePath);
	}

	public CVSReader(Stream stream)
	{
		Open(stream);
	}

	public CVSReader()
	{
	}

	public int GetRowCount()
	{
		return MAX_ROWS;
	}

	public int GetColCount()
	{
		return MAX_COLUMNS;
	}

	public int GetCellBytesOffset()
	{
		return CELLS_BYTES_OFFSET;
	}

	public int GetKeyBytesOffset()
	{
		return KEY_BYTES_OFFSET;
	}

	public CSVTypeInfo[] GetTypeInfoTable()
	{
		return typeLookUp;
	}

	~CVSReader()
	{
		Close();
		reader = null;
	}

	public void Close()
	{
		if (!isFileOpen)
		{
			return;
		}
		try
		{
			if (reader != null)
			{
				reader.Close();
			}
			reader = null;
			isFileOpen = false;
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	public bool IsOpen()
	{
		return isFileOpen;
	}

	public bool Open(string path)
	{
		if (isFileOpen)
		{
			return false;
		}
		if (!File.Exists(path))
		{
			Debug.Log("CSVReader: Couldn't open file: " + path);
			return false;
		}
		try
		{
			reader = new StreamReader(path);
			isFileOpen = true;
		}
		catch
		{
			isFileOpen = false;
		}
		ParseTypeLine();
		return isFileOpen;
	}

	public bool Open(Stream stream)
	{
		if (isFileOpen)
		{
			return false;
		}
		if (stream == null)
		{
			Debug.Log("CSVReader: Invalid File Stream");
			return false;
		}
		try
		{
			reader = new StreamReader(stream);
			isFileOpen = true;
		}
		catch
		{
			isFileOpen = false;
		}
		ParseTypeLine();
		return isFileOpen;
	}

	public string ReadLine()
	{
		return reader.ReadLine();
	}

	private bool IsSkipLine(string str)
	{
		char[] array = str.ToCharArray();
		for (int i = 0; i < str.Length; i++)
		{
			if (array[i] != ',' && array[i] != '_')
			{
				return false;
			}
		}
		return true;
	}

	public MArray ParseLine(ref string key)
	{
		string text = ReadLine();
		while (text != null && IsSkipLine(text))
		{
			text = ReadLine();
		}
		if (text == null)
		{
			return null;
		}
		char[] array = text.ToCharArray();
		int num = 0;
		if (array[num] == ',')
		{
			num++;
			while (num < text.Length && array[num] == ',')
			{
				if (array[num] == ',')
				{
					num++;
				}
			}
			if (num > text.Length)
			{
				return null;
			}
		}
		MArray mArray = new MArray(array.Length);
		MObject obj = null;
		int num2 = 0;
		string text2 = null;
		sbyte b = -1;
		for (int i = num; i <= array.Length; i++)
		{
			if (i != array.Length && array[i] == '"')
			{
				b = (sbyte)((b != -1) ? 1 : 0);
			}
			if (i != array.Length && (array[i] != ',' || b == 0))
			{
				continue;
			}
			b = -1;
			text2 = new string(array, num, i - num);
			text2 = TrimString(text2);
			if (key == null)
			{
				key = text2;
				KEY_BYTES_OFFSET += 2;
				KEY_BYTES_OFFSET += 1 * text2.Length;
			}
			try
			{
				switch (typeLookUp[num2].id)
				{
				case TypeID.TYPE_INT:
					obj = new MObject(int.Parse(text2));
					CELLS_BYTES_OFFSET += 4;
					break;
				case TypeID.TYPE_FLOAT:
					obj = new MObject(float.Parse(text2));
					CELLS_BYTES_OFFSET += 4;
					break;
				case TypeID.TYPE_STRING:
				{
					int length = text2.Length;
					if (length >= 3 && text2[0] == '"')
					{
						text2 = text2.Substring(1, length - 2);
					}
					obj = new MObject(text2);
					CELLS_BYTES_OFFSET += 2;
					CELLS_BYTES_OFFSET += 1 * length;
					break;
				}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("CSVReader failed to convert: " + text2 + ". string length: " + text2.Length);
				throw ex;
			}
			mArray.addObject(obj);
			num2++;
			num = i;
			num++;
		}
		return mArray;
	}

	private void ParseTypeLine()
	{
		string text = ReadLine();
		char[] array = text.ToCharArray();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == ',')
				{
					num3++;
				}
			}
			MAX_COLUMNS = num3 + 1;
		}
		typeLookUp = new CSVTypeInfo[MAX_COLUMNS];
		int num4 = 0;
		for (num2 = num; num2 <= array.Length; num2++)
		{
			if (num2 == array.Length || array[num2] == ',')
			{
				string data = new string(array, num, num2 - num);
				SetTypeData(data, num4);
				num4++;
				num = num2;
				num++;
			}
		}
	}

	private void SetTypeData(string data, int colNum)
	{
		char[] value = data.ToCharArray();
		int index = 0;
		int num = 0;
		if (data[index] == 'i')
		{
			typeLookUp[colNum].id = TypeID.TYPE_INT;
		}
		else if (data[index] == 'f')
		{
			typeLookUp[colNum].id = TypeID.TYPE_FLOAT;
		}
		else if (data[index] == 's')
		{
			typeLookUp[colNum].id = TypeID.TYPE_STRING;
		}
		else
		{
			typeLookUp[colNum].id = TypeID.TYPE_UNKNOWN;
		}
		index = 2;
		num = data.Length;
		typeLookUp[colNum].colName = new string(value, index, num - index);
	}

	private static string TrimString(string str)
	{
		str = str.Trim();
		while (str.Length > 0)
		{
			char c = str[0];
			if (c == '\0' || c >= ' ')
			{
				break;
			}
			str = str.Remove(0, 1);
		}
		while (str.Length > 0)
		{
			char c2 = str[str.Length - 1];
			if (c2 == '\0' || c2 >= ' ')
			{
				break;
			}
			str = str.Remove(str.Length - 1);
		}
		return str;
	}
}
