using System.Collections.Generic;
using UnityEngine;

public class SBGUIModalDialog : SBGUIScreen
{
	private const int REWARD_GAP_SIZE = 10;

	public GameObject rewardWidgetPrefab;

	private float markerXOffset;

	protected SBGUIElement rewardMarker;

	private SBGUIElement parentElement;

	private List<SBGUIRewardWidget> rewards;

	protected override void Awake()
	{
	}

	public override void SetParent(SBGUIElement element)
	{
	}

	private void ZShuffle()
	{
	}

	public override void Close()
	{
	}

	public virtual void AddItem(string texture, int amount, string prefix)
	{
	}

	private void ClearItems()
	{
	}

	private void InitializeRewardComponentAmounts(Reward reward, Dictionary<int, int> componentAmounts, Dictionary<int, int> outAmounts)
	{
	}

	public void SetRewardIcons(Session session, List<Reward> rewards, string prefix)
	{
	}

	public void CenterRewards()
	{
	}
}
