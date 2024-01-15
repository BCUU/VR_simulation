using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabHandPose : MonoBehaviour
{
    public HandData rightHandData;

    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;
    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;
    private void Start()
    {
        XRGrabInteractable grabInteractable=GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetPose);

        rightHandData.gameObject.SetActive(false);
        

    }
    public void UnSetPose(BaseInteractionEventArgs args){
        HandData handData=args.interactorObject.transform.GetComponentInChildren<HandData>();
        handData.animator.enabled=true;

        SetHandDataValues(handData,rightHandData);
        SetHandData(handData,finalHandPosition,finalHandRotation,finalFingerRotations);
    }
    public void SetupPose(BaseInteractionEventArgs args){
        HandData handData=args.interactorObject.transform.GetComponentInChildren<HandData>();
        handData.animator.enabled=false;

        
        SetHandData(handData,startingHandPosition,startingHandRotation,startingFingerRotations);
    }
    public void SetHandDataValues(HandData h1,HandData h2){
        startingHandPosition=h1.root.localPosition;
        finalHandPosition=h2.root.localPosition;

        startingHandRotation=new Quaternion(h1.root.localRotation.x,h1.root.localRotation.y,h1.root.localRotation.z-90,0);
        //startingHandRotation=h1.root.localRotation;
        //finalHandRotation=new Quaternion(h1.root.localRotation.x,h1.root.localRotation.y,h1.root.localRotation.z-90,0);
        finalHandRotation=h2.root.localRotation;

        startingFingerRotations=new Quaternion[h1.fingerBones.Length];
        finalFingerRotations=new Quaternion[h1.fingerBones.Length];

        for(int i =0;i<h1.fingerBones.Length;i++){
            startingFingerRotations[i]=h1.fingerBones[i].localRotation;
            finalFingerRotations[i]=h2.fingerBones[i].localRotation;
        }
    }
    public void SetHandData(HandData h,Vector3 newPosition, Quaternion newRotation,Quaternion[] newBonesRotation){
        h.root.localPosition=newPosition;
        h.root.localRotation=newRotation;
        for(int i =0; i < newBonesRotation.Length;i++){
            h.fingerBones[i].localRotation=newBonesRotation[i];
        }
    }
}
