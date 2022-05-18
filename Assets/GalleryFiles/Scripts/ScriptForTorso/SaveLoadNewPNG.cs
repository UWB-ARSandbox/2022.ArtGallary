using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using SimpleFileBrowser;

public class SaveLoadNewPNG : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		FileBrowser.SetDefaultFilter(".png");
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	public void LoadNewPNG(Texture2D texture)
	{
		StartCoroutine(LoadWindow(texture));
	}

	IEnumerator LoadWindow(Texture2D texture)
	{
		yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true,
			null, null, "Load Files and Folders", "Load");

		if (FileBrowser.Success && FileBrowser.Result.Length == 1)
		{
			Texture2D text2D = (Texture2D)texture;
			Debug.Log(FileBrowser.Result[0]);
			text2D.LoadImage(System.IO.File.ReadAllBytes(FileBrowser.Result[0]));
			text2D.Apply();
			GetComponent<PaintOnCanvas>().LoadCompleteCanvas(texture);
			yield return FileBrowser.Result[0];
		}
	}

	public string SaveNewPNG(Texture2D texture)
	{
		return "";
	}
}
