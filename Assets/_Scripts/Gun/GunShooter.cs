using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GunShooter : XRGrabInteractable
{
    public GunManager[] gunManagers;
    private List<GameObject> pool,poolEmptyBullet;
    private int objectPoolNumber,capacityBullet;
    private float destroyerTime,shootSpeed;
    private GameObject prefab,emptyBullet;
    public GameObject tShoot,tEmpty;
    public GameObject muzzleFlashPrefab;
    private AudioSource audioSource;
    private AudioClip pistolShoot,emptyShoot,reloadSound,tape;
    private Animator animator;
    private RuntimeAnimatorController animatorController;
    private bool objectpoolcheck=false;
    public bool reload=false;
    private GameObject magazineObj;
    private Magazine magazine;
    public SocketObjectController socketObjectController;
    public bool hasSlide;
    
    
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
        //capacityBullet=gunManagers[0].capacityBullet;
        // magazine.maxAmmo = gunManagers[0].capacityBullet;
        emptyShoot=gunManagers[0].emptyClip;
        emptyBullet=gunManagers[0].emptyBullet;
        reloadSound=gunManagers[0].reloadSound;
        tape=gunManagers[0].tape;

        
    }
    public void CheckMagazine(){
         if(socketObjectController.obj){
            audioSource.PlayOneShot(tape);
            magazineObj =socketObjectController.obj;
            magazine=magazineObj.GetComponent<Magazine>();
        }
        else{
            audioSource.PlayOneShot(tape);
            magazineObj =null;
            magazine=null;
            reload=false;
        }

    }
    public void ObjectPool()
    { 
        if(objectpoolcheck==false)
        {
        pool = new List<GameObject>();
        poolEmptyBullet=new List<GameObject>();

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
        GameObject objE = GameObject.Instantiate(emptyBullet);
        obj.SetActive(false);
        objE.SetActive(false);
        pool.Add(obj);
        poolEmptyBullet.Add(objE);
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
    public GameObject GetEmptyBullet(){
          foreach (var objE in poolEmptyBullet)
        {
            if (!objE.activeInHierarchy)
            {
                objE.SetActive(true);
                return objE;
            }
        }
        GameObject newObjE = CreateObject();
        newObjE.SetActive(true);
        return newObjE;
    }
    public void Shoot()
    {
        magazine.UseAmmo();
        if(animatorController)
        {
            animator.SetTrigger("shootTriger");

        }
        audioSource.PlayOneShot(pistolShoot);

        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, tShoot.transform.position, gameObject.transform.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyerTime);
        }
        GameObject bullet =GetObject();
        bullet.transform.position=tShoot.transform.position;
        bullet.transform.rotation=tShoot.transform.rotation;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = transform.forward *shootSpeed;
        StartCoroutine(DestroyAfterDelay(bullet,destroyerTime));

        GameObject bulletE =GetEmptyBullet();
        bulletE.transform.position=tEmpty.transform.position;
        bulletE.transform.rotation=tEmpty.transform.rotation;
        Rigidbody rbE = bulletE.GetComponent<Rigidbody>();
        rbE.velocity = transform.right*3;
        StartCoroutine(DestroyAfterDelayEmptyBullet(bulletE,destroyerTime));
    }
    public void nullShoot(){
        audioSource.PlayOneShot(emptyShoot);
        animator.SetTrigger("shootTriger");
    }
    [Obsolete("Use OnActivate(XRBaseInteractable interactable, XRBaseInteractor interactor) instead.")]
    protected override void OnActivate(XRBaseInteractor interactor)
    {
        base.OnActivate(interactor);
        if(hasSlide)
        {
            if(magazine!=null&&magazine.currentAmmo>0)
            Shoot();
            else
            nullShoot();
        }
        else
        nullShoot();
        
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
    IEnumerator DestroyAfterDelayEmptyBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false);
    }
    public void Slide(){
        hasSlide=true;
        audioSource.PlayOneShot(reloadSound);
    }
    public void DropMagazine(){
        if(magazine!=null){
            Debug.Log("AAAa");
            magazine.interactionLayers =LayerMask.GetMask("Default");
            magazine.DestroyMagazine();
        }
    }
}
