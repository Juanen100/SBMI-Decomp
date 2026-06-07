using System.Collections.Generic;

public class StateMachine<State, Command>
{
	private class Entry
	{
		public State state;

		public Dictionary<Command, Entry> transitions;

		public Dictionary<Command, Entry> delegates;

		public Entry(State state)
		{
		}

		public bool Transition(Command command, out Entry result)
		{
			result = null;
			return false;
		}

		public bool Delegate(Command command, out Entry result)
		{
			result = null;
			return false;
		}
	}

	private Dictionary<State, Entry> states;

	public ICollection<State> States
	{
		get
		{
			return null;
		}
	}

	public void AddState(State state)
	{
	}

	public void AddState_Unsafe(State state)
	{
	}

	public void AddTransition(State current, Command command, State result)
	{
	}

	public void AddDelegate(State deferer, Command command, State handler)
	{
	}

	public bool Transition(State current, Command command, out State result)
	{
		result = default(State);
		return false;
	}

	public bool Delegate(State current, Command command, out State result)
	{
		result = default(State);
		return false;
	}
}
