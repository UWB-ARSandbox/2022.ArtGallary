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

    Transform lastTran;
    bool clicked = false;

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
        if (lockAtCanvas == false && !clicked)
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

        // Call for moving/zooming into the canvas
        if (Input.GetMouseButtonDown(0) && clicked == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layerMask = LayMaskForRay();

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask) && hit.transform.gameObject.
                name.Contains("StuCanvas"))
            {
                lastTran = transform;
                transform.GetChild(1).position = hit.transform.position;
                transform.GetChild(1).eulerAngles = new Vector3(0, hit.transform.eulerAngles.y + 180, 0);

                transform.GetChild(1).GetComponent<FirstPersonCamera>().SetCursorLock(true);
                transform.GetChild(1).GetComponent<FirstPersonCamera>().SetIsLocked(true);

                DoNotRenderPlayer();

                // Front wall
                if (hit.transform.eulerAngles.y == 180)
				{
                    transform.GetChild(1).position -= new Vector3(0, 0, 2);
                }
                // Right wall
                else if(hit.transform.eulerAngles.y == 270)
				{
                    transform.GetChild(1).position -= new Vector3(2, 0, 0);
                }
                // Back wall
                else if(hit.transform.eulerAngles.y == 0)
				{
                    transform.GetChild(1).position += new Vector3(0, 0, 2);
                }
                else
				{
                    transform.GetChild(1).position += new Vector3(2, 0, 0);
                }
                clicked = true;
            }
        }

        // Resetting camera back to the player
        else if(clicked && Input.anyKeyDown && !Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetMouseButtonDown(0))
		{
            clicked = false;
            transform.GetChild(1).GetComponent<FirstPersonCamera>().SetCursorLock(false);
            transform.GetChild(1).GetComponent<FirstPersonCamera>().SetIsLocked(false);
            
            DoRenderPlayer();

            transform.GetChild(1).position = lastTran.position;
            transform.GetChild(1).rotation = lastTran.rotation;
        }
    }

    int LayMaskForRay()
	{
        PaintOnCanvas gone = transform.parent.GetChild(1).GetComponent<PaintOnCanvas>();
        int layerMask = 1 << 30;
        if (gone.GetClickStatus())
        {
            // Set students to be raycasted through
            layerMask |= (1 << 9);
            // Set canvas to be raycasted through
            layerMask |= (1 << 10);
            // Set teacher canvas to be raycasted through
            layerMask |= (1 << 11);
        }
        layerMask = ~layerMask;

        return layerMask;
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

        // Render player models
        if(lockAtCanvas)
		{
            DoNotRenderPlayer();
		}
        else
		{
            DoRenderPlayer();
		}
    }

    public void DoNotRenderPlayer()
	{
        // Get this players camera
        Camera cam = transform.GetChild(1).GetComponent<Camera>();
        // Sets player to not render to camera
        cam.cullingMask &= ~(1 << 9);
    }

    public void DoRenderPlayer()
    {
        // Get this players camera
        Camera cam = transform.GetChild(1).GetComponent<Camera>();
        // Set player to render to camera
        cam.cullingMask |= (1 << 9);
    }

    public void SetPosition(Vector3 pos)
    {
        m_ASLObject.SendAndSetClaim(() =>
            {
                m_ASLObject.SendAndSetWorldPosition(pos);
            });
    }
}

