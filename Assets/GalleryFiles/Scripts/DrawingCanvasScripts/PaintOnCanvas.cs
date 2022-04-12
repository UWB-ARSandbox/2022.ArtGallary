/* Author: Tyler Miller
 * Date: 4/7/2022
 * Description: PaintOnCanvas allows user to create a blank canvas
 * then paint on it. When the user leaves the same canvas will be loaded.
 * The user is allowed to change the brush size and color.
*/

using System.Media;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PaintOnCanvas : MonoBehaviour
{
	//the canvas of the students
	Texture2D studentCanvas;
	
	//directory of where the exe is ran for windows and save place for canvas
	string dirPath;
	
	//how big is the size of the brush
	int brushSize;

	//Is the player erasing
	bool eraseMode;

	//Is the player typing things in
	bool textMode;

	//Has the player clicked save canvas button
	bool canSave;

	//Has the player clicked load canvas button
	bool canLoad;

	//string for typed text
	string textOnType;

	//what color the user wants to paint with
	Color brushColor;

	//alphabet of characters
	Texture2D alphabet;

	Vector2 pixelToDraw;

	// Start is called before the first frame update
	void Start()
	{
		brushSize = 10;
		eraseMode = false;
		textMode = false;
		canSave = false;
		canLoad = false;
		textOnType = "";
		brushColor = Color.black;
		pixelToDraw = new Vector2(0, 0);
		alphabet = new Texture2D(252, 14, TextureFormat.RGBA32, false);
		dirPath = Application.dataPath + "/GalleryFiles/CanvasAssets";
		alphabet.LoadImage(System.IO.File.ReadAllBytes(dirPath + "/alphabet.png"));
		GameObject brushColorUI = GameObject.Find("BrushColor");
		brushColorUI.GetComponent<Image>().color = brushColor;

		if (System.IO.Directory.Exists(dirPath) == false)
		{
			brushColorUI.GetComponent<Image>().color = Color.cyan;
		}

		//this covers RGBA format and is good for opacity changing if needed
		studentCanvas = new Texture2D(256, 512, TextureFormat.RGBA32, false);
		for(int x = 0; x < 256; x++)
		{
			for(int y = 0; y < 512; y++)
			{
				studentCanvas.SetPixel(x, y, Color.white);
			}
		}
		//allocate amount of bytes needed for the png image
		byte[] bytes = studentCanvas.EncodeToPNG();
		if (System.IO.Directory.Exists(dirPath))
		{
			Debug.Log(dirPath);
			if (System.IO.File.Exists(dirPath + "/BlankCanvas.png") == false)
			{
				System.IO.File.WriteAllBytes(dirPath + "/BlankCanvas.png", bytes);
				Debug.Log(bytes.Length / 1024 + "Kb and saved as: " + dirPath + "BlankCanvas.png");
			}
			else
			{
				Debug.Log("BlankCanvas.png already exist");
			}
			studentCanvas.LoadImage(System.IO.File.ReadAllBytes(dirPath + "/BlankCanvas.png"));
			gameObject.GetComponent<Renderer>().material.mainTexture = studentCanvas;
		}
		else
		{
			Debug.Log("Not sure how you got here without deleting the exe");
			Application.Quit();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButton(0) == true)
		{
			RaycastHit raycastHit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out raycastHit) == true
			&& raycastHit.transform.name == "Canvas")
			{
				Vector2 uv = new Vector2(raycastHit.point.x - (transform.position.x - (transform.localScale.x / 2)),
				(raycastHit.point.y - (transform.position.y - (transform.localScale.y / 2))) / 2);
				Vector2 pixelCoord = new Vector2((int)(uv.x * 256), (int)(uv.y * 512));
				dirPath = Application.dataPath;
				//draw area of brush size if greater than 1
				if (brushSize > 1)
				{
					if (textMode == false)
					{
						for (int x = (int)(pixelCoord.x - (brushSize / 2)); x < (int)(pixelCoord.x + (brushSize / 2)); x++)
						{
							if (x >= 256 || x < 0)
							{
								continue;
							}
							for (int y = (int)(pixelCoord.y - (brushSize / 2)); y < (int)(pixelCoord.y + (brushSize / 2)); y++)
							{
								if (y >= 512 || y < 0)
								{
									continue;
								}
								if (eraseMode == false)
								{
									studentCanvas.SetPixel(x, y, brushColor);
								}
								else
								{
									studentCanvas.SetPixel(x, y, Color.white);
								}
							}
						}
					}
					else
					{
						pixelToDraw = pixelCoord;
					}
				}
				else
				{
					if(textMode == false && eraseMode == false && textMode == false)
					{
						studentCanvas.SetPixel((int)pixelCoord.x, (int)pixelCoord.y, brushColor);
					}
					else if(eraseMode == true)
					{
						studentCanvas.SetPixel((int)pixelCoord.x, (int)pixelCoord.y, Color.white);
					}
				}
				studentCanvas.Apply();
			}
		}
		else if(Input.GetMouseButtonUp(0) == true)
		{
			if (textOnType.Equals("") == false && textMode == true)
			{
				Vector2 startChar = new Vector2(pixelToDraw.x, pixelToDraw.y);
				for (int i = 0; i < textOnType.Length; i++)
				{
					int spot = DetermineCharacter(textOnType[i]);
					if (spot != -1)
					{
						DrawCharacter(startChar, spot);
						startChar.x += 7;
					}
				}
			}
			else
			{
				GameObject.Find("TextPlaceholder").GetComponent<Text>().text = "empty response not allowed";
			}
		}
	}

	public void SaveOrLoadToPNG(string png)
	{
		if (canSave == true)
		{
			//dont add any directories or file extensions
			if(png.Contains("/") == false && png.EndsWith(".png") == false && png.Equals("alphabet") == false)
			{
				GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "Successfully saved";
				byte[] bytes = studentCanvas.EncodeToPNG();
				System.IO.File.WriteAllBytes(dirPath + "/" + png + ".png", bytes);
				//studentCanvas.LoadImage(System.IO.File.ReadAllBytes(dirPath + "/" + png + ".png"));
			}
			else if(png.Contains("/") == false && png.EndsWith(".png") == true && png.Equals("alphabet.png") == false)
			{
				GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "Successfully saved";
				byte[] bytes = studentCanvas.EncodeToPNG();
				System.IO.File.WriteAllBytes(dirPath + "/" + png, bytes);
				//studentCanvas.LoadImage(System.IO.File.ReadAllBytes(dirPath + "/" + png));
			}
			else
			{
				GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "Do not enter '/'";
			}
		}
		else
		{
			//dont add any directories or file extensions
			if (png.Contains("/") == false && png.EndsWith(".png") == false && png.Equals("alphabet") == false)
			{
				GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "File_Name";
				studentCanvas.LoadImage(System.IO.File.ReadAllBytes(dirPath + "/" + png + ".png"));
				studentCanvas.Apply();
			}
			else if(png.Contains("/") == false && png.EndsWith(".png") == true && png.Equals("alphabet.png") == false)
			{
				GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "File_Name";
				studentCanvas.LoadImage(System.IO.File.ReadAllBytes(dirPath + "/" + png));
				studentCanvas.Apply();
			}
			else
			{
				GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "Do not enter '/'";
			}
		}
	}

	//sets whether save field will display
	public void SetCanSave()
	{
		canSave = !canSave;
		canLoad = false;
		if (canSave == true)
		{
			GameObject.Find("SaveField").GetComponent<InputField>().interactable = true;
			GameObject.Find("TextInput").GetComponent<InputField>().interactable = false;
			GameObject.Find("TextToggle").GetComponent<Toggle>().isOn = false;
		}
		else
		{
			GameObject.Find("SaveField").GetComponent<InputField>().interactable = false;
			GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "File_Name";
		}
	}
	public void SetCanLoad()
	{
		canLoad = !canLoad;
		canSave = false;
		if (canLoad == true)
		{
			GameObject.Find("SaveField").GetComponent<InputField>().interactable = true;
			GameObject.Find("TextInput").GetComponent<InputField>().interactable = false;
			GameObject.Find("TextToggle").GetComponent<Toggle>().isOn = false;
		}
		else
		{
			GameObject.Find("SaveField").GetComponent<InputField>().interactable = false;
			GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "File_Name";
		}
	}
	//sets erase mode on and off
	public void SetErase(bool erase)
	{
		eraseMode = erase;
		if(eraseMode == true)
		{
			textMode = false;
			GameObject.Find("TextInput").GetComponent<InputField>().interactable = false;
			GameObject.Find("TextToggle").GetComponent<Toggle>().isOn = false;
		}
	}
	//sets text mode on and off
	public void SetText(bool text)
	{
		textMode = text;
		if(textMode == true)
		{
			eraseMode = false;
			GameObject.Find("TextInput").GetComponent<InputField>().interactable = true;
			GameObject.Find("SaveField").GetComponent<InputField>().interactable = false;
			GameObject.Find("EraserToggle").GetComponent<Toggle>().isOn = false;
			canLoad = false;
			canSave = false;
		}
		else
		{
			GameObject.Find("TextInput").GetComponent<InputField>().interactable = false;
		}
	}
	public void SetTextOnType(string output)
	{
		textOnType = output;
	}
	public void SetBrushSize(string size)
	{
		int.TryParse(size, out brushSize);
	}
	public void ChangeRed(string r)
	{
		float red;
		float.TryParse(r, out red);
		brushColor = new Color(red, brushColor.g, brushColor.b, brushColor.a);
		GameObject brushColorUI = GameObject.Find("BrushColor");
		brushColorUI.GetComponent<Image>().color = brushColor;
		Debug.Log(brushColorUI.GetComponent<Image>().color);

	}
	public void ChangeGreen(string g)
	{
		float green;
		float.TryParse(g, out green);
		brushColor = new Color(brushColor.r, green, brushColor.b, brushColor.a);
		GameObject brushColorUI = GameObject.Find("BrushColor");
		brushColorUI.GetComponent<Image>().color = brushColor;
	}
	public void ChangeBlue(string b)
	{
		float blue;
		float.TryParse(b, out blue);
		brushColor = new Color(brushColor.r, brushColor.g, blue, brushColor.a);
		GameObject brushColorUI = GameObject.Find("BrushColor");
		brushColorUI.GetComponent<Image>().color = brushColor;
	}
	public void ChangeAlpha(string a)
	{
		float alpha;
		float.TryParse(a, out alpha);
		brushColor = new Color(brushColor.r, brushColor.g, brushColor.b, alpha);
		GameObject brushColorUI = GameObject.Find("BrushColor");
		brushColorUI.GetComponent<Image>().color = brushColor;
	}
	public void EraseEntireCanvas()
	{
		for (int x = 0; x < 256; x++)
		{
			for (int y = 0; y < 512; y++)
			{
				studentCanvas.SetPixel(x, y, Color.white);
			}
		}
		byte[] bytes = studentCanvas.EncodeToPNG();
		System.IO.File.WriteAllBytes(dirPath + "/BlankCanvas.png", bytes);
		studentCanvas.LoadImage(System.IO.File.ReadAllBytes(dirPath + "/BlankCanvas" + ".png"));
		gameObject.GetComponent<Renderer>().material.mainTexture = studentCanvas;
	}
	void DrawCharacter(Vector2 currUV, int spot)
	{
		spot *= 7;
		int currX = (int)currUV.x;
		int currY = (int)currUV.y;
		for (int x = spot; x < (spot + 7); x++)
		{
			for (int y = 0; y < 14; y++)
			{
				Color pixelColor = alphabet.GetPixel(x, y);
				studentCanvas.SetPixel(currX, currY + y, pixelColor);
				studentCanvas.Apply();
			}
			currX += 1;
		}
	}
	int DetermineCharacter(char c)
	{
		int modifiedVal = -1;
		if(c >= 97 && c < 123)
		{
			modifiedVal = c - 97; 
		}
		else if(c >= 48 && c < 58)
		{
			modifiedVal = c - 48 + 26;
		}
		return modifiedVal;
	}

	public void SubmitPaiting()
	{
		GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "Successfully saved";
		byte[] bytes = studentCanvas.EncodeToPNG();
		System.IO.File.WriteAllBytes(dirPath + "/" + "StudentCanvas1" + ".png", bytes);

		Texture2D tex = new Texture2D(256, 512, TextureFormat.RGBA32, false);
		tex.LoadImage(System.IO.File.ReadAllBytes(dirPath + "/" + "StudentCanvas1" + ".png"));

		GameObject stuCanvas = GameObject.Find("StuCanvas0");

		if(stuCanvas != null)
		{
			stuCanvas.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
			{
				stuCanvas.GetComponent<ASL.ASLObject>().SendAndSetTexture2D(tex, 
					changeTexture, true);
			});
		}
	}

	public static void changeTexture(GameObject gameObject, Texture2D tex)
	{
		gameObject.GetComponent<Renderer>().material.mainTexture = tex;
	}
}
