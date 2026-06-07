using System.Collections.Generic;

public class SoaringSaveMigration : Migration
{
	public override bool MigrateGamestate(Dictionary<string, object> gamestate, StaticContentLoader contentLoader)
	{
		return false;
	}
}
