using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallTeleportScript : MonoBehaviour
{
    public float _minAltitude = -100;

    private Rigidbody _rigidBody;
    private void Start() {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.y < _minAltitude)
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
