//Developed by Halil Emre Yildiz - @Jahn_Star
//https://github.com/JahnStar/Hey-Area-Object-Spawner
//https://jahnstar.github.io/donate/
// Last Update: 30.10.2021
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
        public LineRenderer lineRenderer;
        [HideInInspector]
        public int pointCount;
        float pointHeight;
        private AreaSpawner spawner;
        public List<SpawnObjectProperty> objects = new List<SpawnObjectProperty>();
        private void Update()
        {
            if (!spawner) spawner = transform.root.GetComponent<AreaSpawner>();
            if (!lineRenderer) 
            {
                if (GetComponent<LineRenderer>()) lineRenderer = GetComponent<LineRenderer>();
                else
                {
                    lineRenderer = gameObject.AddComponent<LineRenderer>();
                    lineRenderer.loop = true;
                    lineRenderer.material = spawner.selectedMat;
                    lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
            }

            pointCount = transform.childCount;

            lineRenderer.positionCount = pointCount;
            for (int i = 0; i < pointCount; i++)
            {
                Vector3 poz = transform.GetChild(i).position;
                poz.y = Terrain.activeTerrain.SampleHeight(poz) + pointHeight;
                transform.GetChild(i).position = poz;

                lineRenderer.SetPosition(i, transform.GetChild(i).position);
            }
            if (spawner.editMode && Selection.activeObject == gameObject) spawner.ChooseArea(int.Parse(gameObject.name.Split('_')[1]));
        }
        public void ResizePoint(float pointSize, float lineThickness)
        {
            if (lineRenderer) 
            {
                pointHeight = lineThickness / 2;
                lineRenderer.startWidth = lineRenderer.endWidth = lineThickness;
            }
            for (int i = 0; i < pointCount; i++) transform.GetChild(i).localScale = new Vector3(pointSize, pointSize, pointSize);
        }
        public void DestroySelf() => Undo.DestroyObjectImmediate(gameObject);
        // block deleting
        protected virtual void OnEnable() => EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        protected virtual void OnDisable() => EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyGUI;
        // disable the ability to delete GameObjects in Scene view
        protected virtual void OnSceneGUI() => InterceptKeyboardDelete();
        // disable the ability to delete GameObjects in Hierarchy view
        protected virtual void OnHierarchyGUI(int instanceID, Rect selectionRect) => InterceptKeyboardDelete();
        // intercept keyboard delete event
        private void InterceptKeyboardDelete()
        {
            var e = Event.current;
            if (Selection.activeObject == gameObject && e.keyCode == KeyCode.Delete)
            {
                //e.Use(); // warning
                e.type = EventType.Used;
                spawner.ChooseArea(int.Parse(gameObject.name.Split('_')[1]));
            }
        }
    }
}
#endif