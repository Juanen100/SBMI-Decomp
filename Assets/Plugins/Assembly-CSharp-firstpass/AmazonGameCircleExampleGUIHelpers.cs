using UnityEngine;

public static class AmazonGameCircleExampleGUIHelpers
{
	private static readonly Color foldoutOpenColor;

	private const float foldoutButtonWidth = 48f;

	private const float foldoutButtonHeight = 48f;

	private const float sliderMinMaxValuesLabelWidth = 75f;

	private const float uiHeight = 48f;

	private const float uiSliderWidth = 48f;

	private const float uiSliderHeight = 48f;

	private const float uiScrollBarWidth = 48f;

	private const float menuPadding = 0.075f;

	public static void SetGUISkinTouchFriendly(GUISkin skin)
	{
	}

	public static void CenteredLabel(string text, params GUILayoutOption[] options)
	{
	}

	public static void AnchoredLabel(string text, TextAnchor alignment, params GUILayoutOption[] options)
	{
	}

	public static bool FoldoutWithLabel(bool currentValue, string label)
	{
		return false;
	}

	public static void BoxedCenteredLabel(string text)
	{
	}

	public static float DisplayCenteredSlider(float currentValue, float minValue, float maxValue, string valueDisplayString)
	{
		return 0f;
	}

	public static void BeginMenuLayout()
	{
	}

	public static void EndMenuLayout()
	{
	}

	private static bool FoldoutButton()
	{
		return false;
	}
}
