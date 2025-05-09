using System.Collections.Generic;

public class StateMachine<State, Command>
{
	private class Entry
	{
		public State state;

		public Dictionary<Command, Entry> transitions = new Dictionary<Command, Entry>();

		public Dictionary<Command, Entry> delegates = new Dictionary<Command, Entry>();

		public Entry(State state)
		{
			this.state = state;
		}

		public bool Transition(Command command, out Entry result)
		{
			return transitions.TryGetValue(command, out result);
		}

		public bool Delegate(Command command, out Entry result)
		{
			return delegates.TryGetValue(command, out result);
		}
	}

	private Dictionary<State, Entry> states = new Dictionary<State, Entry>();

	public ICollection<State> States
	{
		get
		{
			return states.Keys;
		}
	}

	public void AddState(State state)
	{
		if (!states.ContainsKey(state))
		{
			states.Add(state, new Entry(state));
		}
	}

	public void AddState_Unsafe(State state)
	{
		states.Add(state, new Entry(state));
	}

	public void AddTransition(State current, Command command, State result)
	{
		states[current].transitions.Add(command, states[result]);
	}

	public void AddDelegate(State deferer, Command command, State handler)
	{
		states[deferer].delegates.Add(command, states[handler]);
	}

	public bool Transition(State current, Command command, out State result)
	{
		Entry result2;
		if (states[current].Transition(command, out result2))
		{
			result = result2.state;
			return true;
		}
		result = default(State);
		return false;
	}

	public bool Delegate(State current, Command command, out State result)
	{
		Entry result2;
		if (states[current].Delegate(command, out result2))
		{
			result = result2.state;
			return true;
		}
		result = default(State);
		return false;
	}
}
