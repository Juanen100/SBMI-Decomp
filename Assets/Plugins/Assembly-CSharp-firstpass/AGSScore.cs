using System.Collections;
using System.Collections.Generic;

public class AGSScore
{
	public string playerAlias;

	public int rank;

	public string scoreString;

	public long scoreValue;

	public static AGSScore fromHashtable(Hashtable ht)
	{
		AGSScore aGSScore = new AGSScore();
		aGSScore.playerAlias = ht["playerAlias"].ToString();
		aGSScore.rank = int.Parse(ht["rank"].ToString());
		aGSScore.scoreString = ht["scoreString"].ToString();
		aGSScore.scoreValue = long.Parse(ht["scoreValue"].ToString());
		return aGSScore;
	}

	public static List<AGSScore> fromArrayList(ArrayList list)
	{
		List<AGSScore> list2 = new List<AGSScore>();
		foreach (Hashtable item in list)
		{
			list2.Add(fromHashtable(item));
		}
		return list2;
	}

	public override string ToString()
	{
		return string.Format("playerAlias: {0}, rank: {1}, scoreString: {2}", playerAlias, rank, scoreString);
	}
}
