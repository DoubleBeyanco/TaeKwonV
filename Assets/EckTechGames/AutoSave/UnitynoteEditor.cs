using UnityEngine;
using UnityEditor;

public class UnitynoteEditor : EditorWindow
{
	private	static	UnitynoteEditor	window;

	private	GUIStyle	titleStyle;

	[MenuItem("Window/Unitynote/Unitynote - Custom Editor")]
	private static void Setup()
	{
		window = GetWindow<UnitynoteEditor>();
		window.titleContent = new GUIContent("Unitynote");
		window.minSize = new Vector2(300, 300);
		window.maxSize = new Vector2(1920, 1080);
	}

	private void Awake()
	{
		titleStyle					= new GUIStyle(EditorStyles.label);
		titleStyle.fontStyle		= FontStyle.Bold;
		titleStyle.normal.textColor	= Color.yellow;
	}

	private void OnGUI()
	{
		EditorGUILayout.LabelField("고박사의 유니티 노트", titleStyle);
		if ( EditorGUILayout.LinkButton("https://www.youtube.com/@unitynote") )
		{
			Application.OpenURL("https://www.youtube.com/@unitynote");
		}

		// 실행할 씬 선택 [0번 씬부터, 현재 씬부터]
		Editor_SelectPlayScene.OnEditorGUI(titleStyle);

		// 씬 자동 저장
		Editor_SceneAutoSave.OnEditorGUI(titleStyle);
	}

	private void OnInspectorUpdate()
	{
		Repaint();
	}
}

