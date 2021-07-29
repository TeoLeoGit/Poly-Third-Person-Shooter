using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class ActiveWeapon : MonoBehaviour
{
    public Transform raycastDestination;
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    
    RaycastWeapon weapon;
    Animator anim;
    AnimatorOverrideController overrides;

    public Transform weaponParent;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;
    public UnityEngine.Animations.Rigging.Rig handIK;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        overrides = anim.runtimeAnimatorController as AnimatorOverrideController;

        muzzleFlash.Stop();
        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon) {
            Equip(existingWeapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(weapon) {
            handIK.weight = 1.0f;
            if(Input.GetButtonDown("Fire1")) {
                weapon.StartFiring();
            }

            if(Input.GetButtonUp("Fire1")) {
                weapon.StopFiring();
            }
        } else {
            handIK.weight = 0.0f;
            anim.SetLayerWeight(1, 0.0f);
        }
    }

    public void Equip(RaycastWeapon newWeapon) {
        if(weapon) {
            Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        weapon.raycastDestination = raycastDestination;
        weapon.muzzleFlash = muzzleFlash;
        weapon.hitEffect = hitEffect;

        weapon.transform.parent = weaponParent;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        handIK.weight = 1.0f;
        anim.SetLayerWeight(1, 1.0f);

        
        handIK.GetComponent<FixHand>().hasWeapon = true;
        Invoke(nameof(SetAnimationDelayed), 0.001f);
    }

    void SetAnimationDelayed() {
        overrides["weapon_anim_empty"] = weapon.weaponAnimation;
    }

    [ContextMenu("Save weapon pose")]
    void SaveWeaponPose() {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(weapon.weaponAnimation);
        UnityEditor.AssetDatabase.SaveAssets();
    }
}
