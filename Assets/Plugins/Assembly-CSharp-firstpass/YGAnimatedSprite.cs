using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class YGAnimatedSprite : YGSprite
{
	[Serializable]
	public class FrameLayout
	{
		public Vector2 size = new Vector2(50f, 50f);

		public Vector2 layout = new Vector2(4f, 3f);

		public int count;
	}

	public FrameLayout frameLayout;

	public int framesPerSecond = 10;

	public bool startAutomatically = true;

	protected int currentFrame;

	protected Rect[] frames;

	public WrapMode wrapMode = WrapMode.Loop;

	private float sleep;

	private Func<IEnumerator> animFunc;

	public bool IsPlaying { get; protected set; }

	protected override void OnEnable()
	{
		IsPlaying = false;
		base.OnEnable();
		sleep = 1f / (float)framesPerSecond;
	}

	protected override void OnDisable()
	{
		StopAnimation();
		base.OnDisable();
	}

	private void Start()
	{
		if (frames == null)
		{
			Load();
		}
		if (startAutomatically)
		{
			StartAnimation();
		}
	}

	public void StartAnimation()
	{
		switch (wrapMode)
		{
		case WrapMode.Once:
		case WrapMode.ClampForever:
			animFunc = AnimateClamp;
			break;
		case WrapMode.Default:
			animFunc = AnimateDefault;
			break;
		case WrapMode.Loop:
			animFunc = AnimateLoop;
			break;
		case WrapMode.PingPong:
			animFunc = AnimatePingPong;
			break;
		}
		StartCoroutine(animFunc());
	}

	public void StopAnimation()
	{
		StopAllCoroutines();
	}

	private IEnumerator PlayForward(int startFrame)
	{
		for (int i = startFrame; i < frames.Length; i++)
		{
			if (!IsPlaying)
			{
				break;
			}
			currentFrame = i;
			SetFrame(i);
			yield return new WaitForSeconds(sleep);
		}
	}

	private IEnumerator PlayBackward(int startFrame)
	{
		int i = startFrame;
		while (i > 0 && IsPlaying)
		{
			currentFrame = i;
			SetFrame(i);
			yield return new WaitForSeconds(sleep);
			i--;
		}
	}

	private IEnumerator AnimateDefault()
	{
		IsPlaying = true;
		yield return StartCoroutine(PlayForward(0));
		SetFrame(0);
		IsPlaying = false;
	}

	private IEnumerator AnimateClamp()
	{
		IsPlaying = true;
		yield return StartCoroutine(PlayForward(0));
		IsPlaying = false;
	}

	private IEnumerator AnimateLoop()
	{
		IsPlaying = true;
		while (IsPlaying)
		{
			yield return StartCoroutine(PlayForward(0));
		}
	}

	private IEnumerator AnimatePingPong()
	{
		IsPlaying = true;
		while (IsPlaying)
		{
			yield return StartCoroutine(PlayForward(0));
			yield return StartCoroutine(PlayBackward(frames.Length - 2));
		}
	}

	public override void Load()
	{
		if (frameLayout.count <= 0)
		{
			frameLayout.count = (int)(frameLayout.layout.x * frameLayout.layout.y);
		}
		frames = new Rect[frameLayout.count];
		int num = 0;
		float num2 = frameLayout.size.x / textureSize.x;
		float num3 = frameLayout.size.y / textureSize.y;
		for (int i = 0; (float)i < frameLayout.layout.y; i++)
		{
			for (int j = 0; (float)j < frameLayout.layout.x; j++)
			{
				frames[num] = new Rect((float)j * num2, (float)i * num3, num2, num3);
				num++;
				if (num >= frameLayout.count)
				{
					break;
				}
			}
		}
		base.Load();
	}

	public override void AssembleMesh()
	{
		MeshUpdate meshUpdate = new MeshUpdate();
		YGSprite.BuildVerts(size, scale, ref verts);
		YGSprite.BuildColors(color, ref colors);
		meshUpdate.verts = verts;
		meshUpdate.normals = normals;
		meshUpdate.colors = colors;
		meshUpdate.tris = YGSprite.BuildTris();
		meshUpdate.uvs = FrameUVs(currentFrame);
		UpdateMesh(meshUpdate);
	}

	public Vector2[] FrameUVs(int frame)
	{
		if (frames == null || frames.Length <= frame)
		{
			Debug.LogWarning(string.Format("frame {0} out of range", frame));
			return null;
		}
		Rect rect = frames[frame];
		return new Vector2[4]
		{
			new Vector2(rect.xMin, 1f - rect.yMin),
			new Vector2(rect.xMax, 1f - rect.yMin),
			new Vector2(rect.xMin, 1f - rect.yMax),
			new Vector2(rect.xMax, 1f - rect.yMax)
		};
	}

	protected void SetFrame(int frame)
	{
		MeshUpdate meshUpdate = new MeshUpdate();
		meshUpdate.uvs = FrameUVs(frame);
		if (meshUpdate.uvs != null)
		{
			UpdateMesh(meshUpdate);
		}
	}
}
