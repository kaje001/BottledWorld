using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class ScaleSugarCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(gameObject, transform.localScale * 1.3f, 1.2f).setLoopPingPong(-1).setEase(LeanTweenType.easeInOutSine);
    }
    
}
