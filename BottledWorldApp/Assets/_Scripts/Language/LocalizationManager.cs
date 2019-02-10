using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour {

    public static LocalizationManager instance;

    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingTextString = "Localized text not found";

	[SerializeField] GameObject canvas;

    // Use this for initialization
    void Awake () 
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this)
        {
            Destroy (gameObject);
        }

        DontDestroyOnLoad (gameObject);
    }

	void Start(){
		canvas.SetActive (false);
		if (CoinController.Instance.state.language != "") {
			if (CoinController.Instance.state.language == "english") {
				LoadLocalizedText ("english");
			} else if (CoinController.Instance.state.language == "german") {
				LoadLocalizedText ("german");
			} else {
				canvas.SetActive (true);
			}
		} else {
			canvas.SetActive (true);
		}
		/*if (Application.systemLanguage == SystemLanguage.German) {
			LoadLocalizedText ("german");
		} else if (Application.systemLanguage == SystemLanguage.English) {
			LoadLocalizedText ("english");
		}else{
			canvas.SetActive (true);
		}*/

	}
    
    public void LoadLocalizedText(string fileName)
    {
		CoinController.Instance.state.language = fileName;
		fileName = fileName + ".json";
        localizedText = new Dictionary<string, string> ();
        string filePath = Path.Combine (Application.streamingAssetsPath, fileName);

        if (File.Exists (filePath)) {
            string dataAsJson = File.ReadAllText (filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData> (dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++) 
            {
                localizedText.Add (loadedData.items [i].key, loadedData.items [i].value);   
            }

            Debug.Log ("Data loaded, dictionary contains: " + localizedText.Count + " entries");
        } else 
        {
            Debug.LogError ("Cannot find file!");
        }

        isReady = true;
    }

    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey (key)) 
        {
            result = localizedText [key];
        }

        return result;

    }

    public bool GetIsReady()
    {
        return isReady;
    }

}