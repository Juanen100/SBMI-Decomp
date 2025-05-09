using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidBack : MonoBehaviour
{
	private static AndroidBack instance;

	private int delay;

	private bool isQuiting;

	private bool isShowingQuitDlg;

	private Session session;

	private Stack<Action> actionStack = new Stack<Action>();

	private Stack<object> objectStack = new Stack<object>();

	public static AndroidBack getInstance()
	{
		if (instance == null)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "AndroidBack";
			instance = gameObject.AddComponent<AndroidBack>();
		}
		return instance;
	}

	public void addSession(Session session)
	{
		this.session = session;
	}

	public int count()
	{
		return actionStack.Count;
	}

	public void push(Action action, object ob)
	{
		if (ob != getTopObject())
		{
			actionStack.Push(action);
			objectStack.Push(ob);
		}
	}

	public Action pop()
	{
		if (actionStack.Count == 0)
		{
			return null;
		}
		Action action = null;
		while (action == null && actionStack.Count > 0)
		{
			action = actionStack.Pop();
			objectStack.Pop();
		}
		if (action != null)
		{
		}
		return action;
	}

	public Action pop(Action action)
	{
		if (actionStack.Contains(action))
		{
			action = actionStack.Pop();
			objectStack.Pop();
		}
		return action;
	}

	public Action getTopAction()
	{
		if (actionStack.Count == 0)
		{
			return null;
		}
		List<Action> list = new List<Action>(actionStack.ToArray());
		return list[0];
	}

	public object getTopObject()
	{
		if (objectStack.Count == 0)
		{
			return null;
		}
		List<object> list = new List<object>(objectStack.ToArray());
		return list[0];
	}

	private void Update()
	{
		if (isQuiting)
		{
			return;
		}
		if (delay > 0)
		{
			delay--;
		}
		else
		{
			if ((!Input.GetKey(KeyCode.Home) && !Input.GetKey(KeyCode.Escape)) || RestrictInteraction.ContainsWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT) || (session != null && session.TheGame != null && session.TheGame.simulation != null && session.TheGame.simulation.Whitelisted))
			{
				return;
			}
			Debug.LogError("-------------------Input.GetKey(KeyCode.Escape)------------------------");
			delay = 20;
			Action action = pop();
			if (action != null)
			{
				action();
				return;
			}
			Debug.Log("quit");
			if (session == null || isShowingQuitDlg)
			{
				return;
			}
			isShowingQuitDlg = true;
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			bool bRestrictAllUI = false;
			if (RestrictInteraction.ContainsWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT))
			{
				bRestrictAllUI = true;
				RestrictInteraction.RemoveWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
			}
			Action cancelButtonHandler = delegate
			{
				isShowingQuitDlg = false;
				session.TheSoundEffectManager.PlaySound("Cancel");
				SBUIBuilder.ReleaseTopScreen();
				if (bRestrictAllUI)
				{
					RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
				}
				if (session.TheState is Session.Playing)
				{
					session.ChangeState("Playing");
				}
			};
			Action okButtonHandler = delegate
			{
				Debug.Log("ok");
				isQuiting = true;
				isShowingQuitDlg = false;
				if (bRestrictAllUI)
				{
					RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
				}
				StartCoroutine(quit());
			};
			SBUIBuilder.MakeAndPushConfirmationDialog(session, null, "Quit", Language.Get("!!ANDROID_QUIT_ALERT"), "YES", "NO", null, okButtonHandler, cancelButtonHandler);
		}
	}

	private IEnumerator quit()
	{
		session.ChangeState("Stopping");
		yield return new WaitForSeconds(0.3f);
		Application.Quit();
	}
}
