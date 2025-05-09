using System.Collections;
using System.Collections.Generic;
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
			this.element = element;
			this.origin = origin;
			this.target = target;
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
				return particleLocation;
			}
		}

		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		public RewardCoinShowerRequestDelegate(Vector3 particleLocation)
		{
			this.particleLocation = particleLocation;
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

	private List<SBGUIElement> elementsList = new List<SBGUIElement>();

	private Dictionary<string, Positioning> elementsToPosition = new Dictionary<string, Positioning>();

	private List<SBGUIElement> elementsToShrink = new List<SBGUIElement>();

	public void Setup(DailyBonusDialogInputData pInputData, Session pSession)
	{
		window = (SBGUIAtlasImage)FindChild("window");
		titleBackground = (SBGUIAtlasImage)FindChild("titleBackground");
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("window_title");
		SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)FindChild("tab_icon");
		SBGUILabel sBGUILabel2 = (SBGUILabel)FindChild("info");
		okayButton = (SBGUIPulseButton)FindChild("okay");
		SBGUILabel sBGUILabel3 = (SBGUILabel)FindChild("okay_label");
		SBGUIAtlasImage sBGUIAtlasImage2 = (SBGUIAtlasImage)FindChild("reward1_icon");
		SBGUIAtlasImage sBGUIAtlasImage3 = (SBGUIAtlasImage)FindChild("reward2_icon");
		SBGUIAtlasImage sBGUIAtlasImage4 = (SBGUIAtlasImage)FindChild("reward3_icon");
		SBGUIAtlasImage sBGUIAtlasImage5 = (SBGUIAtlasImage)FindChild("reward4_icon");
		SBGUIAtlasImage sBGUIAtlasImage6 = (SBGUIAtlasImage)FindChild("reward5_icon");
		pReward6Image = (SBGUIAtlasImage)FindChild("reward6_icon");
		SBGUILabel sBGUILabel4 = (SBGUILabel)FindChild("reward1_label");
		SBGUILabel sBGUILabel5 = (SBGUILabel)FindChild("reward2_label");
		SBGUILabel sBGUILabel6 = (SBGUILabel)FindChild("reward3_label");
		SBGUILabel sBGUILabel7 = (SBGUILabel)FindChild("reward4_label");
		SBGUILabel sBGUILabel8 = (SBGUILabel)FindChild("reward5_label");
		pReward6Label = (SBGUILabel)FindChild("reward6_label");
		pRewardTodayLabel = (SBGUILabel)FindChild("rewardToday_label");
		SBGUILabel sBGUILabel9 = (SBGUILabel)FindChild("reward1Day_label");
		pReward2DayLabel = (SBGUILabel)FindChild("reward2Day_label");
		SBGUILabel sBGUILabel10 = (SBGUILabel)FindChild("reward3Day_label");
		SBGUILabel sBGUILabel11 = (SBGUILabel)FindChild("reward4Day_label");
		SBGUILabel sBGUILabel12 = (SBGUILabel)FindChild("reward5Day_label");
		pReward6DayLabel = (SBGUILabel)FindChild("reward6Day_label");
		pDailyBonusData = pInputData.DailyBonusData;
		currentDay = pInputData.CurrentDay;
		alreadyCollected = pInputData.AlreadyCollected;
		if (pReward6DayLabel == null || pReward6Image == null || pReward6Label == null || sBGUILabel12 == null || sBGUIAtlasImage6 == null || sBGUILabel8 == null || pDailyBonusData.count() < 5)
		{
			Debug.LogError("Missing items for daily bonus");
			pSession.ChangeState("Playing");
			return;
		}
		sBGUIAtlasImage3.tform.localScale = new Vector3(1f, 1f, 1f);
		sBGUIAtlasImage4.tform.localScale = new Vector3(1f, 1f, 1f);
		sBGUIAtlasImage5.tform.localScale = new Vector3(1f, 1f, 1f);
		sBGUIAtlasImage6.tform.localScale = new Vector3(1f, 1f, 1f);
		pReward6Image.tform.localScale = new Vector3(1f, 1f, 1f);
		sBGUIAtlasImage2.tform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		sBGUILabel9.SetActive(false);
		pReward6DayLabel.SetActive(false);
		pReward6Image.SetActive(false);
		pReward6Label.SetActive(false);
		pRewardTodayLabel.SetActive(false);
		okayButton.SetActive(false);
		if (pDailyBonusData.count() == 5)
		{
			sBGUILabel9.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[0].Day);
			pReward2DayLabel.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[1].Day);
			sBGUILabel10.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[2].Day);
			sBGUILabel11.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[3].Day);
			sBGUILabel12.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[4].Day);
			sBGUILabel4.SetText(pDailyBonusData[0].CurrencyAmount.ToString());
			sBGUILabel5.SetText(pDailyBonusData[1].CurrencyAmount.ToString());
			sBGUILabel6.SetText(pDailyBonusData[2].CurrencyAmount.ToString());
			sBGUILabel7.SetText(pDailyBonusData[3].CurrencyAmount.ToString());
			sBGUILabel8.SetText(pDailyBonusData[4].CurrencyAmount.ToString());
			sBGUIAtlasImage2.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[0].CurrencyDID].GetResourceTexture());
			sBGUIAtlasImage3.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[1].CurrencyDID].GetResourceTexture());
			sBGUIAtlasImage4.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[2].CurrencyDID].GetResourceTexture());
			sBGUIAtlasImage5.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[3].CurrencyDID].GetResourceTexture());
			sBGUIAtlasImage6.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[4].CurrencyDID].GetResourceTexture());
			float duration = 0.5f;
			StartCoroutine(EnlargeTodayEffects(duration));
			return;
		}
		if (currentDay == 1)
		{
			sBGUILabel9.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[1].Day);
			pReward2DayLabel.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[2].Day);
			sBGUILabel10.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[3].Day);
			sBGUILabel11.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[4].Day);
			sBGUILabel12.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[5].Day);
			sBGUILabel4.SetText(pDailyBonusData[1].CurrencyAmount.ToString());
			sBGUILabel5.SetText(pDailyBonusData[2].CurrencyAmount.ToString());
			sBGUILabel6.SetText(pDailyBonusData[3].CurrencyAmount.ToString());
			sBGUILabel7.SetText(pDailyBonusData[4].CurrencyAmount.ToString());
			sBGUILabel8.SetText(pDailyBonusData[5].CurrencyAmount.ToString());
			sBGUIAtlasImage2.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[1].CurrencyDID].GetResourceTexture());
			sBGUIAtlasImage3.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[2].CurrencyDID].GetResourceTexture());
			sBGUIAtlasImage4.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[3].CurrencyDID].GetResourceTexture());
			sBGUIAtlasImage5.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[4].CurrencyDID].GetResourceTexture());
			sBGUIAtlasImage6.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[5].CurrencyDID].GetResourceTexture());
			float duration2 = 0.5f;
			StartCoroutine(EnlargeTodayEffects(duration2));
			return;
		}
		if (pDailyBonusData.count() < 6)
		{
			Debug.LogError("Missing items for daily bonus");
			pSession.ChangeState("Playing");
			return;
		}
		sBGUILabel9.SetActive(true);
		sBGUILabel9.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[0].Day);
		pReward2DayLabel.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[1].Day);
		sBGUILabel10.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[2].Day);
		sBGUILabel11.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[3].Day);
		sBGUILabel12.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[4].Day);
		sBGUILabel4.SetText(pDailyBonusData[0].CurrencyAmount.ToString());
		sBGUILabel5.SetText(pDailyBonusData[1].CurrencyAmount.ToString());
		sBGUILabel6.SetText(pDailyBonusData[2].CurrencyAmount.ToString());
		sBGUILabel7.SetText(pDailyBonusData[3].CurrencyAmount.ToString());
		sBGUILabel8.SetText(pDailyBonusData[4].CurrencyAmount.ToString());
		pReward6Label.SetText(pDailyBonusData[5].CurrencyAmount.ToString());
		sBGUIAtlasImage2.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[0].CurrencyDID].GetResourceTexture());
		sBGUIAtlasImage3.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[1].CurrencyDID].GetResourceTexture());
		sBGUIAtlasImage4.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[2].CurrencyDID].GetResourceTexture());
		sBGUIAtlasImage5.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[3].CurrencyDID].GetResourceTexture());
		sBGUIAtlasImage6.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[4].CurrencyDID].GetResourceTexture());
		pReward6Image.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pDailyBonusData[5].CurrencyDID].GetResourceTexture());
		elementsList.Clear();
		elementsToPosition.Clear();
		elementsList.Add(pReward2DayLabel);
		elementsList.Add(sBGUILabel10);
		elementsList.Add(sBGUILabel11);
		elementsList.Add(sBGUILabel12);
		elementsList.Add(sBGUIAtlasImage3);
		elementsList.Add(sBGUIAtlasImage4);
		elementsList.Add(sBGUIAtlasImage5);
		elementsList.Add(sBGUIAtlasImage6);
		elementsList.Add(sBGUILabel5);
		elementsList.Add(sBGUILabel6);
		elementsList.Add(sBGUILabel7);
		elementsList.Add(sBGUILabel8);
		foreach (SBGUIElement elements in elementsList)
		{
			Vector3 localPosition = elements.tform.localPosition;
			Vector3 target = new Vector3(localPosition.x - 1.8f, localPosition.y, localPosition.z);
			elementsToPosition.Add(elements.name, new Positioning(elements, localPosition, target));
		}
		elementsToShrink.Add(sBGUIAtlasImage2);
		elementsToShrink.Add(sBGUILabel4);
		elementsToShrink.Add(sBGUILabel9);
		float duration3 = 0.6f;
		StartCoroutine(shrinkFirstItem(duration3));
	}

	private IEnumerator shrinkFirstItem(float duration)
	{
		yield return new WaitForSeconds(0.3f);
		float normalizedTime = 0f;
		while (normalizedTime <= 1f)
		{
			normalizedTime += Time.deltaTime / duration;
			elementsToShrink[0].tform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, normalizedTime);
			elementsToShrink[1].tform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, normalizedTime);
			elementsToShrink[2].tform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, normalizedTime);
			yield return null;
		}
		float shiftDuration = 0.7f;
		StartCoroutine(ShiftLeftCoroutine(shiftDuration));
		float fadeDuration = 0.55f;
		StartCoroutine(FadeOutSecondDayLabel(fadeDuration));
	}

	private IEnumerator FadeOutSecondDayLabel(float duration)
	{
		float normalizedTime = 0f;
		while (normalizedTime <= 1f)
		{
			normalizedTime += Time.deltaTime / duration;
			pReward2DayLabel.SetAlpha(1f - normalizedTime);
			yield return null;
		}
	}

	private IEnumerator ShiftLeftCoroutine(float duration)
	{
		float normalizedTime = 0f;
		while (normalizedTime <= 1f)
		{
			normalizedTime += Time.deltaTime / duration;
			shiftLeftTransform(normalizedTime);
			yield return null;
		}
		float enlargeLastItemDuration = 0.5f;
		float enlargeTodayEffectsDuration = 0.2f;
		StartCoroutine(EnlargeLastItem(enlargeLastItemDuration));
		StartCoroutine(EnlargeTodayEffects(enlargeTodayEffectsDuration));
	}

	private void shiftLeftTransform(float normalizedTime)
	{
		foreach (string key in elementsToPosition.Keys)
		{
			elementsToPosition[key].element.tform.localPosition = Vector3.Lerp(elementsToPosition[key].origin, elementsToPosition[key].target, normalizedTime);
			elementsToPosition[key].element.tform.localRotation = Quaternion.identity;
			if (key == "reward2_icon")
			{
				elementsToPosition[key].element.tform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.3f, 1.3f, 1.3f), normalizedTime);
			}
		}
	}

	private IEnumerator EnlargeLastItem(float duration)
	{
		float normalizedTime = 0f;
		pReward6Image.SetActive(true);
		pReward6Label.SetActive(true);
		pReward6DayLabel.SetActive(true);
		pReward6DayLabel.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + pDailyBonusData[5].Day);
		while (normalizedTime <= 1f)
		{
			normalizedTime += Time.deltaTime / duration;
			pReward6Image.tform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, normalizedTime);
			pReward6Label.tform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, normalizedTime);
			pReward6DayLabel.tform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, normalizedTime);
			yield return null;
		}
	}

	private IEnumerator EnlargeTodayEffects(float duration)
	{
		pRewardTodayLabel.SetActive(true);
		float normalizedTime = 0f;
		while (normalizedTime <= 1f)
		{
			normalizedTime += Time.deltaTime / duration;
			pRewardTodayLabel.tform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, normalizedTime);
			yield return null;
		}
		okayButton.SetActive(true);
	}

	public void applyReward(Session session)
	{
		int index = 1;
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		dictionary.Add(pDailyBonusData[index].CurrencyDID, pDailyBonusData[index].CurrencyAmount);
		Reward reward = new Reward(dictionary, null, null, null, null, null, null, null, false, null);
		session.TheGame.ApplyReward(reward, TFUtils.EpochTime(), false);
		session.TheGame.ModifyGameState(new ReceiveRewardAction(reward, string.Empty));
		session.TheGame.analytics.LogDailyReward(currentDay);
		AnalyticsWrapper.LogDailyReward(session.TheGame, currentDay, reward);
		Ray ray = session.TheCamera.ScreenPointToRay(Input.mousePosition);
		if (pDailyBonusData[index].CurrencyDID == 3)
		{
			session.TheSoundEffectManager.PlaySound("coin_big");
			for (int i = 0; i < 5; i++)
			{
				session.TheGame.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Coin_Shower", 0, 0, 0f, new RewardCoinShowerRequestDelegate(ray.origin));
			}
		}
		else
		{
			session.TheSoundEffectManager.PlaySound("ItemCollected");
			session.TheGame.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Jelly_Shower", 0, 0, 0f, new RewardCoinShowerRequestDelegate(ray.origin));
		}
	}
}
