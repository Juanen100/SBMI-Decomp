using System;
using UnityEngine;

public class AmazonJavaWrapper : IDisposable
{
	private AndroidJavaObject jo;

	public AmazonJavaWrapper()
	{
	}

	public AmazonJavaWrapper(AndroidJavaObject o)
	{
		setAndroidJavaObject(o);
	}

	public AndroidJavaObject getJavaObject()
	{
		return jo;
	}

	public void setAndroidJavaObject(AndroidJavaObject o)
	{
		jo = o;
	}

	public IntPtr GetRawObject()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return jo.GetRawObject();
		}
		return default(IntPtr);
	}

	public IntPtr GetRawClass()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return jo.GetRawClass();
		}
		return default(IntPtr);
	}

	public void Set<FieldType>(string fieldName, FieldType type)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			jo.Set(fieldName, type);
		}
	}

	public FieldType Get<FieldType>(string fieldName)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return jo.Get<FieldType>(fieldName);
		}
		return default(FieldType);
	}

	public void SetStatic<FieldType>(string fieldName, FieldType type)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			jo.SetStatic(fieldName, type);
		}
	}

	public FieldType GetStatic<FieldType>(string fieldName)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return jo.GetStatic<FieldType>(fieldName);
		}
		return default(FieldType);
	}

	public void CallStatic(string method, params object[] args)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJNI.PushLocalFrame(args.Length + 1);
			jo.CallStatic(method, args);
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
	}

	public void Call(string method, params object[] args)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJNI.PushLocalFrame(args.Length + 1);
			jo.Call(method, args);
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
	}

	public ReturnType CallStatic<ReturnType>(string method, params object[] args)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJNI.PushLocalFrame(args.Length + 1);
			ReturnType result = jo.CallStatic<ReturnType>(method, args);
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return result;
		}
		return default(ReturnType);
	}

	public ReturnType Call<ReturnType>(string method, params object[] args)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJNI.PushLocalFrame(args.Length + 1);
			ReturnType result = jo.Call<ReturnType>(method, args);
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return result;
		}
		return default(ReturnType);
	}

	public void Dispose()
	{
		if (jo != null)
		{
			jo.Dispose();
		}
	}
}
