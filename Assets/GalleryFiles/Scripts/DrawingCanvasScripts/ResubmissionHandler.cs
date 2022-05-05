using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class ResubmissionHandler : MonoBehaviour
{

	bool clicked = true;
    // Start is called before the first frame update
    void Start()
    {
		GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(ResetSubmission);
	}

	public void AllCanSubmit(float sub)
	{

		gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
		{
			float[] f = new float[1];
			f[0] = sub;
			GetComponent<ASLObject>().SendFloatArray(f);
		});
	}

	public bool SubmissionStatus()
	{
		return clicked;
	}

	public void ResetSubmission(string _id, float[] _f)
	{
		if (_f[0] != 0)
		{
			clicked = false;
		}
		else
		{
			clicked = true;
		}
	}
}
