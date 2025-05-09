public class PurchaseCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.PURCHASE, sender, receiver);
	}
}
