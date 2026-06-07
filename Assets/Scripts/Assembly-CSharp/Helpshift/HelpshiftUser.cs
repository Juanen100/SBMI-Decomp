namespace Helpshift
{
	public class HelpshiftUser
	{
		public sealed class Builder
		{
			private string identifier;

			private string email;

			private string name;

			private string authToken;

			public Builder(string identifier, string email)
			{
			}

			public Builder setName(string name)
			{
				return null;
			}

			public Builder setAuthToken(string authToken)
			{
				return null;
			}

			public HelpshiftUser build()
			{
				return null;
			}
		}

		public readonly string identifier;

		public readonly string email;

		public readonly string name;

		public readonly string authToken;

		private HelpshiftUser(string identifier, string email, string name, string authToken)
		{
		}
	}
}
