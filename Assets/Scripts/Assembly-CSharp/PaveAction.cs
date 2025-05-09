using System.Collections.Generic;

public class PaveAction : PersistedTriggerableAction
{
	public class PaveElement
	{
		public GridPosition position;

		public PaveElement(GridPosition position)
		{
			this.position = position;
		}
	}

	public const string PAVE = "np";

	public List<PaveElement> path;

	public Cost cost;

	public TriggerableMixin Triggerable
	{
		get
		{
			return triggerable;
		}
	}

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public PaveAction(Identity id, List<PaveElement> path, Cost cost)
		: base("np", id)
	{
		this.path = path;
		this.cost = cost;
	}

	public new static PaveAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		List<PaveElement> list = new List<PaveElement>();
		List<object> list2 = TFUtils.LoadList<object>(data, "path");
		foreach (Dictionary<string, object> item in list2)
		{
			GridPosition position = new GridPosition(TFUtils.LoadInt(item, "row"), TFUtils.LoadInt(item, "col"));
			list.Add(new PaveElement(position));
		}
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		return new PaveAction(id, list, cost);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		List<object> list = new List<object>();
		foreach (PaveElement item in path)
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2["col"] = item.position.col;
			dictionary2["row"] = item.position.row;
			list.Add(dictionary2);
		}
		dictionary["path"] = list;
		dictionary["cost"] = cost.ToDict();
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		foreach (PaveElement item in path)
		{
			game.terrain.ChangePath(new GridPosition(item.position.row, item.position.col));
		}
		game.resourceManager.Apply(cost, game);
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<GridPosition> list = new List<GridPosition>();
		List<object> list2 = (List<object>)((Dictionary<string, object>)gameState["farm"])["pavement"];
		foreach (Dictionary<string, object> item in list2)
		{
			GridPosition newPos = new GridPosition(TFUtils.LoadInt(item, "row"), TFUtils.LoadInt(item, "col"));
			int num = path.FindIndex((PaveElement pe) => pe.position == newPos);
			if (num >= 0)
			{
				path.RemoveAt(num);
			}
			else
			{
				list.Add(newPos);
			}
		}
		foreach (PaveElement item2 in path)
		{
			list.Add(new GridPosition(item2.position.row, item2.position.col));
		}
		list2.Clear();
		foreach (GridPosition item3 in list)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["row"] = item3.row;
			dictionary["col"] = item3.col;
			list2.Add(dictionary);
		}
		ResourceManager.ApplyCostToGameState(cost, gameState);
		base.Confirm(gameState);
	}

	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		data["pave_type"] = 1;
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}
}
