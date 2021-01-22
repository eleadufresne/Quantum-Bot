using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GenericBuff
{
    public delegate void BuffCallback(PlayerController arg_PC);
    
    private float _duration = 1;
    private string _endQuote = "Default BEGIN";
    private string _startQuote = "Default END";
    
    private BuffCallback _startFunc = null;
    private BuffCallback _endFunc = null;

    public GenericBuff(float duration, string startQuote, string endQuote, BuffCallback onStart, BuffCallback onEnd)
    {
        _duration = duration;
        _startQuote = startQuote;
        _endQuote = endQuote;
        _startFunc = onStart;
        _endFunc = onEnd;
    }

    public IEnumerator ActivateBuff(PlayerController arg_playerObject)
    {
        //where to push Quote?
       // Debug.Log(_startQuote);
        GameObject bText = GameObject.Find("BText");
        bText.GetComponent<TMP_Text>().text = _startQuote;
        bText.GetComponent<Animator>().SetTrigger("ShowTxt");
        //Apply buff
        _startFunc(arg_playerObject);
        if (_duration > 0)
        {
            //Wait
            yield return new WaitForSeconds(_duration);

            //where to push Quote?
          //  Debug.Log(_endQuote);

            //Undo buff
            _endFunc(arg_playerObject);
        }
        else
        {
            yield return null;
        }
    }
}
