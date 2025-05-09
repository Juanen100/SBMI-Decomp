public class TemptExpireCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.EXPIRE, sender, receiver);
	}
}
