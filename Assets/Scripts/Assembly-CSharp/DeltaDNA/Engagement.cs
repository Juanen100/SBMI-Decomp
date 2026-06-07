using System.Collections.Generic;

namespace DeltaDNA
{
	public class Engagement<T> where T : Engagement<T>
	{
		private readonly Params parameters;

		private string response;

		public string DecisionPoint { get; private set; }

		public string Flavour { get; internal set; }

		public string Raw
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public int StatusCode { get; set; }

		public string Error { get; set; }

		public Dictionary<string, object> JSON { get; internal set; }

		public Engagement(string decisionPoint)
		{
		}

		internal Engagement(string decisionPoint, Params parameters)
		{
		}

		public T AddParam(string key, object value)
		{
			return null;
		}

		public Dictionary<string, object> AsDictionary()
		{
			return null;
		}

		internal string GetDecisionPointAndFlavour()
		{
			return null;
		}

		public override string ToString()
		{
			return null;
		}
	}
	public class Engagement : Engagement<Engagement>
	{
		public Engagement(string decisionPoint)
			: base((string)null)
		{
		}

		internal Engagement(string decisionPoint, Params parameters)
			: base((string)null)
		{
		}
	}
}
