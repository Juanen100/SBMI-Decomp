using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace PlistCS
{
	public static class Plist
	{
		private static List<int> offsetTable;

		private static List<byte> objectTable;

		private static int refCount;

		private static int objRefSize;

		private static int offsetByteSize;

		private static long offsetTableOffset;

		public static object readPlist(string path)
		{
			return null;
		}

		public static object readPlistSource(string source)
		{
			return null;
		}

		public static object readPlist(byte[] data)
		{
			return null;
		}

		public static plistType getPlistType(Stream stream)
		{
			return default(plistType);
		}

		public static object readPlist(Stream stream, plistType type)
		{
			return null;
		}

		public static void writeXml(object value, string path)
		{
		}

		public static void writeXml(object value, Stream stream)
		{
		}

		public static string writeXml(object value)
		{
			return null;
		}

		public static void writeBinary(object value, string path)
		{
		}

		public static void writeBinary(object value, Stream stream)
		{
		}

		public static byte[] writeBinary(object value)
		{
			return null;
		}

		private static object readXml(XmlDocument xml)
		{
			return null;
		}

		private static object readBinary(byte[] data)
		{
			return null;
		}

		private static Dictionary<string, object> parseDictionary(XmlNode node)
		{
			return null;
		}

		private static List<object> parseArray(XmlNode node)
		{
			return null;
		}

		private static void composeArray(List<object> value, XmlWriter writer)
		{
		}

		private static object parse(XmlNode node)
		{
			return null;
		}

		private static void compose(object value, XmlWriter writer)
		{
		}

		private static void writeDictionaryValues(Dictionary<string, object> dictionary, XmlWriter writer)
		{
		}

		private static int countObject(object value)
		{
			return 0;
		}

		private static byte[] writeBinaryDictionary(Dictionary<string, object> dictionary)
		{
			return null;
		}

		private static byte[] composeBinaryArray(List<object> objects)
		{
			return null;
		}

		private static byte[] composeBinary(object obj)
		{
			return null;
		}

		public static byte[] writeBinaryDate(DateTime obj)
		{
			return null;
		}

		public static byte[] writeBinaryBool(bool obj)
		{
			return null;
		}

		private static byte[] writeBinaryInteger(int value, bool write)
		{
			return null;
		}

		private static byte[] writeBinaryDouble(double value)
		{
			return null;
		}

		private static byte[] writeBinaryByteArray(byte[] value)
		{
			return null;
		}

		private static byte[] writeBinaryString(string value, bool head)
		{
			return null;
		}

		private static byte[] RegulateNullBytes(byte[] value)
		{
			return null;
		}

		private static byte[] RegulateNullBytes(byte[] value, int minBytes)
		{
			return null;
		}

		private static void parseTrailer(List<byte> trailer)
		{
		}

		private static void parseOffsetTable(List<byte> offsetTableBytes)
		{
		}

		private static object parseBinaryDictionary(int objRef)
		{
			return null;
		}

		private static object parseBinaryArray(int objRef)
		{
			return null;
		}

		private static int getCount(int bytePosition, out int newBytePosition)
		{
			newBytePosition = default(int);
			return 0;
		}

		private static object parseBinary(int objRef)
		{
			return null;
		}

		public static object parseBinaryDate(int headerPosition)
		{
			return null;
		}

		private static object parseBinaryInt(int headerPosition)
		{
			return null;
		}

		private static object parseBinaryInt(int headerPosition, out int newHeaderPosition)
		{
			newHeaderPosition = default(int);
			return null;
		}

		private static object parseBinaryReal(int headerPosition)
		{
			return null;
		}

		private static object parseBinaryAsciiString(int headerPosition)
		{
			return null;
		}

		private static object parseBinaryUnicodeString(int headerPosition)
		{
			return null;
		}

		private static object parseBinaryByteArray(int headerPosition)
		{
			return null;
		}
	}
}
