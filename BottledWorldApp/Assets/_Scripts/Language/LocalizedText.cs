using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour {

    [SerializeField] int key;

    // Use this for initialization
    void Start () 
    {
        Text text = GetComponent<Text> ();
        text.text = LocalizationManager.instance.GetLocalizedValue (key);
    }

}