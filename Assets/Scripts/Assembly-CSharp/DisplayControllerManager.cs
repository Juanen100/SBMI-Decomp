public class DisplayControllerManager
{
	private RenderTextureManager renderTextureManager;

	private SkeletonCollection skeletons;

	public RenderTextureManager RenderTextureManager
	{
		get
		{
			return renderTextureManager;
		}
	}

	public SkeletonCollection Skeletons
	{
		get
		{
			return skeletons;
		}
	}

	public DisplayControllerManager()
	{
		renderTextureManager = new RenderTextureManager();
		skeletons = new SkeletonCollection();
		TerrainSlot.MakeRealtySignPrototype(this);
	}
}
