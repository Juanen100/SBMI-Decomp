using System.IO;

public class SoaringFileTools
{
	private class SoaringFileData
	{
		private Stream stream;

		public virtual Stream Stream()
		{
			return null;
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
		return false;
	}

	private void WriteDictionary(SoaringDictionary data, StreamWriter writer)
	{
	}

	private void WriteArray(SoaringArray data, StreamWriter writer)
	{
	}

	private void WriteValue(SoaringValue data, StreamWriter writer)
	{
	}

	private void _WriteRawString(string str, StreamWriter writer)
	{
	}
}
