using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotFired : MonoBehaviour
{
    public float life = 50f;
    public GameObject EnemyDown;
     public void DamageImpact(float extentoFDamage)
    {
        life -= extentoFDamage;
        if(life <= 0f)
        {
            inValid();
        }
    }
    void inValid()
    {
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        Instantiate(EnemyDown, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
