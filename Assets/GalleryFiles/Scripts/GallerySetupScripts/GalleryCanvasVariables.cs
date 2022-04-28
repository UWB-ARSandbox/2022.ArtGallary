using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryCanvasVariables : MonoBehaviour
{
	int votes = 0;
	bool voted = false;
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

		GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(retrieveVote);
	}

	public void ChangeVoteStatus()
	{
		voted = true;
		float[] tempVote = new float[1];

		//GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(retrieveVote);
		if(transform.GetChild(0).GetComponent<TextMesh>().color == Color.white)
		{
			transform.GetChild(0).GetComponent<TextMesh>().color = Color.green;
			tempVote[0] = 1;
		}
		else
		{
			transform.GetChild(0).GetComponent<TextMesh>().color = Color.white;
			tempVote[0] = -1;
		}

		GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
		{
			GetComponent<ASL.ASLObject>().SendFloatArray(tempVote);
		});
	}

	public void retrieveVote(string id, float[] vote)
	{
		votes += (int)vote[0];
		transform.GetChild(0).GetComponent<TextMesh>().text = 
			"Votes: " +  votes;
	}
}
