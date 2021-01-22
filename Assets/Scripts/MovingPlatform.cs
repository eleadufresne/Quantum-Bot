using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 _maxHeight1;
    private Vector3 _maxHeight2;
    private bool _goingDown;
    [SerializeField]
    public float _platformSpeed = 0.05f;
    [SerializeField]
    public float _distance = 4;
    [SerializeField]
    [Header("If true, move up/down else move left/right")]
    public bool _direction;
    // Start is called before the first frame update
    void Start()
    {
        if (_direction)
        {
            _maxHeight1 = new Vector3(transform.position.x, transform.position.y + _distance, transform.position.z);
            _maxHeight2 = new Vector3(transform.position.x, transform.position.y - _distance, transform.position.z);
        }
        else
        {
            _maxHeight1 = new Vector3(transform.position.x + _distance, transform.position.y, transform.position.z);
            _maxHeight2 = new Vector3(transform.position.x - _distance, transform.position.y, transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == _maxHeight1)
        {
            _goingDown = true;
        }else if (transform.position == _maxHeight2)
        {
            _goingDown = false;
        }


        if (_goingDown)
        {
            transform.position = Vector3.MoveTowards(transform.position, _maxHeight2, Time.deltaTime * _platformSpeed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _maxHeight1, Time.deltaTime * _platformSpeed);
        }

    }



    private void OnCollisionEnter(Collision col)
    {
        Collider other = col.collider;
        if(other.tag == "Player")
        {
            other.transform.SetParent(gameObject.transform);
        }
    }

    private void OnCollisionExit(Collision col)
    {
        Collider other = col.collider;
        if (other.tag == "Player")
        {
            other.transform.SetParent(null);
        }
    }
}
