using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketObjectController : XRSocketInteractor
{
    public GameObject obj;
    public GunShooter gunShooter;

    [Obsolete("Use OnSelectEntered(XRBaseInteractable interactable, XRBaseInteractor interactor) instead.")]
    protected override void OnSelectEntered(XRBaseInteractable interactable)
    {
        base.OnHoverEntered(interactable);
        obj=interactable.gameObject;
        gunShooter.CheckMagazine();
        

    }
    [Obsolete("Use OnSelectExited(XRBaseInteractable interactable, XRBaseInteractor interactor) instead.")]
     protected override void OnSelectExited(XRBaseInteractable interactable)
    {
        base.OnHoverEntered(interactable);
        obj=null;
        gunShooter.CheckMagazine();
        gunShooter.hasSlide=false;
    }

}
