//Developed by Halil Emre Yildiz - @Jahn_Star
//https://github.com/JahnStar/Hey-Area-Object-Spawner
//https://jahnstar.github.io/donate/
// Last Update: 29.10.2021
using UnityEngine;
using RandomType = JahnStar.HeyTriangulator.Generator.RandomType;
[CreateAssetMenu(menuName = "Hey Area Spawner/SpawnObjectProperty", fileName = "Spawn Object Property")]
public class SpawnObjectProperty : ScriptableObject
{
    [Header("Resources")]
    public GameObject gameObject;
    public RandomType type;
    public int count = 1;
    [Header("(Min-Max)")]
    public Vector2 randomScale = Vector2.one;
    public Vector2 randomRotation = new Vector2(-360, 360);
}