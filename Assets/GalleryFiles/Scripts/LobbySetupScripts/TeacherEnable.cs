using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherEnable : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public Camera teacherCamera;

    public MonoBehaviour[] teacherScriptsToEnable;

    Transform teacherSpawn;

    GameObject PlayerManagerObject;
    SpawnPlayer spawnComponent;

    
    public string playerManager;
    void Start()
    {
        if (playerManager == null)
        {
            Debug.Log("playerManager object name not set, set it to the name of the object with the spawn player script");
            return;
        } 

        PlayerManagerObject = GameObject.Find(playerManager);
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

        teacherSpawn = spawnComponent.teacherSpawn;
        int tempID = ASL.GameLiftManager.GetInstance().m_PeerId;
        if(tempID == 1)
        {
            if(teacherCamera == null)
            {
                Debug.Log("No camera attatched tp teacher");
            }
            else{
                teacherCamera.enabled = true;
            }
            
                
            for(int i = 0; i < teacherScriptsToEnable.Length; i++)
                {
                    teacherScriptsToEnable[i].enabled = true;
                }
                
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
