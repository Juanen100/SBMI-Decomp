public class TemptCommand
{
	public static Command Create(Identity sender, Identity receiver, int? productId)
	{
		Command command = new Command(Command.TYPE.TEMPT, sender, receiver);
		command["product_id"] = productId;
		return command;
	}
}
