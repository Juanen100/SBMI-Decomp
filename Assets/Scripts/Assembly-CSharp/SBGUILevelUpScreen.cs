using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SBGUILevelUpScreen : SBGUIScreen
{
	private const int REWARD_GAP_SIZE = 5;

	public GameObject rewardWidgetPrefab;

	private List<SBGUIRewardWidget> rewards;

	private float markerXOffset;

	private SBGUIElement rewardMarker;

	private SBGUILabel unlocked_count;

	private SBGUIElement unlocked_marker;

	private Vector3 rewardCenter;

	private SBGUIImage windows;

	private SBGUIImage spinningPaper;

	private AudioSource spinningAudio;

	private const float BLUEPRINT_GAP_SIZE = 5f;

	public GameObject slotPrefab;

	private List<Blueprint> unlockedItems;

	protected EntityManager entityMgr;

	protected ResourceManager resourceMgr;

	protected SoundEffectManager soundEffectMgr;

	public void Setup(Session session, LevelUpDialogInputData inputData)
	{
	}

	public void CreateUI(Session session, LevelUpDialogInputData inputData)
	{
	}

	public void ShowUnlockedBlueprints()
	{
	}

	public void SetManagers(EntityManager emgr, ResourceManager resMgr, SoundEffectManager sfxMgr)
	{
	}

	private void SetLevelText(Session session, int level)
	{
	}

	private void SetLevelImage(Session session, int level)
	{
	}

	private void SetLevelVoice(Session session, int level)
	{
	}

	public override void Deactivate()
	{
	}

	private void AddItem(string texture, int amount)
	{
	}

	private void ClearItems()
	{
	}

	private void InitializeRewardComponentAmounts(Reward reward, Dictionary<int, int> componentAmounts, Dictionary<int, int> outAmounts)
	{
	}

	private void SetRewardIcons(Session session, List<Reward> rewards)
	{
	}

	private void CenterBlueprints(Vector3 offset)
	{
	}

	private void CenterRewards()
	{
	}

	[DebuggerHidden]
	private IEnumerator AnimateSpinIn(float duration)
	{
		return null;
	}

	private new void OnDestroy()
	{
	}
}
