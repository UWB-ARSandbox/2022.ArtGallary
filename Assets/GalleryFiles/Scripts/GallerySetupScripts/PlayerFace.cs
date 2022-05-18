using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;

public class PlayerFace : MonoBehaviour
{
    Button faceButton;
    bool done = true;

    // Start is called before the first frame update
    void Start()
    {
        faceButton = GameObject.Find("FaceButton").GetComponent<Button>();
        faceButton.onClick.AddListener(ChangeFace);

        FileBrowser.SetDefaultFilter(".png");
    }

    void ChangeFace()
	{
        Debug.Log("Here");
        if (done)
        {
            done = false;
            StartCoroutine(LoadWindow());
        }
	}

    IEnumerator LoadWindow()
	{
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true,
            null, null, "Load Files and Folders", "Load");

        if (FileBrowser.Success && FileBrowser.Result.Length == 1)
		{
            Texture2D text2D = new Texture2D(100,100);
            text2D.LoadImage(System.IO.File.ReadAllBytes(FileBrowser.Result[0]));
            text2D.Apply();

            GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                GetComponent<ASL.ASLObject>().SendAndSetTexture2D(text2D,
                    changeTexture, true);
            });
            done = true;
            yield return FileBrowser.Result[0];
        }
    }

    public static void changeTexture(GameObject gameObject, Texture2D tex)
    {
        gameObject.GetComponent<Renderer>().material.mainTexture = tex;
    }
}
