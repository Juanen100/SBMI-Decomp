public class HungerCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.HUNGER, sender, receiver);
	}
}
