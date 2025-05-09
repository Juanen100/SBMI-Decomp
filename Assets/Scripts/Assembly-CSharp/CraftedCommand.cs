public class CraftedCommand
{
	public static Command Create(Identity sender, Identity receiver, int slotId)
	{
		Command command = new Command(Command.TYPE.CRAFTED, sender, receiver);
		command["slot_id"] = slotId;
		return command;
	}
}
