using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBGUICharacterDialog : SBGUIModalDialog
{
	public delegate float EasingFunc(float start, float end, float duration);

	private const float DELAY_BETWEEN_LETTERS = 0.025f;

	public List<DialogPrompt> prompts = new List<DialogPrompt>();

	private SBGUIAtlasImage characterIcon;

	private SBGUILabel dialogText;

	private SBGUIButton skipButton;

	private SBGUIAtlasImage speechBubble;

	private SBGUIAtlasImage dialogBoundary;

	private int dialogIndex = -1;

	private DialogPrompt currentPrompt;

	private bool currentlyTyping;

	private string localizedPrompt;

	private Action m_pSkipAction;

	public EventDispatcher<int> DialogChange = new EventDispatcher<int>();

	public bool autoPlay;

	private Vector3 m_pPortraitSize;

	private Vector3 characterPosition;

	private Bounds viewBounds;

	protected override void Awake()
	{
		characterIcon = (SBGUIAtlasImage)FindChild("character_icon");
		dialogText = (SBGUILabel)FindChild("dialog_label");
		skipButton = (SBGUIButton)FindChild("skip_button");
		speechBubble = (SBGUIAtlasImage)FindChild("speech_bubble");
		dialogBoundary = (SBGUIAtlasImage)FindChild("dialog_area_boundary");
		speechBubble.SetAlpha(0f);
		skipButton.SetAlpha(0f);
		dialogText.SetAlpha(0f);
		m_pPortraitSize = characterIcon.Size;
		m_pSkipAction = delegate
		{
			skipButton.MockClick();
		};
		base.Awake();
	}

	private void Start()
	{
		AttachAnalyticsToButton("skip", skipButton);
		skipButton.ClickEvent += delegate
		{
			if (currentlyTyping)
			{
				StopTyping();
				dialogText.SetText(localizedPrompt);
				AndroidBack.getInstance().pop(m_pSkipAction);
				AndroidBack.getInstance().push(m_pSkipAction, this);
			}
			else
			{
				ShowDialog(dialogIndex + 1);
			}
		};
		if (autoPlay && prompts.Count > 0)
		{
			StartSequence();
		}
		characterPosition = characterIcon.tform.localPosition;
		viewBounds = GUIMainView.GetInstance().ViewBounds();
	}

	private void StartSequence()
	{
		if (dialogIndex < 0)
		{
			dialogIndex = 0;
		}
		ShowDialog(0);
	}

	public void LoadSequence(List<object> sequence)
	{
		prompts.Clear();
		foreach (Dictionary<string, object> item in sequence)
		{
			prompts.Add(new DialogPrompt(item));
		}
		StartSequence();
	}

	public void ShowDialog(int index)
	{
		if (index >= prompts.Count || index < 0)
		{
			StartCoroutine(AnimateOut(0.2f, delegate
			{
				SetActive(false);
				dialogIndex = 0;
				DialogChange.FireEvent(-1);
			}));
			return;
		}
		AndroidBack.getInstance().pop(m_pSkipAction);
		AndroidBack.getInstance().push(m_pSkipAction, this);
		if (!IsActive())
		{
			SetActive(true);
		}
		dialogIndex = index;
		Action transitionComplete = delegate
		{
			localizedPrompt = Language.Get(currentPrompt.text);
			AdjustText();
			if (!string.IsNullOrEmpty(currentPrompt.voiceover) && session != null)
			{
				session.TheSoundEffectManager.PlaySound(currentPrompt.voiceover);
			}
			DialogChange.FireEvent(index);
			StartCoroutine("TypeText");
		};
		Action action = delegate
		{
			currentPrompt = prompts[index];
			StartCoroutine(AnimateIn(0.25f, transitionComplete));
		};
		if (currentPrompt == null)
		{
			action();
			return;
		}
		if (prompts[index].texture != currentPrompt.texture)
		{
			StartCoroutine(AnimateOut(0.2f, action));
			return;
		}
		currentPrompt = prompts[index];
		characterIcon.SetSizeNoRebuild(m_pPortraitSize);
		characterIcon.SetTextureFromAtlas(currentPrompt.texture, true, false, true);
		transitionComplete();
	}

	private IEnumerator AnimateOut(float duration, Action completeAction)
	{
		float interp = 0f;
		skipButton.SetActive(false);
		float width = characterIcon.Size.x * 0.01f;
		Vector3 dest = characterPosition;
		dest.x = viewBounds.min.x - width;
		while (interp <= 1f)
		{
			interp += Time.deltaTime / duration;
			dialogText.SetAlpha(Mathf.Lerp(0.5f, 0f, interp));
			speechBubble.SetAlpha(Mathf.Lerp(0.5f, 0f, interp));
			Vector3 interpPos = Vector3.Lerp(characterPosition, dest, interp);
			characterIcon.tform.localPosition = interpPos;
			yield return null;
		}
		characterIcon.tform.localPosition = dest;
		dialogText.SetAlpha(0f);
		speechBubble.SetAlpha(0f);
		if (completeAction != null)
		{
			completeAction();
		}
	}

	private IEnumerator AnimateIn(float duration, Action completeAction)
	{
		characterIcon.SetActive(false);
		yield return null;
		float interp = 0f;
		dialogText.SetText(string.Empty);
		float width = characterIcon.Size.x * 0.01f;
		Vector3 origin = characterPosition;
		origin.x = viewBounds.min.x - width;
		characterIcon.tform.localPosition = origin;
		characterIcon.SetActive(true);
		speechBubble.SetActive(true);
		characterIcon.SetSizeNoRebuild(m_pPortraitSize);
		characterIcon.SetTextureFromAtlas(currentPrompt.texture, true, false, true, true);
		while (interp <= 1f)
		{
			interp += Time.deltaTime / duration;
			Vector3 interpPos = Vector3.Lerp(origin, characterPosition, interp);
			speechBubble.SetAlpha(Mathf.Lerp(0f, 0.5f, interp));
			characterIcon.tform.localPosition = interpPos;
			yield return null;
		}
		characterIcon.tform.localPosition = characterPosition;
		skipButton.SetAlpha(0.5f);
		dialogText.SetAlpha(0.5f);
		if (completeAction != null)
		{
			completeAction();
		}
	}

	private void AdjustText()
	{
		dialogText.SetText(string.Empty);
		int num = 0;
		while (num < localizedPrompt.Length)
		{
			dialogText.Text += localizedPrompt[num++];
			if (!((float)dialogText.Width > dialogBoundary.Size.x))
			{
				continue;
			}
			string text = dialogText.Text;
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
				if (text[num2] == ' ')
				{
					localizedPrompt = localizedPrompt.Remove(num2, 1);
					localizedPrompt = localizedPrompt.Insert(num2, "|");
					dialogText.SetText(localizedPrompt.Substring(0, num));
					break;
				}
			}
		}
		dialogText.SetText(string.Empty);
	}

	private IEnumerator TypeText()
	{
		lock (this)
		{
			if (currentlyTyping)
			{
				StopTyping();
			}
		}
		currentlyTyping = true;
		dialogText.SetText(string.Empty);
		yield return null;
		int i = 0;
		while (i < localizedPrompt.Length)
		{
			dialogText.Text += localizedPrompt[i++];
			while (i < localizedPrompt.Length && (localizedPrompt[i] == ' ' || localizedPrompt[i] == '|'))
			{
				dialogText.Text += localizedPrompt[i++];
			}
			yield return new WaitForSeconds(0.025f);
		}
		currentlyTyping = false;
	}

	private void StopTyping()
	{
		StopCoroutine("TypeText");
		currentlyTyping = false;
	}

	protected override void OnDisable()
	{
		StopAllCoroutines();
		base.OnDisable();
	}
}
