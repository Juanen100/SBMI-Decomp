public class HUBCraftCommand
{
	public static Command Create(Identity sender, Identity receiver, bool start)
	{
		Command command = new Command(Command.TYPE.HUBCRAFT, sender, receiver);
		command["start"] = start;
		return command;
	}
}
