public class WishCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.WISH, sender, receiver);
	}
}
