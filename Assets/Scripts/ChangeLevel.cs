using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    //set the level to be loaded
    [SerializeField]
    private int _levelToLoad;
    [SerializeField]
    private Animator _transitionAnim;

    private void Start()
    {
      _transitionAnim = GameObject.Find("TransitionImage").GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
                _transitionAnim.SetTrigger("fadeIn");
        }
    }

    public void LoadNextScene()
    {

        SceneManager.LoadScene(_levelToLoad);
    }
}
