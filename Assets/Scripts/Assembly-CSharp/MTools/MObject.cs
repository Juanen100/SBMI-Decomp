namespace MTools
{
	public class MObject
	{
		private object mObjVal;

		private long mLongVal;

		private double mDoubleVal;

		public MObject()
		{
		}

		public MObject(object val)
		{
		}

		public MObject(float val)
		{
		}

		public MObject(int val)
		{
		}

		public MObject(bool val)
		{
		}

		~MObject()
		{
		}

		public void setValueAsObject(object v)
		{
		}

		public void setValueAsBool(bool v)
		{
		}

		public void setValueAsString(string v)
		{
		}

		public void setValueAsInt(int v)
		{
		}

		public void setValueAsFloat(float v)
		{
		}

		public void setValueAsLong(long v)
		{
		}

		public void setValueAsULong(ulong v)
		{
		}

		public void setValueAsDouble(double v)
		{
		}

		public object valueAsObject()
		{
			return null;
		}

		public bool valueAsBool()
		{
			return false;
		}

		public string valueAsString()
		{
			return null;
		}

		public int valueAsInt()
		{
			return 0;
		}

		public float valueAsFloat()
		{
			return 0f;
		}

		public ulong valueAsULong()
		{
			return 0uL;
		}

		public long valueAsLong()
		{
			return 0L;
		}

		public double valueAsDouble()
		{
			return 0.0;
		}
	}
}
