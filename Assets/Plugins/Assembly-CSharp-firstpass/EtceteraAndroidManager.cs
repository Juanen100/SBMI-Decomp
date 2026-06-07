using System;
using Prime31;

public class EtceteraAndroidManager : AbstractManager
{
	public static event Action<string> alertButtonClickedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action alertCancelledEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> promptFinishedWithTextEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action promptCancelledEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string, string> twoFieldPromptFinishedWithTextEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action twoFieldPromptCancelledEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action webViewCancelledEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action albumChooserCancelledEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> albumChooserSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action photoChooserCancelledEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> photoChooserSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> videoRecordingSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action videoRecordingCancelledEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action ttsInitializedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action ttsFailedToInitializeEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action askForReviewWillOpenMarketEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action askForReviewRemindMeLaterEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action askForReviewDontAskAgainEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> inlineWebViewJSCallbackEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> notificationReceivedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	static EtceteraAndroidManager()
	{
	}

	public void alertButtonClicked(string positiveButton)
	{
	}

	public void alertCancelled(string empty)
	{
	}

	public void promptFinishedWithText(string text)
	{
	}

	public void promptCancelled(string empty)
	{
	}

	public void twoFieldPromptCancelled(string empty)
	{
	}

	public void webViewCancelled(string empty)
	{
	}

	public void albumChooserCancelled(string empty)
	{
	}

	public void albumChooserSucceeded(string path)
	{
	}

	public void photoChooserCancelled(string empty)
	{
	}

	public void photoChooserSucceeded(string path)
	{
	}

	public void videoRecordingSucceeded(string path)
	{
	}

	public void videoRecordingCancelled(string empty)
	{
	}

	public void ttsInitialized(string result)
	{
	}

	public void ttsUtteranceCompleted(string utteranceId)
	{
	}

	public void askForReviewWillOpenMarket(string empty)
	{
	}

	public void askForReviewRemindMeLater(string empty)
	{
	}

	public void askForReviewDontAskAgain(string empty)
	{
	}

	public void inlineWebViewJSCallback(string message)
	{
	}

	public void notificationReceived(string extraData)
	{
	}
}
