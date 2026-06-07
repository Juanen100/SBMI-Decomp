using System;

namespace Microsoft.AppCenter.Unity.Distribute
{
	public class ReleaseDetails
	{
		public int Id { get; internal set; }

		public string Version { get; internal set; }

		public string ShortVersion { get; internal set; }

		public string ReleaseNotes { get; internal set; }

		public Uri ReleaseNotesUrl { get; internal set; }

		public bool MandatoryUpdate { get; internal set; }

		internal ReleaseDetails()
		{
		}
	}
}
