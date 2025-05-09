using System.Collections.Generic;
using UnityEngine;

public class SBGUICharacterArrowList : SBGUIArrowList
{
	private SBGUIAtlasImage[] m_pWishImages;

	private SBGUIAtlasImage[] m_pWishBubbleImages;

	private Vector2[] m_pWishImagesSizes;

	private SBGUIAtlasImage m_pSingleWishImage;

	private SBGUIAtlasImage m_pSingleWishBubbleImage;

	private Vector2 m_pSingleWishImageSize;

	protected override void Awake()
	{
		base.Awake();
		m_pWishImages = new SBGUIAtlasImage[m_nNumAtlasImages];
		m_pWishBubbleImages = new SBGUIAtlasImage[m_nNumAtlasImages];
		m_pWishImagesSizes = new Vector2[m_nNumAtlasImages];
		for (int i = 0; i < m_nNumAtlasImages; i++)
		{
			SBGUIElement sBGUIElement = FindChild("wish_image_" + (i + 1));
			if (sBGUIElement != null)
			{
				m_pWishImages[i] = (SBGUIAtlasImage)sBGUIElement;
				m_pWishImagesSizes[i] = m_pWishImages[i].Size;
			}
			sBGUIElement = FindChild("wish_bubble_image_" + (i + 1));
			if (sBGUIElement != null)
			{
				m_pWishBubbleImages[i] = (SBGUIAtlasImage)sBGUIElement;
			}
		}
		m_pSingleWishBubbleImage = (SBGUIAtlasImage)FindChild("single_wish_bubble_image");
		m_pSingleWishImage = (SBGUIAtlasImage)FindChild("single_wish_image");
		m_pSingleWishImageSize = Vector2.zero;
		if (m_pSingleWishImage != null)
		{
			m_pSingleWishImageSize = m_pSingleWishImage.Size;
		}
	}

	protected override void UpdateVisuals()
	{
		if (m_pSession != null)
		{
			base.UpdateVisuals();
			UpdateWishIcons();
		}
	}

	private void Update()
	{
		if (m_pSession == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < m_nNumListItems; i++)
		{
			ListItemData listItemData = m_pListItems[i];
			List<Task> activeTasksForSimulated = m_pSession.TheGame.taskManager.GetActiveTasksForSimulated(listItemData.m_nID, null);
			if (activeTasksForSimulated == null || activeTasksForSimulated.Count <= 0 || activeTasksForSimulated[0].GetTimeLeft() == 0)
			{
				m_pListItems.RemoveAt(i);
				if (i == m_nSelectedListItemIndex)
				{
					m_nSelectedListItemIndex = Mathf.Clamp(m_nSelectedListItemIndex - 1, 0, m_nSelectedListItemIndex);
				}
				i--;
				m_nNumListItems--;
				flag = true;
			}
		}
		int num = m_pIgnoreListItemIDs.Count;
		for (int j = 0; j < num; j++)
		{
			int num2 = -1;
			ListItemData listItemData = null;
			for (int k = 0; k < m_nNumListItems; k++)
			{
				listItemData = m_pListItems[k];
				if (listItemData.m_nID == m_pIgnoreListItemIDs[j])
				{
					num2 = k;
					break;
				}
			}
			if (num2 >= 0)
			{
				List<Task> activeTasksForSimulated = m_pSession.TheGame.taskManager.GetActiveTasksForSimulated(listItemData.m_nID, null);
				if (activeTasksForSimulated != null && activeTasksForSimulated.Count > 0 && !activeTasksForSimulated[0].m_bMovingToTarget)
				{
					m_pIgnoreListItemIDs.RemoveAt(j);
					num--;
					j--;
					flag = true;
				}
			}
		}
		if (flag)
		{
			UpdateVisuals();
			UpdateItemClicks();
		}
		else
		{
			UpdateWishIcons();
		}
	}

