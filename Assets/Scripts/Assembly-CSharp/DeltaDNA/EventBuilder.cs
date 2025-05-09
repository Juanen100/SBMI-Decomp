using System.Collections.Generic;

namespace DeltaDNA
{
	public class EventBuilder
	{
		private Dictionary<string, object> dict = new Dictionary<string, object>();

		public EventBuilder AddParam(string key, object value)
		{
			if (value == null)
			{
				return this;
			}
			if (value.GetType() == typeof(ProductBuilder))
			{
				ProductBuilder productBuilder = value as ProductBuilder;
				value = productBuilder.ToDictionary();
			}
			else if (value.GetType() == typeof(EventBuilder))
			{
				EventBuilder eventBuilder = value as EventBuilder;
				value = eventBuilder.ToDictionary();
			}
			dict.Add(key, value);
			return this;
		}

		public Dictionary<string, object> ToDictionary()
		{
			return dict;
		}
	}
}
