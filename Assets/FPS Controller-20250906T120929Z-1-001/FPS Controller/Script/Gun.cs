using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float EnemyDestroyer = 12f;
    public float radius = 150f;

    public Camera CameraFocus;

    public float disfigure = 50f;
    public float DamageForce = 50f;
    public GameObject ParticleEffect;
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GunShoot();
        }

        void GunShoot()
        {
            RaycastHit Target;
            if (Physics.Raycast(CameraFocus.transform.position, CameraFocus.transform.forward, out Target, radius))
            {
                ShotFired shooting = Target.transform.GetComponent<ShotFired>();

                if (shooting != null)
                {
                    shooting.DamageImpact(disfigure);
                }

                if (Target.rigidbody != null)
                {
                    Target.rigidbody.AddForce(-Target.normal * DamageForce);
                }
                GameObject ClearObject = Instantiate(ParticleEffect, Target.point, Quaternion.LookRotation(Target.normal));
                Destroy(ClearObject, 2f);

            }
        }
    }
}


