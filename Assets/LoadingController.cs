using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingController : MonoBehaviour
{
    [SerializeField] float timeToClose;

    private float time;

    private void Start()
    {
        //StartCoroutine(Close());
    }
    
    IEnumerator Close()
    {
        yield return new WaitForSeconds(timeToClose);
        gameObject.SetActive(false);
    }
}
