using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixHand : MonoBehaviour
{
    public Transform rHand;
    public Transform lHand;
    public Transform ref_rHand;
    public Transform ref_lHand;
    public bool hasWeapon = false;
    // Update is called once per frame
    void Update()
    {
        if(hasWeapon) {
            rHand.transform.position = ref_rHand.transform.position;
            lHand.transform.position = ref_lHand.transform.position;
        }
    }
}
