public class ReturnCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.RETURN, sender, receiver);
	}
}
