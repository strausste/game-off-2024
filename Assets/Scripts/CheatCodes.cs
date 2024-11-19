using System;
using UnityEngine;
using UnityEngine.Events;

public class CheatCodes : MonoBehaviour
{
    private string input;
    [SerializeField] private int maxInputLength = 30; 
    
    public bool onePunch = false;   //one shot
    public bool phantomPain = false;    //gain access to symbols meanings
    public bool dollarDollarBillYall = false; //Gain excessive amount of money
    public bool iAmSpeed = false; //Increase max speed
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
        if (!onePunch && input.Contains("onepunch"))
        {
            onePunch = true;
            Debug.Log("OnePunchhhhhhhhhhhhhhhhhhh");
            activatedCheat.Invoke();
        }
        else if (!phantomPain && input.Contains("phantompain"))
        {
            phantomPain = true;
            Debug.Log("Why are we still here, just to suffer?");
            activatedCheat.Invoke();
        }
        else if (!dollarDollarBillYall && input.Contains("dollardollarbillyall"))
        {
            dollarDollarBillYall = true;
            Debug.Log("Dollar Dollar Bill Yall");
            activatedCheat.Invoke();
        }
        else if (!iAmSpeed && input.Contains("kachow"))
        {
            iAmSpeed = true;
            Debug.Log("I am speed");
            activatedCheat.Invoke();
        }
    }
}
