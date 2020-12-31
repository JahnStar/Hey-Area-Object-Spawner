//Developed by Halil Emre Yildiz - @Jahn_Star
//https://www.patreon.com/jahnstar
// Last Edit: 23.10.2020
using UnityEngine;
using RandomType = JahnStar.HeyTriangulator.Generator.RandomType;
[CreateAssetMenu(menuName = "Hey Area Spawner/SpawnObjectProperty", fileName = "Spawn Object Property")]
public class SpawnObjectProperty : ScriptableObject
{
    [Header("Spawn Object")]
    public GameObject nesne;
    [Header("Spawn Type")]
    public RandomType tip;
    [Header("Spawn Count")]
    public int miktar = 1;
    [Header("Random Scale Range (Min-Max)")]
    public Vector2 rastgeleBoyut = Vector2.one;
    [Header("Random Rotation Range (Min-Max)")]
    public Vector2 rastgeleRotasyon = new Vector2(-360, 360);

}