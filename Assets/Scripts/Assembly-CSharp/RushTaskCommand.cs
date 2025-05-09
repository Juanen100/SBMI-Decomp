public class RushTaskCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.RUSH_TASK, sender, receiver);
	}
}
