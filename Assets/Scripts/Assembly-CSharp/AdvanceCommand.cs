public class AdvanceCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.ADVANCE, sender, receiver);
	}
}
