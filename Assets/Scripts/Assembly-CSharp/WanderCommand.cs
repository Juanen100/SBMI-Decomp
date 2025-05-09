public class WanderCommand
{
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.WANDER, sender, receiver);
	}
}
