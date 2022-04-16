using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryCanvasVariables : MonoBehaviour
{
	int votes = 0;
	bool voted = false;
	bool claimed = false;
	string studentName = "Bob";
	ASL.GameLiftManager manager;
	// Start is called before the first frame update
	void Start()
	{
		transform.GetChild(1).GetComponent<TextMesh>().text = studentName;
		manager = GameObject.Find("GameLiftManager").GetComponent<ASL.GameLiftManager>();
		int host = manager.GetLowestPeerId();
		if(host != manager.m_PeerId)
		{
			transform.GetChild(1).gameObject.SetActive(false);
		}
		else
		{
			transform.GetChild(1).gameObject.SetActive(true);
		}
	}

	// Update is called once per frame
	void Update()
	{
		/*
		GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
		{
			GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(retrieveVote);
			//Debug.Log(votes);
		});
		*/
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit raycastHit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out raycastHit) == true
			&& voted == false && raycastHit.transform.parent != null)
			{
				if (raycastHit.transform.parent.name.Equals(transform.name) &&
				raycastHit.transform.parent.TryGetComponent(out GalleryCanvasVariables a) == true)
				{
					Debug.Log(a.studentName);
					Debug.Log("Clicked once");
					voted = true;
					float[] tempVote = new float[1];
					tempVote[0] = 1;
					//GetComponent<ASL.ASLObject>().SendFloatArray(tempVote);
					transform.GetChild(0).GetComponent<TextMesh>().color = Color.green;
					claimed = false;
				}
			}
			Debug.Log("votes " + votes);
		}
	}

	public void retrieveVote(string id, float[] vote)
	{
		votes += (int)vote[0];
		claimed = true;
	}
}
