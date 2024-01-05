using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GunShooter : XRGrabInteractable
{
    public GunManager[] gunManagers;
    private List<GameObject> pool;
    private int objectPoolNumber;
    private float destroyerTime,shootSpeed;
    private GameObject prefab;
    public GameObject transformers;
    public GameObject muzzleFlashPrefab;
    private AudioSource audioSource;
    private AudioClip pistolShoot;
    private Animator animator;
    private RuntimeAnimatorController animatorController;
    private bool objectpoolcheck=false;
    
    
    private void Start()
    {
        audioSource=GetComponent<AudioSource>();
        pistolShoot=gunManagers[0].shotClip;
        objectPoolNumber=gunManagers[0].objectPoolNumber;
        destroyerTime=gunManagers[0].destroyerTime;
        prefab=gunManagers[0].bulletPrefab;
        shootSpeed=gunManagers[0].shootSpeed;
        animator=GetComponent<Animator>();
        animatorController=gunManagers[0].animatorController;
        animator.runtimeAnimatorController=animatorController;
        
    }
    public void ObjectPool()
    { 
        if(objectpoolcheck==false)
        {
        pool = new List<GameObject>();

        for (int i = 0; i < objectPoolNumber; i++)
        {
            CreateObject();
        }
        objectpoolcheck=true;
        } 
    }
    private GameObject CreateObject()
    {
        GameObject obj = GameObject.Instantiate(prefab);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }
    public GameObject GetObject()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        
        GameObject newObj = CreateObject();
        newObj.SetActive(true);
        return newObj;
    }
    public void Shoot()
    {
        if(animatorController)
        {
            animator.SetTrigger("shootTriger");

        }
        audioSource.PlayOneShot(pistolShoot);

        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, transformers.transform.position, gameObject.transform.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyerTime);
        }
        GameObject bullet =GetObject();
        bullet.transform.position=transformers.transform.position;
        bullet.transform.rotation=transformers.transform.rotation;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = transform.forward *shootSpeed;
        StartCoroutine(DestroyAfterDelay(bullet,destroyerTime));
    }

    protected override void OnActivate(XRBaseInteractor interactor)
    {
        base.OnActivate(interactor);
        Shoot();
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        ObjectPool();
    }
    IEnumerator DestroyAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false);
    }
}
