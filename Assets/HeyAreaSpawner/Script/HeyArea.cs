//Developed by Halil Emre Yildiz - @Jahn_Star
//https://www.patreon.com/jahnstar
// Last Edit: 18.10.2020
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace JahnStar.AreaSpawner
{
    [ExecuteInEditMode]
    public class HeyArea : MonoBehaviour
    {
        [HideInInspector]
        public LineRenderer cizgiOlusturucu;
        [HideInInspector]
        public int noktaMiktari;
        float noktaYuksekligi;
        private AreaSpawner anaOlusturucu;
        [Header("Olusturulacak Nesneler Ozellikleri")]
        public List<SpawnObjectProperty> nesneler;
        private void Update()
        {
            if (!anaOlusturucu) anaOlusturucu = transform.root.GetComponent<AreaSpawner>();
            if (!cizgiOlusturucu) 
            {
                if (GetComponent<LineRenderer>()) cizgiOlusturucu = GetComponent<LineRenderer>();
                else
                {
                    cizgiOlusturucu = gameObject.AddComponent<LineRenderer>();
                    cizgiOlusturucu.loop = true;
                    cizgiOlusturucu.material = anaOlusturucu.selectedMat;
                    cizgiOlusturucu.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
            }

            noktaMiktari = transform.childCount;

            cizgiOlusturucu.positionCount = noktaMiktari;
            for (int i = 0; i < noktaMiktari; i++)
            {
                Vector3 poz = transform.GetChild(i).position;
                poz.y = Terrain.activeTerrain.SampleHeight(poz) + noktaYuksekligi;
                transform.GetChild(i).position = poz;

                cizgiOlusturucu.SetPosition(i, transform.GetChild(i).position);
            }
            if (anaOlusturucu.duzenlemeModu && Selection.activeObject == gameObject) 
            { 
                anaOlusturucu.AlanSec(int.Parse(gameObject.name.Split('_')[1]));
            }
        }
        public void NoktalariBoyutlandir(float yeniBoyut)
        {
            if (cizgiOlusturucu) noktaYuksekligi = cizgiOlusturucu.startWidth = cizgiOlusturucu.endWidth = yeniBoyut / 2;
            for (int i = 0; i < noktaMiktari; i++) transform.GetChild(i).localScale = new Vector3(yeniBoyut, yeniBoyut, yeniBoyut);
        }
        public void KendiniImhaEt()
        {
            Undo.DestroyObjectImmediate(gameObject);
        }
        // Silme Engelleyici
        protected virtual void OnEnable()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }
        protected virtual void OnDisable()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyGUI;
        }
        // disable the ability to delete GameObjects in Scene view
        protected virtual void OnSceneGUI()
        {
            InterceptKeyboardDelete();
        }
        // disable the ability to delete GameObjects in Hierarchy view
        protected virtual void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            InterceptKeyboardDelete();
        }
        // intercept keyboard delete event
        private void InterceptKeyboardDelete()
        {
            var e = Event.current;
            if (Selection.activeObject == gameObject && e.keyCode == KeyCode.Delete)
            {
                //e.Use(); // warning
                e.type = EventType.Used;
                anaOlusturucu.AlanSec(int.Parse(gameObject.name.Split('_')[1]));
            }
        }
    }
}
#endif