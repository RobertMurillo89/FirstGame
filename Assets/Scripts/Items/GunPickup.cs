using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GunStats Gun;
    void Start()
    {
        Gun.ammoCur = Gun.ammoMax;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) ;
        {
            GameManager.instance.playerScript.GunPickup(Gun);
            Destroy(gameObject);
        }
    }
}
