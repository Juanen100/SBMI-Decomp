public class ResumeWishingCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.RESUME_WISHING, sender, receiver);
	}
}
