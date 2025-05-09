using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBGUILevelUpScreen : SBGUIScreen
{
	private const int REWARD_GAP_SIZE = 5;

	private const float BLUEPRINT_GAP_SIZE = 5f;

	public GameObject rewardWidgetPrefab;

	private List<SBGUIRewardWidget> rewards = new List<SBGUIRewardWidget>();

	private float markerXOffset;

	private SBGUIElement rewardMarker;

	private SBGUILabel unlocked_count;

	private SBGUIElement unlocked_marker;

	private Vector3 rewardCenter;

	private SBGUIImage windows;

	private SBGUIImage spinningPaper;

	private AudioSource spinningAudio;

	public GameObject slotPrefab;

	private List<Blueprint> unlockedItems = new List<Blueprint>();

	protected EntityManager entityMgr;

	protected ResourceManager resourceMgr;

	protected SoundEffectManager soundEffectMgr;

	public void Setup(Session session, LevelUpDialogInputData inputData)
	{
		spinningPaper = (SBGUIImage)FindChild("spinning_paper");
		windows = (SBGUIImage)FindChild("windows");
		spinningPaper.SetTextureFromTexturePath(TextureLibrarian.PathLookUp("Textures/GUI/LevelUpScreen/levelup_spinning"));
		windows.SetTextureFromTexturePath(TextureLibrarian.PathLookUp("Textures/GUI/LevelUpScreen/levelup_bg"));
		SetLevelText(session, inputData.NewLevel);
		SetLevelImage(session, inputData.NewLevel);
		SetLevelVoice(session, inputData.NewLevel);
		SetRewardIcons(session, inputData.Rewards);
		spinningAudio = soundEffectMgr.PlaySound("JellyfishNet");
		if (spinningAudio != null)
		{
			spinningAudio.Stop();
			spinningAudio.loop = true;
			spinningAudio.pitch = 1.4f;
			spinningAudio.volume = 0.3f;
			spinningAudio.Play();
		}
		int num = resourceMgr.Query(ResourceManager.LEVEL);
		foreach (Blueprint value in entityMgr.Blueprints.Values)
		{
			if (!value.Invariable.ContainsKey("level.minimum"))
			{
				continue;
			}
			int num2 = (int)value.Invariable["level.minimum"];
			if (num2 == num)
			{
				unlockedItems.Add(value);
				if ((bool)value.Invariable["has_move_in"] && value.Invariable.ContainsKey("resident") && value.Invariable["resident"] != null)
				{
					int did = (int)value.Invariable["resident"];
					Blueprint blueprint = EntityManager.GetBlueprint(EntityType.RESIDENT, did);
					string characterName = Language.Get((string)blueprint.Invariable["name"]);
					string buildingName = Language.Get((string)value.Invariable["name"]);
					string portraitTexture = (string)value.Invariable["portrait"];
					MoveInDialogInputData item = new MoveInDialogInputData(characterName, buildingName, portraitTexture, "Beat_MoveIn");
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData> { item });
				}
			}
		}
	}

	public void CreateUI(Session session, LevelUpDialogInputData inputData)
	{
		ShowUnlockedBlueprints();
		foreach (Transform item in windows.transform)
		{
			if (item.name != "unlocked_marker")
			{
				item.gameObject.SetActive(false);
			}
		}
		foreach (Transform item2 in windows.transform.Find("unlocked_marker"))
		{
			item2.GetComponent<Renderer>().enabled = false;
		}
		spinningPaper.SetActive(true);
		StartCoroutine(AnimateSpinIn(1f));
	}

	public void ShowUnlockedBlueprints()
	{
		Vector3 zero = Vector3.zero;
		int num = Math.Min(unlockedItems.Count - 1, 2);
		unlocked_marker = FindChild("unlocked_marker");
		for (int i = 0; i <= num; i++)
		{
			string iconTexture = (string)unlockedItems[i].Invariable["portrait"];
			SBGUILevelUpSlot sBGUILevelUpSlot = SBGUILevelUpSlot.Create(session, this, unlocked_marker, Vector3.zero + zero, iconTexture);
			float x = sBGUILevelUpSlot.FindChild("slot_background").GetComponent<YGSprite>().size.x;
			if (i != num)
			{
				zero += new Vector3((x + 5f) * 0.01f, 0.01f, 0f);
			}
		}
		CenterBlueprints(zero);
	}

	public void SetManagers(EntityManager emgr, ResourceManager resMgr, SoundEffectManager sfxMgr)
	{
		entityMgr = emgr;
		resourceMgr = resMgr;
		soundEffectMgr = sfxMgr;
	}

	private void SetLevelText(Session session, int level)
	{
		SBGUIShadowedLabel sBGUIShadowedLabel = (SBGUIShadowedLabel)FindChild("level_label");
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("population_label");
		SBGUIShadowedLabel component = FindChild("reward_label").GetComponent<SBGUIShadowedLabel>();
		SBGUIShadowedLabel component2 = FindChild("levelup_ribbon_label").GetComponent<SBGUIShadowedLabel>();
		SBGUILabel component3 = FindChild("headline_label").GetComponent<SBGUILabel>();
		SBGUIAtlasImage boundary = (SBGUIAtlasImage)FindChild("headline_label_boundary");
		SBGUILabel component4 = FindChild("levelheadline2_label").GetComponent<SBGUILabel>();
		SBGUILabel component5 = FindChild("unlocked_label").GetComponent<SBGUILabel>();
		sBGUIShadowedLabel.Text = Language.Get("!!PREFAB_LEVEL").ToUpper() + " " + level + "!";
		component.Text = Language.Get("!!PREFAB_REWARD");
		component2.Text = Language.Get("!!PREFAB_LEVELUP").ToUpper();
		component3.SetText(Language.Get(session.TheGame.levelingManager.Headline(level)));
		component3.AdjustText(boundary);
		component4.SetText(Language.Get("!!PREFAB_IS").ToUpper());
		component5.SetText(Language.Get("!!PREFAB_NOW_AVAILABLE").ToUpper());
		int residentPopulation = session.TheGame.GetResidentPopulation();
		sBGUILabel.SetText(Language.Get("!!PREFAB_POPULATION") + " " + residentPopulation.ToString().PadLeft(5, '0'));
		int length = component4.Text.Length;
		float num = 0.225f;
		sBGUIShadowedLabel.transform.Translate(new Vector3(num * (float)length, 0f, 0f), Space.Self);
	}

	private void SetLevelImage(Session session, int level)
	{
		SBGUIImage sBGUIImage = (SBGUIImage)FindChild("headline_image");
		string textureFromTexturePath = TextureLibrarian.PathLookUp(session.TheGame.levelingManager.HeadlineImage(level));
		sBGUIImage.SetTextureFromTexturePath(textureFromTexturePath);
	}

	private void SetLevelVoice(Session session, int level)
	{
		float delaySeconds = 1f;
		soundEffectMgr.PlaySound(session.TheGame.levelingManager.VoiceOver(level), delaySeconds);
	}

	public override void Deactivate()
	{
		unlockedItems.Clear();
		base.Deactivate();
	}

	private void AddItem(string texture, int amount)
	{
		if (amount == 0)
		{
			Debug.LogWarning("rewarding 0 of :" + texture);
			return;
		}
		if (rewards.Count >= 4)
		{
			Debug.LogWarning("only showing first 4 rewards");
			return;
		}
		if (texture == null || string.IsNullOrEmpty(texture.Trim()))
		{
			Debug.LogWarning("resource has no material");
			return;
		}
		SBGUIRewardWidget sBGUIRewardWidget = SBGUIRewardWidget.Create(rewardWidgetPrefab, rewardMarker, markerXOffset, texture, amount, string.Empty);
		sBGUIRewardWidget.tform.Rotate(0f, 0f, 8.94f);
		sBGUIRewardWidget.CreateTextStroke(Color.white);
		sBGUIRewardWidget.SetTextColor(Color.black);
		markerXOffset += (float)(sBGUIRewardWidget.Width + 5) * 0.01f;
		rewards.Add(sBGUIRewardWidget);
	}

	private void ClearItems()
	{
		markerXOffset = 0f;
		foreach (SBGUIRewardWidget reward in rewards)
		{
			reward.gameObject.SetActiveRecursively(false);
			UnityEngine.Object.Destroy(reward.gameObject);
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

	private void SetRewardIcons(Session session, List<Reward> rewards)
	{
		rewardMarker = FindChild("reward_marker");
		rewardCenter = rewardMarker.tform.position;
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
			AddItem(session.TheGame.resourceManager.Resources[key].GetResourceTexture(), value);
		}
		foreach (KeyValuePair<int, int> item2 in dictionary2)
		{
			int key2 = item2.Key;
			int value2 = item2.Value;
			Blueprint blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, key2);
			IDisplayController displayController = (IDisplayController)blueprint.Invariable["display"];
			AddItem(displayController.MaterialName, value2);
		}
		CenterRewards();
	}

	private void CenterBlueprints(Vector3 offset)
	{
		if (!(unlocked_marker == null))
		{
			unlocked_marker.transform.Translate((0f - offset.x) / 2f, 0f, 0f, Space.Self);
		}
	}

	private void CenterRewards()
	{
		if (!(rewardMarker == null))
		{
			Vector3 vector = rewardMarker.TotalBounds.center - rewardCenter;
			Vector3 localPosition = rewardMarker.tform.localPosition;
			localPosition.x -= vector.x;
			rewardMarker.tform.localPosition = localPosition;
		}
	}

	private IEnumerator AnimateSpinIn(float duration)
	{
		float normalizedTime = 0f;
		while (normalizedTime <= 1f)
		{
			normalizedTime += Time.deltaTime / duration;
			base.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, normalizedTime);
			base.transform.RotateAround(new Vector3(0f, 0f, 1f), -20f * Time.deltaTime);
			yield return null;
		}
		foreach (Transform child in windows.transform)
		{
			if (child.name != "unlocked_marker")
			{
				child.gameObject.SetActive(true);
			}
		}
		foreach (Transform child2 in windows.transform.Find("unlocked_marker"))
		{
			child2.GetComponent<Renderer>().enabled = true;
		}
		spinningPaper.SetActive(false);
		base.transform.localRotation = Quaternion.identity;
		if (spinningAudio != null)
		{
			spinningAudio.loop = false;
			spinningAudio.pitch = 1f;
			spinningAudio.volume = 1f;
			spinningAudio.Stop();
			Debug.LogError("spinningAudio killed normally");
		}
	}

	private new void OnDestroy()
	{
		Debug.LogError("OnDestroy is called to kill spinningAudio, spinningAudio.loop was " + spinningAudio.loop);
		if (spinningAudio != null)
		{
			spinningAudio.loop = false;
			spinningAudio.pitch = 1f;
			spinningAudio.volume = 1f;
			spinningAudio.Stop();
		}
	}
}
