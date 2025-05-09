using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class SoaringPlatformWindows : SoaringPlatform.SoaringPlatformDelegate
{
	public override void Init()
	{
	}

	public override SoaringLoginType PreferedLoginType()
	{
		return SoaringLoginType.Soaring;
	}

	public override string PlatformName()
	{
		return "Windows";
	}

	public override string DeviceID()
	{
		return Environment.MachineName;
	}

	public override SoaringDictionary GenerateDeviceDictionary()
	{
		return new SoaringDictionary();
	}

	public override bool OpenURL(string url)
	{
		bool result = false;
		if (url == null)
		{
			return result;
		}
		Application.OpenURL(url);
		return true;
	}

	public override bool SendEmail(string subject, string body, string email)
	{
		bool result = false;
		if (subject == null || body == null || email == null)
		{
			return result;
		}
		subject = WWW.EscapeURL(subject).Replace("+", "%20");
		body = WWW.EscapeURL(body).Replace("+", "%20");
		Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
		return true;
	}

	public override bool OpenPath(string path)
	{
		bool result = false;
		if (path == null)
		{
			return result;
		}
		try
		{
			if (File.Exists(path))
			{
				Process.Start("open", path);
			}
			result = true;
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message, LogType.Error);
		}
		return result;
	}
}
