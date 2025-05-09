using UnityEngine;

public class UVMapTree
{
	public class UVMapBranch : UVMapNode
	{
		public UVMapNode[] Nodes = new UVMapNode[4];

		public UVMapBranch(UVMapTree tree)
		{
			Reset(tree);
		}

		public void Reset(UVMapTree tree)
		{
			for (int i = 0; i < Nodes.Length; i++)
			{
				if (Nodes[i] != null)
				{
					if (Nodes[i].IsLeaf())
					{
						tree.ReturnLeaf((UVMapLeaf)Nodes[i]);
					}
					else
					{
						tree.ReturnBranch((UVMapBranch)Nodes[i]);
					}
				}
				Nodes[i] = tree.GetLeaf();
			}
		}

		public override bool AddTexture(UVMapTree tree, int stepX, int stepY, int nDepth)
		{
			Debug.Log(string.Concat("AddTexture: Test: ", tree.LastTextureSize, " at depth ", tree.NodeLayers[nDepth].ToString()));
			if (nDepth + 1 >= tree.NodeLayers.Length)
			{
				Debug.LogError("Invalid Texture Depth Reached: > " + tree.NodeLayers[tree.NodeLayers.Length - 1].ToString());
				return false;
			}
			if (CullNode(tree, nDepth + 1))
			{
				Debug.Log("Cull Branch");
				return false;
			}
			int num = nDepth + 1;
			int num2 = 0;
			int num3 = Nodes.Length;
			for (int i = 0; i < num3; i++)
			{
				UVMapNode uVMapNode = Nodes[i];
				if (uVMapNode.Clip)
				{
					Debug.Log("Clip Leaf");
					num2++;
					continue;
				}
				if (uVMapNode.IsLeaf())
				{
					Debug.Log(string.Concat("TestLeaf: ", i, " for: ", tree.LastTextureSize, " at: ", tree.NodeLayers[num]));
					if (uVMapNode.IsBestFit(tree, num))
					{
						Debug.Log("Added Texture: " + tree.NodeLayers[num]);
						tree.lastFoundUV.x = (float)stepX + tree.NodeLayers[num].x * (float)(int)tree.UVAdjust[i].x;
						tree.lastFoundUV.y = (float)stepY + tree.NodeLayers[num].y * (float)(int)tree.UVAdjust[i].y;
						uVMapNode.Clip = true;
						return true;
					}
					Debug.Log("Add Branch: " + tree.NodeLayers[nDepth]);
					uVMapNode = tree.ExchangeLeaf((UVMapLeaf)uVMapNode);
					Nodes[i] = uVMapNode;
				}
				if (uVMapNode.AddTexture(tree, (int)((float)stepX + tree.NodeLayers[num].x * (float)(int)tree.UVAdjust[i].x), (int)((float)stepY + tree.NodeLayers[num].y * (float)(int)tree.UVAdjust[i].y), num))
				{
					return true;
				}
			}
			if (num2 >= num3)
			{
				Debug.LogWarning("Clip Branch");
				Clip = true;
				return false;
			}
			Debug.Log("Failed: " + tree.NodeLayers[nDepth]);
			return false;
		}
	}

	public class UVMapLeaf : UVMapNode
	{
		public override bool IsLeaf()
		{
			return true;
		}

		public override bool IsBestFit(UVMapTree tree, int nodeSizeIndex)
		{
			return tree.LastTextureSize.x <= tree.NodeLayers[nodeSizeIndex].x && tree.LastTextureSize.y <= tree.NodeLayers[nodeSizeIndex].y && (tree.LastTextureSize.x > tree.NodeLayers[nodeSizeIndex].x * 0.5f || tree.LastTextureSize.y > tree.NodeLayers[nodeSizeIndex].y * 0.5f);
		}
	}

	public class UVMapNode
	{
		public bool Clip;

		protected UVMapNode()
		{
		}

		public virtual bool IsLeaf()
		{
			return false;
		}

		public virtual bool IsBranch()
		{
			return !IsLeaf();
		}

		public virtual bool CullNode(UVMapTree tree, int nodeSizeIndex)
		{
			return tree.LastTextureSize.x > tree.NodeLayers[nodeSizeIndex].x || tree.LastTextureSize.y > tree.NodeLayers[nodeSizeIndex].y;
		}

		public virtual bool IsBestFit(UVMapTree tree, int nodeSizeIndex)
		{
			return false;
		}

		public virtual bool AddTexture(UVMapTree tree, int stepX, int stepY, int nDepth)
		{
			return false;
		}
	}

	protected Vector2[] NodeLayers;

	protected UVMapNode RootNode;

	protected Vector3 LastTextureSize;

	protected Vector2[] UVAdjust;

	protected Vector2 lastFoundUV;

	public UVMapTree(Vector2 initialSize, int depth = -1)
	{
		if (initialSize.x != initialSize.y)
		{
			Debug.LogError("UVMapTree initial size X must same as Y");
			return;
		}
		LastTextureSize = default(Vector3);
		if (depth <= 0)
		{
			depth = 1;
			int num = (int)initialSize.x;
			while (true)
			{
				num = (int)((float)num * 0.5f);
				if (num < 4)
				{
					break;
				}
				depth++;
			}
		}
		NodeLayers = new Vector2[depth];
		int num2 = (int)initialSize.x;
		for (int i = 0; i < depth; i++)
		{
			NodeLayers[i] = new Vector2(num2, num2);
			num2 /= 2;
		}
		RootNode = GetBranch();
		UVAdjust = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
	}

	public bool AddTexture(Vector2 textureSize, ref Vector2 uvs)
	{
		LastTextureSize.x = textureSize.x;
		LastTextureSize.y = textureSize.y;
		bool result = RootNode.AddTexture(this, 0, 0, 0);
		uvs = lastFoundUV;
		return result;
	}

	protected void ReturnLeaf(UVMapLeaf leaf)
	{
	}

	protected void ReturnBranch(UVMapBranch branch)
	{
	}

	protected UVMapBranch ExchangeLeaf(UVMapLeaf leaf)
	{
		ReturnLeaf(leaf);
		return GetBranch();
	}

	protected UVMapBranch GetBranch()
	{
		return new UVMapBranch(this);
	}

	protected UVMapLeaf GetLeaf()
	{
		return new UVMapLeaf();
	}
}
