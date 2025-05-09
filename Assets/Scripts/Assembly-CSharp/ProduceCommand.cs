public class ProduceCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.PRODUCE, sender, receiver);
	}
}
