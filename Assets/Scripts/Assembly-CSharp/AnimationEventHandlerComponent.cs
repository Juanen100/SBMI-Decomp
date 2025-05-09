using UnityEngine;

public class AnimationEventHandlerComponent : MonoBehaviour
{
	public AnimationEventHandlerDelegate animationEventHandlerDelegate;

	public void HandleAnimationEvent(AnimationEvent animationEvent)
	{
		if (animationEventHandlerDelegate != null)
		{
			animationEventHandlerDelegate.HandleAnimationEvent(animationEvent);
		}
	}
}
