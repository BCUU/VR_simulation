using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction;

public class lefthand_settingsUI : MonoBehaviour
{
    public GameObject canvasObject;
    private bool isCanvasActive = false;

    

    void ToggleCanvas()
    {
        // Canvas durumunu tersine çevir (açık ise kapat, kapalı ise aç)
        isCanvasActive = !isCanvasActive;

        // Canvas'ı etkinleştir veya devre dışı bırak
        canvasObject.SetActive(isCanvasActive);
    }
}

