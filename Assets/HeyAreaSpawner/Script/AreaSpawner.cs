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
    public class AreaSpawner : MonoBehaviour
    {
        [Range(0.5f, 10)]
        public float lineThickness = 5, pointSize = 5;
        public GameObject pointSphere;
        [Space(5)]
        public Material lineMat;
        public Material pointMat, selectedMat;
        private float _prev_childCount;
        private string _prev_size;
        private bool _updateArea;
        [HideInInspector]
        public int selectedAreaID = 0;
        [HideInInspector]
        public bool editMode, show_AreaObjects = true;
        [HideInInspector]
        public Transform parentSpawnedObject;
        [HideInInspector]
        public List<SpawnObjectProperty> clipboard;
        void Update()
        {
            if (pointSize + lineThickness + "" != _prev_size || _updateArea)
            {
                for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).GetComponent<HeyArea>().ResizePoint(pointSize, lineThickness);
                _prev_size = pointSize + lineThickness + "";
                _updateArea = false;
            }
            if (transform.childCount != _prev_childCount)
            {
                for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).name = "Area_" + i;
                _prev_childCount = transform.childCount;
            }
        }
        public void ChooseArea(int areaID)
        {
            if (areaID >= 0) selectedAreaID = areaID;
            try { for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).GetComponent<LineRenderer>().material = i == selectedAreaID ? selectedMat : lineMat; }
            catch { }
            Selection.activeObject = gameObject;
            _updateArea = true;
        }
        public void NewPoint(Vector3 position, bool newArea)
        {
            if (transform.childCount == 0 || newArea) 
            {
                NewArea(position);
                ChooseArea(transform.childCount - 1);
                _prev_childCount = transform.childCount;
            }
            HeyArea selectedArea = transform.GetChild(selectedAreaID).GetComponent<HeyArea>();

            Transform newPoint = GameObject.Instantiate(pointSphere).transform;
            newPoint.parent = transform.GetChild(selectedAreaID);
            newPoint.name = "Point_" + selectedArea.transform.childCount;
            newPoint.position = position;
            Renderer point_R = newPoint.GetComponent<Renderer>();
            point_R.material = pointMat;
            point_R.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            DestroyImmediate(newPoint.GetComponent<Collider>());

            selectedArea.ResizePoint(pointSize, lineThickness);

            Undo.RegisterCreatedObjectUndo(newPoint.gameObject, "Create Object");
        }
        public void NewArea(Vector3 poz)
        {
            Transform newArea = new GameObject("Area_" + transform.childCount).transform;
            newArea.position = poz;
            newArea.parent = transform;
            HeyArea _newArea = newArea.gameObject.AddComponent<HeyArea>();

            Undo.RegisterCreatedObjectUndo(newArea.gameObject, "Create Object");
        }
    }
}
#endif