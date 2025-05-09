using UnityEngine;

public class HelpshiftNotificationCount : MonoBehaviour
{
	private int m_iCount;

	public int Count
	{
		get
		{
			return m_iCount;
		}
	}

	public HelpshiftNotificationCount()
	{
		m_iCount = 0;
	}

	public void didReceiveInAppNotificationCount(string count)
	{
		int result = 0;
		int.TryParse(count, out result);
		m_iCount = result;
		UpdateCountNumber();
	}

	public void ResetCount()
	{
		m_iCount = 0;
		UpdateCountNumber();
	}

	public void UpdateCountNumber()
	{
		SBGUIStandardScreen sBGUIStandardScreen = null;
		SBGUIScreen[] componentsInChildren = GameObject.Find("GUIMainView").GetComponentsInChildren<SBGUIScreen>();
		SBGUIScreen[] array = componentsInChildren;
		foreach (SBGUIScreen sBGUIScreen in array)
		{
			if (sBGUIScreen.name.Contains("StandardUI"))
			{
				sBGUIStandardScreen = (SBGUIStandardScreen)sBGUIScreen;
			}
		}
		if (sBGUIStandardScreen != null)
		{
			sBGUIStandardScreen.HelpshiftNotificationCount = m_iCount;
			sBGUIStandardScreen.ShowHelpshiftNotification();
		}
	}
}
