public class FleeCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.FLEE, sender, receiver);
	}
}
