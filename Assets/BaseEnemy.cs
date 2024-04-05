using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemy : MonoBehaviour, IDamageable
{
    #region Health Management
    private float baseHP = 10;
    private float currentHP;
    private bool isDead = false;
    #endregion
    [SerializeField]
    private float characterSpeed = 2.25f;
    private Rigidbody rb;
    [SerializeField]
    private GameObject currentGun;
    private Transform muzzle;
    private float shootTimer = 0;

    public Transform Muzzle { get => muzzle; set => muzzle = value; }

    public float BaseHP { get => baseHP; set => baseHP = value; }
    public float CurrentHP { get => currentHP; set => currentHP = value; }
    public bool IsDead { get => isDead; set => isDead = value; }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = baseHP;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shootin();
    }

    void Movement()
    {

    }

    void Shootin()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer > 1)
        {
            currentGun.GetComponent<BaseGun>().ShootMe();
            shootTimer = 0;
        }
    }

    public void DamageMe(float damageAmount)
    {
        currentHP -= damageAmount;
        if (currentHP < 0)
        {
            KillMe();
        }
    }

    public void KillMe()
    {
        Destroy(this.gameObject);
    }
}