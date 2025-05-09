using System.Collections.Generic;
using UnityEngine;

public interface AnimationEventNodeDelegate
{
	void SetupAnimationEvents(GameObject rootGameObject, AnimationClip clip, AnimationEventManager mgr);

	void InitializeWithData(Dictionary<string, object> dict);
}
