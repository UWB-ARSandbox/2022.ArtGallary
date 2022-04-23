using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class StudentNames : MonoBehaviour
{
    float[] idName;
    int myID = -1;
    GameLiftManager manager;
    GameObject[] textName;
    bool allSet = false;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameLiftManager").GetComponent<GameLiftManager>();
        idName = new float[manager.m_Players.Count];

        textName = new GameObject[manager.m_Players.Count];
        textName[0] = new GameObject();

        GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(SetName);
    }

    // Update is called once per frame
    void Update()
    {
        // Update the id array if number was not pushed
        if(myID != -1 && myID != idName[myID - 1])
		{
            SendName(myID);
		}
        if(allSet == false)
		{
            textName = GameObject.FindGameObjectsWithTag("StuNames");
            ChangePlayerNames(textName);
            textName = new GameObject[0];
        }
    }

    public void RecievedName(int id)
	{
        myID = id;
        SendName(id);
	}

    void SendName(int id)
	{
        gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            float[] temp = new float[manager.m_Players.Count];
            temp[id - 1] = id;
            GetComponent<ASLObject>().SendFloatArray(temp);
        });
    }

    void ChangePlayerNames(GameObject[] textMeshes)
    {
        for(int i = 0; i < textMeshes.Length; i++)
		{
            // This is the teacher
            if((int)idName[i] != 0 && i + 1 >= textMeshes.Length)
			{
                textMeshes[i].GetComponent<TextMesh>().text = manager.m_Players[(int)idName[0]];
                allSet = true;
            }
            else if((int)idName[i] != 0)
			{
                textMeshes[i].GetComponent<TextMesh>().text = manager.m_Players[(int)idName[i + 1]];
                allSet = true;
            }
			else
			{
                allSet = false;
                break;
			}
        }
    }

    void SetName(string _id, float[] _f)
	{
        // Change order
        for(int i = 0; i < _f.Length; i++)
		{
            if(_f[i] != 0)
			{
                idName[i] = _f[i];
            }
		}
	}
}
