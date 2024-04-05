using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleBullet : BaseBullet
{

    protected bool latched = false;

    public bool Latched { get => latched; set => latched = value; }

    protected override void OnTriggerEnter(Collider other)
    {

        if (other.transform.GetComponent<IDamageable>() != null)
        {
            // Debug.Log("Made it here but I shouldn't have");
            other.transform.GetComponent<IDamageable>().DamageMe(damageAmount);
            Destroy(this.gameObject);
        }

        else
        {
            rb.velocity = Vector3.zero;
            latched = true;
        }
    }

    public void RetractMe()
    {
        Debug.Log("Attempting to retract");
    }

}
