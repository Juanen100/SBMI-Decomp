using System.Collections.Generic;

public class Config
{
	public const string CRAFTING_PATH_SA = "Crafting";

	public const string DIALOG_PACKAGES_PATH_SA = "Dialogs";

	public const string FEATURE_DATA_PATH_SA = "Features";

	public const string MOVIE_PATH_SA = "Video";

	public const string NOTIFICATIONS_PATH_SA = "Notifications";

	public const string QUESTS_PATH_SA = "Quests";

	public const string BONUS_PAYTABLES_SA = "BonusPaytables";

	public const string TASKS_PATH_SA = "Tasks";

	public const string TREASURE_PATH_SA = "Treasure";

	public const string VENDING_PATH_SA = "Vending";

	public const string BLUEPRINT_DIRECTORY_PATH_SA = "Blueprints";

	public const string TERRAIN_PATH_SA = "Terrain";

	public static string[] SA_FILES = new string[6] { "Features", "Video", "Quests", "Tasks", "Blueprints", "Terrain" };

	public static string[] CRAFTING_PATH;

	public static string[] DIALOG_PACKAGES_PATH;

	public static string[] FEATURE_DATA_PATH;

	public static string[] MOVIE_PATH;

	public static string[] NOTIFICATIONS_PATH;

	public static string[] QUESTS_PATH;

	public static string[] BONUS_PAYTABLES;

	public static string[] TASKS_PATH;

	public static string[] BLUEPRINT_DIRECTORY_PATH;

	public static string[] TERRAIN_PATH;

	public static string[] TREASURE_PATH;

	public static string[] VENDING_PATH;

	public static Dictionary<string, string> ACHIEVEMENT_ID_DIC_AMAZON = new Dictionary<string, string>
	{
		{ "SB_Ach_Squid", "SBAM_Ach_Squid" },
		{ "SB_Ach_Gene", "SBAM_Ach_Gene" },
		{ "SB_Ach_Pearl", "SBAM_Ach_Pearl" },
		{ "SB_Ach_Puff", "SBAM_Ach_Puff" },
		{ "SB_Ach_Sandy", "SBAM_Ach_Sandy" },
		{ "SB_Ach_Barn", "SBAM_Ach_Barn" },
		{ "SB_Ach_Larry", "SBAM_Ach_Larry" },
		{ "SB_Ach_ManR", "SBAM_Ach_ManR" },
		{ "SB_Ach_Squill", "SBAM_Ach_Squill" },
		{ "SB_Ach_Reg", "SBAM_Ach_Reg" },
		{ "SB_Ach_Pat", "SBAM_Ach_Pat" },
		{ "SB_Ach_Mer", "SBAM_Ach_Mer" },
		{ "SB_Ach_Kev", "SBAM_Ach_Kev" },
		{ "SB_Ach_King", "SBAM_Ach_King" }
	};

	public static Dictionary<string, string> ACHIEVEMENT_ID_DIC_GOOGLE = new Dictionary<string, string>
	{
		{ "SB_Ach_Squid", "CgkI4ujSoJMCEAIQAQ" },
		{ "SB_Ach_Gene", "CgkI4ujSoJMCEAIQAg" },
		{ "SB_Ach_Pearl", "CgkI4ujSoJMCEAIQAw" },
		{ "SB_Ach_Puff", "CgkI4ujSoJMCEAIQBA" },
		{ "SB_Ach_Sandy", "CgkI4ujSoJMCEAIQBQ" },
		{ "SB_Ach_Barn", "CgkI4ujSoJMCEAIQBg" },
		{ "SB_Ach_Larry", "CgkI4ujSoJMCEAIQBw" },
		{ "SB_Ach_ManR", "CgkI4ujSoJMCEAIQCA" },
		{ "SB_Ach_Squill", "CgkI4ujSoJMCEAIQCQ" },
		{ "SB_Ach_Reg", "CgkI4ujSoJMCEAIQCg" },
		{ "SB_Ach_Pat", "CgkI4ujSoJMCEAIQCw" },
		{ "SB_Ach_Mer", "CgkI4ujSoJMCEAIQDA" },
		{ "SB_Ach_Kev", "CgkI4ujSoJMCEAIQDQ" },
		{ "SB_Ach_King", "CgkI4ujSoJMCEAIQDg" }
	};

	public static Dictionary<string, string> IAP_ID_DIC_AMAZON = new Dictionary<string, string>
	{
		{ "com.mtvn.SBMI.jellybundle1", "com.mtvn.sbmi.amazonjellybundle1" },
		{ "com.mtvn.SBMI.jellybundle2", "com.mtvn.sbmi.amazonjellybundle2" },
		{ "com.mtvn.SBMI.jellybundle3", "com.mtvn.sbmi.amazonjellybundle3" },
		{ "com.mtvn.SBMI.jellybundle4", "com.mtvn.sbmi.amazonjellybundle4" },
		{ "com.mtvn.SBMI.jellybundle5", "com.mtvn.sbmi.amazonjellybundle5" },
		{ "com.mtvn.SBMI.jellybundle6", "com.mtvn.sbmi.amazonjellybundle6" },
		{ "com.mtvn.SBMI.bag1", "com.mtvn.sbmi.amazonbag1" },
		{ "com.mtvn.SBMI.bag2", "com.mtvn.sbmi.amazonbag2" },
		{ "com.mtvn.SBMI.bag3", "com.mtvn.sbmi.amazonbag3" },
		{ "com.mtvn.SBMI.bag4", "com.mtvn.sbmi.amazonbag4" }
	};

	public static Dictionary<string, string> IAP_ID_DIC_GOOGLE = new Dictionary<string, string>
	{
		{ "com.mtvn.SBMI.jellybundle1", "com.mtvn.sbmi.gplayjellybundle1" },
		{ "com.mtvn.SBMI.jellybundle2", "com.mtvn.sbmi.gplayjellybundle2" },
		{ "com.mtvn.SBMI.jellybundle3", "com.mtvn.sbmi.gplayjellybundle3" },
		{ "com.mtvn.SBMI.jellybundle4", "com.mtvn.sbmi.gplayjellybundle4" },
		{ "com.mtvn.SBMI.jellybundle5", "com.mtvn.sbmi.gplayjellybundle5" },
		{ "com.mtvn.SBMI.jellybundle6", "com.mtvn.sbmi.gplayjellybundle6" },
		{ "com.mtvn.SBMI.bag1", "com.mtvn.sbmi.gplaybag1" },
		{ "com.mtvn.SBMI.bag2", "com.mtvn.sbmi.gplaybag2" },
		{ "com.mtvn.SBMI.bag3", "com.mtvn.sbmi.gplaybag3" },
		{ "com.mtvn.SBMI.bag4", "com.mtvn.sbmi.gplaybag4" }
	};
}
