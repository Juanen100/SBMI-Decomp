public class ErectCommand
{
	public static Command Create(Identity sender, Identity receiver, Identity building, ulong timeBuild)
	{
		Command command = new Command(Command.TYPE.ERECT, sender, receiver);
		command["building"] = building;
		command["time.build"] = timeBuild;
		return command;
	}
}
