using System;
using System.IO;
using MTools;
using UnityEngine;

public class ResourceUtils
{
	public const byte FileSys_Persistant = 1;

	public const byte FileSys_Resources = 2;

	public const byte FileSys_Streamed = 4;

	public const byte FileSys_Editor = 8;

	public const byte FileSys_Invalid = 128;

	public const byte FileSys_All = byte.MaxValue;

	public static byte DefaultReadFileOptions = 7;

	public static byte DefaultWriteFileOptions = 1;

	public static void CopyMeta(string filePathNew, string filePathOld, bool deleteOldMeta)
	{
		string text = filePathNew + ".meta";
		string text2 = filePathOld + ".meta";
		if (File.Exists(text))
		{
			File.Delete(text);
		}
		if (File.Exists(text2))
		{
			File.Copy(text2, text);
			if (File.Exists(text2) && deleteOldMeta)
			{
				File.Delete(text2);
			}
		}
	}

	public static string CropOffsetPath(string offset)
	{
		if (string.IsNullOrEmpty(offset))
		{
			return offset;
		}
		int length = offset.Length;
		int num = 0;
		int num2 = length;
		if (offset[0] == '/')
		{
			num = 1;
		}
		if (offset[length - 1] == '/')
		{
			num2 = length - 1;
		}
		if (num >= num2)
		{
			return string.Empty;
		}
		if (num == 0 && num2 == length - 1)
		{
			return offset;
		}
		return offset.Substring(num, num2 - num);
	}

	private static bool _CheckKey(byte key, byte check_option)
	{
		return (key & check_option) == check_option;
	}

	private static string _PersistantPath()
	{
		return Application.persistentDataPath + "/";
	}

	private static string _PersistantPathEditor()
	{
		return Application.dataPath + "/";
	}

	public static string GetFilePath(string fileName, string offsetPath = null, bool checkValidPath = false)
	{
		byte return_file_type = 128;
		return GetFilePath(fileName, offsetPath, DefaultReadFileOptions, checkValidPath, ref return_file_type);
	}

	public static string GetFilePath(string fileName, string offsetPath, byte fileOptions, bool checkValidPath)
	{
		byte return_file_type = 128;
		return GetFilePath(fileName, offsetPath, fileOptions, checkValidPath, ref return_file_type);
	}

