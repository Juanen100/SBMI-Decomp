using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBGUINamebar : SBGUIElement
{
	public delegate Vector3 HostPosition();

	public delegate float EasingFunc(float start, float end, float duration);

	public delegate bool UpdateProgress();

	public float elapsed;

	private Dictionary<string, SBGUIElement> dict;

	private SBGUILabel nameLabel;

	private Action closeFinishedAction;

	private SBGUICharacterArrowList m_pTaskCharacterList;

	private HostPosition m_hPosition;

	protected override void Awake()
	{
		dict = CacheChildren();
		nameLabel = (SBGUILabel)dict["name_label"];
		m_pTaskCharacterList = (SBGUICharacterArrowList)FindChild("character_portrait_parent");
		base.Awake();
	}

	public void Setup(Session session, string name, HostPosition hPosition, Action onFinish, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
		nameLabel.SetText(name);
		closeFinishedAction = onFinish;
		m_hPosition = hPosition;
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
		StartCoroutine(TimeoutCoroutine(5f, hPosition));
	}

	private void Update()
	{
		if (m_hPosition != null)
		{
			Vector3 vector = m_hPosition();
			SetScreenPosition(vector.x, vector.y);
		}
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

	private IEnumerator TimeoutCoroutine(float duration, HostPosition hPosition)
	{
		elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
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
		SBUIBuilder.ReleaseNamebar(this);
	}

	private IEnumerator CloseCoroutine()
	{
		yield return StartCoroutine(ScaleCoroutine(base.tform.localScale.x, 0.1f, 0.5f, Easing.EaseInBack));
		StopAllCoroutines();
		SBUIBuilder.ReleaseNamebar(this);
	}

	public void Close()
	{
		if (IsActive())
		{
			StartCoroutine(CloseCoroutine());
		}
	}

	public void RemoveCompleteAction()
	{
		closeFinishedAction = null;
	}
}
