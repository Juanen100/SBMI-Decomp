namespace MTools
{
	public class MObject
	{
		private object mObjVal;

		private long mLongVal;

		private double mDoubleVal;

		public MObject()
		{
			mObjVal = null;
		}

		public MObject(object val)
		{
			mObjVal = val;
		}

		public MObject(float val)
		{
			mDoubleVal = val;
		}

		public MObject(int val)
		{
			mLongVal = val;
		}

		public MObject(bool val)
		{
			mLongVal = (val ? 1 : 0);
		}

		~MObject()
		{
			mObjVal = null;
		}

		public void setValueAsObject(object v)
		{
			mObjVal = v;
		}

		public void setValueAsBool(bool v)
		{
			mLongVal = (v ? 1 : 0);
		}

		public void setValueAsString(string v)
		{
			mObjVal = v;
		}

		public void setValueAsInt(int v)
		{
			mLongVal = v;
		}

		public void setValueAsFloat(float v)
		{
			mDoubleVal = v;
		}

		public void setValueAsLong(long v)
		{
			mLongVal = v;
		}

		public void setValueAsULong(ulong v)
		{
			mLongVal = (long)v;
		}

		public void setValueAsDouble(double v)
		{
			mDoubleVal = v;
		}

		public object valueAsObject()
		{
			return mObjVal;
		}

		public bool valueAsBool()
		{
			return mLongVal != 0;
		}

		public string valueAsString()
		{
			return (string)mObjVal;
		}

		public int valueAsInt()
		{
			return (int)mLongVal;
		}

		public float valueAsFloat()
		{
			return (float)mDoubleVal;
		}

		public ulong valueAsULong()
		{
			return (ulong)mLongVal;
		}

		public long valueAsLong()
		{
			return mLongVal;
		}

		public double valueAsDouble()
		{
			return mDoubleVal;
		}
	}
}
