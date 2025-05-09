public class DelegateClickCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.DELEGATE_CLICK, sender, receiver);
	}
}
