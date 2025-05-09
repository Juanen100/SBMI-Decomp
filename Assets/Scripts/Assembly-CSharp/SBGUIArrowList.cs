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
			m_nID = nID;
			if (string.IsNullOrEmpty(sTexture))
			{
				sTexture = "_blank_.png";
			}
			m_sTexture = sTexture;
			m_bLocked = bLocked;
		}
	}

	public SBGUIAtlasImage[] m_pAtlasImages;

	public int m_nSelectedAtlasImageIndex;

	public SBGUIButton m_pUpButton;

	public SBGUIButton m_pDownButton;

	public Color m_pNonSelectedColor = Color.white;

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
		m_pSession = pSession;
		m_nSelectedAtlasImageIndex = Mathf.Clamp(m_nSelectedAtlasImageIndex, 0, m_nNumAtlasImages - 1);
		m_pSelectedItemChanged = pSelectedItemChanged;
		m_pIgnoreListItemIDs = ((pIgnoreListItemIDs != null) ? pIgnoreListItemIDs : new List<int>());
		m_pListItems = pListItems;
		m_nNumListItems = m_pListItems.Count;
		m_nSelectedListItemIndex = 0;
		for (int i = 0; i < m_nNumListItems; i++)
		{
			if (pListItems[i].m_nID == nSelectedID)
			{
				m_nSelectedListItemIndex = i;
				break;
			}
		}
		m_pUpButton.ClearClickEvents();
		AttachActionToButton(m_pUpButton, delegate
		{
			UpButtonPressed();
		});
		m_pDownButton.ClearClickEvents();
		AttachActionToButton(m_pDownButton, delegate
		{
			DownButtonPressed();
		});
		m_pItemClicked = pItemClick;
		UpdateVisuals();
		UpdateItemClicks();
	}

	public void SetSelectedID(int nSelectedID)
	{
		if (m_pListItems == null)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < m_nNumListItems; i++)
		{
			if (m_pListItems[i].m_nID == nSelectedID)
			{
				num = i;
				break;
			}
		}
		if (m_nSelectedListItemIndex != num)
		{
			m_nSelectedListItemIndex = num;
			UpdateVisuals();
			UpdateItemClicks();
			if (m_pSelectedItemChanged != null)
			{
				m_pSelectedItemChanged(m_pListItems[m_nSelectedListItemIndex].m_nID);
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
		m_nNumAtlasImages = m_pAtlasImages.Length;
		m_pAtlasImageSizes = new Vector2[m_nNumAtlasImages];
		for (int i = 0; i < m_nNumAtlasImages; i++)
		{
			m_pAtlasImageSizes[i] = m_pAtlasImages[i].Size;
		}
		m_pItemButtons = new SBGUIButton[m_nNumAtlasImages];
		m_pLockedImages = new SBGUIAtlasImage[m_nNumAtlasImages];
		SBGUIElement sBGUIElement;
		for (int j = 0; j < m_nNumAtlasImages; j++)
		{
			sBGUIElement = FindChild("button_" + (j + 1));
			if (sBGUIElement != null)
			{
				m_pItemButtons[j] = sBGUIElement.GetComponent<SBGUIButton>();
			}
			else
			{
				m_pItemButtons[j] = null;
			}
			sBGUIElement = FindChild("locked_icon_" + (j + 1));
			if (sBGUIElement != null)
			{
				m_pLockedImages[j] = sBGUIElement.GetComponent<SBGUIAtlasImage>();
			}
			else
			{
				m_pLockedImages[j] = null;
			}
		}
		m_pSingleItemImageSize = Vector2.zero;
		if (m_pSingleItemImage != null)
		{
			m_pSingleItemImageSize = m_pSingleItemImage.Size;
		}
		m_pSingleItemLockedImage = null;
		sBGUIElement = FindChild("single_locked_icon");
		if (sBGUIElement != null)
		{
			m_pSingleItemLockedImage = sBGUIElement.GetComponent<SBGUIAtlasImage>();
		}
		m_pSingleItemButton = null;
		sBGUIElement = FindChild("single_button");
		if (sBGUIElement != null)
		{
			m_pSingleItemButton = sBGUIElement.GetComponent<SBGUIButton>();
		}
	}

	protected virtual void UpdateVisuals()
	{
		List<string> list = new List<string>();
		int i = m_nSelectedListItemIndex;
		while (i >= 0 && i < m_nNumListItems && m_pIgnoreListItemIDs.Contains(m_pListItems[i].m_nID))
		{
			i--;
		}
		if (i < 0)
		{
			for (i = 0; i >= 0 && i < m_nNumListItems && m_pIgnoreListItemIDs.Contains(m_pListItems[i].m_nID); i++)
			{
			}
		}
		if (i >= m_nNumListItems)
		{
			i = m_nNumListItems - 1;
		}
		if (i < 0)
		{
			i = 0;
		}
		m_nSelectedListItemIndex = i;
		int num = m_nSelectedAtlasImageIndex;
		string empty = string.Empty;
		SBGUIAtlasImage sBGUIAtlasImage = m_pAtlasImages[m_nSelectedAtlasImageIndex];
		YGAtlasSprite component = sBGUIAtlasImage.GetComponent<YGAtlasSprite>();
		if (m_nNumListItems > 0 && !m_pIgnoreListItemIDs.Contains(m_pListItems[m_nSelectedListItemIndex].m_nID))
		{
			if (!sBGUIAtlasImage.gameObject.active)
			{
				sBGUIAtlasImage.SetActive(true);
			}
			empty = m_pListItems[m_nSelectedListItemIndex].m_sTexture;
			SBGUIElement sBGUIElement = FindChildSessionActionId(empty, true);
			if (sBGUIElement != null)
			{
				sBGUIElement.SessionActionId = string.Empty;
			}
			if (string.IsNullOrEmpty(component.nonAtlasName))
			{
				if (!list.Contains(component.nonAtlasName))
				{
					list.Add(component.nonAtlasName);
				}
				else
				{
					base.View.Library.UnLoadTexture(component.nonAtlasName);
				}
			}
			sBGUIAtlasImage.SetSizeNoRebuild(m_pAtlasImageSizes[m_nSelectedAtlasImageIndex]);
			sBGUIAtlasImage.SetTextureFromAtlas(m_pListItems[m_nSelectedListItemIndex].m_sTexture, true, false, true);
			sBGUIAtlasImage.SessionActionId = empty;
			if (m_pListItems[m_nSelectedListItemIndex].m_bLocked)
			{
				sBGUIAtlasImage.SetColor(m_pNonSelectedColor);
				if (m_pLockedImages[m_nSelectedAtlasImageIndex] != null)
				{
					m_pLockedImages[m_nSelectedAtlasImageIndex].SetActive(true);
				}
			}
			else
			{
				if (m_pLockedImages[m_nSelectedAtlasImageIndex] != null)
				{
					m_pLockedImages[m_nSelectedAtlasImageIndex].SetActive(false);
				}
				sBGUIAtlasImage.SetColor(Color.white);
			}
		}
		else if (sBGUIAtlasImage.gameObject.active)
		{
			if (!string.IsNullOrEmpty(component.nonAtlasName))
			{
				base.View.Library.incrementTextureDuplicates(component.nonAtlasName);
			}
			sBGUIAtlasImage.SetActive(false);
		}
		bool flag = true;
		bool flag2 = false;
		do
		{
			num--;
			i--;
			if (i >= 0 && m_pIgnoreListItemIDs.Contains(m_pListItems[i].m_nID))
			{
				num++;
				continue;
			}
			sBGUIAtlasImage = null;
			if (num >= 0)
			{
				sBGUIAtlasImage = m_pAtlasImages[num];
			}
			if (i < 0)
			{
				flag = true;
				if (sBGUIAtlasImage != null && sBGUIAtlasImage.gameObject.active)
				{
					if (!string.IsNullOrEmpty(component.nonAtlasName))
					{
						base.View.Library.incrementTextureDuplicates(component.nonAtlasName);
					}
					sBGUIAtlasImage.SetActive(false);
				}
				continue;
			}
			flag = false;
			if (!(sBGUIAtlasImage != null))
			{
				continue;
			}
			if (!sBGUIAtlasImage.gameObject.active)
			{
				sBGUIAtlasImage.SetActive(true);
			}
			empty = m_pListItems[i].m_sTexture;
			SBGUIElement sBGUIElement = FindChildSessionActionId(empty, true);
			if (sBGUIElement != null)
			{
				sBGUIElement.SessionActionId = string.Empty;
			}
			if (string.IsNullOrEmpty(component.nonAtlasName))
			{
				if (!list.Contains(component.nonAtlasName))
				{
					list.Add(component.nonAtlasName);
				}
				else
				{
					base.View.Library.UnLoadTexture(component.nonAtlasName);
				}
			}
			sBGUIAtlasImage.SetSizeNoRebuild(m_pAtlasImageSizes[num]);
			sBGUIAtlasImage.SetTextureFromAtlas(m_pListItems[i].m_sTexture, true, false, true);
			sBGUIAtlasImage.SessionActionId = empty;
			if (m_pListItems[i].m_bLocked)
			{
				sBGUIAtlasImage.SetColor(m_pNonSelectedColor);
				if (m_pLockedImages[num] != null)
				{
					m_pLockedImages[num].SetActive(true);
				}
			}
			else
			{
				sBGUIAtlasImage.SetColor(Color.white);
				if (m_pLockedImages[num] != null)
				{
					m_pLockedImages[num].SetActive(false);
				}
			}
		}
		while (num > 0);
		num = m_nSelectedAtlasImageIndex;
		i = m_nSelectedListItemIndex;
		bool flag3 = true;
		do
		{
			num++;
			i++;
			if (i < m_nNumListItems && m_pIgnoreListItemIDs.Contains(m_pListItems[i].m_nID))
			{
				num--;
				continue;
			}
			sBGUIAtlasImage = null;
			if (num < m_nNumAtlasImages)
			{
				sBGUIAtlasImage = m_pAtlasImages[num];
			}
			if (i >= m_nNumListItems)
			{
				flag3 = true;
				if (sBGUIAtlasImage != null && sBGUIAtlasImage.gameObject.active)
				{
					if (!string.IsNullOrEmpty(component.nonAtlasName))
					{
						base.View.Library.incrementTextureDuplicates(component.nonAtlasName);
					}
					sBGUIAtlasImage.SetActive(false);
				}
				continue;
			}
			flag3 = false;
			if (!(sBGUIAtlasImage != null))
			{
				continue;
			}
			if (!sBGUIAtlasImage.gameObject.active)
			{
				sBGUIAtlasImage.SetActive(true);
			}
			empty = m_pListItems[i].m_sTexture;
			SBGUIElement sBGUIElement = FindChildSessionActionId(empty, true);
			if (sBGUIElement != null)
			{
				sBGUIElement.SessionActionId = string.Empty;
			}
			if (string.IsNullOrEmpty(component.nonAtlasName))
			{
				if (!list.Contains(component.nonAtlasName))
				{
					list.Add(component.nonAtlasName);
				}
				else
				{
					base.View.Library.UnLoadTexture(component.nonAtlasName);
				}
			}
			sBGUIAtlasImage.SetSizeNoRebuild(m_pAtlasImageSizes[num]);
			sBGUIAtlasImage.SetTextureFromAtlas(m_pListItems[i].m_sTexture, true, false, true);
			sBGUIAtlasImage.SessionActionId = empty;
			if (m_pListItems[i].m_bLocked)
			{
				sBGUIAtlasImage.SetColor(m_pNonSelectedColor);
				if (m_pLockedImages[num] != null)
				{
					m_pLockedImages[num].SetActive(true);
				}
			}
			else
			{
				sBGUIAtlasImage.SetColor(Color.white);
				if (m_pLockedImages[num] != null)
				{
					m_pLockedImages[num].SetActive(false);
				}
			}
		}
		while (num < m_nNumAtlasImages - 1);
		if (flag)
		{
			if (m_pUpButton.gameObject.active)
			{
				m_pUpButton.SetActive(false);
			}
		}
		else if (!m_pUpButton.gameObject.active)
		{
			m_pUpButton.SetActive(true);
		}
		if (flag3)
		{
			if (m_pDownButton.gameObject.active)
			{
				m_pDownButton.SetActive(false);
			}
		}
		else if (!m_pDownButton.gameObject.active)
		{
			m_pDownButton.SetActive(true);
		}
		if (flag && flag3)
		{
			bool flag4 = m_nNumListItems <= 0 || m_pIgnoreListItemIDs.Contains(m_pListItems[m_nSelectedListItemIndex].m_nID);
			if (m_pSingleItemParent != null)
			{
				if (flag4)
				{
					m_pSingleItemParent.SetActive(false);
				}
				else
				{
					m_pSingleItemParent.SetActive(true);
				}
			}
			if (m_pMultipleItemParent != null)
			{
				m_pMultipleItemParent.SetActive(false);
			}
			if (flag4 || !(m_pSingleItemImage != null))
			{
				return;
			}
			empty = m_pListItems[m_nSelectedListItemIndex].m_sTexture;
			SBGUIElement sBGUIElement = FindChildSessionActionId(empty, true);
			if (sBGUIElement != null)
			{
				sBGUIElement.SessionActionId = string.Empty;
			}
			m_pSingleItemImage.SetSizeNoRebuild(m_pSingleItemImageSize);
			m_pSingleItemImage.SetTextureFromAtlas(m_pListItems[m_nSelectedListItemIndex].m_sTexture, true, false, true);
			m_pSingleItemImage.SessionActionId = empty;
			if (m_pListItems[m_nSelectedListItemIndex].m_bLocked)
			{
				m_pSingleItemImage.SetColor(m_pNonSelectedColor);
				if (m_pSingleItemLockedImage != null)
				{
					m_pSingleItemLockedImage.SetActive(true);
				}
			}
			else
			{
				m_pSingleItemImage.SetColor(Color.white);
				if (m_pSingleItemLockedImage != null)
				{
					m_pSingleItemLockedImage.SetActive(false);
				}
			}
		}
		else
		{
			if (m_pSingleItemParent != null)
			{
				m_pSingleItemParent.SetActive(false);
			}
			if (m_pMultipleItemParent != null)
			{
				m_pMultipleItemParent.SetActive(true);
			}
		}
	}

	protected void UpdateItemClicks()
	{
		if (m_pItemClicked == null)
		{
			return;
		}
		for (int i = 0; i < m_nNumAtlasImages; i++)
		{
			SBGUIButton sBGUIButton = m_pItemButtons[i];
			if (!(sBGUIButton != null))
			{
				continue;
			}
			sBGUIButton.ClearClickEvents();
			int nLoopIndex = m_nSelectedListItemIndex - (m_nSelectedAtlasImageIndex - i);
			if (nLoopIndex >= 0 && nLoopIndex < m_nNumListItems)
			{
				Action action = delegate
				{
					ListItemData listItemData = m_pListItems[nLoopIndex];
					m_pItemClicked(listItemData.m_nID);
				};
				AttachActionToButton(sBGUIButton, action);
			}
		}
		if (!(m_pSingleItemButton != null))
		{
			return;
		}
		for (int num = 0; num < m_nNumListItems; num++)
		{
			if (!m_pIgnoreListItemIDs.Contains(m_pListItems[num].m_nID))
			{
				m_pSingleItemButton.ClearClickEvents();
				int nLoopIndex2 = num;
				Action action = delegate
				{
					ListItemData listItemData = m_pListItems[nLoopIndex2];
					m_pItemClicked(listItemData.m_nID);
				};
				AttachActionToButton(m_pSingleItemButton, action);
				break;
			}
		}
	}

	private void DownButtonPressed()
	{
		if (m_nSelectedListItemIndex < m_nNumListItems - 1)
		{
			m_nSelectedListItemIndex++;
			UpdateVisuals();
			UpdateItemClicks();
			if (m_pSelectedItemChanged != null)
			{
				m_pSelectedItemChanged(m_pListItems[m_nSelectedListItemIndex].m_nID);
			}
		}
	}

	private void UpButtonPressed()
	{
		if (m_nSelectedListItemIndex > 0)
		{
			m_nSelectedListItemIndex--;
			UpdateVisuals();
			UpdateItemClicks();
			if (m_pSelectedItemChanged != null)
			{
				m_pSelectedItemChanged(m_pListItems[m_nSelectedListItemIndex].m_nID);
			}
		}
	}
}
