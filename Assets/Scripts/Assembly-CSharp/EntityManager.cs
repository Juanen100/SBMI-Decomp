#define ASSERTS_ON
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiniJSON;
using UnityEngine;
using Yarg;

public class EntityManager
{
	private delegate Blueprint BlueprintMarshaller(Dictionary<string, object> data, EntityManager mgr);

	private delegate void BlueprintAssetsInitializer(Blueprint blueprint, Dictionary<string, object> data, EntityManager mgr);

	public const string FOOTPRINT_MATERIAL = "Materials/unique/footprint";

	private const string DROPSHADOW_TEXTURE = "dropshadow.tga";

	private static readonly string BLUEPRINT_DIRECTORY_PATH;

	public static bool MustRegenerateStates;

	private static Dictionary<string, BlueprintMarshaller> TypeRegistry;

	private static Dictionary<string, BlueprintAssetsInitializer> AssetsInitializerTypeRegistry;

	public static Dictionary<string, Simulated.StateAction> BuildingActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> BuildingMachine;

	public static Dictionary<string, Simulated.StateAction> AnnexActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> AnnexMachine;

	public static Dictionary<string, Simulated.StateAction> DebrisActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> DebrisMachine;

	public static Dictionary<string, Simulated.StateAction> LandmarkActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> LandmarkMachine;

	public static Dictionary<string, Simulated.StateAction> ResidentActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> UnitMachine;

	public static Dictionary<string, Simulated.StateAction> TreasureActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> TreasureMachine;

	public static Dictionary<string, Simulated.StateAction> WorkerActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> WorkerMachine;

	public static Dictionary<string, Simulated.StateAction> WandererActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> WandererMachine;

	private static Dictionary<string, Blueprint> blueprints;

	private string[] blueprintFilePaths;

	private IEnumerator blueprintFileEnumerator;

	private static Dictionary<string, object> _pBpSpreadData;

	private Dictionary<Blueprint, Dictionary<string, object>> blueprintsToData = new Dictionary<Blueprint, Dictionary<string, object>>();

	private Factory<string, Entity> factory;

	private Dictionary<Identity, Entity> entities;

	private Dictionary<string, int> entityCount;

	private DisplayControllerManager displayControllerManager;

	public DisplayControllerManager DisplayControllerManager
	{
		get
		{
			return displayControllerManager;
		}
	}

	public Dictionary<string, Blueprint> Blueprints
	{
		get
		{
			return blueprints;
		}
	}

	public EntityManager(bool friendMode)
	{
		GenerateStates(friendMode);
		factory = new Factory<string, Entity>();
		entities = new Dictionary<Identity, Entity>(new Identity.Equality());
		entityCount = new Dictionary<string, int>();
		displayControllerManager = new DisplayControllerManager();
		blueprintsToData = new Dictionary<Blueprint, Dictionary<string, object>>();
		LoadBlueprints();
	}

	static EntityManager()
	{
		BLUEPRINT_DIRECTORY_PATH = "Blueprints";
		MustRegenerateStates = false;
		TypeRegistry = null;
		AssetsInitializerTypeRegistry = new Dictionary<string, BlueprintAssetsInitializer>();
		blueprints = new Dictionary<string, Blueprint>();
		MustRegenerateStates = true;
		GenerateStates(false);
	}

	public static void GenerateStates(bool friendMode)
	{
		if (MustRegenerateStates)
		{
			TypeRegistry = new Dictionary<string, BlueprintMarshaller>();
			AssetsInitializerTypeRegistry = new Dictionary<string, BlueprintAssetsInitializer>();
			TypeRegistry.Add("building", MarshallBuilding);
			BuildingStateSetup.Generate(out BuildingActions, out BuildingMachine, friendMode);
			TypeRegistry.Add("annex", MarshallAnnex);
			AnnexStateSetup.Generate(out AnnexActions, out AnnexMachine, friendMode);
			TypeRegistry.Add("debris", MarshallDebris);
			DebrisStateSetup.Generate(out DebrisActions, out DebrisMachine, friendMode);
			TypeRegistry.Add("landmark", MarshallLandmark);
			LandmarkStateSetup.Generate(out LandmarkActions, out LandmarkMachine, friendMode);
			TypeRegistry.Add("unit", MarshallUnit);
			AssetsInitializerTypeRegistry.Add("unit", InitializeUnitAssets);
			if (!friendMode)
			{
				UnitStateSetup.Generate(out ResidentActions, out UnitMachine);
			}
			else
			{
				UnitStateSetup.GenerateFriendsStates(out ResidentActions, out UnitMachine);
			}
			TypeRegistry.Add("worker", MarshallWorker);
			AssetsInitializerTypeRegistry.Add("worker", InitializeWorkerAssets);
			WorkerStateSetup.Generate(out WorkerActions, out WorkerMachine);
			TypeRegistry.Add("wanderer", MarshallWanderer);
			AssetsInitializerTypeRegistry.Add("wanderer", InitializeUnitAssets);
			WandererStateSetup.Generate(out WandererActions, out WandererMachine);
			TypeRegistry.Add("treasure", MarshallTreasure);
			TreasureStateSetup.Generate(out TreasureActions, out TreasureMachine, friendMode);
			MustRegenerateStates = false;
		}
	}

