using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StudentPanel : MonoBehaviour
{
    private TMP_Text tmp;
    private bool submitted{get; set;}


    // Start is called before the first frame update
    void Start()
    {
        submitted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Changes name displayed by the student panel. Should only be used at initialization of the UI.
    public void ChangeName(string newName)
    {
        tmp.text = newName;
    }

    // Initialize is called to initialize the component, as Start is called in a random order, resulting in a nullreferenceexception.
    public void Initialize()
    {
        tmp = this.GetComponentInChildren<TMP_Text>();
        Debug.Log("Current Text: " + tmp.text);
    }

    // Placeholder function to toggle the "request for help" functionality of the student name.
    public void HelpToggle()
    {

    }
}
