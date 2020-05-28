using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Text.RegularExpressions;
using System;
using Tapdaq;


using TDEditor.iOS.Xcode;


public class TapdaqBuildPostprocessor : MonoBehaviour{
	private static string FrameworksPath = "Pods/";
	private static string FrameworksDir = "iOS/";
	private const string BuildPathKey = "IOSBuildProjectPath";
	[MenuItem ("Tapdaq/Run iOS Build Postprocess", false, 2222)]
	private static void RunIOSPostProcessManually()	{
		var path = EditorPrefs.GetString (BuildPathKey, null);
		OnPostprocessBuild (BuildTarget.iOS, path);
	}

	[MenuItem ("Tapdaq/Run iOS Build Postprocess", true)]
	static bool validateRunPostBuilder(){
		Debug.Log("validateRunPost");
		var path = EditorPrefs.GetString (BuildPathKey, null);
		if( path == null || !Directory.Exists( path ) )
			return false;

		var projectFile = Path.Combine( path, "Unity-iPhone.xcodeproj/project.pbxproj" );
		if( !File.Exists( projectFile ) )
			return false;

		return true;
	}

	private static void processExistingiOSPaths (string targetPath)  {
		Debug.Log("processExtisting");
        if (!Directory.Exists(targetPath + "/" + FrameworksPath)) {
            FrameworksPath = "Frameworks/";
            FrameworksDir = "";
        }

		foreach (string dirPath in Directory.GetDirectories(targetPath + "/" + FrameworksPath)) {
			var dirName = Path.GetFileName(dirPath);
			if (String.Compare(dirName, "ios", true) == 0) {
				FrameworksDir = dirName + "/";
			}
		}
	}


	[PostProcessBuild(101)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject) {
		if (buildTarget != BuildTarget.iOS) return;
        TDDebugLogger.Log("OnPostprocessBuild - Path: " + pathToBuiltProject);
        
		EditorPrefs.SetString (BuildPathKey, pathToBuiltProject);

        FrameworksPath = (TDSettings.getInstance().useCocoapodsMaven ? "Pods/" : "Frameworks/Plugins/");
        TDDebugLogger.Log("FrameworksPath: " + FrameworksPath);

        var path = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        if (!File.Exists(path))
        {
            TDDebugLogger.LogError(string.Format("pbxproj '{0}' does not exists", path));
            return;
        }

        var proj = new PBXProject();
        proj.ReadFromString(File.ReadAllText(path));
        var target = proj.TargetGuidByName("Unity-iPhone");

        processExistingiOSPaths(pathToBuiltProject);
        SetBuildProperties(proj, target);

        AddLibraries(proj, target, pathToBuiltProject);

        SetPListProperties(pathToBuiltProject);

        File.WriteAllText(path, proj.WriteToString());
    }

