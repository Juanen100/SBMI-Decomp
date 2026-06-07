using System.IO;
using MTools;

public class ResourceUtils
{
	public const byte FileSys_Persistant = 1;

	public const byte FileSys_Resources = 2;

	public const byte FileSys_Streamed = 4;

	public const byte FileSys_Editor = 8;

	public const byte FileSys_Invalid = 128;

	public const byte FileSys_All = byte.MaxValue;

	public static byte DefaultReadFileOptions;

	public static byte DefaultWriteFileOptions;

	public static void CopyMeta(string filePathNew, string filePathOld, bool deleteOldMeta)
	{
	}

	public static string CropOffsetPath(string offset)
	{
		return null;
	}

	private static bool _CheckKey(byte key, byte check_option)
	{
		return false;
	}

	private static string _PersistantPath()
	{
		return null;
	}

	private static string _PersistantPathEditor()
	{
		return null;
	}

	public static string GetFilePath(string fileName, string offsetPath = null, bool checkValidPath = false)
	{
		return null;
	}

	public static string GetFilePath(string fileName, string offsetPath, byte fileOptions, bool checkValidPath)
	{
		return null;
	}

	public static string GetFilePath(string fileName, string offsetPath, byte fileOptions, bool checkValidPath, ref byte return_file_type)
	{
		return null;
	}

	public static string GetWritePath(string fileName, string offsetPath, byte option = 1)
	{
		return null;
	}

	public static MBinaryReader GetFileStream(string filename)
	{
		return null;
	}

	public static MBinaryReader GetFileStream(string filename, string directory, string ext)
	{
		return null;
	}

	public static MBinaryReader GetFileStream(string filename, string directory, string ext, byte options)
	{
		return null;
	}

	public static Stream GetRawFileStream(string filename)
	{
		return null;
	}

	public static Stream GetRawFileStream(string filename, string directory, string ext)
	{
		return null;
	}

	public static Stream GetRawFileStream(string filename, string directory, string ext, byte options)
	{
		return null;
	}

	public static byte[] GetVersionedFileBytes(string filename)
	{
		return null;
	}

	public static byte[] GetVersionedFileBytes(string filename, string ext)
	{
		return null;
	}

	public static byte[] GetVersionedFileBytes(string filename, string directory, string ext)
	{
		return null;
	}

	public static byte[] GetFileBytes(string filename, string directory, string ext, byte file_type)
	{
		return null;
	}

	public static string FileNameWithoutExtension(string fileExt)
	{
		return null;
	}

	public static string FileNameWithoutPath(string fileExt)
	{
		return null;
	}

	public static bool FileExists(string path)
	{
		return false;
	}
}
