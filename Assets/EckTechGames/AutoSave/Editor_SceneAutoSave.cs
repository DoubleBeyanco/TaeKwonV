using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class Editor_SceneAutoSave
{
	private	static	readonly	string		prefs = "UNITYNOTE_SCENEAUTOSAVE_";
	private	static	readonly	string[]	timeTable = { "5��", "10��", "30��", "1�ð�", "Custom" };
	private	static	readonly	double[]	timeToSeconds = { 300, 600, 1800, 3600 };
	private	static	readonly	string		timeNotation = "[yyyy-MM-dd] HH:mm:ss";

	private	static	bool		isActivated = false;
	private	static	bool		isShowLogExpanded = false;
	private	static	int			selectedTimeTableIndex = 0;

	private	static	double		saveCycle = 0;
	private	static	double		nextSaveRemainingTime = 0;
	private	static	DateTime	lastSaveTime = DateTime.Now;

	[InitializeOnLoadMethod]
	private static void Initialize()
	{
		Load();

		lastSaveTime = DateTime.Now;

		var handlers = EditorApplication.update.GetInvocationList();

		bool hasAlready = false;
		foreach ( var handler in handlers )
		{
			if ( handler.Method.Name.Equals(nameof(UpdateAutoSave)) )
			{
				hasAlready = true;
				break;
			}
		}

		if ( hasAlready == false )
		{
			EditorApplication.update += UpdateAutoSave;
		}
	}

	public static void OnEditorGUI(GUIStyle titleStyle)
	{
		EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
		EditorGUILayout.LabelField("Scene Auto Save", titleStyle);

		EditorGUI.BeginChangeCheck();

		// �ڵ����� On/Off Toggle UI
		string textActive = isActivated == true ? "ON" : "OFF";
		isActivated = EditorGUILayout.Toggle($"�ڵ� ���� {textActive}", isActivated);

		EditorGUI.BeginDisabledGroup(!isActivated);

		// ���� �ֱ� Dropdown UI
		selectedTimeTableIndex = EditorGUILayout.Popup("���� �ֱ�", selectedTimeTableIndex, timeTable);
		// ���� �ֱ⸦ "Custom"���� �������� �� (�� ������ ���� �ֱ� �Է�)
		if ( selectedTimeTableIndex == timeTable.Length - 1 )
		{
			EditorGUI.indentLevel ++;
			saveCycle = EditorGUILayout.DoubleField("�ð�(��) : ", saveCycle);
			EditorGUI.indentLevel --;

			// ���� �ֱ�� �ּ� 10�� �̻�
			if ( saveCycle < 10 )
			{
				saveCycle = 10;
			}
		}
		else
		{
			saveCycle = timeToSeconds[selectedTimeTableIndex];
		}

		EditorGUILayout.Space(EditorGUIUtility.singleLineHeight * 0.5f);
		isShowLogExpanded = EditorGUILayout.Foldout(isShowLogExpanded, "�α� ����", true);
		if ( isShowLogExpanded == true )
		{
			EditorGUI.indentLevel ++;
			EditorGUILayout.LabelField($"������ ���� �ð� : {lastSaveTime.ToString(timeNotation)}");
			EditorGUILayout.LabelField($"���� ������� ���� �ð� : {nextSaveRemainingTime:00.00}");
			EditorGUI.indentLevel --;
		}

		EditorGUI.EndDisabledGroup();

		if ( EditorGUI.EndChangeCheck() )
		{
			Save();
		}
	}

	private static void Load()
	{

		isActivated				= EditorPrefs.GetBool($"{prefs}{nameof(isActivated)}", false);
		isShowLogExpanded		= EditorPrefs.GetBool($"{prefs}{nameof(isShowLogExpanded)}", false);
		selectedTimeTableIndex	= EditorPrefs.GetInt($"{prefs}{nameof(selectedTimeTableIndex)}", 0);
		saveCycle				= EditorPrefs.GetFloat($"{prefs}{nameof(saveCycle)}", 300f);
	}

	private static void Save()
	{

		EditorPrefs.SetBool($"{prefs}{nameof(isActivated)}", isActivated);
		EditorPrefs.SetBool($"{prefs}{nameof(isShowLogExpanded)}", isShowLogExpanded);
		EditorPrefs.SetInt($"{prefs}{nameof(selectedTimeTableIndex)}", selectedTimeTableIndex);
		EditorPrefs.SetFloat($"{prefs}{nameof(saveCycle)}", (float)saveCycle);
	}

	private static void UpdateAutoSave()
	{
		DateTime dateTime = DateTime.Now;

		if ( isActivated == false || EditorApplication.isPlaying == true )
		{
			lastSaveTime			= dateTime;
			nextSaveRemainingTime	= saveCycle;

			return;
		}

		double diff = dateTime.Subtract(lastSaveTime).TotalSeconds;

		nextSaveRemainingTime = saveCycle - diff;
		if ( nextSaveRemainingTime < 0 )
		{
			nextSaveRemainingTime = 0;
		}

		// ���� �ð��� �Ǿ��� �� �� �ڵ� ����
		if ( diff > saveCycle )
		{

			EditorSceneManager.SaveOpenScenes();
			lastSaveTime = dateTime;
		}
	}
}

