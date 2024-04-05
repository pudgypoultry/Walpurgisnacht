using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    #region Health Management
    [SerializeField]
    private float baseHP = 100;
    [SerializeField]
    private float currentHP;
    private bool isDead = false;
    #endregion
    [SerializeField]
    protected float playerSpeed = 2.25f;
    [SerializeField]
    protected float dashPower = 1;
    [SerializeField]
    protected float dashCoyoteTime = 2.0f;
    [SerializeField]
    protected float dashCoyoteTimer = 0.0f;
    [SerializeField]
    protected bool dashing = false;
    [SerializeField]
    protected float cameraSpeedX = 2f;
    [SerializeField]
    protected float cameraSpeedY = 2f;
    [SerializeField]
    protected float jumpForce = 10f;
    protected bool hasDoubleJumped = false;
    protected bool overrideDash = false;
    [SerializeField]
    protected float doubleJumpTolerance = 0.2f;
    [SerializeField]
    protected float meleeRange = 5;

    protected float cameraY;
    protected float cameraX;
    protected Camera cam;

    protected Rigidbody rb;
    [SerializeField]
    protected GameObject currentGun;
    [SerializeField]
    protected Transform doubleJumpCheck;
    [SerializeField]
    protected Transform gunPosition;

    protected Vector3 moveDirection = Vector3.zero;

    Ray doubleJumpRay;
    RaycastHit hit;

    [SerializeField]
    protected List<BaseGun> gunInventory = new List<BaseGun>();
    protected int currentGunIndex = 0;
    protected bool swapped = false;

    public List<BaseGun> GunInventory { get => gunInventory; set=> gunInventory = value; }
    public GameObject CurrentGun { get => currentGun; set => currentGun = value; }
    public float BaseHP { get => baseHP; set => baseHP = value; }
    public float CurrentHP { get => currentHP; set => currentHP = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public bool HasDoubleJumped { get => hasDoubleJumped; set => hasDoubleJumped = value; }
    public bool OverrideDash { get => overrideDash; set => overrideDash = value; }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = baseHP;
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        currentGun = gunInventory[0].gameObject;
        currentGun.GetComponent<BaseGun>().Player = this;

        foreach (BaseGun gun in gunInventory)
        {
            gun.PrepareMe(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = Vector3.zero;
        Movement();
        Rotation();
        Jumpin();
        Shootin();
        Reloadin();
        SwapGuns();
        Collectin();
    }

    protected virtual void Movement()
    {

        // Vector3 is a type, Vector3.zero = Vector3(0,0,0), think of this as (x, y, z)
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += -1 * new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += new Vector3(cam.transform.right.x, 0, cam.transform.right.z).normalized;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += -1 * new Vector3(cam.transform.right.x, 0, cam.transform.right.z).normalized;
        }

        moveDirection = moveDirection.normalized;

        if (!dashing)
        {
            transform.position += moveDirection * playerSpeed * Time.deltaTime;
        }
        else
        {
            dashCoyoteTimer += Time.deltaTime;
            if (dashCoyoteTimer > dashCoyoteTime)
            {
                if (!overrideDash)
                {
                    // rb.velocity = new Vector3(0, rb.velocity.y, 0);
                }
                dashCoyoteTimer = 0;
                dashing = false;
                rb.useGravity = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashing && !overrideDash)
        {
            rb.velocity = new Vector3(0, 0, 0);
            rb.AddForce(moveDirection * dashPower, ForceMode.Force);
            rb.useGravity = false;
            dashing = true;
        }

    }

    protected virtual void Rotation()
    {
        cameraX += cameraSpeedX * Input.GetAxis("Mouse X");
        cameraY -= cameraSpeedY * Input.GetAxis("Mouse Y");
        cameraY = Mathf.Clamp(cameraY, -80, 80);
        transform.eulerAngles = new Vector3(0.0f, cameraX, 0.0f);
        cam.transform.eulerAngles = new Vector3(cameraY, cameraX, 0.0f);
    }

    protected virtual void Jumpin()
    {
        doubleJumpRay = new Ray(doubleJumpCheck.position, Vector3.down);

        if (Input.GetKeyDown(KeyCode.Space) && !hasDoubleJumped)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(0f, jumpForce, 0f, ForceMode.Impulse);


            if (!Physics.Raycast(doubleJumpRay, out hit, doubleJumpTolerance))
            {
                hasDoubleJumped = true;

            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && hasDoubleJumped)
        {
            // Debug.Log("Land First");

        }

        if (Physics.Raycast(doubleJumpRay, out hit, doubleJumpTolerance) && hit.transform.tag != "Player")
        {
            // Debug.Log("RESETTING JUMP");
            hasDoubleJumped = false;
        }


    }

    protected virtual void Shootin()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentGun.GetComponent<BaseGun>().ShootMe();
        }
    }

    protected virtual void Reloadin()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Hey I'm trying to reload");
            currentGun.GetComponent<IShootable>().ReloadMe();
        }
    }

    public virtual void Collectin()
    {
        Debug.DrawLine(transform.position, transform.position + (transform.forward * meleeRange), Color.red);
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Debug.Log("Collecting something");
            Ray collectRay = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(collectRay, out hit))
            {
                Debug.Log("Attempting to collect: " + hit.transform.name);
            }

            if (Physics.Raycast(collectRay, out hit, meleeRange) && hit.transform.GetComponent<ICollectable>() != null) 
            {
                Debug.Log("Attempting to collect: " + hit.transform.name);
                hit.transform.GetComponent<ICollectable>().CollectMe(this);
            }
        }
    }

    public virtual void DamageMe(float damageAmount)
    { 
        currentHP -= damageAmount;
        if (currentHP < 0)
        {
            KillMe();
        }
    }

    public virtual void KillMe()
    {
        Debug.Log("I'm dead");
    }

    protected virtual void SwapGuns()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            currentGunIndex += 1;
            currentGunIndex = currentGunIndex % gunInventory.Count;
            swapped = true;
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            currentGunIndex -= 1;
            currentGunIndex = (currentGunIndex + gunInventory.Count) % gunInventory.Count;
            swapped = true;
        }

        if (swapped)
        {
            foreach (BaseGun gun in gunInventory)
            {
                gun.gameObject.SetActive(false);
                if (gunInventory.IndexOf(gun) == currentGunIndex)
                {
                    gun.gameObject.SetActive(true);
                }
            }

            swapped = false;
            currentGun = gunInventory[currentGunIndex].gameObject;
        }
    }

    public virtual void AddGun(IShootable gunToAdd)
    {
        if (gunInventory.Contains(null))
        {
            currentGunIndex = gunInventory.IndexOf(null);
        }
        else
        {
            SubtractGun(currentGunIndex);
        }
        gunInventory[currentGunIndex] = gunToAdd.gameObject.GetComponent<BaseGun>();
        swapped = true;
        gunToAdd.transform.SetParent(cam.transform);
        gunToAdd.transform.position = gunPosition.transform.position;
        gunToAdd.transform.rotation = gunPosition.rotation;
        
    }

    public virtual void SubtractGun(int indexToSubtract)
    {
        BaseGun gunToSubtract = gunInventory[indexToSubtract];
        gunToSubtract.ThrowMe();
        gunInventory[indexToSubtract] = null;
    }



}