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
			image = (string)source["image"];
			name = (string)source["name"];
			premultipliedAlpha = (bool)source["premultipliedAlpha"];
			scale = Convert.ToSingle(source["scale"]);
			Dictionary<string, object> dictionary = (Dictionary<string, object>)source["size"];
			if (dictionary != null)
			{
				size = new Rect(0f, 0f, Convert.ToSingle(dictionary["w"]), Convert.ToSingle(dictionary["h"]));
			}
			invScale.x = 1f / size.width;
			invScale.y = 1f / size.height;
		}

		public AtlasMetaData(BinaryReader reader)
		{
			name = TextureAtlas._ReadString(reader);
			image = TextureAtlas._ReadString(reader);
			premultipliedAlpha = reader.ReadBoolean();
			size.x = reader.ReadSingle();
			size.y = reader.ReadSingle();
			size.width = reader.ReadSingle();
			size.height = reader.ReadSingle();
			invScale.x = 1f / size.width;
			invScale.y = 1f / size.height;
			scale = reader.ReadSingle();
		}
	}
}
