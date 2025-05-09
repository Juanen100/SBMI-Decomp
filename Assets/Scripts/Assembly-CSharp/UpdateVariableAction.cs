#define ASSERTS_ON
using System;
using System.Collections.Generic;

public class UpdateVariableAction<T> : PersistedTriggerableAction
{
	public const string UPDATE_VARIABLE = "uv";

	private string m_sVarName;

	private T m_pVariable;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public UpdateVariableAction(string sVarName, T pVariable)
		: base("uv", Identity.Null())
	{
		m_sVarName = sVarName;
		m_pVariable = pVariable;
	}

	public new static UpdateVariableAction<T> FromDict(Dictionary<string, object> pData)
	{
		string sVarName = TFUtils.LoadString(pData, "var_name");
		Type typeFromHandle = typeof(T);
		T pVariable;
		if (typeFromHandle == typeof(int))
		{
			pVariable = (T)(object)TFUtils.LoadInt(pData, "variable");
		}
		else if (typeFromHandle == typeof(uint))
		{
			pVariable = (T)(object)TFUtils.LoadUint(pData, "variable");
		}
		else if (typeFromHandle == typeof(long))
		{
			pVariable = (T)(object)TFUtils.LoadLong(pData, "variable");
		}
		else if (typeFromHandle == typeof(ulong))
		{
			pVariable = (T)(object)TFUtils.LoadUlong(pData, "variable");
		}
		else if (typeFromHandle == typeof(float))
		{
			pVariable = (T)(object)TFUtils.LoadFloat(pData, "variable");
		}
		else if (typeFromHandle == typeof(double))
		{
			pVariable = (T)(object)TFUtils.LoadDouble(pData, "variable");
		}
		else if (typeFromHandle == typeof(bool))
		{
			pVariable = (T)(object)TFUtils.LoadBool(pData, "variable");
		}
		else if (typeFromHandle == typeof(string))
		{
			pVariable = (T)(object)TFUtils.LoadString(pData, "variable");
		}
		else
		{
			TFUtils.Assert(false, "UpdateVariableAction.cs unsupported variable type.");
			pVariable = default(T);
		}
		return new UpdateVariableAction<T>(sVarName, pVariable);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["var_name"] = m_sVarName;
		dictionary["variable"] = m_pVariable;
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary;
		if (gameState.ContainsKey("variables"))
		{
			dictionary = (Dictionary<string, object>)gameState["variables"];
		}
		else
		{
			dictionary = new Dictionary<string, object>();
			gameState.Add("variables", dictionary);
		}
		if (dictionary.ContainsKey(m_sVarName))
		{
			dictionary[m_sVarName] = m_pVariable;
		}
		else
		{
			dictionary.Add(m_sVarName, m_pVariable);
		}
		base.Confirm(gameState);
	}
}
