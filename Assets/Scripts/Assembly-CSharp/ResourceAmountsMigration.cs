using System.Collections.Generic;

public class ResourceAmountsMigration : Migration
{
	public override bool MigrateGamestate(Dictionary<string, object> gamestate, StaticContentLoader contentLoader)
	{
		return false;
	}
}
