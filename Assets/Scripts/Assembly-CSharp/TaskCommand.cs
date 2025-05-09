public class TaskCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.TASK, sender, receiver);
	}
}
