using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviour {

    public static LocalizationManager instance;

    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingTextString = "Localized text not found";

	[SerializeField] GameObject canvas;

    [SerializeField] string[] lexEnglish;
    [SerializeField] string[] lexGerman;

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
		if (CoinController.Instance.state.language != "german" && CoinController.Instance.state.language != "english") {
           /* if (Application.systemLanguage == SystemLanguage.German)
            {
                SetLanguage("german");
            }
            else if (Application.systemLanguage == SystemLanguage.English)
            {
                SetLanguage("english");
            }
            else
            {*/
                canvas.SetActive(true);
           // }
        }
        else
        {
            SceneManager.LoadScene("StartUp");
        }
		/*if (Application.systemLanguage == SystemLanguage.German) {
			LoadLocalizedText ("german");
		} else if (Application.systemLanguage == SystemLanguage.English) {
			LoadLocalizedText ("english");
		}else{
			canvas.SetActive (true);
		}*/

	}
    
    /*public void LoadLocalizedText(string fileName)
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
    }*/

    public string GetLocalizedValue(int id)
    {
        string result = missingTextString;
       
        if(CoinController.Instance.state.language == "english")
        {
            result = lexEnglish[id];
        }
        else if (CoinController.Instance.state.language == "german")
        {
            result = lexGerman[id];
        }

        return result;

    }

    public void SetLanguage(string s)
    {
        CoinController.Instance.state.language = s;
        CoinController.Instance.Save();
        SceneManager.LoadScene("StartUp");
    }

}