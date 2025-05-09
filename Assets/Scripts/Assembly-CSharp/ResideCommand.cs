public class ResideCommand
{
	public static Command Create(Identity sender, Identity receiver, Identity residence)
	{
		Command command = new Command(Command.TYPE.RESIDE, sender, receiver);
		command["residence"] = residence;
		return command;
	}
}
