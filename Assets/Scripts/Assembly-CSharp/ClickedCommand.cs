public class ClickedCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.CLICKED, sender, receiver);
	}
}
