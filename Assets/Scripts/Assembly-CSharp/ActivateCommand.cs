public class ActivateCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.ACTIVATE, sender, receiver);
	}
}
