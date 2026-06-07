using System;

namespace DeltaDNA
{
	[Serializable]
	public sealed class Configuration
	{
		public string environmentKeyDev;

		public string environmentKeyLive;

		public int environmentKey;

		public string collectUrl;

		public string engageUrl;

		public string hashSecret;

		public string clientVersion;

		public bool useApplicationVersion;
	}
}
