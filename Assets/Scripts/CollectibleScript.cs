using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    // Start is called before the first frame update

    private GenericBuff _buff = null;
    public void SetBuff(GenericBuff arg_buff) { _buff = arg_buff; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>()?.ApplyBuff(_buff);
            gameObject.SetActive(false);
        }
        
    }
}
