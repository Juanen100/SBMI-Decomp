using System;
using System.Collections.Generic;

public class ConditionalProgressSerializer
{
	public ConditionalProgress DeserializeProgress(ICollection<object> loadedData)
	{
		List<uint> list = new List<uint>(loadedData.Count);
		foreach (object loadedDatum in loadedData)
		{
			list.Add(Convert.ToUInt32(loadedDatum));
		}
		return new ConditionalProgress(list);
	}

	public List<object> SerializeProgress(ConditionalProgress progress)
	{
		return TFUtils.CloneAndCastList<uint, object>(progress.MetIds);
	}
}
