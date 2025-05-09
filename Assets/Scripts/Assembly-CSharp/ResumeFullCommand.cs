public class ResumeFullCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.RESUME_FULL, sender, receiver);
	}
}
