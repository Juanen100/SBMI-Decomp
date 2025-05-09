using System;
using System.Collections.Generic;

[Serializable]
public class DialogPrompt
{
	public string texture;

	public string text;

	public string voiceover;

	public bool atlased;

	public DialogPrompt(string _texture, string _text, string _vo)
	{
		texture = _texture;
		text = _text;
		voiceover = _vo;
	}

	public DialogPrompt(Dictionary<string, object> dict)
	{
		texture = (string)dict["character_icon"];
		text = (string)dict["text"];
		if (dict.ContainsKey("voiceover"))
		{
			voiceover = (string)dict["voiceover"];
		}
	}
}
