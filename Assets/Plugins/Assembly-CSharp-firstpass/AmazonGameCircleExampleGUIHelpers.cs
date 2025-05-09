using UnityEngine;

public static class AmazonGameCircleExampleGUIHelpers
{
	private const float foldoutButtonWidth = 48f;

	private const float foldoutButtonHeight = 48f;

	private const float sliderMinMaxValuesLabelWidth = 75f;

	private const float uiHeight = 48f;

	private const float uiSliderWidth = 48f;

	private const float uiSliderHeight = 48f;

	private const float uiScrollBarWidth = 48f;

	private const float menuPadding = 0.075f;

	private static readonly Color foldoutOpenColor = new Color(0.2f, 0.2f, 0.2f, 1f);

	public static void SetGUISkinTouchFriendly(GUISkin skin)
	{
		skin.button.fixedHeight = 48f;
		skin.label.fixedHeight = 48f;
		skin.textField.fixedHeight = 48f;
		skin.horizontalSlider.fixedHeight = 48f;
		skin.toggle.fixedHeight = 48f;
		skin.horizontalSlider.fixedHeight = 48f;
		skin.horizontalSliderThumb.fixedHeight = 48f;
		skin.horizontalSliderThumb.fixedWidth = 48f;
		skin.verticalScrollbar.fixedWidth = 48f;
		skin.verticalScrollbarThumb.fixedWidth = 48f;
	}

	public static void CenteredLabel(string text, params GUILayoutOption[] options)
	{
		AnchoredLabel(text, TextAnchor.MiddleCenter, options);
	}

	public static void AnchoredLabel(string text, TextAnchor alignment, params GUILayoutOption[] options)
	{
		TextAnchor alignment2 = GUI.skin.label.alignment;
		GUI.skin.label.alignment = alignment;
		GUILayout.Label(text, options);
		GUI.skin.label.alignment = alignment2;
	}

	public static bool FoldoutWithLabel(bool currentValue, string label)
	{
		GUILayout.BeginHorizontal();
		Color color = GUI.color;
		if (currentValue)
		{
			GUI.color = foldoutOpenColor;
		}
		if (FoldoutButton())
		{
			currentValue = !currentValue;
		}
		GUI.color = color;
		AnchoredLabel(label, TextAnchor.UpperCenter);
		GUILayout.Label(GUIContent.none, GUILayout.Width(48f));
		GUILayout.EndHorizontal();
		return currentValue;
	}

	public static void BoxedCenteredLabel(string text)
	{
		GUILayout.BeginHorizontal(GUI.skin.box);
		CenteredLabel(text);
		GUILayout.EndHorizontal();
	}

	public static float DisplayCenteredSlider(float currentValue, float minValue, float maxValue, string valueDisplayString)
	{
		GUILayout.BeginHorizontal();
		AnchoredLabel(string.Format(valueDisplayString, minValue), TextAnchor.UpperCenter, GUILayout.Width(75f));
		GUILayout.BeginVertical();
		currentValue = GUILayout.HorizontalSlider(currentValue, minValue, maxValue);
		AnchoredLabel(string.Format(valueDisplayString, currentValue), TextAnchor.UpperCenter);
		GUILayout.EndVertical();
		AnchoredLabel(string.Format(valueDisplayString, maxValue), TextAnchor.UpperCenter, GUILayout.Width(75f));
		GUILayout.EndHorizontal();
		return currentValue;
	}

	public static void BeginMenuLayout()
	{
		GUILayout.BeginHorizontal(GUILayout.Width(Screen.width), GUILayout.Height(Screen.height));
		GUILayout.BeginVertical(GUILayout.Width((float)Screen.width * 0.075f));
		GUILayout.Label(GUIContent.none, GUILayout.Width((float)Screen.width * 0.075f));
		GUILayout.EndVertical();
		GUILayout.BeginVertical(GUI.skin.box);
	}

	public static void EndMenuLayout()
	{
		GUILayout.EndVertical();
		GUILayout.BeginVertical(GUILayout.Width((float)Screen.width * 0.075f));
		GUILayout.Label(GUIContent.none, GUILayout.Width((float)Screen.width * 0.075f));
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	private static bool FoldoutButton()
	{
		float fixedHeight = GUI.skin.button.fixedHeight;
		GUI.skin.button.fixedHeight = 48f;
		bool result = GUILayout.Button(GUIContent.none, GUILayout.Width(48f), GUILayout.Height(48f));
		GUI.skin.button.fixedHeight = fixedHeight;
		return result;
	}
}
