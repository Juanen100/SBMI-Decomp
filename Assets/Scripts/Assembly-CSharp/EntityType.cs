using System;

[Flags]
public enum EntityType
{
	INVALID = 0,
	CORE = 1,
	RESIDENT = 2,
	WORKER = 4,
	DEBRIS = 8,
	LANDMARK = 0x10,
	BUILDING = 0x20,
	ANNEX = 0x40,
	TREASURE = 0x80,
	WANDERER = 0x100,
	COSTUME = 0x200
}
