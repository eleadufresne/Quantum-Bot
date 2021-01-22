using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
public class CollectibleSpawner : MonoBehaviour
{
    private GenericBuff[] _BuffList = {
         // Speed buff
         new GenericBuff(10, "Gotta go fast", "End Speed",
         (PlayerController p) => { p._playerSpeed *= (2f); }, 
         (PlayerController p) => { p._playerSpeed *= (1 / 2f); }),
         
         // Jump buff
         new GenericBuff(10, "Haha gravity go brrrr", "End Jump",
         (PlayerController p) => { p._jumpHeight *= (2f); }, 
         (PlayerController p) => { p._jumpHeight *= (1 / 2f); }),

         // Control invert
         new GenericBuff(10, "slortnoC detrevnI", "End Invert Control",
         (PlayerController p) => { p.controlModifier *= -1; },
         (PlayerController p) => { p.controlModifier *= -1; }),
     
         // Camera FOV Increase
         new GenericBuff(10, "This is fine :)", "End FOV inc",
         (PlayerController p) => {
             if (!p._hasFovChange)
             {
                p._hasFovChange = true;
                p.StartCoroutine(p.FovChange(40, 125));
             }
         },
         (PlayerController p) => {
             if (p._hasFovChange)
             {
                p.StartCoroutine(p.FovChange(125, 40));
                p._hasFovChange = false;
             }
         }),

         // Camera FOV Decrease
         new GenericBuff(10, "Zooooooom", "End FOV dec",
         (PlayerController p) => {
             if (!p._hasFovChange)
             {
                p._hasFovChange = true;
                p.StartCoroutine(p.FovChange(40, 20));
             }
         },
         (PlayerController p) => { 
             if (p._hasFovChange)
             {
                 p.StartCoroutine(p.FovChange(20, 40));
                 p._hasFovChange = false;
             }
         }),

         //SlowDown
         new GenericBuff(5, "SUPER HOT", "End Slowdown",
         (PlayerController p) => { Time.timeScale = 0.5f; },
         (PlayerController p) => { Time.timeScale = 1f; }),

         //Level Reload
       /*  new GenericBuff(0, "Level reload", "",
         (PlayerController p) => { SceneManager.LoadScene(SceneManager.GetActiveScene().name); },
         (PlayerController p) => {  }),*/

         //Increase the player size
         new GenericBuff(10, "Chonkers", "End increase size",
         (PlayerController p) => { p.transform.localScale *= 5; },
         (PlayerController p) => { p.transform.localScale /= 5; }),

         //Decrease the player size
         new GenericBuff(10, "Sm0l b0i", "End decrease size",
         (PlayerController p) => { p.transform.localScale *= 0.25f; },
         (PlayerController p) => { p.transform.localScale /= 0.25f; }),
    };
    //
    private GameObject[] _PrefabList;
    private int _smallestListSize = 0;
    // Start is called before the first frame update
    void Start()
    {
        _PrefabList = Resources.LoadAll<GameObject>("Collectibles");
        _smallestListSize = Mathf.Min(_BuffList.Length, _PrefabList.Length);
        ShuffleLists();
    }

    public void SpawnCollectible(Vector3 arg_position)
    {
        if (_smallestListSize == 0) return; //nothing to spawn!
        int randomCollectibleID = Random.Range(0, _smallestListSize);

        GameObject obj = Instantiate(_PrefabList[randomCollectibleID], arg_position, Quaternion.identity);
        obj.GetComponent<CollectibleScript>()?.SetBuff(_BuffList[randomCollectibleID]);
    }

    void ShuffleLists()
    {
        _BuffList = _BuffList.OrderBy(c => Random.Range(0f, 100f)).ToArray();
        _PrefabList = _PrefabList.OrderBy(c => Random.Range(0f, 100f)).ToArray();
    }
}
