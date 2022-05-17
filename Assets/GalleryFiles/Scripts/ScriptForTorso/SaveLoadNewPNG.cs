using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Windows;

public class SaveLoadNewPNG : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	public string LoadNewPNG(Texture2D texture)
	{
		string directory = EditorUtility.OpenFilePanel("File To Load", "", "png");
		if(directory != null)
		{
			Texture2D text2D = (Texture2D)texture;
			text2D.LoadImage(System.IO.File.ReadAllBytes(directory));
			text2D.Apply();
			return directory;
		}
		else
		{
			return null;
		}
	}
	public string SaveNewPNG(Texture2D texture)
	{
		string directory = EditorUtility.SaveFilePanel(texture.ToString(), "File To Save", "", "png");
		byte[] bytes = texture.EncodeToPNG();
		System.IO.File.WriteAllBytes(directory, bytes);
		return directory;
	}
}