	public static string GetFilePath(string fileName, string offsetPath, byte fileOptions, bool checkValidPath, ref byte return_file_type)
	{
		string text = null;
		string text2 = fileName;
		if (!string.IsNullOrEmpty(offsetPath))
		{
			offsetPath = CropOffsetPath(offsetPath);
			if (!string.IsNullOrEmpty(offsetPath))
			{
				text2 = offsetPath + "/" + fileName;
			}
		}
		if (_CheckKey(fileOptions, 1))
		{
			text = _PersistantPath() + text2;
			if (!checkValidPath)
			{
				TFUtils.DebugLog("ResourceUtils.Persistant: " + text + " Name: " + fileName);
				return_file_type = 1;
				return text;
			}
			if (FileExists(text))
			{
				TFUtils.DebugLog("ResourceUtils.Persistant: " + text + " Name: " + fileName);
				return_file_type = 1;
				return text;
			}
		}
		if (_CheckKey(fileOptions, 4))
		{
			string text3 = string.Empty;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				text = Application.dataPath + "/Raw/" + text2;
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				text3 = "jar:file://";
				checkValidPath = false;
				text = Application.dataPath + "!/assets/" + text2;
			}
			else
			{
				text = Application.dataPath + "/StreamingAssets/" + text2;
			}
			if (!checkValidPath)
			{
				TFUtils.DebugLog("ResourceUtils.Streamed: " + text + " Name: " + fileName);
				return_file_type = 4;
				return text3 + text;
			}
			if (FileExists(text))
			{
				TFUtils.DebugLog("ResourceUtils.Streamed: " + text + " Name: " + fileName);
				return_file_type = 4;
				return text3 + text;
			}
		}
		if (_CheckKey(fileOptions, 2))
		{
			text = FileNameWithoutExtension(text2);
			TFUtils.DebugLog("ResourceUtils.Resources: " + text + " Name: " + fileName);
			return_file_type = 2;
			return text;
		}
		return null;
	}

	public static string GetWritePath(string fileName, string offsetPath, byte option = 1)
	{
		offsetPath = CropOffsetPath(offsetPath);
		if (offsetPath != null)
		{
			fileName = offsetPath + "/" + fileName;
		}
		return _PersistantPath() + fileName;
	}

	public static MBinaryReader GetFileStream(string filename)
	{
		return GetFileStream(filename, null, null, DefaultReadFileOptions);
	}

	public static MBinaryReader GetFileStream(string filename, string directory, string ext)
	{
		return GetFileStream(filename, directory, ext, DefaultReadFileOptions);
	}

	public static MBinaryReader GetFileStream(string filename, string directory, string ext, byte options)
	{
		MBinaryReader mBinaryReader = null;
		string text = null;
		if (filename == null)
		{
			return mBinaryReader;
		}
		string text2 = filename;
		if (ext != null)
		{
			text2 = filename + "." + ext;
		}
		byte return_file_type = 128;
		text = GetFilePath(text2, directory, options, true, ref return_file_type);
		TFUtils.DebugLog("ResourceUtils.Path: " + text + " Name: " + text2);
		if (text != null)
		{
			if (_CheckKey(return_file_type, 4))
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					try
					{
						WWW wWW = new WWW(text);
						while (!wWW.isDone)
						{
							if (wWW.error != null)
							{
								TFUtils.DebugLog("ResourceUtils.Error: " + wWW.error);
								return null;
							}
						}
						mBinaryReader = new MBinaryReader(wWW.bytes);
					}
					catch (Exception ex)
					{
						TFUtils.DebugLog("ResourceUtils.Exception: " + ex.Message);
						return null;
					}
				}
				else
				{
					mBinaryReader = new MBinaryReader(text);
				}
			}
			else if (_CheckKey(return_file_type, 2))
			{
				TextAsset textAsset = (TextAsset)Resources.Load(text);
				TFUtils.DebugLog("ResourceUtils.Loading");
				if (textAsset != null)
				{
					mBinaryReader = new MBinaryReader(textAsset.bytes);
					Resources.UnloadAsset(textAsset);
				}
				else
				{
					TFUtils.DebugLog("ResourceUtils.Failed");
				}
			}
			else
			{
				mBinaryReader = new MBinaryReader(text);
			}
		}
		if (mBinaryReader == null || !mBinaryReader.IsOpen())
		{
			return null;
		}
		return mBinaryReader;
	}

	public static Stream GetRawFileStream(string filename)
	{
		return GetRawFileStream(filename, null, null, DefaultReadFileOptions);
	}

	public static Stream GetRawFileStream(string filename, string directory, string ext)
	{
		return GetRawFileStream(filename, directory, ext, DefaultReadFileOptions);
	}

	public static Stream GetRawFileStream(string filename, string directory, string ext, byte options)
	{
		Stream stream = null;
		string text = null;
		if (filename == null)
		{
			return stream;
		}
		string text2 = filename;
		if (ext != null)
		{
			text2 = filename + "." + ext;
		}
		byte return_file_type = 128;
		text = GetFilePath(text2, directory, options, true, ref return_file_type);
		TFUtils.DebugLog("ResourceUtils.Path: " + text + " Name: " + text2);
		if (text != null)
		{
			if (_CheckKey(return_file_type, 4))
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					try
					{
						WWW wWW = new WWW(text);
						while (!wWW.isDone)
						{
							if (wWW.error != null)
							{
								TFUtils.DebugLog("ResourceUtils.Error: " + wWW.error);
								return null;
							}
						}
						stream = new MemoryStream(wWW.bytes);
					}
					catch (Exception ex)
					{
						TFUtils.DebugLog("ResourceUtils.Exception: " + ex.Message);
						return null;
					}
				}
				else if (File.Exists(text))
				{
					stream = File.OpenRead(text);
				}
			}
			else if (_CheckKey(return_file_type, 2))
			{
				TextAsset textAsset = (TextAsset)Resources.Load(text);
				TFUtils.DebugLog("ResourceUtils.Loading");
				if (textAsset != null)
				{
					stream = new MemoryStream(textAsset.bytes);
					Resources.UnloadAsset(textAsset);
				}
				else
				{
					TFUtils.DebugLog("ResourceUtils.Failed");
				}
			}
			else if (File.Exists(text))
			{
				stream = File.OpenRead(text);
			}
		}
		if (stream == null || stream.Length == 0L)
		{
			return null;
		}
		return stream;
	}

	public static byte[] GetVersionedFileBytes(string filename)
	{
		return GetFileBytes(filename, null, null, DefaultReadFileOptions);
	}

	public static byte[] GetVersionedFileBytes(string filename, string ext)
	{
		return GetFileBytes(filename, null, ext, DefaultReadFileOptions);
	}

	public static byte[] GetVersionedFileBytes(string filename, string directory, string ext)
	{
		return GetFileBytes(filename, directory, ext, DefaultReadFileOptions);
	}

	public static byte[] GetFileBytes(string filename, string directory, string ext, byte file_type)
	{
		byte[] result = null;
		string text = null;
		if (filename == null)
		{
			return result;
		}
		string text2 = filename;
		if (ext != null)
		{
			text2 = filename + "." + ext;
		}
		byte return_file_type = 128;
		text = GetFilePath(text2, directory, file_type, true, ref return_file_type);
		Debug.Log("ResourceUtils.Path: " + text + " Name: " + text2);
		if (text != null)
		{
			if (_CheckKey(return_file_type, 4))
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					try
					{
						WWW wWW = new WWW(text);
						while (!wWW.isDone)
						{
							if (wWW.error != null)
							{
								Debug.Log("ResourceUtils.Error: " + wWW.error);
								return null;
							}
						}
						Debug.Log(wWW.bytes.Length);
						MBinaryReader mBinaryReader = new MBinaryReader(wWW.bytes);
						result = mBinaryReader.ReadAllBytes();
						mBinaryReader.Close();
						mBinaryReader = null;
					}
					catch (Exception ex)
					{
						Debug.Log("ResourceUtils.Exception: " + ex.Message);
						return null;
					}
				}
				else
				{
					MBinaryReader mBinaryReader2 = new MBinaryReader(text);
					result = mBinaryReader2.ReadAllBytes();
					mBinaryReader2.Close();
					mBinaryReader2 = null;
				}
			}
			else if (_CheckKey(return_file_type, 2))
			{
				TextAsset textAsset = (TextAsset)Resources.Load(text);
				if (textAsset != null)
				{
					result = textAsset.bytes;
					Resources.UnloadAsset(textAsset);
				}
			}
			else
			{
				MBinaryReader mBinaryReader3 = new MBinaryReader(text);
				result = mBinaryReader3.ReadAllBytes();
				mBinaryReader3.Close();
				mBinaryReader3 = null;
			}
		}
		return result;
	}

	public static string FileNameWithoutExtension(string fileExt)
	{
		int num = fileExt.LastIndexOf('.');
		if (num == -1)
		{
			return fileExt;
		}
		return fileExt.Substring(0, num);
	}

	public static string FileNameWithoutPath(string fileExt)
	{
		int num = fileExt.LastIndexOf('/');
		if (num == -1)
		{
			return fileExt;
		}
		return fileExt.Substring(num + 1, fileExt.Length - (num + 1));
	}

	public static bool FileExists(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return false;
		}
		FileInfo fileInfo = new FileInfo(path);
		if (fileInfo == null)
		{
			return false;
		}
		return fileInfo.Exists;
	}
}
