using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ASL;

public class ViewStudent : MonoBehaviour
{
    [SerializeField]
    Button View;
    [SerializeField]
    GameObject target, myUser;

    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        View = this.gameObject.GetComponent<Button>();
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToCanvas()
    {
        StudentPanel panel = this.transform.parent.gameObject.GetComponent<StudentPanel>();
        target = panel.GetPlayer();
        
        
        myUser = cam.transform.parent.gameObject;
        ASLObject m_ASLObject = myUser.GetComponent<ASLObject>();

        Debug.Assert(m_ASLObject != null);
        if(m_ASLObject != null)
        {
            m_ASLObject.SendAndSetClaim(() =>
            {
                m_ASLObject.SendAndSetWorldPosition(target.transform.position);
                m_ASLObject.SendAndSetWorldRotation(Quaternion.Euler(new Vector3(0, 0, 0)));
            });
        }
    }

    public void ReturnToMyCanvas()
    {
        myUser = cam.transform.parent.gameObject;
        ASLObject m_ASLObject = myUser.GetComponent<ASLObject>();

        Debug.Assert(m_ASLObject != null);
        if(m_ASLObject != null)
        {
            m_ASLObject.SendAndSetClaim(() =>
            {
                m_ASLObject.SendAndSetLocalPosition(new Vector3(0,0,0));
                m_ASLObject.SendAndSetWorldRotation(Quaternion.Euler(new Vector3(0, 0, 0)));
            });
        }
    }
}
