using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/New Gun", order = 0)]
public class GunScriptableObject : ScriptableObject
{
    public string gunName;
    public float damage;
    public float range;
    public float coolDown;

    public float clipSize;
    public float ammoSize;
}
