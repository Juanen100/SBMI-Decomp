using System.Collections.Generic;

namespace DeltaDNA
{
	internal class EngageRequest
	{
		public string DecisionPoint { get; private set; }

		public string Flavour { get; set; }

		public Dictionary<string, object> Parameters { get; set; }

		public EngageRequest(string decisionPoint)
		{
		}

		public string ToJSON()
		{
			return null;
		}

		public override string ToString()
		{
			return null;
		}
	}
}
