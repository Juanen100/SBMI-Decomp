using System;
using System.Collections.Generic;
using UnityEngine;

public class SBGUIArrowList : SBGUIElement
{
	public class ListItemData
	{
		public int m_nID { get; private set; }

		public string m_sTexture { get; private set; }

		public bool m_bLocked { get; private set; }

		public ListItemData(int nID, string sTexture, bool bLocked = false)
		{
		}
	}

	public SBGUIAtlasImage[] m_pAtlasImages;

	public int m_nSelectedAtlasImageIndex;

	public SBGUIButton m_pUpButton;

	public SBGUIButton m_pDownButton;

	public Color m_pNonSelectedColor;

	public GameObject m_pSingleItemParent;

	public GameObject m_pMultipleItemParent;

	public SBGUIAtlasImage m_pSingleItemImage;

	protected int m_nNumAtlasImages;

	protected List<ListItemData> m_pListItems;

	protected int m_nNumListItems;

	protected int m_nSelectedListItemIndex;

	protected Action<int> m_pSelectedItemChanged;

	protected Action<int> m_pItemClicked;

	protected Vector2[] m_pAtlasImageSizes;

	protected Vector2 m_pSingleItemImageSize;

	protected Session m_pSession;

	protected List<int> m_pIgnoreListItemIDs;

	private SBGUIAtlasImage[] m_pLockedImages;

	private SBGUIAtlasImage m_pSingleItemLockedImage;

	private SBGUIButton[] m_pItemButtons;

	private SBGUIButton m_pSingleItemButton;

	public virtual void SetData(Session pSession, List<ListItemData> pListItems, int nSelectedID, List<int> pIgnoreListItemIDs, Action<int> pSelectedItemChanged, Action<int> pItemClick)
	{
	}

	public void SetSelectedID(int nSelectedID)
	{
	}

	protected override void Awake()
	{
	}

	protected virtual void UpdateVisuals()
	{
	}

	protected void UpdateItemClicks()
	{
	}

	private void DownButtonPressed()
	{
	}

	private void UpButtonPressed()
	{
	}
}
