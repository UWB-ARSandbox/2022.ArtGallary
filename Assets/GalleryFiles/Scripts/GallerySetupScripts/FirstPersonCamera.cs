using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class FirstPersonCamera : MonoBehaviour
{
    

    public float mouseSensitivity = 100f;
    public Transform playerBody;

    public float xRotation = 0f;

    public float yRotation = 0f;
    ASLObject m_ASLObject;
    
    public GameObject crosshair;
    
    bool isLocked, isActive;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isLocked = true;
        isActive = false;
        crosshair = GameObject.Find("Crosshair");
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive && isLocked)
        {
            // Code for first person camera movement taken from Brackeys. https://www.youtube.com/watch?v=_QajrabyTJc
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            yRotation += mouseX;
            
            m_ASLObject.SendAndSetClaim(() =>
                    {
                        //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                        //playerBody.Rotate(Vector3.up * mouseX);
                        //m_ASLObject.SendAndIncrementLocalRotation(Quaternion.Euler(0f, mouseX, 0f));
                        m_ASLObject.SendAndSetLocalRotation(Quaternion.Euler(xRotation, yRotation, 0f));
                        //Debug.Log("Sent Local Rotation");
                    });

        }
        
        
        // Toggle Cursor Lock
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isLocked = !isLocked;
            SetCursorLock(isLocked);
            crosshair.SetActive(isLocked);
        }
       
    }

    public void ReinitializeParent(GameObject parent)
    {
        m_ASLObject = parent.GetComponent<ASLObject>();
        this.gameObject.transform.position = parent.transform.position;
        this.gameObject.transform.rotation = parent.transform.rotation;
        playerBody = parent.transform;
        isActive = true;
    }

    public void SetCursorLock(bool isLocked)
    {
        if(isLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void SetIsLocked(bool locked)
	{
        isLocked = locked;
	}
}
