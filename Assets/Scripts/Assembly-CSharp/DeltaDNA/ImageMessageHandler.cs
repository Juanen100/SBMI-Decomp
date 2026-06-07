using System;

namespace DeltaDNA
{
	public class ImageMessageHandler : EventActionHandler
	{
		private readonly DDNA ddna;

		private readonly Action<ImageMessage> callback;

		public ImageMessageHandler(DDNA ddna, Action<ImageMessage> callback)
		{
		}

		internal override bool Handle(EventTrigger trigger, ActionStore store)
		{
			return false;
		}

		internal override string Type()
		{
			return null;
		}
	}
}
