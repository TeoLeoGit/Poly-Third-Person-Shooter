using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    public bool isFiring = false;
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public Transform raycastOrigin;
    public Transform raycastDestination;

    public AnimationClip weaponAnimation;
    public TrailRenderer tracerEffect;
    Ray ray;
    RaycastHit hitInfo;

    //adding
    public float fireRate = 0.1F;
    private float nextFire = 0.0F;

    public void StartFiring() {
        isFiring = true;
    }

    void Update() {
        if(isFiring && Time.time > nextFire) {
            nextFire = Time.time + fireRate;
            ray.origin = raycastOrigin.position;
            ray.direction = raycastDestination.position - raycastOrigin.position;

            muzzleFlash.Emit(20);
            var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
            tracer.AddPosition(ray.origin);

            if(Physics.Raycast(ray, out hitInfo, 100)) {
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                hitEffect.Emit(1);

                tracer.transform.position = hitInfo.point;
            }
        }
    }

    public void StopFiring() {
        isFiring = false;
    }
}
