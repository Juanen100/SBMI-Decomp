using System.Collections.Generic;
using UnityEngine;

public class SBGUITabBar : SBGUIElement
{
	public SBGUIScrollRegion scrollRegion;

	public string onTexture;

	public string offTexture;

	private SBGUITabButton selected;

	private SBGUITabButton[] buttons;

	private YGTextureLibrary.FoundMaterial foundOnMat;

	private YGTextureLibrary.FoundMaterial foundOffMat;

	public EventDispatcher<SBGUITabButton> TabChangeEvent = new EventDispatcher<SBGUITabButton>();

	public void SetupCategories(Dictionary<string, SBTabCategory> categories, Session session)
	{
		int num = 0;
		if (buttons != null)
		{
			int num2 = buttons.Length;
			for (int i = 0; i < num2; i++)
			{
				Object.Destroy(buttons[i].gameObject);
			}
			buttons = null;
		}
		if (buttons != null)
		{
			return;
		}
		buttons = new SBGUITabButton[categories.Count];
		foreach (SBTabCategory value in categories.Values)
		{
			if (value.MicroEventDID >= 0)
			{
				MicroEvent microEvent = session.TheGame.microEventManager.GetMicroEvent(value.MicroEventDID);
				if (microEvent == null || !microEvent.IsActive() || (value.MicroEventOnly && microEvent.IsCompleted()))
				{
					continue;
				}
			}
			buttons[num] = SBGUITabButton.CreateTabButton(this, value, num);
			num++;
		}
	}

	protected override void OnEnable()
	{
		YGTextureLibrary library = base.View.Library;
		foundOnMat = library.FindSpriteMaterial(onTexture);
		foundOffMat = library.FindSpriteMaterial(offTexture);
		base.OnEnable();
	}

	private void Start()
	{
		if (!(scrollRegion != null))
		{
			return;
		}
		scrollRegion.ReadyEvent.AddListener(delegate
		{
			if (buttons != null && buttons.Length > 0)
			{
				TabClick(buttons[0]);
			}
		});
	}

	public void TabClick(int index)
	{
		int num = buttons.Length;
		if (index >= 0 && index < num)
		{
			TabClick(buttons[index]);
		}
	}

	public void TabClick(SBGUITabButton button)
	{
		if (!(selected == button))
		{
			if (selected != null)
			{
				selected.SetTextureFromFound(foundOffMat);
				selected.Selected(false);
			}
			selected = button;
			selected.SetTextureFromFound(foundOnMat);
			selected.Selected(true);
			TabChangeEvent.FireEvent(selected);
		}
	}

	public SBGUITabButton FindButton(string name, bool includeInactive)
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			if (includeInactive)
			{
				if (buttons[i].name == name)
				{
					return buttons[i];
				}
			}
			else if (buttons[i].IsActive() && buttons[i].name == name)
			{
				return buttons[i];
			}
		}
		return null;
	}
}
