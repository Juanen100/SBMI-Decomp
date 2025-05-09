using System;
using System.Collections.Generic;
using System.IO;
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
				return new CharData
				{
					id = id,
					size = size,
					offset = offset,
					xadvance = xadvance,
					page = page,
					chnl = chnl,
					letter = (char)letter
				};
			}
		}

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
				first = _first;
				second = _second;
				amount = _amount;
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

		private struct KernPair
		{
			public int first;

			public int second;

			public KernPair(int _first, int _second)
			{
				first = _first;
				second = _second;
			}

			public override int GetHashCode()
			{
				return (first * 251 + second) * 251 + second;
			}

			public override bool Equals(object other)
			{
				return other is KernPair && Equals((KernPair)other);
			}

			public bool Equals(KernPair other)
			{
				return first == other.first && second == other.second;
			}
		}

		private struct FontPair
		{
			public string key;

			public object value;

			public FontPair(string k, object v)
			{
				key = k;
				value = v;
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
				if (info.files != null)
				{
					string[] files = info.files;
					foreach (string text in files)
					{
						if (!string.IsNullOrEmpty(text))
						{
							return text;
						}
					}
				}
				return null;
			}
		}

		public CharData this[char chr]
		{
			get
			{
				BuildCharDictionary();
				CharData value;
				chars.TryGetValue(chr, out value);
				return value;
			}
		}

		public int Kerning(int _first, int _second)
		{
			BuildKernDictionary();
			int value = 0;
			kernSearch.first = _first;
			kernSearch.second = _second;
			kernings.TryGetValue(kernSearch, out value);
			return value;
		}

		private void BuildCharDictionary()
		{
			if (chars == null || chars.Count != info.count)
			{
				if (charArray == null || charArray.Length == 0)
				{
					Load();
				}
				chars = new Dictionary<char, CharData>(charArray.Length);
				for (int i = 0; i < charArray.Length; i++)
				{
					CharData value = charArray[i].CharData();
					chars[value.letter] = value;
				}
			}
		}

		private void BuildKernDictionary()
		{
			if (kernings == null || kernings.Count != info.kernCount)
			{
				if (kernArray == null || kernArray.Length == 0)
				{
					Load();
				}
				kernings = new Dictionary<KernPair, int>();
				for (int i = 0; i < kernArray.Length; i++)
				{
					KernData kernData = kernArray[i];
					kernings[new KernPair(kernData.first, kernData.second)] = kernData.amount;
				}
			}
		}

		private static FontPair GetKeyValuePair(StringReader line, StringBuilder buffer)
		{
			buffer.Length = 0;
			while ((ushort)line.Peek() == 32)
			{
				line.Read();
			}
			string text = null;
			object obj = null;
			int num = 0;
			bool flag = false;
			while (num != -1)
			{
				num = line.Read();
				char c = (char)num;
				switch (c)
				{
				case ' ':
					if (text == null)
					{
						text = buffer.ToString();
						buffer.Length = 0;
					}
					if (flag)
					{
						buffer.Append(c);
					}
					else
					{
						num = -1;
					}
					break;
				case '=':
					if (flag)
					{
						buffer.Append(c);
						break;
					}
					text = buffer.ToString();
					buffer.Length = 0;
					break;
				case '"':
					if (flag)
					{
						int num2 = line.Peek();
						if ((ushort)num2 == 32 || num2 == -1)
						{
							num = -1;
						}
						else
						{
							buffer.Append(c);
						}
					}
					else
					{
						flag = true;
					}
					break;
				default:
					if (num != -1)
					{
						buffer.Append(c);
					}
					break;
				}
			}
			if (flag)
			{
				obj = buffer.ToString();
			}
			else if (buffer.Length == 0)
			{
				obj = null;
			}
			else
			{
				string text2 = buffer.ToString();
				if (text2.IndexOf(',') != -1)
				{
					string[] array = text2.Split(',');
					obj = Array.ConvertAll(array, (string x) => int.Parse(x));
				}
				else
				{
					obj = int.Parse(buffer.ToString());
				}
			}
			return new FontPair(text, obj);
		}

		public void Load()
		{
			if (!(fnt == null) && !string.IsNullOrEmpty(fnt.text))
			{
				FontAtlas fontAtlas = Load(fnt.text);
				chars = fontAtlas.chars;
				charArray = fontAtlas.charArray;
				kernArray = fontAtlas.kernArray;
				info = fontAtlas.info;
			}
		}

		public static FontAtlas Load(string fnt)
		{
			StringBuilder buffer = new StringBuilder();
			FontAtlas fontAtlas = new FontAtlas();
			List<SerializedCharData> list = new List<SerializedCharData>();
			List<KernData> list2 = new List<KernData>();
			FontData fontData = new FontData();
			List<string> list3 = new List<string>();
			using (StringReader stringReader = new StringReader(fnt))
			{
				while (true)
				{
					string text = stringReader.ReadLine();
					if (text == null)
					{
						break;
					}
					StringReader stringReader2 = new StringReader(text);
					FontPair keyValuePair = GetKeyValuePair(stringReader2, buffer);
					if (keyValuePair.key == null)
					{
						continue;
					}
					DATATYPE dATATYPE;
					switch (keyValuePair.key)
					{
					default:
						continue;
					case "info":
						dATATYPE = DATATYPE.INFO;
						break;
					case "common":
						dATATYPE = DATATYPE.COMMON;
						break;
					case "page":
						dATATYPE = DATATYPE.PAGE;
						break;
					case "char":
						dATATYPE = DATATYPE.CHAR;
						break;
					case "kerning":
						dATATYPE = DATATYPE.KERN;
						break;
					}
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					while (true)
					{
						keyValuePair = GetKeyValuePair(stringReader2, buffer);
						if (keyValuePair.key == null)
						{
							break;
						}
						dictionary[keyValuePair.key] = keyValuePair.value;
					}
					stringReader2.Dispose();
					switch (dATATYPE)
					{
					case DATATYPE.CHAR:
					{
						SerializedCharData serializedCharData = new SerializedCharData();
						serializedCharData.chnl = (int)dictionary["chnl"];
						serializedCharData.id = (int)dictionary["id"];
						serializedCharData.size = new Rect((int)dictionary["x"], (int)dictionary["y"], (int)dictionary["width"], (int)dictionary["height"]);
						serializedCharData.offset = new Vector2(-(int)dictionary["xoffset"], (int)dictionary["yoffset"]);
						serializedCharData.xadvance = (int)dictionary["xadvance"];
						serializedCharData.page = (int)dictionary["page"];
						string text2 = (string)dictionary["letter"];
						if (text2.Equals("space"))
						{
							serializedCharData.letter = 32;
						}
						else
						{
							serializedCharData.letter = text2.ToCharArray()[0];
						}
						list.Add(serializedCharData);
						break;
					}
					case DATATYPE.COMMON:
						fontData.lineHeight = (int)dictionary["lineHeight"];
						fontData._base = (int)dictionary["base"];
						fontData.scale = new Vector2((int)dictionary["scaleW"], (int)dictionary["scaleH"]);
						fontData.pages = (int)dictionary["pages"];
						fontData.packed = (int)dictionary["packed"] == 1;
						break;
					case DATATYPE.INFO:
					{
						fontData.face = (string)dictionary["face"];
						fontData.size = (int)dictionary["size"];
						fontData.bold = (int)dictionary["bold"] == 1;
						fontData.italic = (int)dictionary["italic"] == 1;
						fontData.charset = (string)dictionary["charset"];
						fontData.unicode = (int)dictionary["unicode"] == 1;
						fontData.stretchH = (int)dictionary["stretchH"];
						fontData.smooth = (int)dictionary["smooth"] == 1;
						fontData.aa = (int)dictionary["aa"] == 1;
						int[] array = (int[])dictionary["padding"];
						fontData.padding = new RectOffset(array[0], array[1], array[2], array[3]);
						int[] array2 = (int[])dictionary["spacing"];
						fontData.spacing = new Vector2(array2[0], array2[1]);
						break;
					}
					case DATATYPE.KERN:
						list2.Add(new KernData((int)dictionary["first"], (int)dictionary["second"], (int)dictionary["amount"]));
						break;
					case DATATYPE.PAGE:
						list3.Add((string)dictionary["file"]);
						break;
					}
				}
			}
			fontData.files = list3.ToArray();
			fontData.count = list.Count;
			fontData.kernCount = list2.Count;
			fontAtlas.info = fontData;
			fontAtlas.charArray = list.ToArray();
			fontAtlas.kernArray = list2.ToArray();
			return fontAtlas;
		}
	}
}
