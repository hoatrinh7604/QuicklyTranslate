using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using UnityEditor;


[System.Serializable]
public class SupprotLanguageData
{
    public List<Language> languages = new List<Language>();
}
[System.Serializable]
public class Language
{
    public string language = "";
    public string name = "";
}
[System.Serializable]
public class SupprotLanguageRoot
{
    public SupprotLanguageData data = new SupprotLanguageData();
}

[System.Serializable]
public class TranslationData
{
    public List<Translation> translations = new List<Translation>();
}
[System.Serializable]
public class TranslationRoot
{
    public TranslationData data = new TranslationData();
}
[System.Serializable]
public class Translation
{
    public string translatedText = "";
    public string detectedSourceLanguage = "";
}

public class Controller : MonoBehaviour
{
    [SerializeField] TMP_InputField paragraph;

    [SerializeField] GameObject listFieldParent;
    [SerializeField] GameObject groupAnswer;


    public string CURRENCY_FORMAT = "#,##0.00";
    public NumberFormatInfo NFI = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };

    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TextMeshProUGUI resultTranslate;
    [SerializeField] Button translateButton;
    [SerializeField] Button copyButton;
    [SerializeField] GameObject copyPopup;
    [SerializeField] GameObject loading;

    SupprotLanguageData languageData = new SupprotLanguageData();
    //Singleton
    public static Controller Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        //Screen.SetResolution(1920, 1080, false);
        Reset();

        translateButton.onClick.AddListener(delegate { Translate(); });
        copyButton.onClick.AddListener(delegate { CopyToClipboard(); });
        StartCoroutine(GetSupportLanguage(UpdateDropdown));
    }

    public void LoadData()
    {
        groupAnswer.SetActive(true);
        translateButton.interactable = true;
        loading.SetActive(false);
    }
    IEnumerator GetSupportLanguage(System.Action callback)
    {
        var url = "https://google-translate1.p.rapidapi.com/language/translate/v2/languages?target=en";

        UnityWebRequest wwwLogin = UnityWebRequest.Get(url);
        wwwLogin.SetRequestHeader("X-RapidAPI-Key", "8780c671c2msh34873a0f47bb233p15fd52jsn0311c30c572d");
        wwwLogin.SetRequestHeader("X-RapidAPI-Host", "google-translate1.p.rapidapi.com");

        yield return wwwLogin.SendWebRequest();

        if (wwwLogin.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wwwLogin.error);
        }
        else
        {
            var res = JsonUtility.FromJson<SupprotLanguageRoot>(wwwLogin.downloadHandler.text);
            languageData = res.data;
            callback();
        }
    }

    IEnumerator GetTranslateLanguage()
    {
        var url = "https://google-translate1.p.rapidapi.com/language/translate/v2";
        WWWForm form = new WWWForm();
        form.AddField("q", paragraph.text);
        form.AddField("target", languageData.languages[dropdown.value].language);
        UnityWebRequest wwwLogin = UnityWebRequest.Post(url, form);
        wwwLogin.SetRequestHeader("X-RapidAPI-Key", "8780c671c2msh34873a0f47bb233p15fd52jsn0311c30c572d");
        wwwLogin.SetRequestHeader("X-RapidAPI-Host", "google-translate1.p.rapidapi.com");

        yield return wwwLogin.SendWebRequest();

        if (wwwLogin.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wwwLogin.error);
        }
        else
        {
            var res = JsonUtility.FromJson<TranslationRoot>(wwwLogin.downloadHandler.text);
            if(res.data.translations.Count > 0)
                resultTranslate.text = res.data.translations[0].translatedText;
            //callback(res.data);
        }
    }

    public string GetResultString()
    {
        return resultTranslate.text;
    }
    public void CopyToClipboard()
    {
        EditorGUIUtility.systemCopyBuffer = GetResultString();
        StartCoroutine(Copied());
    }

    IEnumerator Copied()
    {
        copyPopup.SetActive(true);
        yield return new WaitForSeconds(1f);
        copyPopup.SetActive(false);
    }

    public void UpdateDropdown()
    {
        dropdown.ClearOptions();
        List<string> listString = new List<string>();
        foreach(Language language in languageData.languages)
        {
            listString.Add(language.name);
        }

        dropdown.AddOptions(listString);
        LoadData();
    }

    public void Translate()
    {
        StartCoroutine(GetTranslateLanguage());
    }


    public void Reset()
    {
        loading.SetActive(true);
        copyPopup.SetActive(false);
        listFieldParent.SetActive(false);
        groupAnswer.SetActive(false);
        paragraph.text = "";
        resultTranslate.text = "";
        translateButton.interactable = false;

    }

    public void Clear()
    {
        //dropdown.value = 0;
        paragraph.text = "";
        resultTranslate.text = "";
    }

    public void Quit()
    {
        Clear();
        Application.Quit();
    }
}
