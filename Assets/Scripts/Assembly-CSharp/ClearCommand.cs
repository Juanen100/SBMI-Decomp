public class ClearCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.CLEAR, sender, receiver);
	}
}
