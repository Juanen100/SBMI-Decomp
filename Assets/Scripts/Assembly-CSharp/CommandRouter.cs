using System;
using System.Collections.Generic;

public class CommandRouter
{
	public const bool DEBUG_LOG_COMMANDS = false;

	private Dictionary<Identity, Simulated> simulated;

	private List<Command> commands;

	public void Register(Simulated entity)
	{
	}

	public void Unregister(Simulated entity)
	{
	}

	public void Send(Command command)
	{
	}

	public void Send(Command command, Action onComplete)
	{
	}

	public void Send(Command command, ulong delay)
	{
	}

	public int CancelMatching(Command.TYPE type, Identity sender, Identity receiver, Dictionary<string, object> matching = null)
	{
		return 0;
	}

	public void Route()
	{
	}

	private bool RouteCommand(Command command)
	{
		return false;
	}
}
