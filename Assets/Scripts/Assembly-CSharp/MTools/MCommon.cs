namespace MTools
{
	public class MCommon
	{
		public delegate void StandardDelegate();

		public delegate void StandardDelegate_Object(object o);

		public delegate void StandardDelegate_String(string s);

		public delegate void StandardDelegate_Key(string s, object o);

		public delegate bool StandardDelegate_Check(object o);

		private MCommon()
		{
		}

		public static string CreateFileHash(string filePath)
		{
			return null;
		}

		public static string CreateStringHash(string message)
		{
			return null;
		}

		public static bool ValidateFileHash(string filePath, string hash)
		{
			return false;
		}

		public static bool ValidateStringHash(string message, string existing_hash)
		{
			return false;
		}
	}
}
