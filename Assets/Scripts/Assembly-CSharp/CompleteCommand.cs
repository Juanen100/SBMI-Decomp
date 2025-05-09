public class CompleteCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.COMPLETE, sender, receiver);
	}
}
