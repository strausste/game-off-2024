using System;
using UnityEngine;
using UnityEngine.Events;

public class CheatCodes : MonoBehaviour
{
    private string input;
    [SerializeField] private int maxInputLength = 30; 
    
    public bool oneShot = false;   //one shot
    public bool unlockMeanings = false;    //gain access to symbols meanings
    public bool getMoney = false; //Gain excessive amount of money
    public bool speedIncrease = false; //Increase max speed
    public bool noDamage = false;
    //other cheat codes bools here
    
    public static UnityEvent activatedCheat = new UnityEvent();
    
    // Update is called once per frame
    void Update()
    {
        input += Input.inputString;

        if (input.Length > maxInputLength)
        {
            input = input.Substring(1, input.Length-1);
        }

        input = input.ToLower();    
        if (!oneShot && input.Contains("onepunch"))
        {
            oneShot = true;
            Debug.Log("OnePunchhhhhhhhhhhhhhhhhhh");
            activatedCheat.Invoke();
        }
        else if (!unlockMeanings && input.Contains("phantompain"))
        {
            unlockMeanings = true;
            Debug.Log("Why are we still here, just to suffer?");
            activatedCheat.Invoke();
        }
        else if (!getMoney && input.Contains("dollardollarbillyall"))
        {
            getMoney = true;
            Debug.Log("Dollar Dollar Bill Yall");
            activatedCheat.Invoke();
        }
        else if (!speedIncrease && input.Contains("kachow"))
        {
            speedIncrease = true;
            Debug.Log("I am speed");
            activatedCheat.Invoke();
        }
        else if (noDamage)
        {
            noDamage = true;
            Debug.Log("UNLIMITED POWAAAAAAAAAA");
            activatedCheat.Invoke();
        }
    }
}
