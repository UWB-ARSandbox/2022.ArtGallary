using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class Pavel_Player : MonoBehaviour
{
    [Tooltip("This determines the speed that the PlayerCube will move.")]
    public float MovementSpeed = 3f;

    ASLObject m_ASLObject;

    // Start is called before the first frame update
    void Start()
    {
        m_ASLObject = gameObject.GetComponent<ASLObject>();
        Debug.Assert(m_ASLObject != null);
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        m_ASLObject.SendAndSetClaim(() =>
        {
            Vector3 m_AdditiveMovementAmount = move * MovementSpeed * Time.deltaTime;
            m_ASLObject.SendAndIncrementLocalPosition(m_AdditiveMovementAmount);
        });
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
}

