using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SliderManager : MonoBehaviour
{
    public GameObject sliderTarget;
    public float threshold=0.02f;
    public UnityEvent OnReached;
    private bool wasReached =false;
    
    private void FixedUpdate()
    {
        float distance =Vector3.Distance(transform.position,sliderTarget.gameObject.transform.position);
        if(distance<threshold && !wasReached){
            OnReached.Invoke();
            wasReached=true;
        }
        else if (distance>=threshold){
            wasReached=false;
        }
    }
}
