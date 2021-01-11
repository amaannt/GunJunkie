using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySettings : MonoBehaviour
{

    public GameObject NikkiHead;
    // Start is called before the first frame update
   


    public void NikkiHeadToggleOff() {
        NikkiHead.SetActive(false);
    }
    public void NikkiHeadToggleOn()
    {
        NikkiHead.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
