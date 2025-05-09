#define ASSERTS_ON
using System.Collections.Generic;

public class InteractionState
{
	private bool hasSendClickAction;

	private bool isGrabbable;

	private bool isSelectable;

	private bool isEditable;

	private BaseTransitionBinding selectedStateTransition;

	private Stack<ICollection<IControlBinding>> controls = new Stack<ICollection<IControlBinding>>();

	public bool HasClickCommandFunctionality
	{
		get
		{
			return hasSendClickAction;
		}
	}

	public bool IsGrabbable
	{
		get
		{
			return isGrabbable;
		}
	}

	public bool IsSelectable
	{
		get
		{
			return isSelectable;
		}
		set
		{
			isSelectable = value;
		}
	}

	public bool IsEditable
	{
		get
		{
			return isEditable;
		}
	}

	public BaseTransitionBinding SelectedTransition
	{
		get
		{
			return selectedStateTransition;
		}
		set
		{
			selectedStateTransition = value;
		}
	}

	public ICollection<IControlBinding> Controls
	{
		get
		{
			if (controls.Count > 0)
			{
				return controls.Peek();
			}
			return null;
		}
	}

	public void SetInteractions(bool isEditable, bool isGrabbable, bool isSelectable, bool hasSendClickAction, BaseTransitionBinding transition = null, ICollection<IControlBinding> newControls = null)
	{
		this.isEditable = isEditable;
		this.isGrabbable = isGrabbable;
		this.isSelectable = isSelectable;
		this.hasSendClickAction = hasSendClickAction;
		selectedStateTransition = transition;
		ClearControls();
		if (newControls != null)
		{
			controls.Push(newControls);
		}
	}

	public void Clear()
	{
		isEditable = false;
		isGrabbable = false;
		isSelectable = false;
		hasSendClickAction = false;
		selectedStateTransition = null;
		controls.Clear();
	}

	public void PushControls(ICollection<IControlBinding> newControls)
	{
		controls.Push(newControls);
	}

	public ICollection<IControlBinding> PopControls()
	{
		TFUtils.Assert(controls.Count > 0, "Trying to pop the controls stack, but it has nothing on it");
		return controls.Pop();
	}

	public void ClearControls()
	{
		controls.Clear();
	}
}
