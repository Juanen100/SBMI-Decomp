public class EnterCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.ENTER, sender, receiver);
	}
}
