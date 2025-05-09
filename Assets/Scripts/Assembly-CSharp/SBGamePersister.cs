using System.Timers;

public class SBGamePersister
{
	private Session session;

	private Timer timer;

	public SBGamePersister(Session session)
	{
		this.session = session;
	}

	public void Start()
	{
		if (SBSettings.SAVE_INTERVAL >= 0)
		{
			Stop();
			timer = new Timer(SBSettings.SAVE_INTERVAL * 1000);
			timer.Elapsed += TimerTick;
			timer.Start();
		}
	}

	public void Stop()
	{
		if (timer != null)
		{
			timer.Stop();
			timer = null;
		}
	}

	public void TimerTick(object sender, ElapsedEventArgs e)
	{
		SaveOrPatch();
	}

	public void SaveOrPatch()
	{
		session.CheckForPatching(false);
	}
}
