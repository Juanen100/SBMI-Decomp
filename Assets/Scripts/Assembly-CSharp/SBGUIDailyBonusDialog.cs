using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SBGUIDailyBonusDialog : SBGUIScreen
{
	public class Positioning
	{
		public SBGUIElement element;

		public Vector3 origin;

		public Vector3 target;

		public Positioning(SBGUIElement element, Vector3 origin, Vector3 target)
		{
		}
	}

	private class RewardCoinShowerRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		protected Vector3 particleLocation;

		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		public Vector3 Position
		{
			get
			{
				return default(Vector3);
			}
		}

		public bool isVisible
		{
			get
			{
				return false;
			}
		}

		public RewardCoinShowerRequestDelegate(Vector3 particleLocation)
		{
		}
	}

	private SBGUIPulseButton okayButton;

	private SBGUIAtlasImage window;

	private SBGUIAtlasImage titleBackground;

	private SBGUILabel pRewardTodayLabel;

	private SBGUILabel pReward2DayLabel;

	private SBGUILabel pReward6DayLabel;

	private SBGUILabel pReward6Label;

	private SBGUIAtlasImage pReward6Image;

	private SoaringArray<SBMISoaring.SBMIDailyBonusDay> pDailyBonusData;

	private int currentDay;

	private bool alreadyCollected;

	private List<SBGUIElement> elementsList;

	private Dictionary<string, Positioning> elementsToPosition;

	private List<SBGUIElement> elementsToShrink;

	public void Setup(DailyBonusDialogInputData pInputData, Session pSession)
	{
	}

	[DebuggerHidden]
	private IEnumerator shrinkFirstItem(float duration)
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator FadeOutSecondDayLabel(float duration)
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator ShiftLeftCoroutine(float duration)
	{
		return null;
	}

	private void shiftLeftTransform(float normalizedTime)
	{
	}

	[DebuggerHidden]
	private IEnumerator EnlargeLastItem(float duration)
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator EnlargeTodayEffects(float duration)
	{
		return null;
	}

	public void applyReward(Session session)
	{
	}
}
