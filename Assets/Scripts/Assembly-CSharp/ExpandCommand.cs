public class ExpandCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.EXPAND, sender, receiver);
	}
}
