using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentEnable : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera studentCamera;

    public MonoBehaviour[] studentScriptsToEnable;

    Transform[] studentSpawn;

    GameObject PlayerManagerObject;
    SpawnPlayer spawnComponent;

    
    public string playerManager;
    void Start()
    {
        if (playerManager == null)
        {
            Debug.Log("playerManager object name not set, set it to the name of the " +
                "object with the spawn player script");
            return;
        } 
        PlayerManagerObject = GameObject.Find(playerManager);
        if(PlayerManagerObject == null)
        {
            Debug.Log("Could not find PlayerManager object, make sure the playerManager field " +
                "is set to name of the object");
            return;
        }
        spawnComponent = PlayerManagerObject.GetComponent<SpawnPlayer>();
        if(PlayerManagerObject == null)
        {
            Debug.Log("PlayerManager is missing the PlayerSpawn script");
            return;
        }
        studentSpawn = spawnComponent.studentSpawn;
        int tempID = ASL.GameLiftManager.GetInstance().m_PeerId;
        if(tempID == 1)
        {
            return;
        }
        else
        {

            if(tempID - 2 > studentSpawn.Length -1)
            {
                Debug.Log("Student attempted to spawn without a valid spawn location");
                return;
            }
            else if(transform.position == studentSpawn[tempID - 2].position)
            {
                if(studentCamera == null)
                {
                    Debug.Log("No camera attatched to student");
                }
                else{
                    studentCamera.enabled = true;
                }
                
                for(int i = 0; i < studentScriptsToEnable.Length; i++)
                {
                    studentScriptsToEnable[i].enabled = true;
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
