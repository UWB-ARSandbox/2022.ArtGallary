using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class StudentEnable : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera studentCamera;

    Transform[] studentSpawn;

    GameObject PlayerManagerObject;
    SpawnPlayer spawnComponent;
    GameLiftManager manager;


    
    public string playerManager;
    void Start()
    {
        manager = GameObject.Find("GameLiftManager").GetComponent<GameLiftManager>();

        if (playerManager == null)
        {
            Debug.Log("playerManager object name not set, set it to the name of the object with the spawn player script");
            return;
        } 
        PlayerManagerObject = GameObject.Find("PlayerManager");
        if(PlayerManagerObject == null)
        {
            Debug.Log("Could not find PlayerManager object, make sure the playerManager field is set to name of the object");
            return;
        }
        spawnComponent = PlayerManagerObject.GetComponent<SpawnPlayer>();
        if(PlayerManagerObject == null)
        {
            Debug.Log("PlayerManager is missing the PlayerSpawn script");
            return;
        }
        studentSpawn = spawnComponent.studentSpawn;
        int tempID = manager.m_PeerId;

        if(tempID == manager.GetLowestPeerId())
        {
            return;
        }
        else
        {

            if(tempID > studentSpawn.Length + 1)
            {
                Debug.Log("Student attempted to spawn without a valid spawn location");
                return;
            }

            else if(transform.position == studentSpawn[tempID - 2].position)
            {
                studentCamera = Camera.main;
                studentCamera.transform.SetParent(this.gameObject.transform);
        
                if(studentCamera == null)
                {
                    Debug.Log("No camera attached to student");
                }
                else{
                    studentCamera.GetComponent<FirstPersonCamera>().ReinitializeParent(this.gameObject);
                }
                
                this.gameObject.GetComponent<Pavel_Player>().enabled = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
