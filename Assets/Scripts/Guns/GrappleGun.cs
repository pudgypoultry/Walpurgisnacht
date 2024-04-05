using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGun : BaseGun
{
    protected bool grappleDeployed = false;
    protected GrappleBullet hook;
    protected Vector3 hookLocation = Vector3.zero;
    protected float maxDistance = Mathf.Infinity;
    protected bool confirmed;
    [SerializeField]
    protected float pullForce = 1;

    protected Transform[] points = new Transform[2];
    [SerializeField]
    protected GrappleLineRenderer grappleLine;
    protected GrappleLineRenderer currentLine;
    protected bool lineIsOut = false;

    protected void Start()
    {
        points[0] = muzzle.transform;
    }

    protected void Update()
    {
        Debug.Log("Player: " + player);

        if (ammoInClip == 0 && totalAmmo == 0 && !grappleDeployed)
        {
            totalAmmo = 1;
            confirmed = false;
            grappleDeployed = false;
        }

        if (grappleDeployed && hook != null && hook.Latched && !confirmed)
        {
            // Debug.Log("I've confirmed");
            Debug.Log("Hook: " + hook + "\nLatched: " + hook.Latched + "\nConfirmed: " + confirmed);
            hookLocation = hook.transform.position;
            maxDistance = (player.transform.position - hookLocation).magnitude;
            Debug.Log("Max distance is set to: " + maxDistance);
            confirmed = true;
        }

        if (confirmed)
        {
            // Debug.Log("I SHOULD BE GRAPPLING");
            if ((player.transform.position - hookLocation).magnitude >= maxDistance)
            {
                // Debug.Log("hookLocation: " + hookLocation);
                player.GetComponent<Rigidbody>().AddForce((hookLocation - player.transform.position).normalized * pullForce, ForceMode.Force);
            }
        }


    }

    public override void ShootMe()
    {
        if (ammoInClip > 0)
        {
            ammoInClip--;
            BaseBullet currentBullet = Instantiate(bullet, muzzle.transform.position, Quaternion.identity).GetComponent<BaseBullet>();
            // Debug.Log(currentBullet);
            currentBullet.Fire(muzzle.transform, bulletSpeed, bulletDamage);
            // Debug.Log(muzzle.transform + " " + bulletSpeed + " " + bulletDamage);
            grappleDeployed = true;
            confirmed = false;
            hook = currentBullet.GetComponent<GrappleBullet>();
            points[1] = hook.transform;
            currentLine = Instantiate(grappleLine);
            currentLine.SetUpLine(points);
        }
        else
        {
            grappleDeployed = false;
            confirmed = false;
            lineIsOut = false;
            // Destroy(grappleLine.gameObject);
            if (currentLine != null)
            {
                currentLine.Ready = false;
                currentLine.DestroyMe();
            }

            ReloadMe();
            if (hook != null)
            {
                Destroy(hook.gameObject);
            }

        }
    }

    private void OnDisable()
    {
        // Destroy(grappleLine.gameObject);
        if (currentLine != null)
        {
            currentLine.Ready = false;
            currentLine.DestroyMe();
        }

        if (hook != null)
        {
            lineIsOut = false;
            Destroy(hook.gameObject);
        }
    }
}
