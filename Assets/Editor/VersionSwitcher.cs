using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class VersionSwitcher : EditorWindow
{
    private static readonly string[] versionKeys = {
        "Release",
        "Halloween",
        "Christmas",
        "20Anniversary",
        "Latest",
    };

    private static readonly string[] dropdownLabels = {
        "Select Version",
        "Release",
        "Halloween",
        "Christmas",
        "20th Anniversary",
        "Latest Update",
    };

    private static readonly Dictionary<string, string> versionIDs = new Dictionary<string, string>
    {
        { "Release",       "21198" },
        { "Halloween",     "23142" },
        { "Christmas",     "23145" },
        { "20Anniversary", "23136" },
        { "Latest",        "23304" },
    };

    private static readonly Dictionary<string, string> versionLabels = new Dictionary<string, string>
    {
        { "Release",       "Release" },
        { "Halloween",     "Halloween" },
        { "Christmas",     "Christmas" },
        { "20Anniversary", "20th Anniversary" },
        { "Latest",        "Latest Update" },
    };

    private static string basePath = Application.persistentDataPath;
    private static string targetPath = Application.streamingAssetsPath;

    private string currentVersion = "None";
    private string currentID = "-";
    private int selectedIndex = 0;

    [MenuItem("Markut/SpongeBob/Tools/Version Switcher")]
    public static void ShowWindow()
    {
        GetWindow<VersionSwitcher>("Version Switcher");
    }

    void OnEnable()
    {
        LoadCurrentVersion();
    }

    void OnGUI()
    {
        GUILayout.Space(8);
        GUILayout.Label("SpongeBob Moves In", EditorStyles.boldLabel);
        GUILayout.Label("Version Switcher", EditorStyles.boldLabel);

        GUILayout.Space(10);
        DrawSeparator();
        GUILayout.Space(6);

        GUILayout.Label("Active Version", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Version:", GUILayout.Width(60));
        GUILayout.Label(currentVersion, EditorStyles.wordWrappedLabel);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("ID:", GUILayout.Width(60));
        GUILayout.Label(currentID, EditorStyles.wordWrappedLabel);
        GUILayout.EndHorizontal();

        GUILayout.Space(6);
        DrawSeparator();
        GUILayout.Space(10);

        GUILayout.Label("Select Version", EditorStyles.boldLabel);
        GUILayout.Space(4);

        selectedIndex = EditorGUILayout.Popup(selectedIndex, dropdownLabels);

        GUILayout.Space(6);

        bool canApply = selectedIndex > 0;

        GUI.enabled = canApply;
        if (GUILayout.Button("Apply", GUILayout.Height(30)))
        {
            string chosenKey = versionKeys[selectedIndex - 1];
            SwitchVersion(chosenKey);
        }
        GUI.enabled = true;

        GUILayout.Space(10);
        DrawSeparator();
        GUILayout.Space(6);

        GUI.color = new Color(1f, 0.4f, 0.4f);
        string clearLabel = currentVersion == "None" ? "Clear Contents (No Version)" : "Clear Contents";
        if (GUILayout.Button(clearLabel, GUILayout.Height(25)))
            ClearContents();
        GUI.color = Color.white;

        GUILayout.FlexibleSpace();
        DrawSeparator();
        GUILayout.Space(4);
        GUI.color = new Color(0.6f, 0.6f, 0.6f);
        GUILayout.Label("Made by Markut.", EditorStyles.miniLabel);
        GUI.color = Color.white;
        GUILayout.Space(4);
    }

    private void SwitchVersion(string version)
    {
        string src = Path.Combine(basePath, "Contents_" + version);
        string dst = targetPath;

        if (!Directory.Exists(src))
        {
            EditorUtility.DisplayDialog(
                "Not Found",
                "Folder not found:\nContents_" + version +
                "\n\nMake sure the folder exists in:\n" + basePath,
                "OK"
            );
            return;
        }

        bool confirm = EditorUtility.DisplayDialog(
            "Switch Version",
            "Switch to " + versionLabels[version] + "?\n\nThis will replace the current StreamingAssets folder.",
            "Yes", "Cancel"
        );

        if (!confirm) return;

        if (Directory.Exists(dst))
            Directory.Delete(dst, true);

        CopyDirectory(src, dst);
        SaveCurrentVersion(version);

        currentVersion = versionLabels[version];
        currentID = versionIDs[version];
        selectedIndex = 0;

        EditorUtility.DisplayDialog(
            "Done",
            "Version activated:\n" + versionLabels[version] + "\nID: " + versionIDs[version] +
            "\n\nOpen Scene0 and press Play.",
            "OK"
        );

        Repaint();
    }

    private void ClearContents()
    {
        bool confirm = EditorUtility.DisplayDialog(
            "Clear Contents",
            "This will delete the StreamingAssets folder.\nNo version will be active.",
            "Yes", "Cancel"
        );

        if (!confirm) return;

        string dst = targetPath;
        if (Directory.Exists(dst))
            Directory.Delete(dst, true);

        Directory.CreateDirectory(dst);
        SaveCurrentVersion("None");

        currentVersion = "None";
        currentID = "-";
        selectedIndex = 0;

        Repaint();
    }

    private void LoadCurrentVersion()
    {
        string configPath = Path.Combine(basePath, "active_version.txt");
        if (File.Exists(configPath))
        {
            string saved = File.ReadAllText(configPath).Trim();
            if (saved == "None" || saved == "")
            {
                currentVersion = "None";
                currentID = "-";
            }
            else if (versionIDs.ContainsKey(saved))
            {
                currentVersion = versionLabels[saved];
                currentID = versionIDs[saved];
            }
        }
        else
        {
            currentVersion = "None";
            currentID = "-";
        }
    }

    private void SaveCurrentVersion(string version)
    {
        string configPath = Path.Combine(basePath, "active_version.txt");
        File.WriteAllText(configPath, version);
    }

    private static void CopyDirectory(string src, string dst)
    {
        Directory.CreateDirectory(dst);
        foreach (string file in Directory.GetFiles(src, "*", SearchOption.AllDirectories))
        {
            string relative = file.Substring(src.Length + 1);
            string destFile = Path.Combine(dst, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(destFile));
            File.Copy(file, destFile, true);
        }
    }

    private void DrawSeparator()
    {
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
    }
}
