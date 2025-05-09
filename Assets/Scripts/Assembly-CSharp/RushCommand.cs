public class RushCommand
{
	public static Command Create(Identity target)
	{
		return new Command(Command.TYPE.RUSH, target, target);
	}

	public static Command Create(Identity target, int slotId)
	{
		Command command = new Command(Command.TYPE.RUSH, target, target);
		command["slot_id"] = slotId;
		return command;
	}
}
