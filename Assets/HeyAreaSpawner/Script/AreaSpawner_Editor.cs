//Developed by Halil Emre Yildiz - @Jahn_Star
//https://github.com/JahnStar/Hey-Area-Object-Spawner
//https://jahnstar.github.io/donate/
// Last Update: 30.10.2021
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using JahnStar.AreaSpawner;
using System.Linq;

[CustomEditor(typeof(AreaSpawner))]
public class AreaSpawner_Editor : Editor
{
    private AreaSpawner _target;
    private int _tab, _language;
    private string[,] _texts = 
    {{"Hey Area Object Spawner v1.2", "Editor", "Settings", "Exit Edit Mode", "Edit Mode", "Selected Area: ", "Spawn Objects (in selected area)", "Copy List", "Paste List", "Clear Clipboard", "Spawn", "Created Objects (HeySpawner)", 
        "Create Object", "Move Area", "Delete Area", "\n    Edit Mode Shortcuts: \n\n    Create Point: SHIFT+CLICK \n    Create Area: CTRL+SHIFT+CLICK \n    Delete Selected Area: BACKSPACE \n    Choose Area: Edit Mode => Click Area Line \n\n    If this doesn't work, you can try Reset All Layout. \n\n    Developed by Halil Emre Yildiz - @JahnStar \n",
        "Documentation & Licence", "Support the Developer"}, 
    {"Hey Area Object Spawner v1.2", "Duzenleyici", "Ayarlar", "Duzenlemeden Cik", "Duzenleme Modu", "Secili Alan: ", "Nesneler Olustur (secili alanda)", "Listeyi Kopyala", "Listeyi Yapistir", "Panoyu Temizle", "Olustur", "Olusturulan Nesneler (HeySpawner)", 
        "Nesne Olustur", "Alani Tasi", "Alani Sil", "\n    Duzenleme Modu Kisayollari: \n\n    Nokta Yarat: SHIFT+CLICK \n    Alan Yarat: CTRL+SHIFT+CLICK \n    Secili Alani Sil: BACKSPACE \n    Alani Sec: Duzenleme Modu => Alan Cizgisine Tikla \n\n    Calismiyorsa, 'Reset All Layout'a tiklayin. \n\n    Gelistiren, Halil Emre Yildiz - @JahnStar \n",
        "Dokumantasyon & Lisans", "Gelistiriciyi Destekle"}};
    public override void OnInspectorGUI()
    {
        try { _target = (AreaSpawner)target; }
        catch { }
        Color cyanBlue = (Color.cyan + Color.blue) * Color.white * 0.9f;
        Color gray = Color.white * 0.4f;
        GUIStyle title = GUIStyle.none;
        title.alignment = TextAnchor.MiddleCenter;
        title.fontSize = 14;
        title.fontStyle = FontStyle.Bold;
        title.normal.textColor = Color.white;
        GUILayout.Space(5);
        GUILayout.Label(_texts[_language, 0], title);
        GUILayout.Space(8);
        GUILayout.BeginHorizontal();
        GUI.backgroundColor = _language == 0 ? cyanBlue : Color.red + Color.gray;
        _tab = GUILayout.Toolbar(_tab, new string[] { _texts[_language, 1], _texts[_language, 2] });
        _language = GUILayout.Toolbar(_language, new string[] { "ENG", "TR" }, GUILayout.Width(75));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        switch (_tab)
        {
            case 0:
                {
                    GUILayout.Space(5);
                    GUI.backgroundColor = _target.editMode ? Color.red: cyanBlue;
                    if (GUILayout.Button(_target.editMode ? _texts[_language, 3] : _texts[_language, 4]))
                    {
                        _target.editMode = !_target.editMode;
                        Tools.current = _target.editMode ? Tool.None : Tool.Move;
                    }

                    GUI.backgroundColor = Color.green;
                    GUILayout.Space(5);

                    if (_target.transform.childCount > 0 && _target.selectedAreaID < _target.transform.childCount)
                    {
                        HeyArea selectedArea = _target.transform.GetChild(_target.selectedAreaID).GetComponent<HeyArea>();
                        GUILayout.Space(5);
                        GUIStyle foldoutLabel = EditorStyles.foldout;
                        foldoutLabel.fontSize = 13;
                        foldoutLabel.fontStyle = FontStyle.Bold;
                        _target.show_AreaObjects = EditorGUILayout.Foldout(_target.show_AreaObjects, _texts[_language, 6], true, foldoutLabel);
                        GUILayout.Space(10);
                        if (_target.show_AreaObjects)
                        {
                            List<SpawnObjectProperty> properties = selectedArea.objects ?? new List<SpawnObjectProperty>();

                            int sil = -1;
                            for (int i = 0; i < properties.Count; i++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                properties[i] = (SpawnObjectProperty)EditorGUILayout.ObjectField(properties[i], typeof(SpawnObjectProperty), true);
                                GUI.backgroundColor = Color.red + gray;
                                if (GUILayout.Button("-", GUILayout.Width(30))) sil = i;
                                GUI.backgroundColor = Color.green + gray;
                                EditorGUILayout.EndHorizontal();
                            }
                            if (sil != -1) properties.RemoveAt(sil);
                            GUILayout.BeginHorizontal();
                            try
                            {
                                if (selectedArea.objects.Count > 0 && GUILayout.Button(_texts[_language, 7]))
                                    _target.clipboard = properties.ToList();

                                if (_target.clipboard.Count != 0 && GUILayout.Button(_texts[_language, 8] + " (" + _target.clipboard.Count + ")"))
                                    properties = _target.clipboard.ToList();

                                if (_target.clipboard.Count != 0 && GUILayout.Button(_texts[_language, 9]))
                                    _target.clipboard.Clear();

                            }
                            catch { }
                            if (GUILayout.Button("  +  ")) properties.Add(null);
                            GUILayout.EndHorizontal();
                            selectedArea.objects = properties;

                            GUILayout.Space(10);

                            Rect rect = EditorGUILayout.GetControlRect(false, 12f);
                            rect.height = 2f;
                            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
                        }
                        else
                        {
                            GUILayout.Label(_texts[_language, 5] + _target.selectedAreaID, title);
                            GUILayout.Space(10);
                        }
                        GUI.backgroundColor = cyanBlue + gray;

                        if (GUILayout.Button(_texts[_language, 10]))
                        {
                            if (_target.parentSpawnedObject == null) _target.parentSpawnedObject = new GameObject(_texts[_language, 11]).transform;
                            for (int i = 0; i < selectedArea.objects.Count; i++)
                            {
                                SpawnObjectProperty prefab = selectedArea.objects[i];
                                if (!prefab) break;
                                Transform objects = Creator.SpawnObjectInArea(prefab.gameObject, prefab.randomScale, prefab.randomRotation, prefab.count, selectedArea.transform, _target.parentSpawnedObject, prefab.type);
                                Selection.activeObject = objects;
                                Undo.RegisterCreatedObjectUndo(objects.gameObject, _texts[_language, 12]);
                            }
                        }

                        GUI.backgroundColor = Color.yellow + Color.gray;

                        if (GUILayout.Button(_texts[_language, 13]))
                        {
                            List<GameObject> noktalar = new List<GameObject>();
                            foreach (Transform nokta in selectedArea.transform) noktalar.Add(nokta.gameObject);

                            Selection.objects = noktalar.ToArray();
                        }
                        GUI.backgroundColor = Color.red + gray;
                        if (GUILayout.Button(_texts[_language, 14]))
                        {
                            selectedArea.gameObject.GetComponent<HeyArea>().DestroySelf();
                            _target.ChooseArea(_target.selectedAreaID - 1);
                        }

                        GUILayout.Space(5);
                    }
                    else if (_target.transform.childCount > 0) _target.ChooseArea(_target.transform.childCount - 1);
                    GUI.backgroundColor = cyanBlue;
                    try
                    {
                        EditorGUILayout.HelpBox(_texts[_language, 15], MessageType.Info);
                        if (GUILayout.Button(_texts[_language, 16])) Application.OpenURL("https://github.com/JahnStar/Hey-Area-Object-Spawner");
                        if (GUILayout.Button(_texts[_language, 17])) Application.OpenURL("https://jahnstar.github.io/donate/");

                    }
                    catch { }
                }
                break;
            case 1: GUILayout.Space(8); base.OnInspectorGUI(); break;
        }
    }
    bool ctrl, ctrl_ok;

    private Vector3 prev_point;
    public void OnSceneGUI()
    {
        try { _target = (AreaSpawner)target; }
        catch { }
        Event input = Event.current;

        if (_target.editMode)
        {
            if (ctrl != input.control)
            {
                if (input.control) ctrl_ok = true;
                ctrl = input.control;

            }
            if (input.shift && input.type == EventType.MouseDown)
            {
                Ray worldRay = HandleUtility.GUIPointToWorldRay(input.mousePosition);
                if (Physics.Raycast(worldRay, out RaycastHit hitInfo, 10000)) if (prev_point != hitInfo.point)
                    {
                        _target.NewPoint(hitInfo.point, ctrl_ok && input.control);
                        ctrl_ok = false;
                        prev_point = hitInfo.point;
                    }
            }
            if (input.shift) Selection.activeGameObject = _target.gameObject;
        }

        if (input.keyCode == KeyCode.Backspace && input.type == EventType.KeyUp && _target.selectedAreaID < _target.transform.childCount)
            _target.transform.GetChild(_target.selectedAreaID).GetComponent<HeyArea>().DestroySelf();
    }
}
#endif
