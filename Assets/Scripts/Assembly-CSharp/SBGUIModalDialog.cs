using System.Collections.Generic;
using UnityEngine;

public class SBGUIModalDialog : SBGUIScreen
{
	private const int REWARD_GAP_SIZE = 10;

	public GameObject rewardWidgetPrefab;

	private float markerXOffset;

	protected SBGUIElement rewardMarker;

	private SBGUIElement parentElement;

	private List<SBGUIRewardWidget> rewards = new List<SBGUIRewardWidget>();

	protected override void Awake()
	{
		if (rewardWidgetPrefab == null)
		{
			rewardWidgetPrefab = (GameObject)Resources.Load("Prefabs/GUI/Widgets/RewardWidget");
		}
		rewardMarker = FindChild("reward_marker");
		base.Awake();
	}

	public override void SetParent(SBGUIElement element)
	{
		parentElement = element;
		base.View.RefreshEvent += ZShuffle;
		base.SetParent(element);
	}

	private void ZShuffle()
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		Vector3 zero = Vector3.zero;
		Bounds totalBounds = TotalBounds;
		if (totalBounds.min.z <= 0f)
		{
			zero.z = Mathf.Abs(totalBounds.min.z) + 1f;
			totalBounds.center += zero;
			base.tform.localPosition += zero;
		}
		if (!(parentElement == null))
		{
			Bounds totalBounds2 = parentElement.TotalBounds;
			if (totalBounds2.min.z <= totalBounds.max.z)
			{
				zero.z = totalBounds.max.z - totalBounds2.min.z + 1f;
				parentElement.tform.localPosition += zero;
			}
		}
	}

	public override void Close()
	{
		Object.Destroy(base.gameObject);
	}

	public virtual void AddItem(string texture, int amount, string prefix)
	{
		if (amount == 0)
		{
			Debug.LogWarning("rewarding 0 of :" + texture);
			return;
		}
		if (texture == null || string.IsNullOrEmpty(texture.Trim()))
		{
			Debug.LogWarning("resource has no texture");
			return;
		}
		SBGUIRewardWidget item = SBGUIRewardWidget.Create(rewardWidgetPrefab, rewardMarker, markerXOffset, texture, amount, prefix);
		rewards.Add(item);
		markerXOffset = 0f;
		float num = 1f;
		float y = 0f;
		if (rewards.Count > 3)
		{
			num = 0.5f;
			y = 0.1f;
		}
		foreach (SBGUIRewardWidget reward in rewards)
		{
			reward.transform.localScale = new Vector3(num, num, num);
			reward.transform.localPosition = new Vector3(markerXOffset, y, 0f);
			markerXOffset += (float)(reward.Width + 10) * num * 0.01f;
		}
	}

	private void ClearItems()
	{
		markerXOffset = 0f;
		foreach (SBGUIRewardWidget reward in rewards)
		{
			reward.gameObject.SetActiveRecursively(false);
			Object.Destroy(reward.gameObject);
		}
		rewards.Clear();
	}

	private void InitializeRewardComponentAmounts(Reward reward, Dictionary<int, int> componentAmounts, Dictionary<int, int> outAmounts)
	{
		outAmounts.Clear();
		foreach (KeyValuePair<int, int> componentAmount in componentAmounts)
		{
			int key = componentAmount.Key;
			int value = componentAmount.Value;
			if (!outAmounts.ContainsKey(key))
			{
				outAmounts[key] = 0;
			}
			Dictionary<int, int> dictionary2;
			Dictionary<int, int> dictionary = (dictionary2 = outAmounts);
			int key3;
			int key2 = (key3 = key);
			key3 = dictionary2[key3];
			dictionary[key2] = key3 + value;
		}
	}

	public void SetRewardIcons(Session session, List<Reward> rewards, string prefix)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
		ClearItems();
		foreach (Reward reward in rewards)
		{
			if (reward != null)
			{
				InitializeRewardComponentAmounts(reward, reward.ResourceAmounts, dictionary);
				InitializeRewardComponentAmounts(reward, reward.BuildingAmounts, dictionary2);
			}
		}
		foreach (KeyValuePair<int, int> item in dictionary)
		{
			int key = item.Key;
			int value = item.Value;
			AddItem(session.TheGame.resourceManager.Resources[key].GetResourceTexture(), value, prefix);
		}
		foreach (KeyValuePair<int, int> item2 in dictionary2)
		{
			int value2 = item2.Value;
			Blueprint blueprint = EntityManager.GetBlueprint("building", item2.Key);
			AddItem((string)blueprint.Invariable["portrait"], value2, prefix);
		}
	}

	public void CenterRewards()
	{
		Vector3 position = rewardMarker.tform.position;
		Vector3 vector = rewardMarker.TotalBounds.center - position;
		Vector3 localPosition = rewardMarker.tform.localPosition;
		localPosition.x -= vector.x;
		rewardMarker.tform.localPosition = localPosition;
	}
}
