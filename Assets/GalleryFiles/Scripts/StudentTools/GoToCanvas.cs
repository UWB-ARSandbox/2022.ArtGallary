using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToCanvas : MonoBehaviour
{
	GameObject crosshair;
	bool isLocked = false;
	// Start is called before the first frame update
	void Start()
	{
		crosshair = GameObject.Find("Crosshair");
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.LeftControl) == true)
		{
			if (isLocked == false)
			{
				isLocked = true;
				Cursor.lockState = CursorLockMode.Locked;
				transform.Find("Main Camera").GetComponent<FirstPersonCamera>().SetCursorLock(false);
				GameObject canvas = transform.parent.GetChild(1).gameObject;
				transform.LookAt(canvas.transform);
				transform.Find("Main Camera").GetComponent<FirstPersonCamera>().SetIsLocked(false);
				crosshair.SetActive(false);
				transform.position = canvas.transform.position;
				transform.position += (3f * canvas.transform.forward);
				GetComponent<Pavel_Player>().SetLockAtCanvas(true);
			}
			else
			{
				isLocked = false;
				Cursor.lockState = CursorLockMode.None;
				transform.Find("Main Camera").GetComponent<FirstPersonCamera>().SetCursorLock(true);
				transform.Find("Main Camera").GetComponent<FirstPersonCamera>().SetIsLocked(true);
				crosshair.SetActive(true);
				GetComponent<Pavel_Player>().SetLockAtCanvas(false);
			}
		}
	}
}
