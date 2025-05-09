public class BonusRewardCommand
{
	public static Command Create(Identity sender, Identity receiver, int sourceID)
	{
		Command command = new Command(Command.TYPE.BONUS_REWARD, sender, receiver);
		command["source_id"] = sourceID;
		return command;
	}
}
