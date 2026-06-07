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
				return false;
			}
			set
			{
			}
		}

		public bool rotated
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool processed
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public AtlasCoords()
		{
		}

		public AtlasCoords(string key, Dictionary<string, object> source)
		{
		}

		public AtlasCoords(BinaryReader reader, int version)
		{
		}
	}
}
