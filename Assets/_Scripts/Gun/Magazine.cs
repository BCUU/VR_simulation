using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Magazine : XRGrabInteractable
{
    public GunManager[] gunManagers;
    public GameObject bullet;
    public int currentAmmo;
    public  void Start(){
        
        currentAmmo=gunManagers[0].capacityBullet;

    }

    public void UseAmmo()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
        }
        if(currentAmmo==0){
             bullet.SetActive(false);

        }
        
    }
   
    private void Update()
    {
        
    }
    public void DestroyMagazine(){
        if(interactionLayers!=LayerMask.GetMask("Magazine")){
            StartCoroutine(enumerator());
        }
    }
    IEnumerator enumerator()
    {
        yield return new WaitForSeconds(3f);
            Destroy(gameObject);
    }
}
