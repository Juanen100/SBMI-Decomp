using System.Collections.Generic;

public static class BuildingStateSetup
{
	public static void Generate(out Dictionary<string, Simulated.StateAction> actions, out StateMachine<Simulated.StateAction, Command.TYPE> machine, bool friendMode)
	{
		actions = new Dictionary<string, Simulated.StateAction>();
		machine = new StateMachine<Simulated.StateAction, Command.TYPE>();
		actions.Add("placing", Simulated.Building.Placing);
		actions.Add("erecting", Simulated.Building.Erecting);
		actions.Add("rushingbuild", Simulated.Building.RushingBuild);
		if (friendMode)
		{
			actions.Add("prime_erecting", Simulated.Building.PrimeErectingFriend);
			actions.Add("inactive", Simulated.Building.FriendParkInactive);
		}
		else
		{
			actions.Add("prime_erecting", Simulated.Building.PrimeErecting);
			actions.Add("inactive", Simulated.Building.Inactive);
		}
		actions.Add("activating", Simulated.Building.Activating);
		actions.Add("active", Simulated.Building.Active);
		actions.Add("requesting_interface", Simulated.Building.RequestingInterface);
		actions.Add("reflecting", Simulated.Building.Reflecting);
		actions.Add("producing", Simulated.Building.Producing);
		actions.Add("produced", Simulated.Building.Produced);
		actions.Add("rushingproduct", Simulated.Building.RushingProduct);
		actions.Add("crafting", Simulated.Building.Crafting);
		actions.Add("craftcycling", Simulated.Building.CraftCycling);
		actions.Add("craftcollect", Simulated.Building.CraftingCollect);
		actions.Add("crafted", Simulated.Building.Crafted);
		actions.Add("rushingcraft", Simulated.Building.RushingCraft);
		actions.Add("replacing", Simulated.Building.Replacing);
		actions.Add("reactiving", Simulated.Building.Reactivating);
		actions.Add("task_feed", Simulated.Building.TaskFeed);
		actions.Add("task_feed_collecting", Simulated.Building.TaskFeedCollecting);
		foreach (Simulated.StateAction value in actions.Values)
		{
			machine.AddState(value);
		}
		if (!friendMode)
		{
			machine.AddTransition(actions["placing"], Command.TYPE.EMPLOY, actions["prime_erecting"]);
			machine.AddTransition(actions["prime_erecting"], Command.TYPE.COMPLETE, actions["erecting"]);
			machine.AddTransition(actions["erecting"], Command.TYPE.COMPLETE, actions["inactive"]);
			machine.AddTransition(actions["erecting"], Command.TYPE.RUSH, actions["rushingbuild"]);
			machine.AddTransition(actions["rushingbuild"], Command.TYPE.COMPLETE, actions["inactive"]);
			machine.AddTransition(actions["inactive"], Command.TYPE.CLICKED, actions["activating"]);
			machine.AddTransition(actions["activating"], Command.TYPE.COMPLETE, actions["active"]);
			machine.AddTransition(actions["reflecting"], Command.TYPE.ACTIVATE, actions["active"]);
			machine.AddTransition(actions["reflecting"], Command.TYPE.PRODUCE, actions["producing"]);
			machine.AddTransition(actions["reflecting"], Command.TYPE.CRAFT, actions["crafting"]);
			machine.AddTransition(actions["reflecting"], Command.TYPE.ADVANCE, actions["craftcycling"]);
			machine.AddTransition(actions["reflecting"], Command.TYPE.CRAFTED, actions["craftcycling"]);
			machine.AddTransition(actions["reflecting"], Command.TYPE.BONUS_REWARD, actions["task_feed"]);
			machine.AddTransition(actions["active"], Command.TYPE.PRODUCE, actions["producing"]);
			machine.AddTransition(actions["active"], Command.TYPE.CRAFT, actions["crafting"]);
			machine.AddTransition(actions["active"], Command.TYPE.DELEGATE_CLICK, actions["requesting_interface"]);
			machine.AddTransition(actions["active"], Command.TYPE.HUBCRAFT, actions["active"]);
			machine.AddTransition(actions["active"], Command.TYPE.BONUS_REWARD, actions["task_feed"]);
			machine.AddTransition(actions["requesting_interface"], Command.TYPE.COMPLETE, actions["reflecting"]);
			machine.AddTransition(actions["producing"], Command.TYPE.COMPLETE, actions["produced"]);
			machine.AddTransition(actions["producing"], Command.TYPE.RUSH, actions["rushingproduct"]);
			machine.AddTransition(actions["producing"], Command.TYPE.BONUS_REWARD, actions["task_feed"]);
			machine.AddTransition(actions["produced"], Command.TYPE.CLICKED, actions["producing"]);
			machine.AddTransition(actions["rushingproduct"], Command.TYPE.COMPLETE, actions["produced"]);
			machine.AddTransition(actions["crafting"], Command.TYPE.CRAFTED, actions["craftcycling"]);
			machine.AddTransition(actions["crafting"], Command.TYPE.CRAFT, actions["crafting"]);
			machine.AddTransition(actions["crafting"], Command.TYPE.RUSH, actions["rushingcraft"]);
			machine.AddTransition(actions["crafting"], Command.TYPE.DELEGATE_CLICK, actions["requesting_interface"]);
			machine.AddTransition(actions["crafting"], Command.TYPE.BONUS_REWARD, actions["task_feed"]);
			machine.AddTransition(actions["craftcycling"], Command.TYPE.COMPLETE, actions["crafted"]);
			machine.AddTransition(actions["craftcycling"], Command.TYPE.CRAFTED, actions["craftcycling"]);
			machine.AddTransition(actions["craftcycling"], Command.TYPE.CRAFT, actions["craftcycling"]);
			machine.AddTransition(actions["craftcycling"], Command.TYPE.CLICKED, actions["craftcollect"]);
			machine.AddTransition(actions["craftcycling"], Command.TYPE.RUSH, actions["rushingcraft"]);
			machine.AddTransition(actions["craftcollect"], Command.TYPE.COMPLETE, actions["crafting"]);
			machine.AddTransition(actions["craftcollect"], Command.TYPE.CRAFTED, actions["craftcycling"]);
			machine.AddTransition(actions["crafted"], Command.TYPE.CLICKED, actions["active"]);
			machine.AddTransition(actions["crafted"], Command.TYPE.CRAFT, actions["craftcycling"]);
			machine.AddTransition(actions["rushingcraft"], Command.TYPE.CRAFTED, actions["craftcycling"]);
			machine.AddTransition(actions["task_feed"], Command.TYPE.CLICKED, actions["task_feed_collecting"]);
			machine.AddTransition(actions["task_feed_collecting"], Command.TYPE.COMPLETE, actions["reflecting"]);
			machine.AddTransition(actions["replacing"], Command.TYPE.COMPLETE, actions["reactiving"]);
			machine.AddTransition(actions["reactiving"], Command.TYPE.COMPLETE, actions["active"]);
		}
	}
}
