public class WaitAsTaskTargetCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.WAIT_AS_TASK_TARGET, sender, receiver);
	}
}
