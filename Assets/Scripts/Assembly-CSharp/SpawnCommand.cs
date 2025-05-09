public class SpawnCommand
{
	public static Command Create(Identity sender, Identity receiver, string blueprint)
	{
		Command command = new Command(Command.TYPE.SPAWN, sender, receiver);
		command["blueprint"] = blueprint;
		return command;
	}
}
