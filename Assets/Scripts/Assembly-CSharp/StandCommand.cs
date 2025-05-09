public class StandCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.STAND, sender, receiver);
	}
}