	private void UpdateWishIcons()
	{
		int num = 0;
		int? nHungerResourceDID = null;
		if (m_pSingleItemParent != null && m_pSingleItemParent.activeSelf)
		{
			num = m_pListItems[m_nSelectedListItemIndex].m_nID;
			Simulated simulated = m_pSession.TheGame.simulation.FindSimulated(num);
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			nHungerResourceDID = entity.HungerResourceId;
			SetWishImagesForHungerDID(m_pSingleWishImage, m_pSingleWishImageSize, m_pSingleWishBubbleImage, nHungerResourceDID);
			return;
		}
		int num2 = m_nSelectedAtlasImageIndex;
		int num3 = m_nSelectedListItemIndex;
		if (m_nNumListItems > 0 && !m_pIgnoreListItemIDs.Contains(m_pListItems[m_nSelectedListItemIndex].m_nID))
		{
			num = m_pListItems[num3].m_nID;
			Simulated simulated = m_pSession.TheGame.simulation.FindSimulated(num);
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			nHungerResourceDID = entity.HungerResourceId;
		}
		if (num2 >= 0 && num2 < m_pWishImages.Length)
		{
			SetWishImagesForHungerDID(m_pWishImages[num2], m_pWishImagesSizes[num2], m_pWishBubbleImages[num2], nHungerResourceDID);
		}
		while (num2 > 0)
		{
			num2--;
			num3--;
			if (num3 >= 0 && m_pIgnoreListItemIDs.Contains(m_pListItems[num3].m_nID))
			{
				num2++;
				continue;
			}
			if (num3 < 0)
			{
				nHungerResourceDID = null;
			}
			else
			{
				num = m_pListItems[num3].m_nID;
				Simulated simulated = m_pSession.TheGame.simulation.FindSimulated(num);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				nHungerResourceDID = entity.HungerResourceId;
			}
			if (num2 >= 0 && num2 < m_pWishImages.Length)
			{
				SetWishImagesForHungerDID(m_pWishImages[num2], m_pWishImagesSizes[num2], m_pWishBubbleImages[num2], nHungerResourceDID);
			}
		}
		num2 = m_nSelectedAtlasImageIndex;
		num3 = m_nSelectedListItemIndex;
		while (num2 < m_nNumAtlasImages - 1)
		{
			num2++;
			num3++;
			if (num3 < m_nNumListItems && m_pIgnoreListItemIDs.Contains(m_pListItems[num3].m_nID))
			{
				num2--;
				continue;
			}
			if (num3 >= m_nNumListItems)
			{
				nHungerResourceDID = null;
			}
			else
			{
				num = m_pListItems[num3].m_nID;
				Simulated simulated = m_pSession.TheGame.simulation.FindSimulated(num);
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				nHungerResourceDID = entity.HungerResourceId;
			}
			if (num2 >= 0 && num2 < m_pWishImages.Length)
			{
				SetWishImagesForHungerDID(m_pWishImages[num2], m_pWishImagesSizes[num2], m_pWishBubbleImages[num2], nHungerResourceDID);
			}
		}
	}

	private void SetWishImagesForHungerDID(SBGUIAtlasImage pWishImage, Vector2 pWishImageSize, SBGUIAtlasImage pWishBubbleImage, int? nHungerResourceDID)
	{
		if (pWishImage != null)
		{
			if (nHungerResourceDID.HasValue)
			{
				Resource resource = m_pSession.TheGame.resourceManager.Resources[nHungerResourceDID.Value];
				if (!pWishImage.gameObject.active)
				{
					pWishImage.SetActive(true);
				}
				if (pWishImage.name != resource.GetResourceTexture())
				{
					pWishImage.SetSizeNoRebuild(pWishImageSize);
					pWishImage.SetTextureFromAtlas(resource.GetResourceTexture(), true, false, true);
				}
			}
			else if (pWishImage.gameObject.active)
			{
				pWishImage.SetActive(false);
			}
		}
		if (!(pWishBubbleImage != null))
		{
			return;
		}
		if (nHungerResourceDID.HasValue)
		{
			if (!pWishBubbleImage.gameObject.active)
			{
				pWishBubbleImage.SetActive(true);
			}
		}
		else if (pWishBubbleImage.gameObject.active)
		{
			pWishBubbleImage.SetActive(false);
		}
	}
}
