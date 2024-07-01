using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class Editor_SelectPlayScene
{
	private	static	readonly	string		prefs = "UNITYNOTE_SELECTPLAYSCENE_";
	private	static	readonly	string[]	playSceneTable = { "0�� ������ ���", "���� ������ ���" };

	private	static	int			selectedIndex = 0;

	[InitializeOnLoadMethod]
	private static void Initialize()
	{
		Load();
	}

	public static void OnEditorGUI(GUIStyle titleStyle)
	{
		EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
		EditorGUILayout.LabelField("Select Play Scene", titleStyle);

		EditorGUI.BeginChangeCheck();
		selectedIndex = EditorGUILayout.Popup("��� �� ����", selectedIndex, playSceneTable);

		if ( EditorGUI.EndChangeCheck() )
		{
			Debug.Log("Debug Check.. ��� �� ���� ��ȣ�ۿ� [Ȯ�� �� ����]");

			Save();

			if ( selectedIndex == 0 )
			{
				StartFromFirstScene();
			}
			else if ( selectedIndex == 1 )
			{
				StartFromCurrentScene();
			}
		}
	}

	private static void Load()
	{
		selectedIndex = EditorPrefs.GetInt($"{prefs}{nameof(selectedIndex)}");
	}

	private static void Save()
	{
		EditorPrefs.SetInt($"{prefs}{nameof(selectedIndex)}", selectedIndex);
	}

	private static void StartFromFirstScene()
	{
		var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
		var sceneAsset		 = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);

		EditorSceneManager.playModeStartScene = sceneAsset;
	}

	private static void StartFromCurrentScene()
	{
		EditorSceneManager.playModeStartScene = null;
	}
}

