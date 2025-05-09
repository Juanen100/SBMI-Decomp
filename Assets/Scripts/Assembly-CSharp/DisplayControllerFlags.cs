using System;

[Flags]
public enum DisplayControllerFlags
{
	VISIBLE_AND_VALID_STATE = 1,
	SWITCHED_STATE = 2,
	NEED_UPDATE = 4
}
