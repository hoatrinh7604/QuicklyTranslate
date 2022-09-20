using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IAPGameController : MonoBehaviour
{
    [SerializeField] GameObject successfulPopUp;
    [SerializeField] GameObject listIAP;
    [SerializeField] TextMeshProUGUI successfulTitle;

    private void Start()
    {
        successfulPopUp.SetActive(false);
        //listIAP.SetActive(false);
    }

    public void SuccessfulPurchase(string namePack)
    {
        UpdateSucessfullTitle(namePack);
        DisplaySuccessfulPopup(true);
    }

    public void FailPurchase()
    {
        UpdateFailTitle();
        DisplaySuccessfulPopup(true);
    }

    public void UpdateSucessfullTitle(string namePack)
    {
        successfulTitle.text = namePack;
    }

    public void DisplaySuccessfulPopup(bool isShown)
    {
        successfulPopUp.SetActive(isShown);
    }

    public void UpdateFailTitle()
    {
        successfulTitle.text = "You purchase is not completed!";
    }
}
