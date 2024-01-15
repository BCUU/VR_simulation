using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Game/gun")]

public class GunManager : ScriptableObject
{
    public string gunName;
    public float shootSpeed;
    public GameObject bulletPrefab;
    public int objectPoolNumber;
    public float destroyerTime;
    public AudioClip shotClip,emptyClip,reloadSound,tape;
    public RuntimeAnimatorController animatorController;
    public int capacityBullet;
    public GameObject emptyBullet;
}
