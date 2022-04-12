using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class ClassScroll : MonoBehaviour
{
    GameLiftManager manager;
    public int host;
    GameObject content;
    Canvas myCanvas;

    // Start is called before the first frame update
    void Start()
    {
        content = GameObject.Find("Students");

        manager = GameObject.Find("GameLiftManager").GetComponent<GameLiftManager>();
        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        host = manager.GetLowestPeerId();
        Debug.Log(manager.m_PeerId);

        if(manager.m_PeerId == host){
            Debug.Log("This instance is the host.");
            this.gameObject.SetActive(true);
            Debug.Log(manager.m_Players.Count);
            foreach(var item in manager.m_Players)
            {
                int peerId = item.Key;
                string username = item.Value;

                if (peerId != host)
                {
                    GameObject student = Instantiate(Resources.Load<GameObject>("MyPrefabs/StudentPanel"), 
                        content.transform.position, content.transform.rotation, content.transform);
                    StudentPanel panel = student.GetComponent<StudentPanel>();
                    panel.Initialize();
                    panel.ChangeName(username);
                }
            }
        }
        else{
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
