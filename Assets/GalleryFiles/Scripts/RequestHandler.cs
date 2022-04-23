using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class RequestHandler : MonoBehaviour
{
    static GameLiftManager manager;
    ASLObject m_ASLObject;
    [SerializeField]
    static int host, peerid, request;
    
    [SerializeField]
    List<GameObject> StudentPanels;
    
    [SerializeField]
    GameObject Requestor, Canvas;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameLiftManager").GetComponent<GameLiftManager>();
        host = manager.GetLowestPeerId();
        m_ASLObject = gameObject.GetComponent<ASLObject>();
        Debug.Assert(m_ASLObject != null);

        m_ASLObject._LocallySetFloatCallback(ReceiveHelpRequest);
    }

    // Update is called once per frame
    void Update()
    {
        if(Requestor == null && Camera.main.transform.parent.parent != null)
        {
            Requestor = Camera.main.transform.parent.parent.gameObject;
            Canvas = Requestor.transform.GetChild(1).gameObject;
        }
    }

    public void Initialize()
    {
        StudentPanels.Clear();
        GameObject[] Panels = GameObject.FindGameObjectsWithTag("StuPanel");
        foreach(GameObject p in Panels)
        {
            StudentPanels.Add(p);
        }
    }

    public void SendHelpRequest()
    {
        peerid = manager.m_PeerId;
        float[] request = {peerid};

        ASLObject canASL = Canvas.GetComponent<ASLObject>();
        Texture2D text = (Texture2D)Canvas.GetComponent<Renderer>().material.mainTexture;
        canASL.SendAndSetClaim(() =>
        {
            canASL.GetComponent<ASLObject>().SendAndSetTexture2D(text, changeTexture);
        });


        m_ASLObject.SendAndSetClaim(() =>
        {
            m_ASLObject.SendFloatArray(request);
        }, 0);
    }

    public void ReceiveHelpRequest(string _id, float[] _myFloats)
    {
        ASLHelper.m_ASLObjects.TryGetValue(_id, out ASLObject myObject);
        peerid = (int)_myFloats[0];
        foreach(GameObject panel in StudentPanels)
        {
            if(panel != null)
            {
                StudentPanel p = panel.GetComponent<StudentPanel>();
                p.HelpToggle(peerid);
                Debug.Log("Searched " + peerid);
            }
            
        }
    }

    public static void changeTexture(GameObject gameObject, Texture2D tex)
	{
        if(host == manager.m_PeerId)
		{
            gameObject.GetComponent<Renderer>().material.mainTexture = tex;
        }
    }

}
