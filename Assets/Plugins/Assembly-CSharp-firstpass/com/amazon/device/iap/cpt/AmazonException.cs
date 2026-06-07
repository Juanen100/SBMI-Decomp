using System;
using System.Runtime.Serialization;

namespace com.amazon.device.iap.cpt
{
	public class AmazonException : ApplicationException
	{
		public AmazonException()
		{
		}

		public AmazonException(string message)
		{
		}

		public AmazonException(string message, Exception inner)
		{
		}

		protected AmazonException(SerializationInfo info, StreamingContext context)
		{
		}
	}
}
