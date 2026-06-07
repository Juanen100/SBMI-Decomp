namespace Microsoft.AppCenter.Unity.Crashes
{
	public class ErrorAttachmentLog
	{
		public enum AttachmentType
		{
			Text = 0,
			Binary = 1
		}

		public string Text { get; private set; }

		public byte[] Data { get; private set; }

		public string FileName { get; private set; }

		public string ContentType { get; private set; }

		public AttachmentType Type { get; private set; }

		public static ErrorAttachmentLog AttachmentWithText(string text, string fileName)
		{
			return null;
		}

		public static ErrorAttachmentLog AttachmentWithBinary(byte[] data, string fileName, string contentType)
		{
			return null;
		}
	}
}
