public class IdlePauseCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.IDLE_PAUSE, sender, receiver);
	}
}
