using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBGUITimebar : SBGUIElement
{
	public delegate Vector3 HostPosition();

	public delegate float EasingFunc(float start, float end, float duration);

	public delegate bool UpdateProgress();

	public float elapsed;

	private Dictionary<string, SBGUIElement> dict;

	private SBGUIProgressMeter meter;

	private SBGUILabel durationLabel;

	private SBGUILabel rushLabel;

	private SBGUIButton rushButton;

	private Action closeFinishedAction;

	private SBGUICharacterArrowList m_pTaskCharacterList;

	private int maxJellyCost = -1;

	private string originalRushButtonSessionActionId;

	public SBGUIButton RushButton
	{
		get
		{
			return rushButton;
		}
	}

	protected override void Awake()
	{
		dict = CacheChildren();
		meter = (SBGUIProgressMeter)dict["progress_meter"];
		durationLabel = (SBGUILabel)dict["duration_label"];
		m_pTaskCharacterList = (SBGUICharacterArrowList)FindChild("character_portrait_parent");
		base.Awake();
	}

	public void Setup(Session session, uint ownerDid, string description, ulong completeTime, ulong totalTime, float duration, Cost rushCost, Action onRush, HostPosition hPosition, Action onFinish, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
		SBGUILabel sBGUILabel = (SBGUILabel)dict["task_label"];
		sBGUILabel.SetText(description);
		SBGUIElement sBGUIElement = dict["rush_button"];
		closeFinishedAction = onFinish;
		if (onRush == null)
		{
			sBGUIElement.SetActive(false);
			rushButton = null;
		}
		else
		{
			rushButton = sBGUIElement.GetComponent<SBGUIButton>();
			rushButton.ClearClickEvents();
			AttachAnalyticsToButton("rush", rushButton);
			rushButton.ClickEvent += onRush;
			if (originalRushButtonSessionActionId == null)
			{
				originalRushButtonSessionActionId = rushButton.SessionActionId;
			}
			rushButton.SessionActionId = SessionActionSimulationHelper.DecorateSessionActionId(ownerDid, originalRushButtonSessionActionId);
			rushLabel = (SBGUILabel)dict["rush_cost_label"];
			rushLabel.SetText(rushCost.ResourceAmounts[rushCost.GetOnlyCostKey()].ToString());
			maxJellyCost = rushCost.ResourceAmounts[rushCost.GetOnlyCostKey()];
		}
		if (m_pTaskCharacterList != null)
		{
			if (pTaskCharacterDIDs == null || pTaskCharacterDIDs.Count <= 0)
			{
				m_pTaskCharacterList.SetActive(false);
			}
			else
			{
				m_pTaskCharacterList.SetActive(true);
				List<SBGUIArrowList.ListItemData> list = new List<SBGUIArrowList.ListItemData>();
				List<int> list2 = new List<int>();
				int count = pTaskCharacterDIDs.Count;
				for (int i = 0; i < count; i++)
				{
					Simulated simulated = session.TheGame.simulation.FindSimulated(pTaskCharacterDIDs[i]);
					ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
					list.Add(new SBGUIArrowList.ListItemData(entity.DefinitionId, entity.QuestReminderIcon));
					List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(entity.DefinitionId, null);
					if (activeTasksForSimulated != null && activeTasksForSimulated.Count > 0 && activeTasksForSimulated[0].m_bMovingToTarget)
					{
						list2.Add(entity.DefinitionId);
					}
				}
				m_pTaskCharacterList.SetData(session, list, (list.Count > 0) ? list[0].m_nID : 0, list2, null, pTaskCharacterClicked);
			}
		}
		StartCoroutine(ScaleCoroutine(0.1f, 1f, 0.5f, Easing.EaseOutBack));
		UpdateProgress updateProgress = delegate
		{
			ulong num = completeTime - totalTime;
			ulong num2 = TFUtils.EpochTime();
			float b = (float)(num2 - num) / (float)totalTime;
			ulong num3 = completeTime - num2;
			if (num3 < 0)
			{
				num3 = 0uL;
			}
			SetProgress(Mathf.Min(1f, b), num3);
			return num3 != 0;
		};
		StartCoroutine(TimeoutCoroutine(duration, hPosition, updateProgress));
	}

	public Vector2 GetRushButtonScreenPosition()
	{
		return dict["rush_button"].GetScreenPosition();
	}

	private IEnumerator ScaleCoroutine(float startScale, float endScale, float duration, EasingFunc easing)
	{
		float elapsed = 0f;
		float inverseDuration = 1f / duration;
		base.gameObject.transform.localScale = new Vector3(startScale, startScale, startScale);
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float elapsedOverDuration = elapsed * inverseDuration;
			float currentScale = easing(startScale, endScale, elapsedOverDuration);
			base.gameObject.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
			yield return null;
		}
		base.gameObject.transform.localScale = new Vector3(endScale, endScale, endScale);
	}

	private IEnumerator TimeoutCoroutine(float duration, HostPosition hPosition, UpdateProgress updateProgress)
	{
		elapsed = 0f;
		while (elapsed < duration || SBGUI.GetInstance().CheckWhitelisted(rushButton))
		{
			elapsed += Time.deltaTime;
			if (!updateProgress())
			{
				break;
			}
			if (hPosition != null)
			{
				Vector3 pos = hPosition();
				SetScreenPosition(pos.x, pos.y);
			}
			yield return null;
		}
		if (closeFinishedAction != null)
		{
			closeFinishedAction();
		}
		yield return StartCoroutine(ScaleCoroutine(1f, 0.1f, 0.5f, Easing.EaseInBack));
		SBUIBuilder.ReleaseTimebar(this);
	}

	public void SetProgress(float percent, ulong duration)
	{
		meter.Progress = percent;
		durationLabel.SetText(TFUtils.DurationToString(duration));
		if (maxJellyCost > 0)
		{
			rushLabel.SetText(Resource.Prorate(maxJellyCost, 1f - percent).ToString());
		}
	}

	private IEnumerator CloseCoroutine()
	{
		yield return StartCoroutine(ScaleCoroutine(base.tform.localScale.x, 0.1f, 0.5f, Easing.EaseInBack));
		StopAllCoroutines();
		SBUIBuilder.ReleaseTimebar(this);
	}

	public void Close()
	{
		if (IsActive())
		{
			rushButton.ClearClickEvents();
			StartCoroutine(CloseCoroutine());
		}
	}

	public void RemoveCompleteAction()
	{
		closeFinishedAction = null;
	}
}
