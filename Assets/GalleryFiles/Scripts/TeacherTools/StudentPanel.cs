using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StudentPanel : MonoBehaviour
{
    private TMP_Text studentName, help;
    
    [SerializeField]
    private int ID;
    
    private GameObject viewButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Changes name displayed by the student panel. Should only be used at initialization of the UI.
    public void ChangeName(string newName)
    {
        studentName.text = newName;
    }

    public string GetName() { return studentName.text; }

    // Initialize is called to initialize the component, as Start is called in a random order, resulting in a nullreferenceexception.
    public void Initialize(int peerId)
    {
        studentName = this.transform.GetChild(0).GetComponent<TMP_Text>();
        help = this.transform.GetChild(1).GetComponent<TMP_Text>();
        viewButton = this.transform.GetChild(2).gameObject;

        help.enabled = false;
        viewButton.SetActive(false);

        Debug.Log("Current Text: " + studentName.text);
        ID = peerId;
    }

    // Placeholder function to toggle the "request for help" functionality of the student name.
    public void HelpToggle(int peerId)
    {
        if(ID == peerId)
        {
            help.enabled = true;
            viewButton.SetActive(true);
            Debug.Log("Set: " + ID);
        }
    }
}
