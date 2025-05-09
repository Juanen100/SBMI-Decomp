public class AbortCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.ABORT, sender, receiver);
	}
}
