public class WorkableDecorator : EntityDecorator
{
	private Identity worker;

	public Identity Worker
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public WorkableDecorator(Entity toDecorate)
		: base(null)
	{
	}
}
