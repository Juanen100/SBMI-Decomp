public class BonusCommand
{
	public static Command Create(Identity senderReceiver)
	{
		return new Command(Command.TYPE.BONUS, senderReceiver, senderReceiver);
	}
}
