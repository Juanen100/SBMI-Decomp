public class ExpireCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.EXPIRE, sender, receiver);
	}
}
