using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DebugText : MonoBehaviour
{
    public PlayerController player;
    private TMP_Text text;
    int currentAmmo = 0;
    int maxAmmo = 0;
    int totalAmmo = 0;
    IShootable gun;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        gun = player.CurrentGun.GetComponent<IShootable>();
    }

    private void Update()
    {
        currentAmmo = gun.AmmoInClip;
        maxAmmo = gun.ClipSize;
        totalAmmo = gun.TotalAmmo;

        text.text = "Total Ammo: " + totalAmmo + "\n" + currentAmmo + "/" + maxAmmo;
    }
}
