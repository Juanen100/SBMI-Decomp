public interface SBTabCategory
{
	string Name { get; set; }

	string Label { get; set; }

	string Type { get; set; }

	string Texture { get; set; }

	int MicroEventDID { get; set; }

	bool MicroEventOnly { get; set; }
}
