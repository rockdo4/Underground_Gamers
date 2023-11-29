//유니티 툴에는 자동 저장 기능이 없어서 간단하게 만든 자동저장 스크립트
//이전 작업한 씬들을 백업용으로 저장
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
   
[InitializeOnLoad]  //유니티 툴 실행시 실행
public class AutoSave : EditorWindow {
 static string autoSaveValueINIpath = "AutoSave"; //ini파일이 저장될 폴더
 
    static private bool autoSaveScene = true;
    static private bool showMessage = true;    
    static private int intervalScene = 10;    
 static private string sceneName = "backup";
 static int numScene = 10;
 
 static private string scenePath;
 static private bool isStarted = false;
    static private DateTime lastSaveTimeScene = DateTime.Now; 
 static private int lastMinute = DateTime.Now.Minute;
 static private int curMinute = 0;
    private string projectPath = Application.dataPath;
 static int curSaveSceneNum = 0; 
 
    static AutoSave() {
  EditorApplication.update += AutoSaveUpdate; //edit 업데이트에 등록하여 auto윈도우가 활성화 되지 않을 때에도 update를 시킨다.
  if (!Directory.Exists(autoSaveValueINIpath))
         Directory.CreateDirectory(autoSaveValueINIpath);
  if(File.Exists(autoSaveValueINIpath + "/autoSave.ini"))
  {
   StreamReader reader = new StreamReader(autoSaveValueINIpath + "/autoSave.ini", false);
   string szAutoSaveScene = reader.ReadLine();
   autoSaveScene = bool.Parse(szAutoSaveScene);
   
   string szShowMessage = reader.ReadLine();
   showMessage = bool.Parse(szShowMessage);
   
   string szIntervalScene = reader.ReadLine();
   intervalScene = int.Parse(szIntervalScene);
   
   sceneName = reader.ReadLine();
   string szNumScene = reader.ReadLine();
   numScene = int.Parse(szNumScene);
   reader.Close();
  }
 }
 
 ~AutoSave() {
  if (!Directory.Exists(autoSaveValueINIpath))
         Directory.CreateDirectory(autoSaveValueINIpath);
  StreamWriter writer = new StreamWriter(autoSaveValueINIpath + "/autoSave.ini", false);
  writer.WriteLine(autoSaveScene.ToString());
  writer.WriteLine(showMessage.ToString());
  writer.WriteLine(intervalScene.ToString());
  writer.WriteLine(sceneName.ToString());
  writer.WriteLine(numScene.ToString());
  writer.Close();
 }
 
    [MenuItem ("Window/AutoSave")]
    static void Init () {
        AutoSave saveWindow = (AutoSave)EditorWindow.GetWindow (typeof (AutoSave));
        saveWindow.Show();
    }
    
    void OnGUI () {   
        GUILayout.Label ("Info:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField ("Saving to:", ""+projectPath);
        EditorGUILayout.LabelField ("Saving scene:", ""+scenePath);
        GUILayout.Label ("Options:", EditorStyles.boldLabel);
        autoSaveScene = EditorGUILayout.BeginToggleGroup ("Auto save", autoSaveScene);
        intervalScene = EditorGUILayout.IntSlider ("Interval (minutes)", intervalScene, 1, 30);
  numScene =EditorGUILayout.IntSlider ("BackUp Scene", numScene, 1, 30);
  sceneName = EditorGUILayout.TextField("Temp SceneName", sceneName);
     
        if(isStarted) {
            EditorGUILayout.LabelField ("Last save:", ""+lastSaveTimeScene);
        }
        EditorGUILayout.EndToggleGroup();
        showMessage = EditorGUILayout.BeginToggleGroup ("Show Message", showMessage);
        EditorGUILayout.EndToggleGroup ();
    }
    


    static void AutoSaveUpdate(){        
  scenePath = EditorApplication.currentScene;
  if(scenePath.Length != 0)
  {
         if(autoSaveScene) 
   {
    if(lastMinute != DateTime.Now.Minute)
    {
     curMinute++;
     lastMinute = DateTime.Now.Minute;
     if(curMinute >= intervalScene)
     {
                 saveScene();
      curMinute = 0;
     }
             }
         } 
   else 
   {
            isStarted = false;
         }
  }
    }
    
    static void saveScene() {  
  int curSeneNumber = curSaveSceneNum++ % numScene;
  string[] splitScenePath = scenePath.Split('.');
  string path = autoSaveValueINIpath+ '/' + sceneName + '_' + curSeneNumber.ToString() + '.' + splitScenePath[1];
  
        EditorApplication.SaveScene(path, true);
  EditorApplication.SaveScene(scenePath);
  
        lastSaveTimeScene = DateTime.Now;
        isStarted = true;
        if(showMessage){
            Debug.Log("AutoSave saved: "+scenePath+" on "+lastSaveTimeScene);
        }
    }
} 
