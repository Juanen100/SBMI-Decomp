using System.Collections.Generic;

public class InteractionState
{
	private bool hasSendClickAction;

	private bool isGrabbable;

	private bool isSelectable;

	private bool isEditable;

	private BaseTransitionBinding selectedStateTransition;

	private Stack<ICollection<IControlBinding>> controls;

	public bool HasClickCommandFunctionality
	{
		get
		{
			return false;
		}
	}

	public bool IsGrabbable
	{
		get
		{
			return false;
		}
	}

	public bool IsSelectable
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool IsEditable
	{
		get
		{
			return false;
		}
	}

	public BaseTransitionBinding SelectedTransition
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public ICollection<IControlBinding> Controls
	{
		get
		{
			return null;
		}
	}

	public void SetInteractions(bool isEditable, bool isGrabbable, bool isSelectable, bool hasSendClickAction, BaseTransitionBinding transition = null, ICollection<IControlBinding> newControls = null)
	{
	}

	public void Clear()
	{
	}

	public void PushControls(ICollection<IControlBinding> newControls)
	{
	}

	public ICollection<IControlBinding> PopControls()
	{
		return null;
	}

	public void ClearControls()
	{
	}
}
