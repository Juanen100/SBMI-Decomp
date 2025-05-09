public class WorkableDecorator : EntityDecorator
{
	private Identity worker;

	public Identity Worker
	{
		get
		{
			return worker;
		}
		set
		{
			worker = value;
		}
	}

	public WorkableDecorator(Entity toDecorate)
		: base(toDecorate)
	{
	}
}
