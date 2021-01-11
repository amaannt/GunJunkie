using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableAllCursors : MonoBehaviour
{
    public GameObject crosshairParent;
    public GameObject OneCrossHair;
    public void DisableCrossHairs() {
        for (int i = 0; i < crosshairParent.transform.childCount; i++)
        {
            var child = crosshairParent.transform.GetChild(i).gameObject;
            if (child != null)
                child.SetActive(false);
        }
        OneCrossHair.SetActive(true);
    }

}
