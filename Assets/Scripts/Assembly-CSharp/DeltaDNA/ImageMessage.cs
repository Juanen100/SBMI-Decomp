using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeltaDNA
{
	public class ImageMessage
	{
		public class EventArgs : System.EventArgs
		{
			public string ID { get; set; }

			public string ActionType { get; set; }

			public string ActionValue { get; set; }

			public EventArgs(string id, string type, string value)
			{
			}

			internal static EventArgs Create(string platform, string id, string type, object value)
			{
				return null;
			}
		}

		public class StoreEventArgs : EventArgs
		{
			public StoreEventArgs(string platform, string id, string type, object value)
				: base(null, null, null)
			{
			}
		}

		private class SpriteMap : MonoBehaviour
		{
			private ImageMessageStore store;

			private Dictionary<string, object> configuration;

			private Texture2D texture;

			public string URL { get; private set; }

			public int Width { get; private set; }

			public int Height { get; private set; }

			public Texture Texture
			{
				get
				{
					return null;
				}
			}

			public Texture Background
			{
				get
				{
					return null;
				}
			}

			public List<Texture> Buttons
			{
				get
				{
					return null;
				}
			}

			public void Build(DDNA ddna, Dictionary<string, object> configuration)
			{
			}

			public void LoadResource(Action<string> callback)
			{
			}

			public Texture2D GetSubRegion(int x, int y, int width, int height)
			{
				return null;
			}

			public Texture2D GetSubRegion(Rect rect)
			{
				return null;
			}
		}

		private class Layer : MonoBehaviour
		{
			protected DDNA ddna;

			protected GameObject parent;

			protected ImageMessage imageMessage;

			protected List<Action> actions;

			protected int depth;

			protected void RegisterAction()
			{
			}

			protected void RegisterAction(Dictionary<string, object> action, string id)
			{
			}

			protected void PositionObject(GameObject obj, Rect position)
			{
			}
		}

		private class ShimLayer : Layer
		{
			private Texture2D texture;

			private readonly byte dimmedMaskAlpha;

			public void Build(DDNA ddna, GameObject parent, ImageMessage imageMessage, Dictionary<string, object> config, int depth)
			{
			}

			private void Start()
			{
			}
		}

		private class BackgroundLayer : Layer
		{
			private Texture texture;

			private Rect position;

			private float scale;

			public Rect Position
			{
				get
				{
					return default(Rect);
				}
			}

			public float Scale
			{
				get
				{
					return 0f;
				}
			}

			public void Build(DDNA ddna, GameObject parent, ImageMessage imageMessage, Dictionary<string, object> layout, Texture texture, int depth)
			{
			}

			private void Start()
			{
			}

			private Rect RenderAsCover(Dictionary<string, object> rules)
			{
				return default(Rect);
			}

			private Rect RenderAsContain(Dictionary<string, object> rules)
			{
				return default(Rect);
			}

			private float GetConstraintPixels(string constraint, float edge)
			{
				return 0f;
			}
		}

		private class ButtonsLayer : Layer
		{
			private List<Texture> textures;

			private List<Rect> positions;

			public void Build(DDNA ddna, GameObject parent, ImageMessage imageMessage, Dictionary<string, object> orientation, List<Texture> textures, BackgroundLayer content, int depth)
			{
			}

			private void Start()
			{
			}
		}

		private readonly DDNA ddna;

		private Dictionary<string, object> configuration;

		private GameObject gameObject;

		private SpriteMap spriteMap;

		private int depth;

		private bool showing;

		private Engagement engagement;

		private string name;

		public Dictionary<string, object> Parameters { get; private set; }

		public event Action OnDidReceiveResources
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<string> OnDidFailToReceiveResources
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<EventArgs> OnDismiss
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<EventArgs> OnAction
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<EventArgs> OnStore
		{
			add
			{
			}
			remove
			{
			}
		}

		private ImageMessage(DDNA ddna, Dictionary<string, object> configuration, string name, int depth, Engagement engagement)
		{
		}

		private void redraw()
		{
		}

		public static ImageMessage Create(Engagement engagement)
		{
			return null;
		}

		public static ImageMessage Create(Engagement engagement, Dictionary<string, object> options)
		{
			return null;
		}

		public static ImageMessage Create(DDNA ddna, Engagement engagement, Dictionary<string, object> options)
		{
			return null;
		}

		private static bool ValidConfiguration(Dictionary<string, object> c)
		{
			return false;
		}

		public void FetchResources()
		{
		}

		public bool IsReady()
		{
			return false;
		}

		public void Show()
		{
		}

		public bool IsShowing()
		{
			return false;
		}

		public void Close()
		{
		}
	}
}
