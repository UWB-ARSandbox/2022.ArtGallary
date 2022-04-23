using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using UnityEngine.UI;

public class Pavel_Player : MonoBehaviour
{
    [Tooltip("This determines the speed that the PlayerCube will move.")]
    public float MovementSpeed = 3f;
    bool lockAtCanvas = false;

    ASLObject m_ASLObject;
    GameLiftManager manager;
    Button LeaveButton;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameLiftManager").GetComponent<GameLiftManager>();
        if (manager.AmLowestPeer() == false)
		{
            LeaveButton = GameObject.Find("LeaveTheClassButton").GetComponent<Button>();
            LeaveButton.onClick.AddListener(LeaveClass);
        }

        m_ASLObject = gameObject.GetComponent<ASLObject>();
        Debug.Assert(m_ASLObject != null);
    }

    // Update is called once per frame
    void Update()
    {
        if (lockAtCanvas == false)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            m_ASLObject.SendAndSetClaim(() =>
            {
                Vector3 m_AdditiveMovementAmount = move * MovementSpeed * Time.deltaTime;
                m_AdditiveMovementAmount.y = 0;
                m_ASLObject.SendAndIncrementWorldPosition(m_AdditiveMovementAmount);
                //m_ASLObject.SendAndIncrementLocalPosition(m_AdditiveMovementAmount);
            });
        }
        /* if (Input.GetKey(KeyCode.W) ^ Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.W))
            {
                m_ASLObject.SendAndSetClaim(() =>
                {
                    Vector3 m_AdditiveMovementAmount = Vector3.forward * MovementSpeed * Time.deltaTime;
                    m_ASLObject.SendAndIncrementLocalPosition(m_AdditiveMovementAmount);
                });
            }
            else
            {
                m_ASLObject.SendAndSetClaim(() =>
                {
                    Vector3 m_AdditiveMovementAmount = Vector3.back * MovementSpeed * Time.deltaTime;
                    m_ASLObject.SendAndIncrementLocalPosition(m_AdditiveMovementAmount);
                });
            }
        }
        if (Input.GetKey(KeyCode.D) ^ Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.D))
            {
                m_ASLObject.SendAndSetClaim(() =>
                {
                    Vector3 m_AdditiveMovementAmount = Vector3.right * MovementSpeed * Time.deltaTime;
                    m_ASLObject.SendAndIncrementLocalPosition(m_AdditiveMovementAmount);
                });
            }
            else
            {
                m_ASLObject.SendAndSetClaim(() =>
                {
                    Vector3 m_AdditiveMovementAmount = Vector3.left * MovementSpeed * Time.deltaTime;
                    m_ASLObject.SendAndIncrementLocalPosition(m_AdditiveMovementAmount);
                });
            }
        } */
    }

    public void LeaveClass()
    {
        // Delete player model in space
        gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            GetComponent<ASL.ASLObject>().DeleteObject();
            manager.DisconnectFromServer();
            Application.Quit();
        });
    }

    public void SetLockAtCanvas(bool locked)
    {
        lockAtCanvas = locked;
    }
}

