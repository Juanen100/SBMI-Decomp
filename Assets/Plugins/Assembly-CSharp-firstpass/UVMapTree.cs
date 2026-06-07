using UnityEngine;

public class UVMapTree
{
	public class UVMapBranch : UVMapNode
	{
		public UVMapNode[] Nodes;

		public UVMapBranch(UVMapTree tree)
		{
		}

		public void Reset(UVMapTree tree)
		{
		}

		public override bool AddTexture(UVMapTree tree, int stepX, int stepY, int nDepth)
		{
			return false;
		}
	}

	public class UVMapLeaf : UVMapNode
	{
		public override bool IsLeaf()
		{
			return false;
		}

		public override bool IsBestFit(UVMapTree tree, int nodeSizeIndex)
		{
			return false;
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
			return false;
		}

		public virtual bool CullNode(UVMapTree tree, int nodeSizeIndex)
		{
			return false;
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
	}

	public bool AddTexture(Vector2 textureSize, ref Vector2 uvs)
	{
		return false;
	}

	protected void ReturnLeaf(UVMapLeaf leaf)
	{
	}

	protected void ReturnBranch(UVMapBranch branch)
	{
	}

	protected UVMapBranch ExchangeLeaf(UVMapLeaf leaf)
	{
		return null;
	}

	protected UVMapBranch GetBranch()
	{
		return null;
	}

	protected UVMapLeaf GetLeaf()
	{
		return null;
	}
}
