using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Yarg
{
	[Serializable]
	public sealed class AtlasMetaData
	{
		public string image;

		public Rect size;

		public string name;

		public bool premultipliedAlpha;

		public float scale;

		[NonSerialized]
		[HideInInspector]
		public Vector2 invScale;

		public AtlasMetaData()
		{
		}

		public AtlasMetaData(Dictionary<string, object> source)
		{
		}

		public AtlasMetaData(BinaryReader reader)
		{
		}
	}
}
