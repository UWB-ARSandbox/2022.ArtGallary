using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
    GameObject saveButton, SaveConfirmMenu;

    public GameObject loadMenu, FaceButton;
    
    Button Save;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("q"))
        {
            //loadMenu.SetActive(!loadMenu.activeSelf);
        }
    }

    public void Initialize()
    {
        loadMenu = GameObject.Find("Load Menu");
        SaveConfirmMenu = GameObject.Find("SaveConfirmMenu");

        saveButton = GameObject.Find("SaveButton");

        FaceButton = GameObject.Find("FaceButton");
    }

    // Toggles object based on argument.
    public void ToggleActive(GameObject button)
    {
        button.SetActive(!button.activeSelf);

    }

}
