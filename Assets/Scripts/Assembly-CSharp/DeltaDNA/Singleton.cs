using UnityEngine;

namespace DeltaDNA
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;

		private static object _lock;

		private static bool applicationIsQuitting;

		public static T Instance
		{
			get
			{
				return null;
			}
		}

		public virtual void OnDestroy()
		{
		}
	}
}
