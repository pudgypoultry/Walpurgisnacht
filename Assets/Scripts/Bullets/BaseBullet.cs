using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    protected Rigidbody rb;
    [SerializeField]
    protected float damageAmount = 0;
    protected float lifeSpan = 0;
    [SerializeField]
    protected float maxLifeSpan = 3;

    // Start is called before the first frame update
    protected void Update()
    {
        lifeSpan += Time.deltaTime;
        if (lifeSpan > maxLifeSpan)
        {
            Destroy(this.gameObject);
        }
    }

    public virtual void Fire(Transform positionFiredFrom, float speed, float damage)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(positionFiredFrom.forward * speed, ForceMode.Impulse);
        damageAmount = damage;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseGun>() == null)
        {
            if (other.transform.GetComponent<IDamageable>() != null)
            {
                // Debug.Log("Made it here but I shouldn't have");
                other.transform.GetComponent<IDamageable>().DamageMe(damageAmount);
                Destroy(this.gameObject);
            }

            else
            {
                //  Debug.Log("Made it here");
                Destroy(this.gameObject);
            }
        }

    }

}
