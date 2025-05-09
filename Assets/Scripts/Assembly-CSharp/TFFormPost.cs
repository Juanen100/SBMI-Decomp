using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

public static class TFFormPost
{
	private static readonly Encoding encoding = Encoding.UTF8;

	public static HttpWebResponse PostForm(Uri postUri, string userAgent, Dictionary<string, object> postParameters, CookieContainer cookies)
	{
		string text = string.Format("----------{0:N}", Guid.NewGuid());
		string contentType = "multipart/form-data; boundary=" + text;
		byte[] formData = GetFormData(postParameters, text);
		HttpWebRequest httpWebRequest = WebRequest.Create(postUri) as HttpWebRequest;
		if (httpWebRequest == null)
		{
			throw new NullReferenceException("Request is not a valid http request.");
		}
		httpWebRequest.Method = "POST";
		httpWebRequest.ContentType = contentType;
		httpWebRequest.UserAgent = userAgent;
		httpWebRequest.CookieContainer = cookies;
		httpWebRequest.ContentLength = formData.Length;
		using (Stream stream = httpWebRequest.GetRequestStream())
		{
			stream.Write(formData, 0, formData.Length);
			stream.Close();
		}
		return httpWebRequest.GetResponse() as HttpWebResponse;
	}

	private static byte[] GetFormData(Dictionary<string, object> postParameters, string boundary)
	{
		Stream stream = new MemoryStream();
		bool flag = false;
		foreach (KeyValuePair<string, object> postParameter in postParameters)
		{
			if (flag)
			{
				stream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));
			}
			flag = true;
			if (postParameter.Value.GetType() == typeof(byte[]))
			{
				byte[] array = (byte[])postParameter.Value;
				string s = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n", boundary, postParameter.Key, postParameter.Key, "application/octet-stream");
				stream.Write(encoding.GetBytes(s), 0, encoding.GetByteCount(s));
				stream.Write(array, 0, array.Length);
			}
			else
			{
				string s2 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}", boundary, postParameter.Key, postParameter.Value);
				stream.Write(encoding.GetBytes(s2), 0, encoding.GetByteCount(s2));
			}
		}
		string s3 = "\r\n--" + boundary + "--\r\n";
		stream.Write(encoding.GetBytes(s3), 0, encoding.GetByteCount(s3));
		stream.Position = 0L;
		byte[] array2 = new byte[stream.Length];
		stream.Read(array2, 0, array2.Length);
		stream.Close();
		return array2;
	}
}
