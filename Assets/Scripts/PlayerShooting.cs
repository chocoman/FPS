using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    public Weapon weapon;

    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))   // MouseButton1, but can be changed
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            Debug.Log("hit" + hit.collider.name);   // => Doesn't work/Needs a change - 90° difference in vertical 
        }
    }
}
