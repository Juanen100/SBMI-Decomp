using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Yarg
{
	[Serializable]
	public sealed class FontAtlas : ILoadable
	{
		[Serializable]
		public sealed class SerializedCharData
		{
			public int id;

			public Rect size;

			public Vector2 offset;

			public int xadvance;

			public int page;

			public int chnl;

			public int letter;

			public CharData CharData()
			{
				return default(CharData);
			}
		}

		[StructLayout((LayoutKind)0, Size = 44)]
		public struct CharData
		{
			public int id;

			public Rect size;

			public Vector2 offset;

			public int xadvance;

			public int page;

			public int chnl;

			public char letter;
		}

		[Serializable]
		public sealed class FontData
		{
			public string face;

			public int size;

			public bool bold;

			public bool italic;

			public string charset;

			public bool unicode;

			public int stretchH;

			public bool smooth;

			public bool aa;

			public RectOffset padding;

			public Vector2 spacing;

			public int lineHeight;

			public int _base;

			public Vector2 scale;

			public int pages;

			public bool packed;

			public string[] files;

			public int count;

			public int kernCount;
		}

		[Serializable]
		public sealed class KernData
		{
			public int first;

			public int second;

			public int amount;

			public KernData(int _first, int _second, int _amount)
			{
			}
		}

		private enum DATATYPE
		{
			INFO = 0,
			COMMON = 1,
			PAGE = 2,
			CHAR = 3,
			KERN = 4
		}

		[StructLayout((LayoutKind)0, Size = 8)]
		private struct KernPair
		{
			public int first;

			public int second;

			public KernPair(int _first, int _second)
			{
				first = 0;
				second = 0;
			}

			public override int GetHashCode()
			{
				return 0;
			}

			public override bool Equals(object other)
			{
				return false;
			}

			public bool Equals(KernPair other)
			{
				return false;
			}
		}

		[StructLayout((LayoutKind)0, Size = 16)]
		private struct FontPair
		{
			public string key;

			public object value;

			public FontPair(string k, object v)
			{
				key = null;
				value = null;
			}
		}

		public TextAsset fnt;

		public Material material;

		public FontData info;

		private Dictionary<KernPair, int> kernings;

		private Dictionary<char, CharData> chars;

		public SerializedCharData[] charArray;

		public KernData[] kernArray;

		[NonSerialized]
		private KernPair kernSearch;

		public string filename
		{
			get
			{
				return null;
			}
		}

		public CharData Item
		{
			get
			{
				return default(CharData);
			}
		}

		public int Kerning(int _first, int _second)
		{
			return 0;
		}

		private void BuildCharDictionary()
		{
		}

		private void BuildKernDictionary()
		{
		}

		private static FontPair GetKeyValuePair(StringReader line, StringBuilder buffer)
		{
			return default(FontPair);
		}

		public void Load()
		{
		}

		public FontAtlas Load(string fnt)
		{
			return null;
		}
	}
}
