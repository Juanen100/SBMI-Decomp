using System;
using System.Collections.Generic;

public class CommandRouter
{
	public const bool DEBUG_LOG_COMMANDS = false;

	private Dictionary<Identity, Simulated> simulated;

	private List<Command> commands;

	public CommandRouter()
	{
		simulated = new Dictionary<Identity, Simulated>(new Identity.Equality());
		commands = new List<Command>();
	}

	public void Register(Simulated entity)
	{
		simulated.Add(entity.Id, entity);
	}

	public void Unregister(Simulated entity)
	{
		simulated.Remove(entity.Id);
		Predicate<Command> match = (Command command) => command.Receiver.Equals(entity.Id);
		commands.RemoveAll(match);
	}

	public void Send(Command command)
	{
		commands.Add(command);
	}

	public void Send(Command command, Action onComplete)
	{
		command.OnComplete = onComplete;
		Send(command);
	}

	public void Send(Command command, ulong delay)
	{
		command.TimeEpoch = TFUtils.EpochTime() + delay;
		Send(command);
	}

	public int CancelMatching(Command.TYPE type, Identity sender, Identity receiver, Dictionary<string, object> matching = null)
	{
		return commands.RemoveAll((Command element) => element.Type == type && element.Sender.Equals(sender) && element.Receiver.Equals(receiver) && (matching == null || element.Match(matching)));
	}

	public void Route()
	{
		commands.RemoveAll(RouteCommand);
	}

	private bool RouteCommand(Command command)
	{
		ulong num = TFUtils.EpochTime();
		if (num >= command.TimeEpoch)
		{
			Simulated value;
			if (simulated.TryGetValue(command.Receiver, out value))
			{
				value.Push(command);
			}
			else
			{
				TFUtils.WarningLog(string.Format("Dropped command({0}). Could not find receiver({1}).", command.Describe(), command.Receiver));
			}
			return true;
		}
		return false;
	}
}
