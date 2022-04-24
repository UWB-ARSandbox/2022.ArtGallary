using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ASL;

public class KickStudents : MonoBehaviour
{
    Button KickButton;
    GameLiftManager manager;
    float isKicking = 0;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameLiftManager").GetComponent<GameLiftManager>();
        if (manager.AmLowestPeer() == true)
        {
            KickButton = GameObject.Find("DisbandClass").GetComponent<Button>();
            KickButton.onClick.AddListener(DisbandClass);
        }

        GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(StartKickingStudents);
    }

    // Update is called once per frame
    void Update()
    {
        // Student
        if(isKicking != 0 && manager.AmLowestPeer() == false)
		{
            LeaveClass();
		}
        // Teacher leaves last
        else if(isKicking != 0 && manager.AmLowestPeer() == true &&
            manager.m_Players.Count == 1)
		{
            LeaveClass();
		}
    }

    public void LeaveClass()
    {
        manager.DisconnectFromServer();
        Application.Quit();
    }

    void DisbandClass()
	{
        gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            Debug.Log("Called");
            float[] temp = new float[1];
            temp[0] = 1;
            GetComponent<ASLObject>().SendFloatArray(temp);
        });

    }

    public void StartKickingStudents(string _id, float[] _f)
	{
        Debug.Log("KickStud " + _f[0]);
        isKicking = _f[0];
	}
}
