using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinText : MonoBehaviour
{
    [SerializeField] 
    private GameObject _winText;

    private void Start()
    {
        _winText = GameObject.Find("Win text");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0.01f;
            Cursor.visible = true;
            _winText.GetComponent<TMP_Text>().text = "Congratulations, you restored the quantum state of the universe!";
            _winText.transform.GetChild(0).gameObject.SetActive(true);
        }
           
    }

}
