using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable
{
    GameObject gameObject { get; }
    Transform transform { get; }
    GameObject Muzzle { get; }
    GameObject Bullet { get; }
    int ClipSize { get; set; }
    int AmmoInClip { get; set; }
    int TotalAmmo { get; set; }
    float BulletDamage { get; set; }
    float BulletSpeed { get; set; }
    void ShootMe();
    void ReloadMe();
}
