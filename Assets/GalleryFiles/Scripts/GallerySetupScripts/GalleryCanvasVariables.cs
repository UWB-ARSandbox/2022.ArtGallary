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

	public void ChangeName(string name)
	{
		// One is addded onto the end of array so that vote is not triggered
		// for players who have one char names
		float[] fName = new float[name.Length + 1];
		for(int i = 0; i < name.Length; i++)
		{
			fName[i] = (float)name[i];
		}

		GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
		{
			GetComponent<ASL.ASLObject>().SendFloatArray(fName);
		});
	}

	public void retrieveVote(string id, float[] vote)
	{
		// Votes will always be a length of one
		if(vote.Length == 1)
		{
			votes += (int)vote[0];
			transform.GetChild(0).GetComponent<TextMesh>().text =
				"Votes: " + votes;
		}
		// This is for changing the name on the canvas
		else
		{
			// Null the placeholder name
			studentName = "";
			for(int i = 0; i < vote.Length - 1; i++)
			{
				studentName += (char)vote[i];
			}
			// Only teacher can see the names
			if(manager.AmLowestPeer())
			{
				transform.GetChild(1).GetComponent<TextMesh>().text = studentName;
				transform.GetChild(1).GetComponent<TextMesh>().color = Color.white;
			}
		}
	}
}
