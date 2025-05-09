#define ASSERTS_ON
using UnityEngine;

public class TextPromptSpawn : SessionActionSpawn
{
	private UiSpawnMixin uiMixin = new UiSpawnMixin();

	private static SessionActionTextPromptPrefab cachedPrefab;

	private SessionActionTextPromptPrefab prompt;

	private int instanceCount;

	private TextPrompt.Anchor anchorTarget;

	public void Spawn(Game game, SessionActionTracker parentAction, SBGUIScreen parentScreen, string text, TextPrompt.Anchor anchor)
	{
		TextPromptSpawn textPromptSpawn = new TextPromptSpawn();
		textPromptSpawn.RegisterNewInstance(game, parentAction, parentScreen, text, anchor);
	}

	protected void RegisterNewInstance(Game game, SessionActionTracker parentAction, SBGUIScreen parentScreen, string text, TextPrompt.Anchor anchor)
	{
		base.RegisterNewInstance(game, parentAction);
		uiMixin.OnRegisterNewInstance(parentAction, parentScreen);
		prompt = GetPrefab();
		prompt.SetParent(parentScreen);
		prompt.transform.localPosition = Vector3.zero;
		prompt.SetLabel(text);
		anchorTarget = anchor;
		prompt.SetAnchoredPosition(anchor);
		if (!parentAction.ManualSuccess)
		{
			prompt.SetClickCallback(delegate
			{
				parentAction.MarkSucceeded(false);
			});
		}
	}

	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		if (prompt != null)
		{
			prompt.SetAnchoredPosition(anchorTarget);
		}
		return base.OnUpdate(game);
	}

	public override void Destroy()
	{
		uiMixin.Destroy();
		if (prompt != null)
		{
			prompt.MuteButtons(false);
			prompt.SetParent(null);
			prompt.SetActive(false);
		}
		instanceCount--;
	}

	private SessionActionTextPromptPrefab GetPrefab()
	{
		TFUtils.Assert(instanceCount < 1, "Text Prompts only support 1 instance at a time!");
		if (cachedPrefab == null && instanceCount < 1)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Prefabs/GUI/Widgets/TextPrompt"));
			cachedPrefab = gameObject.GetComponent<SessionActionTextPromptPrefab>();
		}
		else
		{
			cachedPrefab.MuteButtons(false);
		}
		cachedPrefab.gameObject.SetActiveRecursively(true);
		instanceCount++;
		return cachedPrefab;
	}
}
