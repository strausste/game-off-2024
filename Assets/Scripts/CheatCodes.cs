using System;
using UnityEngine;

public class CheatCodes : MonoBehaviour
{
    private string input;
    [SerializeField] private int maxInputLength = 30; 
    
    public bool onePunch = false;   //one shot
    public bool phantomPain = false;    //gain access to symbols meanings
    //other cheat codes bools here
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

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
        }
        else if (!phantomPain && input.Contains("phantompain"))
        {
            phantomPain = true;
        }
    }
    
    
}
