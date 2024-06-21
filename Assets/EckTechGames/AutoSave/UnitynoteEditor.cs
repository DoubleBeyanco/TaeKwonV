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
		EditorGUILayout.LabelField("��ڻ��� ����Ƽ ��Ʈ", titleStyle);
		if ( EditorGUILayout.LinkButton("https://www.youtube.com/@unitynote") )
		{
			Application.OpenURL("https://www.youtube.com/@unitynote");
		}

		// ������ �� ���� [0�� ������, ���� ������]
		Editor_SelectPlayScene.OnEditorGUI(titleStyle);

		// �� �ڵ� ����
		Editor_SceneAutoSave.OnEditorGUI(titleStyle);
	}

	private void OnInspectorUpdate()
	{
		Repaint();
	}
}

