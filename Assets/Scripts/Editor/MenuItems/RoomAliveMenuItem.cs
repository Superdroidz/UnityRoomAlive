using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using Assets.Parsing;

public class RoomAliveMenuItem : EditorWindow
{
    // Class for setting up the scene, as well as verifying required prefabs
    // exist for doing so.
    class SceneSetup
    {

        private static string[] managerPrefabFilters = {
            "BodySourceManager t:GameObject",
            "EnsembleManager t:GameObject",
            "ProjectorManager t:GameObject"
        };
        private static string headTrackerPrefabFilter = "HeadTracker t:prefab";

        public static void SetupScene()
        {
            foreach (string managerPrefabFilter in managerPrefabFilters)
            {
                InstantiatePrefabFromFilter(managerPrefabFilter);
            }
            if (SettingsWindow.Settings != null && SettingsWindow.Settings.IsTrackingHead)
            {
                InstantiatePrefabFromFilter(headTrackerPrefabFilter);
            }
        }

        public static bool DoPrefabsExist()
        {
            return managerPrefabFilters.All(filter => AssetDatabase.FindAssets(filter).Count() > 0) &&
                   (SettingsWindow.Settings == null || !SettingsWindow.Settings.IsTrackingHead ||
                    AssetDatabase.FindAssets(headTrackerPrefabFilter).Count() > 0);
        }
    }

    public static ParseWindow ParseWindow;
    public static SettingsWindow SettingsWindow;

    static ITKWrapper wrapper = new RoomAliveWrapper();
    static string currentXMLFilePath;

    static bool fileSetupComplete;
    static bool calibrationComplete;
    static bool fileLoaded;

    [MenuItem("RoomAlive/Start Kinect Server", false, 1)]
    private static void RunKinectServer()
    {
        wrapper.StartCameraServer();
    }

    [MenuItem("RoomAlive/Start Projector Server", false, 2)]
    private static void RunProjectorServer()
    {
        wrapper.StartProjectorServer();
    }

    [MenuItem("RoomAlive/Stop Servers", false, 3)]
    private static void StopServers()
    {
        wrapper.StopServers();
    }

    /* Prevents the user from stopping servers if they
    *  are not running.
    */
    [MenuItem("RoomAlive/Stop Servers", true)]
    private static bool ValidateStopServers()
    {
        return wrapper.ServersAreRunning();
    }

    [MenuItem("RoomAlive/Create New Setup", false, 51)]
    private static void CreateSetup()
    {
        fileSetupComplete = false;
        calibrationComplete = false;
        currentXMLFilePath = EditorUtility.SaveFilePanel("Save Setup File", "", "cal", "xml");
        if (currentXMLFilePath == null || currentXMLFilePath.Equals("")) return; // must check for empty string here, as file will not exist

        wrapper.CreateNewSetup(currentXMLFilePath);
        fileSetupComplete = true;
    }

    [MenuItem("RoomAlive/Edit Setup", false, 52)]
    private static void ParseXML()
    {
        DisplayParseWindow();
    }

    /* Prevents the user from editing a XML calibration file
    *  before it has been created or loaded.
    */
    [MenuItem("RoomAlive/Edit Setup", true)]
    private static bool ValidateEditSetup()
    {
        return fileSetupComplete;
    }

    [MenuItem("RoomAlive/Load Existing Setup", false, 53)]
    private static void LoadXML()
    {
        currentXMLFilePath = EditorUtility.OpenFilePanel("Load Existing Setup", "", "xml");
        if (!File.Exists(currentXMLFilePath)) return;
        fileSetupComplete = true;
        fileLoaded = true;
        DisplayParseWindow();

    }

    [MenuItem("RoomAlive/Run Calibration", false, 101)]
    private static void Calibrate()
    {
        calibrationComplete = false;
        fileSetupComplete = false;
        wrapper.RunCalibration(currentXMLFilePath);
        fileSetupComplete = true;
        calibrationComplete = true;
    }

    /* Prevents the user from running a calibration before a setup file
    *  has been created or loaded
    */
    [MenuItem("RoomAlive/Run Calibration", true)]
    private static bool CalibrationValidation()
    {
        return fileSetupComplete;
    }

