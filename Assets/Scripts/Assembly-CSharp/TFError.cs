using System;

public class TFError
{
	public const string ERROR_CODE_KEY = "error_code";

	public const int CONNECTION_NO_CONNECTION_AVAILABLE = 101;

	public const int CONNECTION_NO_SOARING_USER = 102;

	public const int SOARING_INTERNAL_ERROR = 103;

	public const int SOARING_AUTH_FAILED = 104;

	public const int INVALID_GAME_STATE = 200;

	public const int SAVE_GAMES_ALL_INVALID = 250;

	public const int SAVE_SERVER_GAME_INVALID = 301;

	public const int SAVE_CLIENT_GAME_INVALID = 302;

	public const int INVALID_RESOURCE = 303;

	public static void DM_LOG_ERROR_INVALID_SHEET(string sheetName)
	{
		TFUtils.ErrorLog("Cannot find database with sheet name: " + sheetName);
	}

	public static void DM_LOG_ERROR_NO_ROWS(string sheetName)
	{
		TFUtils.ErrorLog("No rows in sheet name: " + sheetName);
	}

	public static void DM_LOG_ERROR_INVALID_COLUMN(string col)
	{
		TFUtils.ErrorLog("Invalid Columns Name: " + col);
	}

	public static int GetErrorCode(Exception e, int default_code)
	{
		if (e == null)
		{
			return default_code;
		}
		if (e.Data == null)
		{
			return default_code;
		}
		if (!e.Data.Contains("error_code"))
		{
			return default_code;
		}
		return (int)e.Data["error_code"];
	}
}
