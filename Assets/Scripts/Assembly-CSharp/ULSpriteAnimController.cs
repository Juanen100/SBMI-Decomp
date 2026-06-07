using UnityEngine;

public class ULSpriteAnimController : ULAnimControllerInterface
{
	public bool animate;

	public int[] uvToVertMap;

	public ULSpriteAnimModelInterface animationModel;

	public ULSpriteAnimationSetting currentAnimationSetting;

	public MeshFilter quad;

	public Material spriteMaterial;

	private Vector2[] uvs;

	private float frame;

	private float elapsed;

	private float seconds_per_frame;

	private Material material;

	private Vector2[] uvOrder;

	protected void StartAnim()
	{
	}

	public void OnUpdate()
	{
	}

	private void SetupSprite()
	{
	}

	private void CreateMaterial(MeshRenderer mr, ULSpriteAnimationSetting cs)
	{
	}

	private void UpdateSprite()
	{
	}

	public bool HasAnimation(string animationName)
	{
		return false;
	}

	public bool AnimationEnabled()
	{
		return false;
	}

	public void EnableAnimation(bool enabled)
	{
	}

	private void ApplyAnimation(string animationName)
	{
	}

	public void PlayAnimation(string animationName)
	{
	}

	public void StopAnimation(string animationName)
	{
	}

	public void StopAnimations()
	{
	}

	public void Sample(string animationName, float position)
	{
	}

	public float NormalizedTimePerFrame(string animationName)
	{
		return 0f;
	}
}
