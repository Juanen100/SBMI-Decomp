public class PerformTaskCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.PERFORM_TASK, sender, receiver);
	}
}
