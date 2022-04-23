using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class NameControls : MonoBehaviour
{
    private Transform camera;
    GameLiftManager manager;
    
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        manager = GameObject.Find("GameLiftManager").GetComponent<GameLiftManager>();

        if(this.transform.parent.parent.gameObject.name == ("Teacher2(Clone)"))
        {
            this.gameObject.SetActive(false);
        }
        else if(manager.m_PeerId == this.transform.parent.parent.gameObject.GetComponent<StudentEnable>().studentID)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation((transform.position - camera.position).normalized);
    }
}
