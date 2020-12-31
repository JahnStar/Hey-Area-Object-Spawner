//Developed by Halil Emre Yildiz - @Jahn_Star
//https://www.patreon.com/jahnstar
// Last Edit: 31.12.2020
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace JahnStar.AreaSpawner
{
    [ExecuteInEditMode]
    public class AreaSpawner : MonoBehaviour
    {
        [Space(5)]
        public Material lineMat;
        public Material pointMat, selectedMat;
        [Space(5)]
        public GameObject pointSphere;
        [Range(0.5f, 10)]
        public float pointSize = 5;
        private float oncekiNoktaBuyuklugu, oncekiAltNesneSayisi;
        private bool alanlariGuncelle;
        [HideInInspector]
        public int seciliAlan_id = 0;
        [HideInInspector]
        public bool duzenlemeModu, alanOzellikleri = true;
        [HideInInspector]
        public Transform parentSpawnedObject;
        [HideInInspector]
        public List<SpawnObjectProperty> kopyalananOzellikler;
        void Update()
        {
            if (pointSize != oncekiNoktaBuyuklugu || alanlariGuncelle)
            {
                for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).GetComponent<HeyArea>().NoktalariBoyutlandir(pointSize);
                oncekiNoktaBuyuklugu = pointSize;
                alanlariGuncelle = false;
            }
            if (transform.childCount != oncekiAltNesneSayisi)
            {
                for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).name = "Area_" + i;
                oncekiAltNesneSayisi = transform.childCount;
            }
        }
        public void AlanSec(int alan_id)
        {
            if (alan_id >= 0) seciliAlan_id = alan_id;
            try { for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).GetComponent<LineRenderer>().material = i == seciliAlan_id ? selectedMat : lineMat; }
            catch { }
            Selection.activeObject = gameObject;
            alanlariGuncelle = true;
        }
        public void YeniNokta(Vector3 poz, bool yeniAlan)
        {
            if (transform.childCount == 0 || yeniAlan) 
            {
                YeniAlan(poz);
                AlanSec(transform.childCount - 1);
                oncekiAltNesneSayisi = transform.childCount;
            }
            HeyArea seciliAlan = transform.GetChild(seciliAlan_id).GetComponent<HeyArea>();

            Transform yeniNokta = GameObject.Instantiate(pointSphere).transform;
            yeniNokta.parent = transform.GetChild(seciliAlan_id);
            yeniNokta.name = "Point_" + seciliAlan.transform.childCount;
            yeniNokta.position = poz;
            Renderer notka_R = yeniNokta.GetComponent<Renderer>();
            notka_R.material = pointMat;
            notka_R.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            DestroyImmediate(yeniNokta.GetComponent<Collider>());

            seciliAlan.NoktalariBoyutlandir(pointSize);

            Undo.RegisterCreatedObjectUndo(yeniNokta.gameObject, "Create Object");
        }
        public void YeniAlan(Vector3 poz)
        {
            Transform yeniAlan = new GameObject("Area_" + transform.childCount).transform;
            yeniAlan.position = poz;
            yeniAlan.parent = transform;
            HeyArea yeniAlan_bilesen = yeniAlan.gameObject.AddComponent<HeyArea>();

            Undo.RegisterCreatedObjectUndo(yeniAlan.gameObject, "Create Object");
        }
    }
}
#endif