    private static void SetBuildProperties(PBXProject proj, string target) {
		proj.SetBuildProperty(target, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");
		proj.SetBuildProperty(target, "IPHONEOS_DEPLOYMENT_TARGET", GetIOSDeploymentTarget(proj));
        proj.SetBuildProperty(target, "CLANG_ENABLE_MODULES", "YES");
        proj.UpdateBuildProperty(target, "OTHER_LDFLAGS", new string [] { "-ObjC" }, new string [] {});



        proj.AddFrameworkToProject(target, "MessageUI.framework", false);
        proj.AddFrameworkToProject(target, "AdSupport.framework", false);
        proj.AddFrameworkToProject(target, "CoreData.framework", false);
        proj.AddFrameworkToProject(target, "SystemConfiguration.framework", false);
        proj.AddFrameworkToProject(target, "EventKit.framework", false);
        proj.AddFrameworkToProject(target, "EventKitUI.framework", false);
        proj.AddFrameworkToProject(target, "WatchConnectivity.framework", false);
        proj.AddFrameworkToProject(target, "MobileCoreServices.framework", false);
        proj.AddFrameworkToProject(target, "Social.framework", false);
        proj.AddFrameworkToProject(target, "JavaScriptCore.framework", false);
        proj.AddFrameworkToProject(target, "libz.dylib", false);
        proj.AddFrameworkToProject(target, "libsqlite3.tbd", false);
        proj.AddFrameworkToProject(target, "libc++.tbd", false);
        proj.AddFrameworkToProject(target, "libxml2.tbd", false);
        proj.AddFrameworkToProject(target, "libresolv.9.tbd", false);

		var path = EditorPrefs.GetString (BuildPathKey, null);

        string dir = path + "/Pods/";
        if (Directory.Exists(dir) && Directory.GetFiles(dir, "YouAppiAdapter.framework", SearchOption.AllDirectories) != null
            || AssetDatabase.FindAssets("YouAppiAdapter.framework").Length > 0)
        {
            Debug.Log("SET YOUAPPI PROPS");
            proj.SetBuildProperty(target, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
            proj.SetBuildProperty(target, "DEFINES_MODULE", "YES");
            proj.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
        }
    }

    private static void AddLibraries(PBXProject proj, string target, string projectPath) {
        bool shouldUseNewFolderStructure = !Directory.Exists(projectPath + "/" + FrameworksPath + FrameworksDir + "Tapdaq");
		Debug.Log("Use new folder structure: " + shouldUseNewFolderStructure);
        
 		foreach(TDNetwork network in TDNetwork.Networks) {
			if (network.name.Equals("YouAppiAdapter", StringComparison.CurrentCultureIgnoreCase)) {
                if (TDSettings.getInstance().useCocoapodsMaven)
                {
                    proj.EmbedFramework(target, projectPath + "/Pods/YouAppiMoatSDK/YouAppiMoat.framework");
                }
                else
                {
                    proj.EmbedFramework(target, FrameworksPath + FrameworksDir + (shouldUseNewFolderStructure ? "" : "Tapdaq/NetworkSDKs/YouAppiAdapter/") + "YouAppiMoat.framework");
                }
            }
		}  

        if (AssetDatabase.FindAssets("Tapjoy.framework").Length > 0)
        {
            if (!proj.ContainsFileByProjectPath("TapjoyResources.bundle"))
            {
                var fullPath = FrameworksPath + FrameworksDir
                    + (shouldUseNewFolderStructure ? "" : "Tapdaq/NetworkSDKs/TapjoyAdapter/") + "Tapjoy.framework/Resources/TapjoyResources.bundle";
                if (!Directory.Exists(fullPath))
                {
                    Debug.Log("TapjoyResources.bundle does not exist");
                }
                proj.AddFileToBuild(target, proj.AddFile(fullPath, "TapjoyResources.bundle", PBXSourceTree.Source));
            }
        }

        if (AssetDatabase.FindAssets("PlayableAds.framework").Length > 0)
        {
            if (!proj.ContainsFileByProjectPath("ZplayMuteListener.bundle"))
            {
                var fullPath = FrameworksPath + FrameworksDir + (shouldUseNewFolderStructure ? "" : "Tapdaq/NetworkSDKs/ZPlayAdapter/") + "PlayableAds.framework/Resources/ZplayMuteListener.bundle";
                proj.AddFileToBuild(target, proj.AddFile(fullPath, "ZplayMuteListener.bundle", PBXSourceTree.Source));
            }
        }
    }

	private static void SetPListProperties(string pathToBuiltProject) {
		
		var plistPath = pathToBuiltProject + "/Info.plist";
		var plist = new PlistDocument();

		plist.ReadFromString(File.ReadAllText(plistPath));
		var rootDict = plist.root;

		if(AssetDatabase.FindAssets("AdColonyAdapter.framework").Length > 0) {
			rootDict.SetString("NSMotionUsageDescription", "Some ad content may require access to accelerometer for interactive ad experience.");
			rootDict.SetString("NSPhotoLibraryUsageDescription", "Some ad content may require access to the photo library.");
			rootDict.SetString("NSCalendarsUsageDescription", "Some ad content may create a calendar event.");
			rootDict.SetString("NSCameraUsageDescription", "Some ad content may access camera to take picture.");
		}

		var transportSecurityKey = "NSAppTransportSecurity";

		if (rootDict [transportSecurityKey] == null) {
			rootDict.CreateDict (transportSecurityKey);
		}

		var appTransportSecurity = rootDict [transportSecurityKey].AsDict ();
		
		appTransportSecurity.SetBoolean ("NSAllowsArbitraryLoads", true);

		if(TDSettings.getInstance().admob_appid_ios.Length > 0) {
            rootDict.SetString("GADApplicationIdentifier", TDSettings.getInstance().admob_appid_ios);
		}

		// Write to file
		File.WriteAllText(plistPath, plist.WriteToString());
	}

	private static string GetIOSDeploymentTarget(PBXProject proj) {
		var target = proj.TargetGuidByName("Unity-iPhone");
		var deploymentTargets = proj.GetBuildProperties (target, "IPHONEOS_DEPLOYMENT_TARGET");

		var deploymentTarget = "0";
		if (deploymentTargets.Count > 0) {
			deploymentTarget = deploymentTargets [0];
		}

		if (string.IsNullOrEmpty (deploymentTarget))
			deploymentTarget = "0";

		Regex rgx = new Regex("[^0-9.]");
		var numberOnly = rgx.Replace(deploymentTarget, "");

		var version = Tapdaq.TDExtensionMethods.ParseFloat (numberOnly, 0);

		if (version >= 8.0f)
			return deploymentTarget;

		TDDebugLogger.LogWarning ("TapdaqBuildPostprocessor changes iOS build target version from " + deploymentTarget + " to = 8.0");

		return "8.0";
	}
}
