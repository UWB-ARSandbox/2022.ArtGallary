using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class ClassScroll : MonoBehaviour
{
    static GameLiftManager manager;
    public int host;
    static GameObject content;
    Canvas myCanvas;

    int totalUsers = 0;

    static int iIndexer = 1;

    // Start is called before the first frame update
    void Start()
    {
        content = GameObject.Find("Students");

        manager = GameObject.Find("GameLiftManager").GetComponent<GameLiftManager>();
        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        host = manager.GetLowestPeerId();
        totalUsers = manager.m_Players.Count;

        if(manager.m_PeerId == host){
            this.gameObject.SetActive(true);
            foreach(var item in manager.m_Players)
            {
                int peerId = item.Key;
                string username = item.Value;

                if (peerId != host)
                {
                    ASL.ASLHelper.InstantiateASLObject("StudentPanel",
                        new Vector3(0, 0, 0), Quaternion.identity, "", "", RecievedGameObj);
                }
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    static void RecievedGameObj(GameObject gameObj)
	{
        gameObj.transform.SetParent(content.transform, false);
        gameObj.transform.position = content.transform.position;
        gameObj.transform.rotation = content.transform.rotation;

        StudentPanel panel = gameObj.GetComponent<StudentPanel>();

        int i = 0;
        foreach (var item in manager.m_Players)
		{
            if(iIndexer == i)
			{
                int peerId = item.Key;
                string username = item.Value;

                iIndexer += 1;

                panel.Initialize();

                panel.ChangeName(username);
                break;
            }
            i += 1;
		}

        
    }

    // Update is called once per frame
    void Update()
    {
        int nowPlayers = manager.m_Players.Count;
        if(nowPlayers < totalUsers)
		{
            foreach(var panels in ASLHelper.m_ASLObjects)
			{
                // Delete all panels
                if(panels.Value.gameObject.name.Contains("Panel"))
				{
                    panels.Value.SendAndSetClaim(() =>
					{
                        panels.Value.DeleteObject();
                    });
                }
			}
            // Reconstruct
            Start();
		}
    }

    /*
     * Gary's old code:
     * GameObject student = Instantiate(Resources.Load<GameObject>("MyPrefabs/StudentPanel"),
                        content.transform.position, content.transform.rotation, content.transform);
                    StudentPanel panel = student.GetComponent<StudentPanel>();
                    panel.Initialize();

                    panel.ChangeName(username);
                    panel.ChangeUserID(peerId);
     */
}