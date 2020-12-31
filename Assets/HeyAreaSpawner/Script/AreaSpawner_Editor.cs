//Developed by Halil Emre Yildiz - @Jahn_Star
//https://www.patreon.com/jahnstar
// Last Edit: 31.12.2020
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using JahnStar.AreaSpawner;
using System.Linq;

[CustomEditor(typeof(AreaSpawner))]
public class AreaSpawner_Editor : Editor
{
    AreaSpawner bu;
    int sekme;

    public override void OnInspectorGUI()
    {
        bu = (AreaSpawner)target;

        GUIStyle baslikYaziTipi = GUIStyle.none;
        baslikYaziTipi.alignment = TextAnchor.MiddleCenter;
        baslikYaziTipi.fontSize = 15;
        baslikYaziTipi.fontStyle = FontStyle.Bold;
        baslikYaziTipi.normal.textColor = Color.white;
        GUILayout.Label("Hey Custom Area\nObject Spawner v1", baslikYaziTipi);
        GUILayout.Space(5);
        sekme = GUILayout.Toolbar(sekme, new string[] { "Duzenleyici\n-Editor-", "Ayarlar\n-Settings-" });
        switch (sekme)
        {
            case 0:
                {
                    GUILayout.Space(5);
                    GUI.backgroundColor = Color.yellow;
                    EditorGUILayout.HelpBox("\n   Developed by Halil Emre Yildiz - @Jahn_Star \n", MessageType.None, true);
                    GUILayout.Space(5);
                    GUI.backgroundColor = bu.duzenlemeModu ? Color.red : Color.cyan;
                    if (GUILayout.Button(bu.duzenlemeModu ? "Duzenlemeden Cik\n-Exit Edit-" : "Duzenleme Modu\n-Edit Mode-"))
                    {
                        bu.duzenlemeModu = !bu.duzenlemeModu;
                        Tools.current = bu.duzenlemeModu ? Tool.None : Tool.Move;
                    }

                    GUI.backgroundColor = Color.green;
                    GUILayout.Space(5);
                    if (bu.transform.childCount > 0 && bu.seciliAlan_id < bu.transform.childCount)
                    {
                        GUILayout.Label("Secili Alan - Selected Area: " + bu.seciliAlan_id, baslikYaziTipi);
                        HeyArea seciliAlan = bu.transform.GetChild(bu.seciliAlan_id).GetComponent<HeyArea>();

                        bu.alanOzellikleri = EditorGUILayout.Foldout(bu.alanOzellikleri, "Nesneler -Objects-");
                        if (bu.alanOzellikleri)
                        {
                            List<SpawnObjectProperty> ozellikler = seciliAlan.nesneler != null ? seciliAlan.nesneler : new List<SpawnObjectProperty>();

                            try
                            {
                                if (seciliAlan.nesneler.Count > 0 && GUILayout.Button("Kopyala - Copy"))
                                    bu.kopyalananOzellikler = ozellikler.ToList();

                                if (bu.kopyalananOzellikler.Count != 0 && GUILayout.Button("Yapistir - Paste (" + bu.kopyalananOzellikler.Count + ")"))
                                    ozellikler = bu.kopyalananOzellikler.ToList();

                                if (bu.kopyalananOzellikler.Count != 0 && GUILayout.Button("Temizle - Clear Clipboard"))
                                    bu.kopyalananOzellikler.Clear();

                                Rect rect2 = EditorGUILayout.GetControlRect(false, 12f);
                                rect2.height = 2f;
                                EditorGUI.DrawRect(rect2, new Color(0.5f, 0.5f, 0.5f, 1));
                            }
                            catch { }

                            int sil = -1;
                            for (int i = 0; i < ozellikler.Count; i++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                ozellikler[i] = (SpawnObjectProperty)EditorGUILayout.ObjectField(ozellikler[i], typeof(SpawnObjectProperty), true);
                                GUI.backgroundColor = Color.red;
                                if (GUILayout.Button("-")) sil = i;
                                GUI.backgroundColor = Color.green;
                                EditorGUILayout.EndHorizontal();
                            }
                            if (sil != -1) ozellikler.RemoveAt(sil);
                            if (GUILayout.Button("+")) ozellikler.Add(null);
                            seciliAlan.nesneler = ozellikler;

                            GUILayout.Space(10);
                            Rect rect = EditorGUILayout.GetControlRect(false, 12f);
                            rect.height = 2f;
                            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
                            GUI.backgroundColor = Color.cyan;
                            if (GUILayout.Button("Nesneleri Olustur\n-Spawn Objects-"))
                            {
                                if (bu.parentSpawnedObject == null) bu.parentSpawnedObject = new GameObject("Created (HeySpawner)").transform;
                                for (int i = 0; i < seciliAlan.nesneler.Count; i++)
                                {
                                    SpawnObjectProperty nesne = seciliAlan.nesneler[i];
                                    Transform objects = Creator.SpawnObjectInArea(nesne.nesne, nesne.rastgeleBoyut, nesne.rastgeleRotasyon, nesne.miktar, seciliAlan.transform, bu.parentSpawnedObject, nesne.tip);
                                    Selection.activeObject = objects;
                                    Undo.RegisterCreatedObjectUndo(objects.gameObject, "Create Object");
                                }
                            }
                            GUI.backgroundColor = Color.yellow;
                        }
                        if (GUILayout.Button("Alani Tasi\n-Move Area-"))
                        {
                            List<GameObject> noktalar = new List<GameObject>();
                            foreach (Transform nokta in seciliAlan.transform) noktalar.Add(nokta.gameObject);

                            Selection.objects = noktalar.ToArray();
                        }
                        GUI.backgroundColor = Color.red;
                        if (GUILayout.Button("Alani Sil\n-Delete Area-"))
                        {
                            seciliAlan.gameObject.GetComponent<HeyArea>().KendiniImhaEt();
                            bu.AlanSec(bu.seciliAlan_id - 1);
                        }

                        GUILayout.Space(5);
                    }
                    else if (bu.transform.childCount > 0) bu.AlanSec(bu.transform.childCount - 1);
                    GUI.backgroundColor = Color.cyan;
                    EditorGUILayout.HelpBox("\n    TR:\n    Nokta Yarat: SHIFT+CLICK \n    Alan Yarat: CTRL+SHIFT+CLICK \n    Secili Alani Sil: BACKSPACE \n" +
                        "\n    EN:\n    Create Point: SHIFT+CLICK \n    Create Area: CTRL+SHIFT+CLICK \n    Delete Selected Area: BACKSPACE \n\n    If this doesn't work, you can try Reset All Layout.\n", MessageType.Info);

                    if (GUILayout.Button("Click for tutorials")) Application.OpenURL("www.youtube.com/channel/UCNF_CwE6FxEQMUdhI7tGyDw");
                    if (GUILayout.Button("Patreon")) Application.OpenURL("https://patreon.com/jahnstar");
                } break;
            case 1: base.OnInspectorGUI(); break;
        }
    }
    bool ctrl, ctrl_ok;

    public void OnSceneGUI()
    {
        bu = (AreaSpawner)target;
        Event girdi = Event.current;

        if (bu.duzenlemeModu)
        {
            if (ctrl != girdi.control)
            {
                if (girdi.control) ctrl_ok = true;
                ctrl = girdi.control;

            }
            if (girdi.shift && girdi.type == EventType.MouseDown)
            {
                Ray worldRay = HandleUtility.GUIPointToWorldRay(girdi.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(worldRay, out hitInfo, 10000))
                {
                    bu.YeniNokta(hitInfo.point, ctrl_ok && girdi.control);
                    ctrl_ok = false;
                }
            }
            if (girdi.shift) Selection.activeGameObject = bu.gameObject;
        }

        if (girdi.keyCode == KeyCode.Backspace && girdi.type == EventType.KeyUp && bu.seciliAlan_id < bu.transform.childCount)
            bu.transform.GetChild(bu.seciliAlan_id).GetComponent<HeyArea>().KendiniImhaEt();
    }
}
#endif