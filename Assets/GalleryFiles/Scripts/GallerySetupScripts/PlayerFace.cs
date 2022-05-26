using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;

public class PlayerFace : MonoBehaviour
{
    LoadMenu menu;
    Button faceButton;
    bool done;

    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.Find("UI").GetComponent<LoadMenu>();
        
        faceButton = menu.FaceButton.GetComponent<Button>();
        faceButton.onClick.AddListener(ChangeFace);

        done = true;

        FileBrowser.SetDefaultFilter(".png");
    }

    void ChangeFace()
	{
        Debug.Log("Here");
        if (done)
        {
            done = false;
            StartCoroutine(LoadWindow());
            Debug.Log("Loading... Done = " + done);
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
        // Allow user to open a file explorer without loading a null image
        else
        {
            done = true;
		}
    }

    public static void changeTexture(GameObject gameObject, Texture2D tex)
    {
        gameObject.GetComponent<Renderer>().materials[1].mainTexture = tex;
    }
}
