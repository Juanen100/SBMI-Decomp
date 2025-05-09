public class EmployCommand
{
	public static Command Create(Identity sender, Identity receiver, Identity employee)
	{
		Command command = new Command(Command.TYPE.EMPLOY, sender, receiver);
		command["employee"] = employee;
		return command;
	}
}