	private static void RegisterDisplayOffset(Dictionary<string, object> data, string theKey, Blueprint blueprint)
	{
		Vector3 v;
		if (data.ContainsKey("position_offset"))
		{
			TFUtils.LoadVector3(out v, (Dictionary<string, object>)data["position_offset"]);
			blueprint.Invariable[theKey + ".position_offset"] = v;
			return;
		}
		if (theKey == "display.default.flip")
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)data["display.default.flip"];
			{
				foreach (string key in dictionary.Keys)
				{
					if (key == "position_offset")
					{
						TFUtils.LoadVector3(out v, (Dictionary<string, object>)dictionary["position_offset"]);
						blueprint.Invariable[theKey + ".position_offset"] = v;
					}
				}
				return;
			}
		}
		blueprint.Invariable[theKey + ".position_offset"] = null;
	}

	private static void RegisterTextureOrigin(Dictionary<string, object> data, string theKey, Blueprint blueprint)
	{
		if (data.ContainsKey("texture_origin"))
		{
			Vector3 v;
			TFUtils.LoadVector3(out v, (Dictionary<string, object>)data["texture_origin"]);
			blueprint.Invariable[theKey + ".texture_origin"] = v;
		}
		else
		{
			blueprint.Invariable[theKey + ".texture_origin"] = null;
		}
	}

	private static void RegisterHitArea(Dictionary<string, object> data, QuadHitObject hitObject, string theKey, Blueprint blueprint)
	{
		Vector2 v = Vector2.zero;
		if (data.ContainsKey("mesh_name"))
		{
			string value = Convert.ToString(data["mesh_name"]);
			blueprint.Invariable[theKey + ".mesh_name"] = value;
			if (data.ContainsKey("separate_tap"))
			{
				bool flag = Convert.ToBoolean(data["separate_tap"]);
				blueprint.Invariable[theKey + ".separate_tap"] = flag;
			}
		}
		else if (data.ContainsKey("hit_area"))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)data["hit_area"];
			TFUtils.Assert(dictionary.ContainsKey("center") && dictionary.ContainsKey("width") && dictionary.ContainsKey("height"), "HitArea information must contain center, width and height information");
			TFUtils.LoadVector2(out v, (Dictionary<string, object>)dictionary["center"]);
			float width = TFUtils.LoadInt(dictionary, "width");
			float height = TFUtils.LoadInt(dictionary, "height");
			hitObject.Initialize(v, width, height);
			blueprint.Invariable[theKey + ".mesh_name"] = null;
		}
	}

	private static TFAnimatedSprite CreateAnimatedSpritePrototype(Dictionary<string, object> data, string theKey, Blueprint blueprint, Dictionary<string, object> fullData)
	{
		float width = TFUtils.LoadFloat(data, "width");
		float height = TFUtils.LoadInt(data, "height");
		return CreateAnimatedSpritePrototype(data, theKey, blueprint, width, height, fullData);
	}

	private static TFAnimatedSprite CreateAnimatedSpritePrototype(Dictionary<string, object> data, string theKey, Blueprint blueprint, float width, float height, Dictionary<string, object> fullData)
	{
		RegisterDisplayOffset(data, theKey, blueprint);
		if (fullData.ContainsKey("display.default.flip"))
		{
			RegisterDisplayOffset(fullData, "display.default.flip", blueprint);
		}
		RegisterTextureOrigin(data, theKey, blueprint);
		SpriteAnimationModel animModel = new SpriteAnimationModel();
		TFAnimatedSprite tFAnimatedSprite = new TFAnimatedSprite(new Vector2(0f, -0.5f * height), width, height, animModel);
		RegisterShareableSpaceSnap(data, blueprint);
		RegisterHitArea(data, tFAnimatedSprite.HitObject, theKey, blueprint);
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (string key in fullData.Keys)
		{
			if (key.Contains("display."))
			{
				dictionary = (Dictionary<string, object>)fullData[key];
				RegisterMeshName(dictionary, blueprint, key);
			}
		}
		return tFAnimatedSprite;
	}

	private static void RegisterShareableSpaceSnap(Dictionary<string, object> data, Blueprint blueprint)
	{
		if (data.ContainsKey("shareable_space_snap"))
		{
			bool flag = Convert.ToBoolean(data["shareable_space_snap"]);
			blueprint.Invariable["shareable_space_snap"] = flag;
		}
	}

	private static void RegisterMeshName(Dictionary<string, object> data, Blueprint blueprint, string theKey)
	{
		if (data.ContainsKey("mesh_name"))
		{
			string value = Convert.ToString(data["mesh_name"]);
			blueprint.Invariable[theKey + ".mesh_name"] = value;
		}
	}

	private static IDisplayController CreatePaperdollPrototype(Dictionary<string, object> data, string theKey, Blueprint blueprint, Paperdoll.PaperdollType paperdollType)
	{
		int num = TFUtils.LoadInt(data, "width");
		int num2 = TFUtils.LoadInt(data, "height");
		RegisterDisplayOffset(data, theKey, blueprint);
		Vector3 v = Vector3.one;
		if (data.ContainsKey("display_scale"))
		{
			TFUtils.LoadVector3(out v, (Dictionary<string, object>)data["display_scale"], 1f);
		}
		bool? flag = TFUtils.TryLoadBool(data, "flippable");
		if (!flag.HasValue)
		{
			flag = true;
		}
		Paperdoll paperdoll = new Paperdoll(new Vector2(0f, -0.5f * (float)num2), num, num2, v, flag.Value, paperdollType);
		RegisterShareableSpaceSnap(data, blueprint);
		RegisterHitArea(data, paperdoll.HitObject, theKey, blueprint);
		return paperdoll;
	}

	private static void LoadCostumeFromBlueprint(Dictionary<string, object> data, string theKey, Blueprint blueprint, EntityManager mgr, Paperdoll.PaperdollType paperdollType)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)data[theKey];
		PaperdollSkin paperdollSkin = null;
		Dictionary<string, PaperdollSkin> dictionary2 = new Dictionary<string, PaperdollSkin>();
		if (!theKey.Equals("costumes"))
		{
			return;
		}
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			paperdollSkin = new PaperdollSkin();
			paperdollSkin.name = item.Key;
			Dictionary<string, object> dictionary3 = (Dictionary<string, object>)item.Value;
			object value = null;
			if (dictionary3.TryGetValue("skeleton", out value))
			{
				foreach (KeyValuePair<string, object> item2 in (Dictionary<string, object>)value)
				{
					paperdollSkin.skeletons.Add(item2.Key, (string)item2.Value);
				}
				paperdollSkin.skeletonKey = paperdollSkin.skeletons.Keys.First();
				paperdollSkin.skeletonReplacement = paperdollSkin.skeletons.Values.First();
			}
			object value2 = null;
			if (dictionary3.TryGetValue("props", out value2))
			{
				List<object> list = (List<object>)value2;
				for (int i = 0; i < list.Count; i++)
				{
					Dictionary<string, object> dictionary4 = (Dictionary<string, object>)list[i];
					Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
					foreach (KeyValuePair<string, object> item3 in dictionary4)
					{
						dictionary5.Add(item3.Key, (string)item3.Value);
					}
					paperdollSkin.propData.Add(dictionary5);
				}
			}
			dictionary2.Add(paperdollSkin.name, paperdollSkin);
		}
		blueprint.Invariable[theKey] = dictionary2;
	}

	private static void LoadDisplayController(Dictionary<string, object> data, string theKey, Blueprint blueprint, EntityManager mgr, Paperdoll.PaperdollType paperdollType)
	{
		bool condition = false;
		string text = theKey;
		if (CommonUtils.TextureLod() < CommonUtils.LevelOfDetail.Standard && data.ContainsKey(theKey + "_lr"))
		{
			theKey += "_lr";
		}
		string text2 = null;
		IDisplayController displayController = null;
		Dictionary<string, object> dictionary = (Dictionary<string, object>)data[theKey];
		if (dictionary.ContainsKey("model_type"))
		{
			text2 = (string)dictionary["model_type"];
			if (text2.Equals("sprite"))
			{
				float num = 1f;
				if (dictionary.ContainsKey("scale"))
				{
					num = TFUtils.LoadFloat(dictionary, "scale");
				}
				string text3 = theKey + ".default";
				TFUtils.Assert(data.ContainsKey(text3), "All sprites must have a *.default defined!\nMissing " + text3 + " in blueprint " + blueprint);
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)data[text3];
				float num2 = 0f;
				float num3 = 0f;
				object value = null;
				object value2 = null;
				if (dictionary2.TryGetValue("width", out value) && dictionary2.TryGetValue("height", out value2))
				{
					num2 = Convert.ToInt32(value);
					num3 = Convert.ToInt32(value2);
				}
				else
				{
					object value3 = null;
					if (dictionary2.TryGetValue("texture", out value3))
					{
						string text4 = (string)value3;
						TFUtils.Assert(YGTextureLibrary.HasAtlasCoords(text4), "The texture atlas does not have an entry for " + text4);
						AtlasCoords atlasCoords = YGTextureLibrary.GetAtlasCoords(text4).atlasCoords;
						num2 = (float)TFAnimatedSprite.CalcWorldSize(atlasCoords.spriteSize.width, 1.0);
						num3 = (float)TFAnimatedSprite.CalcWorldSize(atlasCoords.spriteSize.height, 1.0);
					}
				}
				displayController = CreateAnimatedSpritePrototype(dictionary, text, blueprint, num2 * num, num3 * num, data);
			}
			else if (text2.Equals("paperdoll"))
			{
				displayController = CreatePaperdollPrototype(dictionary, text, blueprint, paperdollType);
			}
			if (dictionary.ContainsKey("perspective_in_art"))
			{
				displayController.isPerspectiveInArt = (bool)dictionary["perspective_in_art"];
			}
			blueprint.Invariable[text] = displayController;
		}
		foreach (string key in data.Keys)
		{
			if (key.Length > theKey.Length && key.Substring(0, theKey.Length).Equals(theKey) && key[theKey.Length] == '.')
			{
				Dictionary<string, object> dictionary3 = (Dictionary<string, object>)data[key];
				object value4 = null;
				if (dictionary3.TryGetValue("quad", out value4))
				{
					RegisterDisplayOffset((Dictionary<string, object>)value4, key, blueprint);
				}
				displayController.AddDisplayState(dictionary3);
				if (key.Contains("default"))
				{
					condition = true;
				}
			}
		}
		TFUtils.Assert(condition, "EntityManager.LoadDisplayController(): '" + theKey + "' is missing a '" + theKey + ".default' in the blueprint.");
	}

	private static void LoadEffects(Dictionary<string, object> data, Blueprint blueprint)
	{
		string text = "fx";
		foreach (string key in data.Keys)
		{
			if (key.Length > text.Length && key.Substring(0, text.Length).Equals(text) && key[text.Length] == '.')
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)data[key];
				string text2 = (string)dictionary["type"];
				if (!text2.Equals("particles"))
				{
					throw new NotImplementedException("fx of type " + text2 + " not supported.");
				}
				ParticleSystemManager.Request request = new ParticleSystemManager.Request();
				request.effectsName = (string)dictionary["fx_name"];
				request.initialPriority = TFUtils.LoadInt(dictionary, "initial_priority");
				request.subsequentPriority = TFUtils.LoadInt(dictionary, "sub_priority");
				request.cyclingPeriod = TFUtils.LoadFloat(dictionary, "cycling_period");
				Vector3 v = Vector3.zero;
				if (dictionary.ContainsKey("position_offset"))
				{
					TFUtils.LoadVector3(out v, (Dictionary<string, object>)dictionary["position_offset"]);
				}
				blueprint.Invariable[key + ".position_offset"] = v;
				blueprint.Invariable[key] = request;
			}
		}
	}

	private static Blueprint MarshallCommon(Dictionary<string, object> data, int width, int height, EntityManager mgr)
	{
		Blueprint blueprint = new Blueprint();
		int num = TFUtils.LoadInt(data, "did");
		blueprint.Invariable["name"] = (string)data["name"];
		blueprint.Invariable["type"] = EntityTypeNamingHelper.StringToType(TFUtils.LoadString(data, "type"));
		blueprint.Invariable["did"] = num;
		blueprint.Invariable["blueprint"] = EntityTypeNamingHelper.GetBlueprintName((string)data["type"], num);
		blueprint.Invariable["footprint"] = new AlignedBox(0f, width, 0f, height);
		blueprint.Invariable["footprintSprite"] = new BasicSprite("Materials/unique/footprint", null, new Vector2(-0.5f * (float)width, -0.5f * (float)height), width, height);
		blueprint.Invariable["footprint.flip"] = new AlignedBox(0f, height, 0f, width);
		blueprint.Invariable["display.position_offset"] = Vector2.zero;
		blueprint.Invariable["dropshadow"] = null;
		blueprint.Invariable["debugBoxSprite"] = new BasicSprite("Materials/unique/footprint", null, new Vector2(-0.5f * (float)width, -0.5f * (float)height), width, height);
		object value = null;
		if (data.TryGetValue("sound_on_select", out value))
		{
			blueprint.Invariable["sound_on_select"] = value;
		}
		else
		{
			blueprint.Invariable["sound_on_select"] = "SelectSimulated";
		}
		if (data.ContainsKey("disabled"))
		{
			blueprint.Invariable["disabled"] = TFUtils.LoadBool(data, "disabled");
		}
		blueprint.Invariable["sound_on_touch_error"] = "Error";
		blueprint.Invariable["sound_on_touch"] = "TouchSimulated";
		if (data.TryGetValue("sound_on_touch", out value))
		{
			blueprint.Invariable["sound_on_touch"] = value;
		}
		if (data.TryGetValue("thought_display_movement", out value))
		{
			blueprint.Invariable["thought_display_movement"] = (bool)value;
		}
		else
		{
			blueprint.Invariable["thought_display_movement"] = true;
		}
		if (data.TryGetValue("instance_limit", out value))
		{
			blueprint.Invariable["instance_limit"] = AmountDictionary.FromJSONDict((Dictionary<string, object>)value);
			Dictionary<int, int> dictionary = (Dictionary<int, int>)blueprint.Invariable["instance_limit"];
			TFUtils.Assert(dictionary.ContainsKey(1), "No limit set at level 1");
		}
		else
		{
			blueprint.Invariable["instance_limit"] = new Dictionary<int, int>();
		}
		LoadEffects(data, blueprint);
		return blueprint;
	}

	private static BasicSprite CreateDropShadow(float width, float height)
	{
		float num = width * 0.5f;
		float num2 = height * 0.5f;
		return new BasicSprite(null, "dropshadow.tga", new Vector2(-0.1f * num, -0.1f * num2), num, num2);
	}

	private static void LoadUnitsFromSpread()
	{
		string text = "Units";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "max paytables");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet6 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		int columnIndexInSheet7 = instance.GetColumnIndexInSheet(sheetIndex, "wishtable did");
		int columnIndexInSheet8 = instance.GetColumnIndexInSheet(sheetIndex, "speed");
		int columnIndexInSheet9 = instance.GetColumnIndexInSheet(sheetIndex, "name");
		int columnIndexInSheet10 = instance.GetColumnIndexInSheet(sheetIndex, "disabled");
		int columnIndexInSheet11 = instance.GetColumnIndexInSheet(sheetIndex, "join paytables");
		int columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "won't go home");
		int columnIndexInSheet13 = instance.GetColumnIndexInSheet(sheetIndex, "disable if will flee");
		int columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "gross item wishtable did");
		int columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "forbidden item wishtable did");
		string value = CommonUtils.PropertyForDeviceOverride("disable_lr_models");
		bool flag = false;
		if (!string.IsNullOrEmpty(value))
		{
			flag = true;
		}
		string text2 = "n/a";
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet3);
			}
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet4);
			string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
			if (_pBpSpreadData.ContainsKey(blueprintName))
			{
				continue;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("did", intCell);
			dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
			dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet6));
			dictionary.Add("wish_table_did", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet7));
			dictionary.Add("gross_items_wish_table_id", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet14));
			dictionary.Add("forbidden_items_wish_table_id", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet15));
			dictionary.Add("speed", instance.GetFloatCell(sheetIndex, rowIndex, columnIndexInSheet8));
			dictionary.Add("type", stringCell);
			dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet9));
			dictionary.Add("disabled", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet10) == 1);
			dictionary.Add("join_paytables", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet11) == 1);
			dictionary.Add("go_home_exempt", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet12) == 1);
			dictionary.Add("disable_if_will_flee", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet13) == 1);
			int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "drop shadow diameter");
			if (intCell2 >= 0)
			{
				dictionary.Add("dropshadow_diameter", intCell2);
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "default costume did");
			if (intCell2 >= 0)
			{
				dictionary.Add("default_costume_did", intCell2);
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "hungry timer");
			if (intCell2 >= 0)
			{
				dictionary.Add("time.hungry", intCell2);
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "timer duration");
			if (intCell2 >= 0)
			{
				dictionary.Add("timer_duration", intCell2);
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "hide duration");
			if (intCell2 >= 0)
			{
				dictionary.Add("hide_duration", intCell2);
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "autoquest intro");
			if (intCell2 >= 0)
			{
				dictionary.Add("auto_quest_intro", intCell2);
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "autoquest outro");
			if (intCell2 >= 0)
			{
				dictionary.Add("auto_quest_outro", intCell2);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, "character dialog portrait");
			if (text2 != stringCell)
			{
				dictionary.Add("character_dialog_portrait", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, "quest reminder icon");
			if (text2 != stringCell)
			{
				dictionary.Add("quest_reminder_icon", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, "sound on touch");
			if (text2 != stringCell)
			{
				dictionary.Add("sound_on_touch", stringCell);
			}
			dictionary.Add("display", new Dictionary<string, object>
			{
				{
					"model_type",
					instance.GetStringCell(text, rowName, "character model type")
				},
				{
					"width",
					instance.GetIntCell(sheetIndex, rowIndex, "character model width")
				},
				{
					"height",
					instance.GetIntCell(sheetIndex, rowIndex, "character model height")
				},
				{
					"display_scale",
					new Dictionary<string, object>
					{
						{
							"x",
							instance.GetFloatCell(sheetIndex, rowIndex, "character model scale x")
						},
						{
							"y",
							instance.GetFloatCell(sheetIndex, rowIndex, "character model scale y")
						},
						{
							"z",
							instance.GetFloatCell(sheetIndex, rowIndex, "character model scale z")
						}
					}
				},
				{
					"position_offset",
					new Dictionary<string, object>
					{
						{
							"x",
							instance.GetFloatCell(sheetIndex, rowIndex, "character model offset x")
						},
						{
							"y",
							instance.GetFloatCell(sheetIndex, rowIndex, "character model offset y")
						}
					}
				}
			});
			if (!flag)
			{
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, "low res character model type");
				if (text2 != stringCell)
				{
					dictionary.Add("display_lr", new Dictionary<string, object>
					{
						{ "model_type", stringCell },
						{
							"width",
							instance.GetIntCell(sheetIndex, rowIndex, "low res character model width")
						},
						{
							"height",
							instance.GetIntCell(sheetIndex, rowIndex, "low res character model height")
						},
						{
							"display_scale",
							new Dictionary<string, object>
							{
								{
									"x",
									instance.GetFloatCell(sheetIndex, rowIndex, "low res character model scale x")
								},
								{
									"y",
									instance.GetFloatCell(sheetIndex, rowIndex, "low res character model scale y")
								},
								{
									"z",
									instance.GetFloatCell(sheetIndex, rowIndex, "low res character model scale z")
								}
							}
						},
						{
							"position_offset",
							new Dictionary<string, object>
							{
								{
									"x",
									instance.GetFloatCell(sheetIndex, rowIndex, "low res character model offset x")
								},
								{
									"y",
									instance.GetFloatCell(sheetIndex, rowIndex, "low res character model offset y")
								}
							}
						}
					});
				}
			}
			dictionary.Add("thought_display", new Dictionary<string, object>
			{
				{
					"model_type",
					instance.GetStringCell(sheetIndex, rowIndex, "thought model type")
				},
				{
					"position_offset",
					new Dictionary<string, object>
					{
						{
							"x",
							instance.GetIntCell(sheetIndex, rowIndex, "thought model offset x")
						},
						{
							"y",
							instance.GetIntCell(sheetIndex, rowIndex, "thought model offset y")
						}
					}
				}
			});
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "thought quad width");
			if (intCell2 >= 0)
			{
				((Dictionary<string, object>)dictionary["thought_display"]).Add("quad", new Dictionary<string, object>
				{
					{ "width", intCell2 },
					{
						"height",
						instance.GetIntCell(sheetIndex, rowIndex, "thought quad height")
					}
				});
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "thought tapbox width");
			if (intCell2 >= 0)
			{
				((Dictionary<string, object>)dictionary["thought_display"]).Add("hit_area", new Dictionary<string, object>
				{
					{ "width", intCell2 },
					{
						"height",
						instance.GetIntCell(sheetIndex, rowIndex, "thought tapbox height")
					},
					{
						"center",
						new Dictionary<string, object>
						{
							{
								"x",
								instance.GetIntCell(sheetIndex, rowIndex, "thought tapbox center x")
							},
							{
								"y",
								instance.GetIntCell(sheetIndex, rowIndex, "thought tapbox center y")
							}
						}
					}
				});
			}
			List<object> list = new List<object>();
			for (int j = 0; j < num2; j++)
			{
				intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "bonus paytable did " + (j + 1));
				if (intCell2 >= 0)
				{
					list.Add(intCell2);
				}
			}
			if (list.Count > 0)
			{
				dictionary.Add("match_bonus_paytables", list);
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "wish cooldown min");
			if (intCell2 >= 0)
			{
				dictionary.Add("wishing", new Dictionary<string, object>
				{
					{ "wish_cooldown_min", intCell2 },
					{
						"wish_cooldown_max",
						instance.GetIntCell(sheetIndex, rowIndex, "wish cooldown max")
					},
					{
						"wish_duration",
						instance.GetIntCell(sheetIndex, rowIndex, "wish duration")
					}
				});
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "idle cooldown min");
			if (intCell2 >= 0)
			{
				dictionary.Add("idle", new Dictionary<string, object>
				{
					{
						"cooldown",
						new Dictionary<string, object>
						{
							{ "min", intCell2 },
							{
								"max",
								instance.GetIntCell(sheetIndex, rowIndex, "idle cooldown max")
							}
						}
					},
					{
						"duration",
						new Dictionary<string, object>
						{
							{
								"min",
								instance.GetIntCell(sheetIndex, rowIndex, "idle duration min")
							},
							{
								"max",
								instance.GetIntCell(sheetIndex, rowIndex, "idle duration max")
							}
						}
					}
				});
			}
			_pBpSpreadData.Add(blueprintName, dictionary);
		}
	}

	private void LoadAnnexesFromSpread()
	{
		string text = "Annexes";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		string text2 = "n/a";
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
			string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
			if (!_pBpSpreadData.ContainsKey(blueprintName))
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				if (num2 < 0)
				{
					num2 = instance.GetIntCell(sheetIndex, rowIndex, "instance limits");
				}
				dictionary.Add("did", intCell);
				dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
				dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
				dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, "level min"));
				dictionary.Add("build_time", instance.GetIntCell(sheetIndex, rowIndex, "build time"));
				dictionary.Add("build_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, "build timer duration"));
				dictionary.Add("type", stringCell);
				dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
				dictionary.Add("portrait", instance.GetStringCell(sheetIndex, rowIndex, "portrait"));
				dictionary.Add("is_waypoint", instance.GetIntCell(sheetIndex, rowIndex, "is waypoint") == 1);
				dictionary.Add("stashable", instance.GetIntCell(sheetIndex, rowIndex, "stashable") == 1);
				int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "rent time");
				int? num3 = ((intCell2 < 0) ? ((int?)null) : new int?(intCell2));
				dictionary.Add("rent_time", num3);
				intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "resident");
				num3 = ((intCell2 < 0) ? ((int?)null) : new int?(intCell2));
				dictionary.Add("resident", num3);
				intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "hub info did");
				if (intCell2 >= 0)
				{
					dictionary.Add("hub_info", new Dictionary<string, object> { { "did", intCell2 } });
				}
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, "accept placement sound");
				if (text2 != stringCell)
				{
					dictionary.Add("accept_placement_sound", stringCell);
				}
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, "hub");
				if (text2 != stringCell)
				{
					dictionary.Add("hub", stringCell);
				}
				dictionary.Add("point_of_interest", new Dictionary<string, object>
				{
					{
						"facing",
						instance.GetStringCell(sheetIndex, rowIndex, "point of interest facing")
					},
					{
						"x",
						instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
					},
					{
						"y",
						instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
					}
				});
				dictionary.Add("completion_reward", new Dictionary<string, object>
				{
					{
						"resources",
						new Dictionary<string, object> { 
						{
							"5",
							instance.GetIntCell(sheetIndex, rowIndex, "completion reward resources xp")
						} }
					},
					{
						"thought_icon",
						instance.GetStringCell(text, rowName, "completion reward thought icon")
					}
				});
				dictionary.Add("instance_limit", new Dictionary<string, object>());
				for (int j = 1; j <= num2; j++)
				{
					((Dictionary<string, object>)dictionary["instance_limit"]).Add(j.ToString(), instance.GetIntCell(sheetIndex, rowIndex, "instance limit level " + j));
				}
				_pBpSpreadData.Add(blueprintName, dictionary);
			}
		}
	}

	private void LoadCharacterBuildingsFromSpread()
	{
		string text = "CharacterBuildings";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		int columnIndexInSheet6 = instance.GetColumnIndexInSheet(sheetIndex, "level min");
		int columnIndexInSheet7 = instance.GetColumnIndexInSheet(sheetIndex, "build time");
		int columnIndexInSheet8 = instance.GetColumnIndexInSheet(sheetIndex, "build timer duration");
		int columnIndexInSheet9 = instance.GetColumnIndexInSheet(sheetIndex, "rent time");
		int columnIndexInSheet10 = instance.GetColumnIndexInSheet(sheetIndex, "rent timer duration");
		int columnIndexInSheet11 = instance.GetColumnIndexInSheet(sheetIndex, "name");
		int columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "portrait");
		int columnIndexInSheet13 = instance.GetColumnIndexInSheet(sheetIndex, "flippable");
		int columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "has move in");
		int columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "rent rushable");
		int columnIndexInSheet16 = instance.GetColumnIndexInSheet(sheetIndex, "completion sound");
		string text2 = "n/a";
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
			string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
			if (_pBpSpreadData.ContainsKey(blueprintName))
			{
				continue;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, "num residents");
			}
			dictionary.Add("did", intCell);
			dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
			dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
			dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet6));
			dictionary.Add("build_time", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet7));
			dictionary.Add("build_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet8));
			dictionary.Add("rent_time", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet9));
			dictionary.Add("rent_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet10));
			dictionary.Add("type", stringCell);
			dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet11));
			dictionary.Add("portrait", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet12));
			dictionary.Add("flippable", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet13) == 1);
			dictionary.Add("has_move_in", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet14) == 1);
			dictionary.Add("rent_rushable", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet15) == 1);
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet16);
			if (text2 != stringCell)
			{
				dictionary.Add("completion_sound", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, "sound on select");
			if (text2 != stringCell)
			{
				dictionary.Add("sound_on_select", stringCell);
			}
			dictionary.Add("point_of_interest", new Dictionary<string, object>
			{
				{
					"facing",
					instance.GetStringCell(sheetIndex, rowIndex, "point of interest facing")
				},
				{
					"x",
					instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
				},
				{
					"y",
					instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
				}
			});
			int? num3 = instance.GetIntCell(sheetIndex, rowIndex, "resident 1");
			if (num3.Value < 0)
			{
				dictionary.Add("resident", (int?)null);
			}
			else
			{
				dictionary.Add("residents", new List<int>());
				((List<int>)dictionary["residents"]).Add(num3.Value);
				for (int j = 2; j <= num2; j++)
				{
					num3 = instance.GetIntCell(sheetIndex, rowIndex, "resident " + j);
					if (num3.Value >= 0)
					{
						((List<int>)dictionary["residents"]).Add(num3.Value);
					}
				}
			}
			dictionary.Add("completion_reward", new Dictionary<string, object>
			{
				{
					"resources",
					new Dictionary<string, object> { 
					{
						"5",
						instance.GetIntCell(sheetIndex, rowIndex, "completion reward resources xp")
					} }
				},
				{
					"thought_icon",
					instance.GetStringCell(sheetIndex, rowIndex, "completion reward thought icon")
				}
			});
			int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "completion reward resources gold");
			if (intCell2 > 0)
			{
				((Dictionary<string, object>)((Dictionary<string, object>)dictionary["completion_reward"])["resources"]).Add("3", intCell2);
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "completion reward movie");
			if (intCell2 > 0)
			{
				((Dictionary<string, object>)dictionary["completion_reward"]).Add("movies", new Dictionary<string, object> { 
				{
					intCell2.ToString(),
					1
				} });
			}
			dictionary.Add("product", new Dictionary<string, object>
			{
				{
					"resources",
					new Dictionary<string, object>()
				},
				{
					"summary",
					new Dictionary<string, object> { { "thought_icon", null } }
				},
				{ "thought_icon", null }
			});
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "product resources gold");
			if (intCell2 > 0)
			{
				((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["resources"]).Add("3", intCell2);
				((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["summary"]).Add("resources", new Dictionary<string, object> { { "3", intCell2 } });
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "product resources xp");
			if (intCell2 > 0)
			{
				((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["resources"]).Add("5", intCell2);
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "product special did");
			if (intCell2 >= 0)
			{
				((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["resources"]).Add(intCell2.ToString(), instance.GetIntCell(sheetIndex, rowIndex, "product special amount"));
			}
			dictionary.Add("instance_limit", new Dictionary<string, object> { 
			{
				"1",
				instance.GetIntCell(sheetIndex, rowIndex, "instance limit 1")
			} });
			_pBpSpreadData.Add(blueprintName, dictionary);
		}
	}

	private void LoadDebrisFromSpread()
	{
		string text = "Debris";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
			string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
			if (_pBpSpreadData.ContainsKey(blueprintName))
			{
				continue;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, "clearing jelly reward columns");
			}
			dictionary.Add("did", intCell);
			dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
			dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
			dictionary.Add("clear_time", instance.GetIntCell(sheetIndex, rowIndex, "clear time"));
			dictionary.Add("timer_duration", instance.GetIntCell(sheetIndex, rowIndex, "timer duration"));
			dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, "level min"));
			dictionary.Add("type", stringCell);
			dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
			dictionary.Add("is_waypoint", instance.GetIntCell(sheetIndex, rowIndex, "is waypoint") == 1);
			dictionary.Add("point_of_interest", new Dictionary<string, object>
			{
				{
					"facing",
					instance.GetStringCell(sheetIndex, rowIndex, "point of interest facing")
				},
				{
					"x",
					instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
				},
				{
					"y",
					instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
				}
			});
			dictionary.Add("cost", new Dictionary<string, object> { 
			{
				"3",
				instance.GetIntCell(sheetIndex, rowIndex, "cost gold")
			} });
			int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "clearing reward xp");
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, "clearing reward thought icon");
			dictionary.Add("clearing_reward", new Dictionary<string, object>
			{
				{
					"resources",
					new Dictionary<string, object> { { "5", intCell2 } }
				},
				{ "thought_icon", stringCell },
				{
					"summary",
					new Dictionary<string, object>
					{
						{ "thought_icon", stringCell },
						{
							"resources",
							new Dictionary<string, object> { { "5", intCell2 } }
						}
					}
				}
			});
			for (int j = 1; j <= num2; j++)
			{
				intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "clearing jelly reward amount " + j);
				if (intCell2 < 0)
				{
					break;
				}
				if (j == 1)
				{
					((Dictionary<string, object>)((Dictionary<string, object>)dictionary["clearing_reward"])["resources"]).Add("2", new Dictionary<string, object>());
				}
				((Dictionary<string, object>)((Dictionary<string, object>)((Dictionary<string, object>)dictionary["clearing_reward"])["resources"])["2"]).Add(intCell2.ToString(), instance.GetFloatCell(sheetIndex, rowIndex, "clearing jelly reward odds " + j));
			}
			_pBpSpreadData.Add(blueprintName, dictionary);
		}
	}

	private void LoadDecorationsFromSpread()
	{
		string text = "Decorations";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
			string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
			if (!_pBpSpreadData.ContainsKey(blueprintName))
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("did", intCell);
				dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
				dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
				dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, "level min"));
				dictionary.Add("build_time", instance.GetIntCell(sheetIndex, rowIndex, "build time"));
				dictionary.Add("type", stringCell);
				dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
				dictionary.Add("portrait", instance.GetStringCell(sheetIndex, rowIndex, "portrait"));
				dictionary.Add("completion_sound", instance.GetStringCell(sheetIndex, rowIndex, "completion sound"));
				dictionary.Add("accept_placement_sound", instance.GetStringCell(sheetIndex, rowIndex, "accept placement sound"));
				dictionary.Add("is_waypoint", instance.GetIntCell(sheetIndex, rowIndex, "is waypoint") == 1);
				dictionary.Add("flippable", instance.GetIntCell(sheetIndex, rowIndex, "flippable") == 1);
				dictionary.Add("shareable_space", instance.GetIntCell(sheetIndex, rowIndex, "shareable space") == 1);
				dictionary.Add("point_of_interest", new Dictionary<string, object>
				{
					{
						"facing",
						instance.GetStringCell(text, rowName, "point of interest facing")
					},
					{
						"x",
						instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
					},
					{
						"y",
						instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
					}
				});
				dictionary.Add("completion_reward", new Dictionary<string, object>
				{
					{
						"resources",
						new Dictionary<string, object> { 
						{
							"5",
							instance.GetIntCell(sheetIndex, rowIndex, "completion reward xp")
						} }
					},
					{
						"thought_icon",
						instance.GetStringCell(text, rowName, "completion reward thought icon")
					}
				});
				dictionary.Add("rent", null);
				dictionary.Add("rent_time", null);
				dictionary.Add("resident", null);
				dictionary.Add("product", null);
				_pBpSpreadData.Add(blueprintName, dictionary);
			}
		}
	}

	private void LoadLandmarksFromSpread()
	{
		string text = "Landmarks";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
			string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
			if (!_pBpSpreadData.ContainsKey(blueprintName))
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("did", intCell);
				dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
				dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
				dictionary.Add("type", stringCell);
				dictionary.Add("name", instance.GetStringCell(text, rowName, "name"));
				dictionary.Add("is_waypoint", instance.GetIntCell(sheetIndex, rowIndex, "is waypoint") == 1);
				dictionary.Add("point_of_interest", new Dictionary<string, object>
				{
					{
						"facing",
						instance.GetStringCell(text, rowName, "point of interest facing")
					},
					{
						"x",
						instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
					},
					{
						"y",
						instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
					}
				});
				_pBpSpreadData.Add(blueprintName, dictionary);
			}
		}
	}

	private void LoadRentOnlyBuildingsFromSpread()
	{
		string text = "RentOnlyBuildings";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		string text2 = "n/a";
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
			string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
			if (_pBpSpreadData.ContainsKey(blueprintName))
			{
				continue;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, "instance limits");
			}
			dictionary.Add("did", intCell);
			dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
			dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
			dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, "level min"));
			dictionary.Add("build_time", instance.GetIntCell(sheetIndex, rowIndex, "build time"));
			dictionary.Add("build_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, "build timer duration"));
			dictionary.Add("rent_time", instance.GetIntCell(sheetIndex, rowIndex, "rent time"));
			dictionary.Add("rent_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, "rent timer duration"));
			dictionary.Add("type", stringCell);
			dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
			dictionary.Add("portrait", instance.GetStringCell(sheetIndex, rowIndex, "portrait"));
			dictionary.Add("flippable", instance.GetIntCell(sheetIndex, rowIndex, "flippable") == 1);
			dictionary.Add("has_move_in", instance.GetIntCell(sheetIndex, rowIndex, "has move in") == 1);
			dictionary.Add("rent_rushable", instance.GetIntCell(sheetIndex, rowIndex, "rent rushable") == 1);
			dictionary.Add("stashable", instance.GetIntCell(sheetIndex, rowIndex, "stashable") == 1);
			dictionary.Add("worker_spawner", instance.GetIntCell(sheetIndex, rowIndex, "worker spawner") == 1);
			stringCell = instance.GetStringCell(text, rowName, "completion sound");
			if (text2 != stringCell)
			{
				dictionary.Add("completion_sound", stringCell);
			}
			stringCell = instance.GetStringCell(text, rowName, "sound on select");
			if (text2 != stringCell)
			{
				dictionary.Add("sound_on_select", stringCell);
			}
			dictionary.Add("point_of_interest", new Dictionary<string, object>
			{
				{
					"facing",
					instance.GetStringCell(sheetIndex, rowIndex, "point of interest facing")
				},
				{
					"x",
					instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
				},
				{
					"y",
					instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
				}
			});
			dictionary.Add("completion_reward", new Dictionary<string, object>
			{
				{
					"resources",
					new Dictionary<string, object> { 
					{
						"5",
						instance.GetIntCell(sheetIndex, rowIndex, "completion reward resources xp")
					} }
				},
				{
					"thought_icon",
					instance.GetStringCell(sheetIndex, rowIndex, "completion reward thought icon")
				}
			});
			int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "completion reward movie");
			if (intCell2 > 0)
			{
				((Dictionary<string, object>)dictionary["completion_reward"]).Add("movies", new Dictionary<string, object> { 
				{
					intCell2.ToString(),
					1
				} });
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "product resources gold");
			if (intCell2 > 0)
			{
				dictionary.Add("product", new Dictionary<string, object>
				{
					{
						"resources",
						new Dictionary<string, object>()
					},
					{
						"summary",
						new Dictionary<string, object> { { "thought_icon", null } }
					},
					{ "thought_icon", null }
				});
				((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["resources"]).Add("3", intCell2);
				((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["summary"]).Add("resources", new Dictionary<string, object> { { "3", intCell2 } });
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "product resources xp");
			if (intCell2 > 0)
			{
				if (!dictionary.ContainsKey("product"))
				{
					dictionary.Add("product", new Dictionary<string, object>
					{
						{
							"resources",
							new Dictionary<string, object>()
						},
						{
							"summary",
							new Dictionary<string, object> { { "thought_icon", null } }
						},
						{ "thought_icon", null }
					});
				}
				((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["resources"]).Add("5", intCell2);
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "product special did");
			if (intCell2 >= 0)
			{
				if (!dictionary.ContainsKey("product"))
				{
					dictionary.Add("product", new Dictionary<string, object>
					{
						{
							"resources",
							new Dictionary<string, object>()
						},
						{
							"summary",
							new Dictionary<string, object> { { "thought_icon", null } }
						},
						{ "thought_icon", null }
					});
				}
				int intCell3 = instance.GetIntCell(sheetIndex, rowIndex, "product special amount");
				((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["resources"]).Add(intCell2.ToString(), intCell3);
				if (((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["summary"]).ContainsKey("resources"))
				{
					((Dictionary<string, object>)((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["summary"])["resources"]).Add(intCell2.ToString(), intCell3);
				}
				else
				{
					((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["summary"]).Add("resources", new Dictionary<string, object> { 
					{
						intCell2.ToString(),
						intCell3
					} });
				}
			}
			dictionary.Add("instance_limit", new Dictionary<string, object>());
			for (int j = 1; j <= num2; j++)
			{
				((Dictionary<string, object>)dictionary["instance_limit"]).Add(j.ToString(), instance.GetIntCell(sheetIndex, rowIndex, "instance limit level " + j));
			}
			dictionary.Add("resident", null);
			_pBpSpreadData.Add(blueprintName, dictionary);
		}
	}

	private void LoadShopsFromSpread()
	{
		string text = "Shops";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		int columnIndexInSheet6 = instance.GetColumnIndexInSheet(sheetIndex, "name");
		int columnIndexInSheet7 = instance.GetColumnIndexInSheet(sheetIndex, "level min");
		int columnIndexInSheet8 = instance.GetColumnIndexInSheet(sheetIndex, "build time");
		int columnIndexInSheet9 = instance.GetColumnIndexInSheet(sheetIndex, "build timer duration");
		int columnIndexInSheet10 = instance.GetColumnIndexInSheet(sheetIndex, "portrait");
		int columnIndexInSheet11 = instance.GetColumnIndexInSheet(sheetIndex, "flippable");
		int columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "has move in");
		int columnIndexInSheet13 = instance.GetColumnIndexInSheet(sheetIndex, "stashable");
		int columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "shunts crafting");
		int columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "is waypoint");
		string text2 = "n/a";
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
			string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
			if (_pBpSpreadData.ContainsKey(blueprintName))
			{
				continue;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, "num residents");
			}
			dictionary.Add("did", intCell);
			dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
			dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
			dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet7));
			dictionary.Add("build_time", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet8));
			dictionary.Add("build_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet9));
			dictionary.Add("type", stringCell);
			dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet6));
			dictionary.Add("portrait", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet10));
			dictionary.Add("flippable", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet11) == 1);
			dictionary.Add("has_move_in", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet12) == 1);
			dictionary.Add("stashable", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet13) == 1);
			dictionary.Add("shunts_crafting", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet14) == 1);
			dictionary.Add("is_waypoint", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet15) == 1);
			int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "rent time");
			int? num3 = ((intCell2 < 0) ? ((int?)null) : new int?(intCell2));
			dictionary.Add("rent_time", num3);
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "rent timer duration");
			if (intCell2 > 0)
			{
				dictionary.Add("rent_timer_duration", intCell2);
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "crafting menu");
			if (intCell2 > 0)
			{
				dictionary.Add("crafting_menu", intCell2);
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "crafting timer duration");
			if (intCell2 > 0)
			{
				dictionary.Add("crafting_timer_duration", intCell2);
			}
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "vendor id");
			if (intCell2 > 0)
			{
				dictionary.Add("vendor_id", intCell2);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, "completion sound");
			if (text2 != stringCell)
			{
				dictionary.Add("completion_sound", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, "sound on select");
			if (text2 != stringCell)
			{
				dictionary.Add("sound_on_select", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, "crafted icon");
			if (text2 != stringCell)
			{
				dictionary.Add("crafted_icon", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, "blueprint");
			if (text2 != stringCell)
			{
				dictionary.Add("blueprint", stringCell);
			}
			num3 = instance.GetIntCell(sheetIndex, rowIndex, "resident 1");
			if (num3.Value < 0)
			{
				dictionary.Add("resident", (int?)null);
			}
			else
			{
				dictionary.Add("residents", new List<int>());
				((List<int>)dictionary["residents"]).Add(num3.Value);
				for (int j = 2; j <= num2; j++)
				{
					num3 = instance.GetIntCell(sheetIndex, rowIndex, "resident " + j);
					if (num3.Value >= 0)
					{
						((List<int>)dictionary["residents"]).Add(num3.Value);
					}
				}
			}
			dictionary.Add("point_of_interest", new Dictionary<string, object>
			{
				{
					"facing",
					instance.GetStringCell(sheetIndex, rowIndex, "point of interest facing")
				},
				{
					"x",
					instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
				},
				{
					"y",
					instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
				}
			});
			dictionary.Add("completion_reward", new Dictionary<string, object>
			{
				{
					"resources",
					new Dictionary<string, object> { 
					{
						"5",
						instance.GetIntCell(sheetIndex, rowIndex, "completion reward resources xp")
					} }
				},
				{
					"thought_icon",
					instance.GetStringCell(sheetIndex, rowIndex, "completion reward thought icon")
				}
			});
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "completion reward movie");
			if (intCell2 > 0)
			{
				((Dictionary<string, object>)dictionary["completion_reward"]).Add("movies", new Dictionary<string, object> { 
				{
					intCell2.ToString(),
					1
				} });
			}
			_pBpSpreadData.Add(blueprintName, dictionary);
		}
	}

	private void LoadTreasureFromSpread()
	{
		string text = "Treasure";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		int num2 = -1;
		int[] array = new int[0];
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
			string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
			if (_pBpSpreadData.ContainsKey(blueprintName))
			{
				continue;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, "clearing reward sets");
				array = new int[num2];
				for (int j = 1; j <= num2; j++)
				{
					array[j - 1] = instance.GetIntCell(sheetIndex, rowIndex, "clearing reward " + j + " columns");
				}
			}
			dictionary.Add("did", intCell);
			dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
			dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
			dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, "level min"));
			dictionary.Add("timer_duration", instance.GetIntCell(sheetIndex, rowIndex, "timer duration"));
			dictionary.Add("clear_time", instance.GetIntCell(sheetIndex, rowIndex, "clear time"));
			dictionary.Add("type", stringCell);
			dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
			dictionary.Add("is_waypoint", instance.GetIntCell(sheetIndex, rowIndex, "is waypoint") == 1);
			dictionary.Add("quick_clear", instance.GetIntCell(sheetIndex, rowIndex, "quick clear") == 1);
			dictionary.Add("point_of_interest", new Dictionary<string, object>
			{
				{
					"facing",
					instance.GetStringCell(sheetIndex, rowIndex, "point of interest facing")
				},
				{
					"x",
					instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
				},
				{
					"y",
					instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
				}
			});
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, "thought icon");
			dictionary.Add("clearing_reward", new Dictionary<string, object>
			{
				{
					"resources",
					new Dictionary<string, object>()
				},
				{ "thought_icon", stringCell },
				{
					"summary",
					new Dictionary<string, object> { { "thought_icon", stringCell } }
				}
			});
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)((Dictionary<string, object>)dictionary["clearing_reward"])["resources"];
			for (int k = 1; k <= num2; k++)
			{
				int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "clearing reward " + k + " did");
				if (intCell2 < 0)
				{
					continue;
				}
				stringCell = intCell2.ToString();
				if (!dictionary2.ContainsKey(stringCell))
				{
					dictionary2.Add(stringCell, new Dictionary<string, object>());
				}
				for (int l = 1; l <= array[k - 1]; l++)
				{
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "clearing reward " + k + " amount " + l);
					if (intCell2 >= 0 && !((Dictionary<string, object>)dictionary2[stringCell]).ContainsKey(intCell2.ToString()))
					{
						((Dictionary<string, object>)dictionary2[stringCell]).Add(intCell2.ToString(), instance.GetFloatCell(sheetIndex, rowIndex, "clearing reward " + k + " odds " + l));
					}
				}
			}
			_pBpSpreadData.Add(blueprintName, dictionary);
		}
	}

	private void LoadTreesFromSpread()
	{
		string text = "Trees";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		string text2 = "n/a";
		int num2 = -1;
		int num3 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
			string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
			if (_pBpSpreadData.ContainsKey(blueprintName))
			{
				continue;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, "product sets");
				num3 = instance.GetIntCell(sheetIndex, rowIndex, "instance limits");
			}
			dictionary.Add("did", intCell);
			dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
			dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
			dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, "level min"));
			dictionary.Add("build_time", instance.GetIntCell(sheetIndex, rowIndex, "build time"));
			dictionary.Add("build_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, "build timer duration"));
			dictionary.Add("crafting_menu", instance.GetIntCell(sheetIndex, rowIndex, "crafting menu"));
			dictionary.Add("crafting_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, "crafting timer duration"));
			dictionary.Add("type", stringCell);
			dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
			dictionary.Add("portrait", instance.GetStringCell(sheetIndex, rowIndex, "portrait"));
			dictionary.Add("flippable", instance.GetIntCell(sheetIndex, rowIndex, "flippable") == 1);
			dictionary.Add("is_waypoint", instance.GetIntCell(sheetIndex, rowIndex, "is waypoint") == 1);
			int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "rent time");
			int? num4 = ((intCell2 < 0) ? ((int?)null) : new int?(intCell2));
			dictionary.Add("rent_time", num4);
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "rent timer duration");
			if (intCell2 > 0)
			{
				dictionary.Add("rent_timer_duration", intCell2);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, "completion sound");
			if (text2 != stringCell)
			{
				dictionary.Add("completion_sound", stringCell);
			}
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, "accept placement sound");
			if (text2 != stringCell)
			{
				dictionary.Add("accept_placement_sound", stringCell);
			}
			dictionary.Add("point_of_interest", new Dictionary<string, object>
			{
				{
					"facing",
					instance.GetStringCell(sheetIndex, rowIndex, "point of interest facing")
				},
				{
					"x",
					instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
				},
				{
					"y",
					instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
				}
			});
			dictionary.Add("completion_reward", new Dictionary<string, object>
			{
				{
					"resources",
					new Dictionary<string, object> { 
					{
						"5",
						instance.GetIntCell(sheetIndex, rowIndex, "completion reward resources xp")
					} }
				},
				{
					"summary",
					new Dictionary<string, object>
					{
						{
							"resources",
							new Dictionary<string, object> { 
							{
								"5",
								instance.GetIntCell(sheetIndex, rowIndex, "completion reward resources xp")
							} }
						},
						{
							"thought_icon",
							instance.GetStringCell(sheetIndex, rowIndex, "completion reward thought icon")
						}
					}
				},
				{
					"thought_icon",
					instance.GetStringCell(sheetIndex, rowIndex, "completion reward thought icon")
				}
			});
			int intCell3 = instance.GetIntCell(sheetIndex, rowIndex, "fruit did");
			dictionary.Add("product", new Dictionary<string, object>
			{
				{
					"cdf",
					new List<object>()
				},
				{
					"summary",
					new Dictionary<string, object>
					{
						{
							"resources",
							new Dictionary<string, object> { 
							{
								intCell3.ToString(),
								instance.GetIntCell(sheetIndex, rowIndex, "product summary")
							} }
						},
						{
							"thought_icon",
							instance.GetStringCell(sheetIndex, rowIndex, "completion reward thought icon")
						}
					}
				}
			});
			for (int j = 1; j <= num2; j++)
			{
				intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "product fruit " + j);
				if (intCell2 >= 0)
				{
					((List<object>)((Dictionary<string, object>)dictionary["product"])["cdf"]).Add(new Dictionary<string, object>
					{
						{
							"p",
							instance.GetFloatCell(sheetIndex, rowIndex, "product odds " + j)
						},
						{
							"value",
							new Dictionary<string, object> { 
							{
								"resources",
								new Dictionary<string, object>
								{
									{
										"5",
										instance.GetIntCell(sheetIndex, rowIndex, "product xp " + j)
									},
									{
										intCell3.ToString(),
										intCell2
									}
								}
							} }
						}
					});
				}
			}
			dictionary.Add("instance_limit", new Dictionary<string, object>());
			for (int k = 1; k <= num3; k++)
			{
				((Dictionary<string, object>)dictionary["instance_limit"]).Add(k.ToString(), instance.GetIntCell(sheetIndex, rowIndex, "instance limit " + k));
			}
			dictionary.Add("resident", null);
			_pBpSpreadData.Add(blueprintName, dictionary);
		}
	}

	private void OverwriteBlueprintDataWithSpread(Dictionary<string, object> data)
	{
		if (data == null || _pBpSpreadData == null)
		{
			return;
		}
		int did = -1;
		string primaryType = string.Empty;
		if (data.ContainsKey("did"))
		{
			did = TFUtils.LoadInt(data, "did");
		}
		if (data.ContainsKey("type"))
		{
			primaryType = TFUtils.LoadString(data, "type");
		}
		primaryType = EntityTypeNamingHelper.GetBlueprintName(primaryType, did);
		if (!_pBpSpreadData.ContainsKey(primaryType))
		{
			return;
		}
		Dictionary<string, object> dictionary = (Dictionary<string, object>)_pBpSpreadData[primaryType];
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			if (data.ContainsKey(item.Key))
			{
				data[item.Key] = item.Value;
			}
			else
			{
				data.Add(item.Key, item.Value);
			}
		}
	}

	private static Blueprint MarshallUnit(Dictionary<string, object> data, EntityManager mgr)
	{
		int width = TFUtils.LoadInt(data, "width");
		int height = TFUtils.LoadInt(data, "height");
		Blueprint blueprint = MarshallCommon(data, width, height, mgr);
		blueprint.Invariable["base_speed"] = TFUtils.LoadFloat(data, "speed");
		blueprint.Invariable["machine"] = UnitMachine;
		blueprint.Invariable["action"] = ResidentActions["idle"];
		blueprint.Invariable["immobile"] = false;
		blueprint.Invariable["join_paytables"] = TFUtils.TryLoadBool(data, "join_paytables");
		blueprint.Variable["speed"] = blueprint.Invariable["base_speed"];
		List<string> list = new List<string>();
		object value = null;
		if (data.TryGetValue("task_open_sounds", out value))
		{
			foreach (object item in value as List<object>)
			{
				list.Add((string)item);
			}
			value = null;
		}
		else
		{
			list.Add("OpenMenu");
		}
		blueprint.Invariable["task_open_sounds"] = list;
		List<string> list2 = new List<string>();
		if (data.TryGetValue("task_selected_sounds", out value))
		{
			foreach (object item2 in value as List<object>)
			{
				list2.Add((string)item2);
			}
			value = null;
		}
		else
		{
			list2.Add("TaskSelected");
		}
		blueprint.Invariable["task_selected_sounds"] = list2;
		MarshallWishingInfo(ref blueprint, data);
		MarshallBonusInfo(ref blueprint, data);
		if (data.ContainsKey("wish_table_did"))
		{
			blueprint.Invariable["wish_table_did"] = TFUtils.LoadInt(data, "wish_table_did");
		}
		else
		{
			blueprint.Invariable["wish_table_did"] = -1;
			TFUtils.Assert(false, "Resident did: " + blueprint.Invariable["did"].ToString() + " does not have a wish table did.");
		}
		if (data.ContainsKey("gross_items_wish_table_id"))
		{
			blueprint.Invariable["gross_items_wish_table_id"] = TFUtils.LoadInt(data, "gross_items_wish_table_id");
		}
		else
		{
			blueprint.Invariable["gross_items_wish_table_id"] = -1;
			TFUtils.Assert(false, "Resident did: " + blueprint.Invariable["gross_items_wish_table_id"].ToString() + " does not have a gross item table did.");
		}
		if (data.ContainsKey("forbidden_items_wish_table_id"))
		{
			blueprint.Invariable["forbidden_items_wish_table_id"] = TFUtils.LoadInt(data, "forbidden_items_wish_table_id");
		}
		else
		{
			blueprint.Invariable["forbidden_items_wish_table_id"] = -1;
			TFUtils.Assert(false, "Resident did: " + blueprint.Invariable["forbidden_items_wish_table_id"].ToString() + " does not have a forbidden item table did.");
		}
		blueprint.Invariable["favorite_reward"] = ((!data.ContainsKey("favorite_reward")) ? null : RewardDefinition.FromObject(data["favorite_reward"]));
		blueprint.Invariable["satisfaction_reward"] = ((!data.ContainsKey("satisfaction_reward")) ? null : RewardDefinition.FromObject(data["satisfaction_reward"]));
		blueprint.Invariable["timer_duration"] = TFUtils.LoadFloat(data, "timer_duration");
		if (data.ContainsKey("idle"))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)data["idle"];
			Dictionary<string, object> d = (Dictionary<string, object>)dictionary["cooldown"];
			Dictionary<string, object> d2 = (Dictionary<string, object>)dictionary["duration"];
			blueprint.Invariable["idle.cooldown.min"] = TFUtils.LoadInt(d, "min");
			blueprint.Invariable["idle.cooldown.max"] = TFUtils.LoadInt(d, "max");
			blueprint.Invariable["idle.duration.min"] = TFUtils.LoadInt(d2, "min");
			blueprint.Invariable["idle.duration.max"] = TFUtils.LoadInt(d2, "max");
		}
		else
		{
			TFUtils.WarningLog("No values specified for idling. Using default values");
			blueprint.Invariable["idle.cooldown.min"] = 10;
			blueprint.Invariable["idle.cooldown.max"] = 45;
			blueprint.Invariable["idle.duration.min"] = 3;
			blueprint.Invariable["idle.duration.max"] = 15;
		}
		if (data.ContainsKey("go_home_exempt"))
		{
			blueprint.Invariable["go_home_exempt"] = TFUtils.LoadBool(data, "go_home_exempt");
		}
		else
		{
			blueprint.Invariable["go_home_exempt"] = false;
		}
		if (data.ContainsKey("auto_quest_intro"))
		{
			blueprint.Invariable["auto_quest_intro"] = TFUtils.LoadInt(data, "auto_quest_intro");
		}
		if (data.ContainsKey("auto_quest_outro"))
		{
			blueprint.Invariable["auto_quest_outro"] = TFUtils.LoadInt(data, "auto_quest_outro");
		}
		if (data.ContainsKey("character_dialog_portrait"))
		{
			blueprint.Invariable["character_dialog_portrait"] = TFUtils.LoadString(data, "character_dialog_portrait");
		}
		if (data.ContainsKey("quest_reminder_icon"))
		{
			blueprint.Invariable["quest_reminder_icon"] = TFUtils.LoadString(data, "quest_reminder_icon");
		}
		int? num = TFUtils.TryLoadInt(data, "default_costume_did");
		if (num.HasValue && num.Value < 0)
		{
			num = null;
		}
		blueprint.Invariable["default_costume_did"] = num;
		return blueprint;
	}

	private static Blueprint MarshallBuilding(Dictionary<string, object> data, EntityManager mgr)
	{
		int width = TFUtils.LoadInt(data, "width");
		int height = TFUtils.LoadInt(data, "height");
		Blueprint blueprint = MarshallCommon(data, width, height, mgr);
		blueprint.Invariable["immobile"] = true;
		object value = null;
		if (data.TryGetValue("portrait", out value))
		{
			blueprint.Invariable["portrait"] = value;
			value = null;
		}
		blueprint.Invariable["has_move_in"] = false;
		if (data.TryGetValue("has_move_in", out value))
		{
			blueprint.Invariable["has_move_in"] = (bool)value;
			value = null;
		}
		blueprint.Invariable["stashable"] = true;
		if (data.TryGetValue("stashable", out value))
		{
			blueprint.Invariable["stashable"] = value;
			value = null;
		}
		blueprint.Invariable["flippable"] = true;
		if (data.TryGetValue("flippable", out value))
		{
			blueprint.Invariable["flippable"] = value;
			value = null;
		}
		blueprint.Invariable["sellable"] = true;
		if (data.TryGetValue("sellable", out value))
		{
			blueprint.Invariable["sellable"] = value;
			value = null;
		}
		ulong num = TFUtils.LoadUlong(data, "build_time");
		TFUtils.Assert(num >= 0, string.Format("blueprint {0} is missing a build_time", data["name"]));
		blueprint.Invariable["time.build"] = num;
		if (num != 0)
		{
			blueprint.Invariable["build_timer_duration"] = TFUtils.LoadFloat(data, "build_timer_duration");
		}
		blueprint.Invariable["build_rush_cost"] = ResourceManager.CalculateConstructionRushCost(num);
		if (data.ContainsKey("shareable_space"))
		{
			blueprint.Invariable["shareable_space"] = TFUtils.LoadBool(data, "shareable_space");
		}
		if (data.ContainsKey("shareable_space_snap"))
		{
			blueprint.Invariable["shareable_space_snap"] = TFUtils.LoadBool(data, "shareable_space_snap");
			Debug.LogError("Does this ever reach?");
		}
		int? num2 = TFUtils.TryLoadInt(data, "level_min");
		if (!num2.HasValue)
		{
			num2 = 1;
		}
		blueprint.Invariable["level.minimum"] = num2.Value;
		blueprint.Invariable["machine"] = BuildingMachine;
		blueprint.Invariable["action"] = BuildingActions["placing"];
		if (data.ContainsKey("pet"))
		{
			blueprint.Invariable["pet"] = TFUtils.LoadNullableInt(data, "pet");
		}
		if (data.TryGetValue("point_of_interest", out value))
		{
			Dictionary<string, object> d = (Dictionary<string, object>)value;
			blueprint.Invariable["point_of_interest"] = new Vector2(TFUtils.LoadInt(d, "x"), TFUtils.LoadInt(d, "y"));
			value = null;
		}
		else
		{
			blueprint.Invariable["point_of_interest"] = new Vector2(0f, 0f);
		}
		if (!data.TryGetValue("product", out value))
		{
			value = null;
		}
		if (value != null)
		{
			ulong num3 = TFUtils.LoadUlong(data, "rent_time");
			blueprint.Invariable["time.production"] = num3;
			blueprint.Invariable["rent_timer_duration"] = TFUtils.LoadFloat(data, "rent_timer_duration");
			blueprint.Invariable["product"] = RewardDefinition.FromObject(data["product"]);
			bool? flag = TFUtils.TryLoadBool(data, "rent_rushable");
			if (!flag.HasValue)
			{
				flag = true;
			}
			blueprint.Invariable["rent_rushable"] = flag.Value;
			blueprint.Invariable["rent_rush_cost"] = ResourceManager.CalculateRentRushCost(num3);
			value = null;
		}
		else
		{
			blueprint.Invariable["time.production"] = null;
			blueprint.Invariable["product"] = null;
			blueprint.Invariable["product.amount"] = null;
		}
		if (data.TryGetValue("worker_spawner", out value))
		{
			blueprint.Invariable["worker_spawner"] = (bool)value;
			value = null;
		}
		if (data.TryGetValue("is_waypoint", out value))
		{
			blueprint.Invariable["is_waypoint"] = (bool)value;
			value = null;
		}
		if (data.TryGetValue("crafting_menu", out value))
		{
			blueprint.Invariable["crafting_menu"] = (int)value;
			blueprint.Invariable["crafting_timer_duration"] = TFUtils.LoadFloat(data, "crafting_timer_duration");
			value = null;
			if (data.TryGetValue("crafted_icon", out value))
			{
				blueprint.Invariable["crafted_icon"] = (string)value;
			}
			value = null;
			MarshallShuntedCraftingInfo(ref blueprint, data);
		}
		if (data.ContainsKey("vendor_id"))
		{
			blueprint.Invariable["vendor_id"] = TFUtils.LoadInt(data, "vendor_id");
			blueprint.Invariable["restock_time"] = 3600uL;
			blueprint.Invariable["special_time"] = 86400uL;
		}
		if (data.ContainsKey("taskbook_id"))
		{
			blueprint.Invariable["taskbook_id"] = TFUtils.LoadInt(data, "taskbook_id");
		}
		if (data.ContainsKey("completion_sound"))
		{
			blueprint.Invariable["completion_sound"] = TFUtils.LoadString(data, "completion_sound");
		}
		if (data.ContainsKey("accept_placement_sound"))
		{
			blueprint.Invariable["accept_placement_sound"] = TFUtils.LoadString(data, "accept_placement_sound");
		}
		blueprint.Invariable["completion_reward"] = ((!data.ContainsKey("completion_reward")) ? null : RewardDefinition.FromObject(data["completion_reward"]));
		if (data.ContainsKey("busy_annex_count"))
		{
			blueprint.Variable["busy_annex_count"] = TFUtils.LoadInt(data, "busy_annex_count");
		}
		else
		{
			blueprint.Variable["busy_annex_count"] = 0;
		}
		MarshalResidentInfo(ref blueprint, data);
		return blueprint;
	}

	private static Blueprint MarshallAnnex(Dictionary<string, object> data, EntityManager mgr)
	{
		Blueprint blueprint = MarshallBuilding(data, mgr);
		blueprint.Invariable["machine"] = AnnexMachine;
		blueprint.Invariable["action"] = AnnexActions["placing"];
		MarshallHubInfo(ref blueprint, data);
		return blueprint;
	}

	private static Blueprint MarshallDebris(Dictionary<string, object> data, EntityManager mgr)
	{
		int width = TFUtils.LoadInt(data, "width");
		int height = TFUtils.LoadInt(data, "height");
		Blueprint blueprint = MarshallCommon(data, width, height, mgr);
		blueprint.Invariable["immobile"] = true;
		if (data.ContainsKey("portrait"))
		{
			blueprint.Invariable["portrait"] = data["portrait"];
		}
		ulong num = TFUtils.LoadUlong(data, "clear_time");
		blueprint.Invariable["time.clear"] = num;
		blueprint.Invariable["cost"] = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		blueprint.Invariable["timer_duration"] = TFUtils.LoadFloat(data, "timer_duration");
		blueprint.Invariable["clear_rush_cost"] = ResourceManager.CalculateDebrisRushCost(num);
		blueprint.Invariable["machine"] = DebrisMachine;
		blueprint.Invariable["action"] = DebrisActions["inactive"];
		if (data.ContainsKey("point_of_interest"))
		{
			Dictionary<string, object> d = (Dictionary<string, object>)data["point_of_interest"];
			blueprint.Invariable["point_of_interest"] = new Vector2(TFUtils.LoadInt(d, "x"), TFUtils.LoadInt(d, "y"));
		}
		else
		{
			blueprint.Invariable["point_of_interest"] = new Vector2(0f, 0f);
		}
		if (data.ContainsKey("is_waypoint"))
		{
			blueprint.Invariable["is_waypoint"] = (bool)data["is_waypoint"];
		}
		RewardDefinition rewardDefinition = RewardDefinition.FromObject(data["clearing_reward"]);
		blueprint.Invariable["clearing_reward"] = rewardDefinition;
		TFUtils.Assert(rewardDefinition.Summary.ThoughtIcon != null, "Debris must specify a reward thought icon");
		return blueprint;
	}

	private static Blueprint MarshallWorker(Dictionary<string, object> data, EntityManager mgr)
	{
		int width = TFUtils.LoadInt(data, "width");
		int height = TFUtils.LoadInt(data, "height");
		Blueprint blueprint = MarshallCommon(data, width, height, mgr);
		blueprint.Invariable["immobile"] = false;
		blueprint.Variable["speed"] = TFUtils.LoadFloat(data, "speed");
		blueprint.Invariable["machine"] = WorkerMachine;
		blueprint.Invariable["action"] = WorkerActions["idle"];
		return blueprint;
	}

	private static Blueprint MarshallWanderer(Dictionary<string, object> data, EntityManager mgr)
	{
		int width = TFUtils.LoadInt(data, "width");
		int height = TFUtils.LoadInt(data, "height");
		Blueprint blueprint = MarshallCommon(data, width, height, mgr);
		blueprint.Invariable["base_speed"] = TFUtils.LoadFloat(data, "speed");
		blueprint.Invariable["machine"] = WandererMachine;
		blueprint.Invariable["action"] = WandererActions["hidden"];
		blueprint.Invariable["immobile"] = false;
		blueprint.Invariable["hide_duration"] = TFUtils.LoadInt(data, "hide_duration");
		blueprint.Invariable["disable_if_will_flee"] = TFUtils.TryLoadBool(data, "disable_if_will_flee");
		blueprint.Variable["speed"] = blueprint.Invariable["base_speed"];
		if (data.ContainsKey("idle"))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)data["idle"];
			Dictionary<string, object> d = (Dictionary<string, object>)dictionary["cooldown"];
			Dictionary<string, object> d2 = (Dictionary<string, object>)dictionary["duration"];
			blueprint.Invariable["idle.cooldown.min"] = TFUtils.LoadInt(d, "min");
			blueprint.Invariable["idle.cooldown.max"] = TFUtils.LoadInt(d, "max");
			blueprint.Invariable["idle.duration.min"] = TFUtils.LoadInt(d2, "min");
			blueprint.Invariable["idle.duration.max"] = TFUtils.LoadInt(d2, "max");
		}
		else
		{
			TFUtils.WarningLog("No values specified for idling. Using default values");
			blueprint.Invariable["idle.cooldown.min"] = 10;
			blueprint.Invariable["idle.cooldown.max"] = 45;
			blueprint.Invariable["idle.duration.min"] = 3;
			blueprint.Invariable["idle.duration.max"] = 15;
		}
		return blueprint;
	}

	private static Blueprint MarshallLandmark(Dictionary<string, object> data, EntityManager mgr)
	{
		int width = TFUtils.LoadInt(data, "width");
		int height = TFUtils.LoadInt(data, "height");
		Blueprint blueprint = MarshallCommon(data, width, height, mgr);
		blueprint.Invariable["immobile"] = true;
		if (data.ContainsKey("point_of_interest"))
		{
			Dictionary<string, object> d = (Dictionary<string, object>)data["point_of_interest"];
			blueprint.Invariable["point_of_interest"] = new Vector2(TFUtils.LoadInt(d, "x"), TFUtils.LoadInt(d, "y"));
		}
		else
		{
			blueprint.Invariable["point_of_interest"] = new Vector2(0f, 0f);
		}
		if (data.ContainsKey("is_waypoint"))
		{
			blueprint.Invariable["is_waypoint"] = (bool)data["is_waypoint"];
		}
		blueprint.Invariable["machine"] = LandmarkMachine;
		blueprint.Invariable["action"] = LandmarkActions["inactive"];
		return blueprint;
	}

	private static Blueprint MarshallTreasure(Dictionary<string, object> data, EntityManager mgr)
	{
		int width = TFUtils.LoadInt(data, "width");
		int height = TFUtils.LoadInt(data, "height");
		Blueprint blueprint = MarshallCommon(data, width, height, mgr);
		blueprint.Invariable["immobile"] = true;
		if (data.ContainsKey("portrait"))
		{
			blueprint.Invariable["portrait"] = data["portrait"];
		}
		ulong num = TFUtils.LoadUint(data, "clear_time");
		blueprint.Invariable["time.clear"] = num;
		blueprint.Invariable["timer_duration"] = TFUtils.LoadFloat(data, "timer_duration");
		if (data.ContainsKey("quick_clear"))
		{
			blueprint.Invariable["quick_clear"] = TFUtils.LoadBool(data, "quick_clear");
		}
		else
		{
			blueprint.Invariable["quick_clear"] = false;
		}
		blueprint.Invariable["machine"] = TreasureMachine;
		blueprint.Invariable["action"] = TreasureActions["buried"];
		if (data.ContainsKey("point_of_interest"))
		{
			Dictionary<string, object> d = (Dictionary<string, object>)data["point_of_interest"];
			blueprint.Invariable["point_of_interest"] = new Vector2(TFUtils.LoadInt(d, "x"), TFUtils.LoadInt(d, "y"));
		}
		else
		{
			blueprint.Invariable["point_of_interest"] = new Vector2(0f, 0f);
		}
		if (data.ContainsKey("is_waypoint"))
		{
			blueprint.Invariable["is_waypoint"] = (bool)data["is_waypoint"];
		}
		RewardDefinition value = RewardDefinition.FromObject(data["clearing_reward"]);
		blueprint.Invariable["clearing_reward"] = value;
		return blueprint;
	}

	private static void MarshallWishingInfo(ref Blueprint blueprint, Dictionary<string, object> data)
	{
		if (data.ContainsKey("wishing"))
		{
			Dictionary<string, object> d = TFUtils.LoadDict(data, "wishing");
			blueprint.Invariable["wish_cooldown_min"] = TFUtils.LoadInt(d, "wish_cooldown_min");
			blueprint.Invariable["wish_cooldown_max"] = TFUtils.LoadInt(d, "wish_cooldown_max");
			blueprint.Invariable["wish_duration"] = TFUtils.LoadInt(d, "wish_duration");
		}
		else if (data.ContainsKey("time.hungry"))
		{
			blueprint.Invariable["wish_cooldown_min"] = 5;
			blueprint.Invariable["wish_cooldown_max"] = 30;
			blueprint.Invariable["wish_duration"] = 60;
		}
	}

	private static void MarshallBonusInfo(ref Blueprint blueprint, Dictionary<string, object> data)
	{
		if (data.ContainsKey("match_bonus_paytables"))
		{
			blueprint.Invariable["match_bonus_paytables"] = TFUtils.LoadList<uint>(data, "match_bonus_paytables");
			return;
		}
		List<uint> list = new List<uint>();
		list.Add(1u);
		blueprint.Invariable["match_bonus_paytables"] = list;
	}

	private static void MarshalResidentInfo(ref Blueprint blueprint, Dictionary<string, object> data)
	{
		string text = "resident";
		string text2 = "residents";
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.Assert(!data.ContainsKey(text) || !data.ContainsKey(text2), "Cannot have both a 'residents' and 'resident' field!");
			if (data.ContainsKey(text))
			{
				List<object> list = data[text] as List<object>;
				if (list != null)
				{
					TFUtils.ErrorLog("Found list for field '" + text + "'. Did you mean to use the 'residents' field?\nBlueprint=" + blueprint);
				}
			}
		}
		blueprint.Invariable[text2] = new List<int>();
		if (data.ContainsKey(text))
		{
			int? num = TFUtils.LoadNullableInt(data, text);
			if (num.HasValue)
			{
				blueprint.Invariable[text2] = new List<int> { num.Value };
			}
		}
		else if (data.ContainsKey(text2))
		{
			blueprint.Invariable[text2] = TFUtils.LoadList<int>(data, text2);
		}
	}

	private static void MarshallHubInfo(ref Blueprint blueprint, Dictionary<string, object> data)
	{
		if (data.ContainsKey("hub_info"))
		{
			Dictionary<string, object> dictionary = TFUtils.TryLoadDict(data, "hub_info");
			if (dictionary.ContainsKey("id"))
			{
				TFUtils.Assert(!dictionary.ContainsKey("did"), string.Format("Should specify {0} xor {1} in the {2} property!", "id", "did", "hub_info"));
				blueprint.Invariable["hub_id"] = new Identity(TFUtils.LoadString(dictionary, "id"));
			}
			else if (dictionary.ContainsKey("did"))
			{
				blueprint.Invariable["hub_did"] = TFUtils.LoadUint(dictionary, "did");
			}
		}
		else
		{
			blueprint.Invariable["hub_id"] = new Identity(TFUtils.LoadString(data, "hub"));
		}
	}

	private static void MarshallShuntedCraftingInfo(ref Blueprint blueprint, Dictionary<string, object> data)
	{
		if (data.ContainsKey("shunts_crafting"))
		{
			blueprint.Invariable["shunts_crafting"] = TFUtils.LoadBool(data, "shunts_crafting");
		}
		else if (data.ContainsKey("shunt_crafting_to_did"))
		{
			blueprint.Invariable["shunts_crafting"] = true;
		}
		else
		{
			blueprint.Invariable["shunts_crafting"] = false;
		}
	}

	private static void InitializeBlueprintAssets(Blueprint blueprint, Dictionary<string, object> data, EntityManager mgr)
	{
		TFUtils.Assert(data.ContainsKey("thought_display"), "Missing thought display in " + (string)data["name"]);
		TFUtils.Assert(data.ContainsKey("thought_item_bubble_display"), "Missing thought item bubble in " + (string)data["name"]);
		TFUtils.Assert(data.ContainsKey("display"), "All blueprints must contain the display controller definition.");
		EntityType primaryType = blueprint.PrimaryType;
		Paperdoll.PaperdollType paperdollType = Paperdoll.PaperdollType.Other;
		switch (primaryType)
		{
		case EntityType.RESIDENT:
		case EntityType.WORKER:
		case EntityType.WANDERER:
			paperdollType = Paperdoll.PaperdollType.Character;
			break;
		case EntityType.BUILDING:
			paperdollType = Paperdoll.PaperdollType.Building;
			break;
		}
		LoadDisplayController(data, "display", blueprint, mgr, paperdollType);
		LoadDisplayController(data, "thought_display", blueprint, mgr, Paperdoll.PaperdollType.Other);
		if (data.ContainsKey("thought_item_bubble_display"))
		{
			LoadDisplayController(data, "thought_item_bubble_display", blueprint, mgr, Paperdoll.PaperdollType.Other);
		}
		else
		{
			blueprint.Invariable["thought_item_bubble_display"] = null;
		}
		if (data.ContainsKey("thought_mask_display"))
		{
			LoadDisplayController(data, "thought_mask_display", blueprint, mgr, Paperdoll.PaperdollType.Other);
		}
		else
		{
			blueprint.Invariable["thought_mask_display"] = null;
		}
		if (data.ContainsKey("costumes"))
		{
			LoadCostumeFromBlueprint(data, "costumes", blueprint, mgr, Paperdoll.PaperdollType.Other);
		}
		else
		{
			blueprint.Invariable["costumes"] = null;
		}
	}

	private static void InitializeUnitAssets(Blueprint blueprint, Dictionary<string, object> data, EntityManager mgr)
	{
		float num = 20f;
		if (data.ContainsKey("dropshadow_diameter"))
		{
			num = TFUtils.LoadFloat(data, "dropshadow_diameter");
		}
		blueprint.Invariable["dropshadow"] = CreateDropShadow(num, num);
	}

	private static void InitializeWorkerAssets(Blueprint blueprint, Dictionary<string, object> data, EntityManager mgr)
	{
		int num = TFUtils.LoadInt(data, "width");
		int num2 = TFUtils.LoadInt(data, "height");
		blueprint.Invariable["dropshadow"] = CreateDropShadow(num, num2);
	}

	public Entity Create(EntityType types, int did, bool incrementCount)
	{
		return Create(types, did, null, incrementCount);
	}

	public Entity Create(EntityType types, int did, Identity id, bool incrementCount)
	{
		string blueprintName = EntityTypeNamingHelper.GetBlueprintName(types, did);
		return Create(blueprintName, id, incrementCount);
	}

	public Entity Create(string blueprint, bool incrementCount)
	{
		return Create(blueprint, null, incrementCount);
	}

	public Entity Create(string blueprint, Identity id, bool incrementCount)
	{
		Entity entity;
		if (id != null && entities.ContainsKey(id))
		{
			entity = entities[id];
		}
		else
		{
			entity = ((id == null) ? factory.Create(blueprint) : factory.Create(blueprint, id));
			entities.Add(entity.Id, entity);
		}
		if (incrementCount)
		{
			IncrementEntityCount(blueprint);
		}
		return entity;
	}

	public static Blueprint GetBlueprint(string primaryType, int did, bool ignoreNotFoundError = false)
	{
		return GetBlueprint(EntityTypeNamingHelper.StringToType(primaryType, ignoreNotFoundError), did, ignoreNotFoundError);
	}

	public static Blueprint GetBlueprint(EntityType type, int did, bool ignoreNotFoundError = false)
	{
		string blueprintName = EntityTypeNamingHelper.GetBlueprintName(type, did, ignoreNotFoundError);
		Blueprint value = null;
		blueprints.TryGetValue(blueprintName, out value);
		return value;
	}

	public static List<string> GetAllBuildingBlueprintKeys()
	{
		List<string> list = new List<string>();
		foreach (string key in blueprints.Keys)
		{
			if (key.StartsWith("building"))
			{
				list.Add(key);
			}
		}
		return list;
	}

	public void Destroy(Identity id)
	{
		if (entities.ContainsKey(id))
		{
			Dictionary<string, int> dictionary2;
			Dictionary<string, int> dictionary = (dictionary2 = entityCount);
			string blueprintName;
			string key = (blueprintName = entities[id].BlueprintName);
			int num = dictionary2[blueprintName];
			dictionary[key] = num - 1;
			entities.Remove(id);
		}
	}

	public Entity GetEntity(Identity id)
	{
		return entities[id];
	}

	public int GetEntityCount(EntityType primaryType, int did)
	{
		string blueprintName = EntityTypeNamingHelper.GetBlueprintName(primaryType, did);
		if (!entityCount.ContainsKey(blueprintName))
		{
			return 0;
		}
		return entityCount[blueprintName];
	}

	public ICollection<Entity> GetEntities()
	{
		return entities.Values;
	}

	private void LoadBlueprintsFromFile(string filePath)
	{
		string filePathFromString = GetFilePathFromString(filePath);
		TFUtils.DebugLog("Loading blueprints: " + filePathFromString);
		string json = TFUtils.ReadAllText(filePathFromString);
		List<object> list = (List<object>)Json.Deserialize(json);
		foreach (Dictionary<string, object> item in list)
		{
			OverwriteBlueprintDataWithSpread(item);
			Blueprint blueprint = LoadBlueprintFromDict(item);
			if (blueprint != null)
			{
				blueprintsToData[blueprint] = item;
				int did = (int)blueprint.Invariable["did"];
				string blueprintName = EntityTypeNamingHelper.GetBlueprintName(blueprint.PrimaryType, did);
				blueprints[blueprintName] = blueprint;
				factory.Register((string)blueprint.Invariable["blueprint"], new EntityCtor(blueprint));
			}
		}
	}

	private Blueprint LoadBlueprintFromDict(Dictionary<string, object> dict)
	{
		string text = (string)dict["type"];
		BlueprintMarshaller value = null;
		if (!TypeRegistry.TryGetValue(text, out value))
		{
			throw new InvalidOperationException("No marshaller for type: " + text);
		}
		return value(dict, this);
	}

	private void LoadResources(Blueprint blueprint, EntityManager mgr)
	{
		if (!blueprint.Disabled)
		{
			TFUtils.DebugLog("Loading resources for " + blueprint.ToString(), TFUtils.LogFilter.Assets);
			Dictionary<string, object> dictionary = blueprintsToData[blueprint];
			string key = (string)dictionary["type"];
			InitializeBlueprintAssets(blueprint, dictionary, mgr);
			if (AssetsInitializerTypeRegistry.ContainsKey(key))
			{
				AssetsInitializerTypeRegistry[key](blueprint, dictionary, this);
			}
		}
	}

	public void LoadBlueprints()
	{
		_pBpSpreadData = new Dictionary<string, object>();
		LoadUnitsFromSpread();
		LoadAnnexesFromSpread();
		LoadCharacterBuildingsFromSpread();
		LoadDebrisFromSpread();
		LoadDecorationsFromSpread();
		LoadLandmarksFromSpread();
		LoadRentOnlyBuildingsFromSpread();
		LoadShopsFromSpread();
		LoadTreasureFromSpread();
		LoadTreesFromSpread();
		blueprintFilePaths = GetFilesToLoad();
		blueprintFileEnumerator = blueprintFilePaths.GetEnumerator();
		TFUtils.Assert(blueprintFileEnumerator != null, "Should have a full list of blueprints");
	}

	public bool IterateLoadOfBlueprints()
	{
		if (blueprintFileEnumerator.MoveNext())
		{
			string filePath = (string)blueprintFileEnumerator.Current;
			LoadBlueprintsFromFile(filePath);
			return true;
		}
		return false;
	}

	public void LoadBlueprintResources()
	{
		IEnumerator enumerator = blueprints.Values.GetEnumerator();
		while (enumerator.MoveNext())
		{
			Blueprint blueprint = (Blueprint)enumerator.Current;
			LoadResources(blueprint, this);
		}
		blueprintsToData.Clear();
	}

	private string[] GetFilesToLoad()
	{
		return Config.BLUEPRINT_DIRECTORY_PATH;
	}

	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	private void IncrementEntityCount(string blueprint)
	{
		if (entityCount.ContainsKey(blueprint))
		{
			Dictionary<string, int> dictionary2;
			Dictionary<string, int> dictionary = (dictionary2 = entityCount);
			string key2;
			string key = (key2 = blueprint);
			int num = dictionary2[key2];
			dictionary[key] = num + 1;
		}
		else
		{
			entityCount[blueprint] = 1;
		}
	}
}
