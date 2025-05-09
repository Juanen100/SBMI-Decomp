using UnityEngine;

public class MoveCommand
{
	public static Command Create(Identity sender, Identity receiver, Vector2 position, bool flip)
	{
		Command command = new Command(Command.TYPE.MOVE, sender, receiver);
		command["position"] = position;
		command["flip"] = flip;
		return command;
	}
}