    [MenuItem("RoomAlive/Import Room", false, 102)]
    private static void ImportRoom()
    {
        string objectPath;
        if (File.Exists(currentXMLFilePath))
        {
            var objectName = Path.GetFileNameWithoutExtension(currentXMLFilePath);
            var objectDirectory = Path.GetDirectoryName(currentXMLFilePath);
            objectPath = Path.Combine(objectDirectory, objectName + ".obj");
            if (File.Exists(objectPath))
            {
                ImportAssetFromPath(objectPath);
                return;
            }
        }

        objectPath = EditorUtility.OpenFilePanel("Import scene object file", "", "obj");
        if (File.Exists(objectPath))
        {
            ImportAssetFromPath(objectPath);
            return;
        }
    }

    [MenuItem("RoomAlive/Create Prefabs", false, 103)]
    private static void CreatePrefabs()
    {
        if (File.Exists(currentXMLFilePath)) {
            string ensembleManagerFilter = "EnsembleManager t:GameObject";
            GameObject managerInstance = InstantiatePrefabFromFilter(ensembleManagerFilter);
            EnsembleManager manager = managerInstance.GetComponent<EnsembleManager>();
            manager.data = new EnsembleData(currentXMLFilePath);

            var guids = AssetDatabase.FindAssets(ensembleManagerFilter);
            string ensembleManagerPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            GameObject managerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(ensembleManagerPath);
            PrefabUtility.ReplacePrefab(manager.gameObject, managerPrefab);
            DestroyImmediate(manager.gameObject);
        }
    }

    [MenuItem("RoomAlive/Create Prefabs", true)]
    static bool ValidateCreatePrefabs()
    {
        return File.Exists(currentXMLFilePath);
    }

    [MenuItem("RoomAlive/Instantiate Prefabs", false, 104)]
    static void InstantiatePrefabs()
    {
        SceneSetup.SetupScene();
    }

    [MenuItem("RoomAlive/Instantiate Prefabs", true)]
    static bool ValidateInstantiatePrefabs()
    {
        return SceneSetup.DoPrefabsExist();
    }

    static void ImportAssetFromPath(string path)
    {
        var name = Path.GetFileNameWithoutExtension(path);
        var ext = Path.GetExtension(path);
        var newPath = @"Assets/" + Path.GetFileName(path);
        var index = 0;
        while (File.Exists(newPath))
        {
            newPath = @"Assets/" + name + index.ToString() + ext;
        }
        name = name + index.ToString();
        try
        {
            File.Copy(path, newPath);
            AssetDatabase.ImportAsset(newPath);
            var scene = AssetDatabase.LoadAssetAtPath<GameObject>(newPath);
            Instantiate(scene);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Could not import object from path " + path + ";\n" + e.Message);
        }
    }

    /*  Prevents  a user from importing a room if a XML calibration file has not been
    *   created or loaded.
    */
    [MenuItem("RoomAlive/Import Room", true)]
    private static bool ImportRoomValidation()
    {
        return calibrationComplete || fileLoaded;
    }

    [MenuItem("RoomAlive/Settings", false, 153)]
    private static void OpenSettings()
    {
        if (SettingsWindow == null)
        {
            SettingsWindow = (SettingsWindow)CreateInstance("SettingsWindow");
        }
        SettingsWindow.ShowWindow();
    }

    private static void DisplayParseWindow()
    {
        if (ParseWindow == null)
        {
            ParseWindow = (ParseWindow)CreateInstance("ParseWindow");
        }
        try
        {
            ParseWindow.setFilePath(currentXMLFilePath);
            ParseWindow.LoadFile();
            ParseWindow.ParseFile();
            ParseWindow.ShowWindow();
        }
        catch (FileNotFoundException)
        {
            Debug.LogError("No XML file found at filepath " + currentXMLFilePath);
        }
    }

    private static GameObject InstantiatePrefabFromFilter(string prefabFilter)
    {
        GameObject instance = null;
        string[] guids = AssetDatabase.FindAssets(prefabFilter);
        if (guids.Count() > 0) {
            var path = AssetDatabase.GUIDToAssetPath(guids[0]); // Could put a check to make sure only one prefab exists.
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        }
        return instance;
    }
}
