using System.Collections;
using System.Collections.Generic;

public class AGSLeaderboard
{
	public string name;

	public string id;

	public string displayText;

	public string scoreFormat;

	public List<AGSScore> scores = new List<AGSScore>();

	public static AGSLeaderboard fromHashtable(Hashtable ht)
	{
		AGSLeaderboard aGSLeaderboard = new AGSLeaderboard();
		aGSLeaderboard.name = ht["name"].ToString();
		aGSLeaderboard.id = ht["id"].ToString();
		aGSLeaderboard.displayText = ht["displayText"].ToString();
		aGSLeaderboard.scoreFormat = ht["scoreFormat"].ToString();
		if (ht.ContainsKey("scores") && ht["scores"] is ArrayList)
		{
			ArrayList list = ht["scores"] as ArrayList;
			aGSLeaderboard.scores = AGSScore.fromArrayList(list);
		}
		return aGSLeaderboard;
	}

	public override string ToString()
	{
		return string.Format("name: {0}, id: {1}, displayText: {2}, scoreFormat: {3}", name, id, displayText, scoreFormat);
	}
}
