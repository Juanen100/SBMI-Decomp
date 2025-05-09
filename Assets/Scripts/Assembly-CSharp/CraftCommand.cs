public class CraftCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.CRAFT, sender, receiver);
	}
}
