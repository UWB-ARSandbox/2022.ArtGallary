using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class SpawnPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public int playerID;

    public string teacherPrefab = "Teacher";

    public string studentPrefab = "Student";

    public Transform teacherSpawn;
    public Transform[] studentSpawn;
    


    void Start()
    {
        playerID = ASL.GameLiftManager.GetInstance().m_PeerId;
        if(playerID == 1)
        {
            ASLHelper.InstantiateASLObject(teacherPrefab, teacherSpawn.position, teacherSpawn.rotation);
        }
        else
        {
            ASLHelper.InstantiateASLObject(studentPrefab, studentSpawn[playerID - 2].position, studentSpawn[playerID - 2].rotation);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
