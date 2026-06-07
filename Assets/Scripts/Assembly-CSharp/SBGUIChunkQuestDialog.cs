using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SBGUIChunkQuestDialog : SBGUIScrollableDialog
{
	public const int STEP_GAP = -10;

	private const float PROGRESSBAR_HEIGHT = 71f;

	private const float PROGRESSBAR_FILLRATE = 0.1f;

	private const int REWARD_GAP_SIZE = 10;

	public GameObject rewardWidgetPrefab;

	public ParticleSystem progressBarParticle;

	private List<SBGUIRewardWidget> rewards;

	private int? prefabIconSize;

	private int? questLineIconSize;

	private int? questIconSize;

	private float markerXOffset;

	private Vector2 scrollSize;

	private Vector2? prefabWindowSize;

	private Vector3? prefabOkayButtonPos;

	private SBGUIProgressMeter progressMeter;

	private SBGUIPulseButton okayButton;

	private SBGUIAtlasImage questlineRewardIcon;

	private SBGUIAtlasImage questRewardIcon;

	private SBGUIAtlasImage window;

	private SBGUIElement rewardItemBg;

	private SBGUIElement progressbar_group;

	private SBGUIElement stepsMarker;

	protected SBGUIElement rewardMarker;

	private double residentPosX;

	private double residentPosY;

	protected override void Awake()
	{
	}

	public override void SetParent(SBGUIElement element)
	{
	}

	public void CreateScrollRegionUI(SBGUIStandardScreen screen, List<QuestBookendInfo.ChunkConditions> chunks, List<ConditionDescription> steps, Action findButtonHandler, string forcedStepPrefabName = null)
	{
	}

	public void SetupChunkDialogInfo(string dialogHeading, string dialogBody, string portrait, string name, bool isComplete, QuestDefinition pQuestDef)
	{
	}

	public void SetQuestLineInfo(QuestLineInfo questLine, float? start, float? progress, bool skipAnimation)
	{
	}

	[DebuggerHidden]
	private IEnumerator AnimateParticlePosition(float duration)
	{
		return null;
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
