public class StoreResidentCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.STORE_RESIDENT, sender, receiver);
	}
}
