//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] float xAxisUpperBoundMultiplier = 2;
    [SerializeField] float yAxisUpperBoundMultiplier = 0.5f;
    [SerializeField] float zAxisUpperBoundMultiplier = 3;

    [SerializeField] float xAxisLowerBoundAdd;
    [SerializeField] float yAxisLowerBoundAdd;
    [SerializeField] float zAxisLowerBoundAdd;
    [SerializeField] int platformsToBeGenerated;


    //Upper contraints for Random number gen
    private float xAxisUpperBoundery;
    private float yAxisUpperBoundery;
    private float zAxisUpperBoundery;

    //Player's info
    private float playerHeight = 2;
    private float playerWidth = 1;

    //Platform info
    private float platformHeight;
    private float platformWidth;
    private float platformDepth;

    //Random number, each x amount of platform generated 


    //Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    [SerializeField] MeshRenderer platform;
    //Reference to the Prefab item
    [SerializeField] MeshRenderer item;
    
    private GameObject[] platforms;
    private GameObject[] floatingObjects;
    //[SerializeField] string platformName;
    [SerializeField] string transitionPortalName;

    [SerializeField] int movingPlatformPercentage;
    [SerializeField] float max_moveSpeed;
    [SerializeField] float min_moveSpeed;
    [SerializeField] float max_moveDist;
    [SerializeField] float min_moveDist;
    [SerializeField] int max_items;

    //Prefab already placed in the scene
    private Vector3 originalPrefabPosition;
    //New prefab's position which used og position 
    private Vector3 currPrefabPosition;
    //Original prefab -> 2nd Generated prefab -> Current prefab
    private Vector3 secGeneratedPosition;
    //Item position
    private Vector3 itemPosition;

    //Var for a array of platforms
    private int numberOfPlatform;
    [SerializeField] int multipleOf = 5;
    private int previousNumberOfPlatforms;

    private CollectibleSpawner _ColSpawner;
    private int myRand;
    // Start is called before the first frame update
    void Start()
    {



        platforms = Resources.LoadAll<GameObject>("Platforms");
        floatingObjects = Resources.LoadAll<GameObject>("FloatingObjects");

        myRand = Random.Range(0, 3);
        platformHeight = platforms[0].GetComponent<MeshRenderer>().bounds.size.y;
        platformWidth = platforms[myRand].GetComponent<MeshRenderer>().bounds.size.x;
        platformDepth = platforms[myRand].GetComponent<MeshRenderer>().bounds.size.z;
        originalPrefabPosition = new Vector3(platforms[0].transform.position.x, platforms[0].transform.position.y, platforms[0].transform.position.z);
        _ColSpawner = GetComponent<CollectibleSpawner>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (numberOfPlatform < platformsToBeGenerated)
        {
            if (numberOfPlatform < platformsToBeGenerated - 1)
            {

                currPrefabPosition = PlatformRandomPosition();
                GameObject platform = Instantiate(platforms[myRand], currPrefabPosition, Quaternion.identity).gameObject;
                myRand = Random.Range(0, 3);
                platformHeight = platforms[0].GetComponent<MeshRenderer>().bounds.size.y;
                platformWidth = platforms[myRand].GetComponent<MeshRenderer>().bounds.size.x;
                platformDepth = platforms[myRand].GetComponent<MeshRenderer>().bounds.size.z;
                

                if (Random.Range(0, 100) < movingPlatformPercentage)
                {
                    platform.AddComponent(typeof(MovingPlatform));
                    MovingPlatform movingPlatform = platform.GetComponent<MovingPlatform>();
                    movingPlatform._distance = Random.Range(min_moveDist, max_moveDist);
                    movingPlatform._platformSpeed = Random.Range(min_moveSpeed, max_moveSpeed);
                    movingPlatform._direction = Random.Range(0.0f, 1.0f) > 0.5f;

                }

                secGeneratedPosition = originalPrefabPosition;
                originalPrefabPosition = currPrefabPosition;
                // IF statements if true it will place a floating object in the vinicity of the current platform
                if (Random.Range(0, 2) == 1)
                {
                    Instantiate(floatingObjects[Random.Range(0, floatingObjects.Length-1)], new Vector3 (currPrefabPosition.x+3, currPrefabPosition.y+3, currPrefabPosition.z+3), Quaternion.identity);
                }

            }

            if (numberOfPlatform == platformsToBeGenerated -1)
            {
                Debug.Log("Portal has been generated");
                
                Instantiate(Resources.Load<MeshRenderer>(transitionPortalName), currPrefabPosition*1.04f, Quaternion.identity);
            }
            
            itemPosition = new Vector3(originalPrefabPosition.x,originalPrefabPosition.y+platformHeight+0.25f, originalPrefabPosition.z);
            
                // if numPlatform multiple of 5 spawn an item
            if (numberOfPlatform%multipleOf == 0 && numberOfPlatform != 1)
            {
                Debug.Log("Item has been generated");
                //Instantiate(item, itemPosition, Quaternion.identity);
                _ColSpawner?.SpawnCollectible(itemPosition);
               
            }
            
            numberOfPlatform ++;
        }
    }

    //Helper method that return a Vector3 for the position of the next platform to be generated
    private Vector3 PlatformRandomPosition()
    {
        Vector3 position = new Vector3(GetRandomXValue(), GetRandomYValue(), GetRandomZValue());

        return position;
    }

    private float GetRandomZValue()
    {
        float lowerBoundVal = platformDepth + playerWidth + originalPrefabPosition.z + zAxisLowerBoundAdd;

        return Random.Range(lowerBoundVal, lowerBoundVal * zAxisUpperBoundMultiplier);
    }

    private float GetRandomYValue()
    {
        float lowerBoundVal = platformHeight + playerHeight + originalPrefabPosition.y + yAxisLowerBoundAdd;

        return Random.Range(lowerBoundVal, lowerBoundVal * yAxisUpperBoundMultiplier);
    }

    private float GetRandomXValue()
    {
        float lowerBoundVal = platformWidth + playerWidth + originalPrefabPosition.x + xAxisLowerBoundAdd;
       
        return Random.Range(lowerBoundVal, lowerBoundVal * xAxisUpperBoundMultiplier);
    }

    //Helper method that's supposed to help with avoiding collision NOT IMPLEMENTED MAY CAUSE FORK BOMB
    private bool DoesItCollide(Vector3 myValue)
    {
        if (secGeneratedPosition.x <= myValue.x || secGeneratedPosition.y <= myValue.y || secGeneratedPosition.z <= myValue.z)
        {
            return true;
        }
        return false;
    }
    
    //X amount of platform generate an item in y = Random.Range (platform.y + 1, jumpMaxHeight); 
    private int randPlatformNum()
    {
        int item = Random.Range(numberOfPlatform, platformsToBeGenerated);
        return item;
    }

}
