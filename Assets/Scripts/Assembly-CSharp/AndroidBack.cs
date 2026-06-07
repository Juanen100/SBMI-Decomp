using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AndroidBack : MonoBehaviour
{
	private static AndroidBack instance;

	private int delay;

	private bool isQuiting;

	private bool isShowingQuitDlg;

	private Session session;

	private Stack<Action> actionStack;

	private Stack<object> objectStack;

	public static AndroidBack getInstance()
	{
		return null;
	}

	public void addSession(Session session)
	{
	}

	public int count()
	{
		return 0;
	}

	public void push(Action action, object ob)
	{
	}

	public Action pop()
	{
		return null;
	}

	public Action pop(Action action)
	{
		return null;
	}

	public Action getTopAction()
	{
		return null;
	}

	public object getTopObject()
	{
		return null;
	}

	private void Update()
	{
	}

	[DebuggerHidden]
	private IEnumerator quit()
	{
		return null;
	}
}
