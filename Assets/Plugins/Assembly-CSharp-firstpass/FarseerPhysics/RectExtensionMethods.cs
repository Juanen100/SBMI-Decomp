using FarseerPhysics.Collision;
using UnityEngine;

namespace FarseerPhysics
{
	public static class RectExtensionMethods
	{
		public static AABB ToAABB(this Rect rect)
		{
			return default(AABB);
		}

		public static Rect ToRect(this AABB bbox)
		{
			return default(Rect);
		}
	}
}
