#define ASSERTS_ON
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBGUIInventoryScreen : SBGUITabbedScrollableDialog
{
	public GameObject slotPrefab;

	public EventDispatcher<SBInventoryItem> BuildingSlotClickedEvent = new EventDispatcher<SBInventoryItem>();

	public EventDispatcher<SBInventoryItem> MovieSlotClickedEvent = new EventDispatcher<SBInventoryItem>();

	protected override Vector2 GetSlotSize()
	{
		YGSprite component = slotPrefab.GetComponent<YGSprite>();
		return component.size * 0.01f;
	}

	public float GetMainWindowZ()
	{
		return FindChild("window").transform.localPosition.z;
	}

	protected override void LoadCategories(Session session)
	{
		categories = new Dictionary<string, SBTabCategory>();
		Catalog catalog = session.TheGame.catalog;
		foreach (object item in (List<object>)catalog.CatalogDict["inventory"])
		{
			SBInventoryCategory sBInventoryCategory = new SBInventoryCategory((Dictionary<string, object>)item);
			categories[sBInventoryCategory.Name] = sBInventoryCategory;
			AddCategory(sBInventoryCategory, session, sBInventoryCategory.Name, sBInventoryCategory.Type, sBInventoryCategory.Texture);
		}
	}

	private void AddCategory(SBInventoryCategory category, Session session, string name, string type, string texture)
	{
		category.Name = name;
		category.Type = type;
		category.Texture = texture;
		switch (type)
		{
		case "building":
			category.NumItems = session.TheGame.inventory.GetNumUniqueItems();
			break;
		case "movie":
			category.NumItems = session.TheGame.movieManager.UnlockedMovies.Count;
			break;
		default:
			TFUtils.Assert(false, "Unknown category type: " + type);
			break;
		}
	}

	protected override IEnumerator BuildTabCoroutine(string tabName)
	{
		if (!categories.ContainsKey(tabName))
		{
			TFUtils.WarningLog(string.Format("Category {0} not found: ", tabName));
			yield break;
		}
		ClearCachedSlotInfos();
		region.ClearSlotActions();
		if (!tabContents.TryGetValue(tabName, out currentTab))
		{
			SBGUIElement anchor = SBGUIElement.Create(region.Marker);
			anchor.name = string.Format(tabName);
			anchor.transform.localPosition = Vector3.zero;
			tabContents[tabName] = anchor;
			currentTab = anchor;
		}
		yield return null;
		SBTabCategory category = categories[tabName];
		LoadSlotInfo(category, currentTab);
	}

	private void LoadSlotInfo(SBTabCategory tabCategory, SBGUIElement anchor)
	{
		SBInventoryCategory sBInventoryCategory = (SBInventoryCategory)tabCategory;
		int num = 0;
		if (sBInventoryCategory.Type == "building")
		{
			List<SBInventoryItem> items = session.TheGame.inventory.GetItems();
			items.Sort();
			foreach (SBInventoryItem item in items)
			{
				region.SetupSlotActions.Insert(num, SetupSlotClosure(session, anchor, item, BuildingSlotClickedEvent, GetSlotOffset(num)));
				string text = SBGUIInventorySlot.CalculateSlotName(item);
				if (sessionActionIdSearchRequests.Contains(text))
				{
					sessionActionSlotMap[text] = num;
				}
				num++;
			}
		}
		else if (sBInventoryCategory.Type == "movie")
		{
			foreach (int unlockedMovie in session.TheGame.movieManager.UnlockedMovies)
			{
				MovieInfo movieInfoById = session.TheGame.movieManager.GetMovieInfoById(unlockedMovie);
				SBInventoryItem invItem = new SBInventoryItem(null, null, "movie", movieInfoById.Name, movieInfoById.Description, movieInfoById.MovieInfoTexture, false, movieInfoById.MovieFile);
				region.SetupSlotActions.Insert(num, SetupSlotClosure(session, anchor, invItem, MovieSlotClickedEvent, GetSlotOffset(num)));
				string text2 = SBGUIInventorySlot.CalculateSlotName(invItem);
				if (sessionActionIdSearchRequests.Contains(text2))
				{
					sessionActionSlotMap[text2] = num;
				}
				num++;
			}
		}
		PostLoadRegionContentInfo(region.SetupSlotActions.Count);
	}

	protected override SBGUIScrollListElement MakeSlot()
	{
		return SBGUIInventorySlot.MakeInventorySlot();
	}

	protected override Rect CalculateTabContentsSize(string tabName)
	{
		SBInventoryCategory sBInventoryCategory = (SBInventoryCategory)categories[tabName];
		return CalculateScrollRegionSize(sBInventoryCategory.NumItems);
	}

	private Action<SBGUIScrollListElement> SetupSlotClosure(Session session, SBGUIElement anchor, SBInventoryItem invItem, EventDispatcher<SBInventoryItem> itemClickedEvent, Vector3 offset)
	{
		return delegate(SBGUIScrollListElement slot)
		{
			((SBGUIInventorySlot)slot).Setup(session, anchor, invItem, itemClickedEvent, offset);
		};
	}
}
