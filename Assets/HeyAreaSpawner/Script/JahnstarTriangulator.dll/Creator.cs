//Developed by Halil Emre Yildiz - @Jahn_Star (All right reserved. 2020)
//https://jahnstar.github.io/donate/
// Last Edit: 18.10.2020
using UnityEngine;
using JahnStar.HeyTriangulator;
using UnityEditor;

namespace JahnStar.AreaSpawner
{
    public class Creator
    {
        /// <summary>
        /// Developed by Halil Emre Yildiz - @Jahn_Star
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="scaleRange"></param>
        /// <param name="count"></param>
        /// <param name="pointsParent"></param>
        /// <param name="mainParent"></param>
        /// <param name="randomType"></param>
        /// <param name="areaRender"></param>
        /// <returns></returns>
        public static Transform SpawnObjectInArea(GameObject gameObject, Vector2 scaleRange, Vector2 rotRange, int count, Transform pointsParent, Transform mainParent, Generator.RandomType randomType, bool areaRender = false)
        {
            // Create Vector2D vertices
            Vector3[] points = new Vector3[pointsParent.childCount];
            for (int i = 0; i < points.Length; i++) points[i] = pointsParent.GetChild(i).localPosition;
            Mesh mesh = Generator.PointsToMesh(points);

            Transform parent = new GameObject(pointsParent.name + " (" + gameObject.name + ")").transform;
            parent.parent = mainParent;
            parent.SetPositionAndRotation(new Vector3(pointsParent.position.x, 0, pointsParent.position.z), pointsParent.rotation);

            Vector3[] randomPoints = Generator.PickRandomLocations(mesh, randomType, count);

            for (int i = 0; i < randomPoints.Length; ++i)
            {
                Vector3 poz3 = randomPoints[i];
                Transform newPoint;
                try
                {
                #if UNITY_EDITOR
                    newPoint = (PrefabUtility.InstantiatePrefab(gameObject) as GameObject).transform;
                    newPoint.transform.parent = parent;
                #else
                    throw new System.Exception();
                #endif
                }
                catch
                {
                    newPoint = GameObject.Instantiate(gameObject, parent).transform;
                }

                float randomScale = Random.Range(scaleRange.x, scaleRange.y);
                Vector3 newScale = newPoint.localScale;
                newScale *= randomScale;
                if (scaleRange.x > 0 && scaleRange.y > 0) newPoint.localScale = newScale;

                float randomRot = Random.Range(rotRange.x, rotRange.y);
                Vector3 newRot = newPoint.localRotation.eulerAngles;
                newRot.y += randomRot;
                if (rotRange != Vector2.zero) newPoint.localRotation = Quaternion.Euler(newRot);

                newPoint.localPosition = new Vector3(poz3.x, 0, poz3.z);
                newPoint.localPosition = new Vector3(poz3.x, Terrain.activeTerrain.SampleHeight(newPoint.position), poz3.z);
            }

            if (areaRender)
            {
                // Set up game object with mesh;
                GameObject alanObj = new GameObject(pointsParent.name + "_mesh");
                alanObj.transform.parent = parent;
                alanObj.transform.position = new Vector3(pointsParent.position.x, pointsParent.position.y, pointsParent.position.z);
                MeshRenderer renderer = (MeshRenderer)alanObj.AddComponent(typeof(MeshRenderer));
                MeshFilter filter = alanObj.AddComponent(typeof(MeshFilter)) as MeshFilter;
                filter.mesh = mesh;
                //renderer.material = GameObject.CreatePrimitive(PrimitiveType.Plane).GetComponent<MeshRenderer>().sharedMaterial;
                //renderer.material.color = Color.cyan;
            }

            return parent;
        }
    }
}