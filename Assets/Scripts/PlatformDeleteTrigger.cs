using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDeleteTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _TrackedObject;

    public float DistanceFromTracked = 20;

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position 
        = (_TrackedObject.transform.position) - (Vector3.forward * DistanceFromTracked);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Platform")
            other.gameObject.SetActive(false);
    }
}
