using UnityEngine;

public class GoHomeCommand
{
	public static Command Create(Identity sender, Identity receiver, Vector2 position)
	{
		Command command = new Command(Command.TYPE.GO_HOME, sender, receiver);
		command["home_position"] = position;
		return command;
	}
}
