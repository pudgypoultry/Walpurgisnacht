using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : MonoBehaviour, IShootable, ICollectable
{
    [SerializeField]
    protected int clipSize;
    [SerializeField]
    protected int ammoInClip;
    [SerializeField]
    protected int totalAmmo;
    [SerializeField]
    protected float bulletSpeed = 10;
    [SerializeField]
    protected float bulletDamage = 1;
    [SerializeField]
    protected GameObject muzzle;
    [SerializeField]
    protected GameObject bullet;
    protected PlayerController player;

    public int ClipSize { get => clipSize; set => clipSize = value; }
    public int AmmoInClip { get => ammoInClip; set => ammoInClip = value; }
    public int TotalAmmo { get => totalAmmo; set => totalAmmo = value; }
    public float BulletDamage { get => bulletDamage; set => bulletDamage = value; }
    public float BulletSpeed { get => bulletSpeed; set => bulletSpeed = value; }
    public GameObject Muzzle { get => muzzle; }
    public GameObject Bullet { get => bullet; }
    public PlayerController Player { get => player; set => player = value; }

    public virtual void ShootMe()
    {
        if (ammoInClip > 0)
        {
            ammoInClip--;
            BaseBullet currentBullet = Instantiate(bullet, muzzle.transform.position, Quaternion.identity).GetComponent<BaseBullet>();
            // Debug.Log(currentBullet);
            currentBullet.Fire(muzzle.transform, bulletSpeed, bulletDamage);
            // Debug.Log(muzzle.transform + " " + bulletSpeed + " " + bulletDamage);
        }
        else
        {
            ReloadMe();
        }
    }

    public virtual void ReloadMe()
    {
        // Debug.Log("RELOADING!");
        if (ammoInClip > 0)
        {
            if (clipSize - ammoInClip > totalAmmo && totalAmmo > 0)
            {
                ammoInClip = totalAmmo;
                totalAmmo = 0;
            }
            else if (totalAmmo == 0)
            {
                // Debug.Log("I'M SPENT");
            }
            else
            {
                totalAmmo -= clipSize - ammoInClip;
                ammoInClip = clipSize;
            }
        }
        else 
        {
            if (clipSize > totalAmmo && totalAmmo > 0)
            {
                ammoInClip = totalAmmo;
                totalAmmo = 0;
            }
            else if (totalAmmo == 0)
            {
                // Debug.Log("I'M SPENT");
            }
            else
            {
                ammoInClip = clipSize;
                totalAmmo -= clipSize;
            }
        }

    }

    public void PrepareMe(PlayerController wielder)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
        player = wielder;
    }

    public void CollectMe(PlayerController player)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
        player.AddGun(this);
    }

    public void ThrowMe()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().enabled = true;
        transform.SetParent(null);
        GetComponent<Rigidbody>().AddForce(transform.forward * 100);
    }

}
