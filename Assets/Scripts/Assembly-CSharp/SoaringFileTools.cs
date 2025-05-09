using System.IO;

public class SoaringFileTools
{
	private class SoaringFileData
	{
		private Stream stream;

		public virtual Stream Stream()
		{
			return stream;
		}

		public virtual SoaringObjectBase DataChunk()
		{
			return null;
		}

		public virtual bool IsDone()
		{
			return false;
		}
	}

	public static bool WriteJsonToFile(string path, SoaringDictionary data)
	{
		bool result = false;
		if (data == null || string.IsNullOrEmpty(path))
		{
			return result;
		}
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		string directoryName = Path.GetDirectoryName(path);
		SoaringDebug.Log(directoryName);
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		StreamWriter streamWriter = new StreamWriter(path);
		if (streamWriter == null)
		{
			return result;
		}
		SoaringFileTools soaringFileTools = new SoaringFileTools();
		soaringFileTools.WriteDictionary(data, streamWriter);
		result = true;
		soaringFileTools = null;
		streamWriter.Flush();
		streamWriter.Close();
		streamWriter = null;
		return result;
	}

	private void WriteDictionary(SoaringDictionary data, StreamWriter writer)
	{
		if (data == null)
		{
			return;
		}
		string[] array = data.allKeys();
		SoaringObjectBase[] array2 = data.allValues();
		int num = data.count();
		_WriteRawString("{\n", writer);
		for (int i = 0; i < num; i++)
		{
			if (i != 0)
			{
				_WriteRawString(",\n", writer);
			}
			_WriteRawString("\"" + array[i] + "\" : ", writer);
			switch (array2[i].Type)
			{
			case SoaringObjectBase.IsType.Dictionary:
				WriteDictionary((SoaringDictionary)array2[i], writer);
				break;
			case SoaringObjectBase.IsType.Array:
				WriteArray((SoaringArray)array2[i], writer);
				break;
			case SoaringObjectBase.IsType.Object:
				_WriteRawString(string.Concat("\"Error:", array2[i].Type, "\""), writer);
				break;
			default:
				WriteValue((SoaringValue)array2[i], writer);
				break;
			}
		}
		_WriteRawString("\n}", writer);
	}

	private void WriteArray(SoaringArray data, StreamWriter writer)
	{
		if (data == null)
		{
			return;
		}
		SoaringObjectBase[] array = data.array();
		int num = data.count();
		_WriteRawString("{\n", writer);
		for (int i = 0; i < num; i++)
		{
			if (i != 0)
			{
				_WriteRawString(",\n", writer);
			}
			switch (array[i].Type)
			{
			case SoaringObjectBase.IsType.Dictionary:
				WriteDictionary((SoaringDictionary)array[i], writer);
				break;
			case SoaringObjectBase.IsType.Array:
				WriteArray((SoaringArray)array[i], writer);
				break;
			case SoaringObjectBase.IsType.Object:
				_WriteRawString(string.Concat("\"Error:", array[i].Type, "\""), writer);
				break;
			default:
				WriteValue((SoaringValue)array[i], writer);
				break;
			}
		}
		_WriteRawString("\n}", writer);
	}

	private void WriteValue(SoaringValue data, StreamWriter writer)
	{
		if (data != null)
		{
			_WriteRawString(data.ToJsonString(), writer);
		}
	}

	private void _WriteRawString(string str, StreamWriter writer)
	{
		int length = str.Length;
		for (int i = 0; i < length; i++)
		{
			writer.Write(str[i]);
		}
	}
}
