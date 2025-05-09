public class CheerCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.CHEER, sender, receiver);
	}
}
