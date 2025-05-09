using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Yarg
{
	public sealed class AtlasCoords
	{
		public string name;

		public Rect frame;

		public Vector2 spriteSourceSize;

		public Rect spriteSize;

		public byte properties;

		public bool trimmed
		{
			get
			{
				return (properties & 1) == 1;
			}
			set
			{
				properties |= 1;
			}
		}

		public bool rotated
		{
			get
			{
				return (properties & 2) == 2;
			}
			set
			{
				properties |= 2;
			}
		}

		public bool processed
		{
			get
			{
				return (properties & 4) == 4;
			}
			set
			{
				properties |= 4;
			}
		}

		public AtlasCoords()
		{
		}

		public AtlasCoords(string key, Dictionary<string, object> source)
		{
			name = key;
			Dictionary<string, object> dictionary = (Dictionary<string, object>)source["frame"];
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)source["spriteOffset"];
			Dictionary<string, object> dictionary3 = (Dictionary<string, object>)source["spriteSize"];
			rotated = (bool)source["rotated"];
			trimmed = (bool)source["trimmed"];
			if (dictionary != null)
			{
				frame = new Rect(Convert.ToSingle(dictionary["x"]), Convert.ToSingle(dictionary["y"]), Convert.ToSingle(dictionary["w"]), Convert.ToSingle(dictionary["h"]));
			}
			if (dictionary3 != null)
			{
				spriteSize = new Rect(0f, 0f, Convert.ToSingle(dictionary3["w"]), Convert.ToSingle(dictionary3["h"]));
			}
			Dictionary<string, object> dictionary4 = (Dictionary<string, object>)source["sourceSize"];
			if (dictionary4 != null)
			{
				spriteSourceSize = new Vector2(Convert.ToSingle(dictionary4["w"]), Convert.ToSingle(dictionary4["h"]));
			}
		}

		public AtlasCoords(BinaryReader reader, int version)
		{
			name = TextureAtlas._ReadString(reader);
			frame.x = reader.ReadSingle();
			frame.y = reader.ReadSingle();
			frame.width = reader.ReadSingle();
			frame.height = reader.ReadSingle();
			spriteSize.x = reader.ReadSingle();
			spriteSize.y = reader.ReadSingle();
			spriteSize.width = reader.ReadSingle();
			spriteSize.height = reader.ReadSingle();
			if (version == 2)
			{
				reader.ReadSingle();
				reader.ReadSingle();
			}
			spriteSourceSize.x = reader.ReadSingle();
			spriteSourceSize.y = reader.ReadSingle();
			properties = reader.ReadByte();
		}
	}
}
