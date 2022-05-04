/* PaintOnCanvas.cs
 * Author: Tyler Miller
 * Date: 4/7/2022
 * Description: PaintOnCanvas allows user to create a blank canvas
 * then paint on it. User must save canvas before quitting application
 * to save progress. The load button will allow the user to load a png.
 * The user is allowed to change the brush size and color.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.EventSystems;

public class PaintOnCanvas : MonoBehaviour
{
	//the canvas of the students
	Texture2D studentCanvas;

	//directory of where the exe is ran for windows and save place for canvas
	string dirPath;

	//how big is the size of the brush
	int brushSize;

	//how wide the canvas is in pixels
	int canvasWidth;

	//how long the canvas is in pixels
	int canvasHeight;
	
	//how wide is one grid character in alphabet.png
	int textWidth;

	//how high is the text character in alphabet.png
	int textHeight;

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
	Texture2D alphabet2;
	Texture2D alphabetUsed;

	Vector2 pixelToDraw;

	public int numberOfInterpolations;

	// Current Canvas has been loaded
	bool clicked = false;

	Vector2 previousCoord;

	bool previousMouseDown = false;

	// UI field listeners
	InputField rField = null;
	InputField gField = null;
	InputField bField = null;
	InputField aField = null;

	InputField slField = null;
	InputField textField = null;
	InputField brushSizeField = null;

	// UI button listeners
	Button loadB = null;
	Button saveB = null;
	Button deleteB = null;

	Button subGalB = null;
	Button subStuB = null;

	// UI toggle listeners
	Toggle eraseTog = null;
	Toggle textTog = null;
	
	//UI dropdown listeners
	Dropdown textSizeDrop = null;
	
	ResubmissionHandler handler;

	// Start is called before the first frame update
	void Start()
	{
		// Allows students to resubmit work
		handler = GameObject.Find("Resubmission").GetComponent<ResubmissionHandler>();

		// UI field code
		rField = GameObject.Find("RedInputField").GetComponent<InputField>();
		gField = GameObject.Find("GreenInputField").GetComponent<InputField>();
		bField = GameObject.Find("BlueInputField").GetComponent<InputField>();
		//aField = GameObject.Find("AlphaInputField").GetComponent<InputField>();

		slField = GameObject.Find("SaveField").GetComponent<InputField>();
		textField = GameObject.Find("TextInput").GetComponent<InputField>();
		brushSizeField = GameObject.Find("SizeInputField").GetComponent<InputField>();
		textSizeDrop = GameObject.Find("TextSizeDropdown").GetComponent<Dropdown>();
		
		textSizeDrop.onValueChanged.AddListener(ChangeTextSize);

		rField.onEndEdit.AddListener(ChangeRed);
		gField.onEndEdit.AddListener(ChangeGreen);
		bField.onEndEdit.AddListener(ChangeBlue);
		//aField.onEndEdit.AddListener(ChangeAlpha);

		slField.onEndEdit.AddListener(SaveOrLoadToPNG);
		slField.onValueChanged.AddListener(ChangeToWhite);
		textField.onEndEdit.AddListener(SetTextOnType);
		brushSizeField.onEndEdit.AddListener(SetBrushSize);

		// UI Button Code
		loadB = GameObject.Find("LoadButton").GetComponent<Button>();
		saveB = GameObject.Find("SaveButton").GetComponent<Button>();
		deleteB = GameObject.Find("DeleteCanvasButton").GetComponent<Button>();

		GameObject tGal = GameObject.Find("SubmitToGalleryButton");
		if (tGal != null)
		{
			subGalB = tGal.GetComponent<Button>();
			subGalB.onClick.AddListener(SubmitPainting);
		}

		GameObject tSubGal = GameObject.Find("SubmitWork");
		if(tSubGal != null)
		{
			subStuB = tSubGal.GetComponent<Button>();
			subStuB.onClick.AddListener(SubmitPainting);
		}
		

		loadB.onClick.AddListener(SetCanLoad);
		saveB.onClick.AddListener(SetCanSave);
		deleteB.onClick.AddListener(EraseEntireCanvas);

		// UI toggle code
		eraseTog = GameObject.Find("EraserToggle").GetComponent<Toggle>();
		textTog = GameObject.Find("TextToggle").GetComponent<Toggle>();

		eraseTog.onValueChanged.AddListener(SetErase);
		textTog.onValueChanged.AddListener(SetText);

		// Canvas variables
		brushSize = 10;
		canvasWidth = 768;
		canvasHeight = 512;
		textWidth = 7;
		textHeight = 14;
		eraseMode = false;
		textMode = false;
		canSave = false;
		canLoad = false;
		textOnType = "";
		brushColor = Color.black;
		pixelToDraw = new Vector2(0, 0);
		dirPath = Application.dataPath;
		//another way to load in alphabet.png
		//alphabet = new Texture2D(456, 14, TextureFormat.RGBA32, false);
		//alphabet.LoadImage(System.IO.File.ReadAllBytes(dirPath + "/alphabet.png"));

		/*ensure that non power of 2 is none, Read/Write enabled is true,
		compression is none, texture type is default, texture shape is 2D,
		FilterPoint is Point(no filter)*/
		alphabet = Resources.Load("alphabet", typeof(Texture2D)) as Texture2D;
		alphabet2 = Resources.Load("alphabet2", typeof(Texture2D)) as Texture2D;
		alphabetUsed = alphabet;
		GameObject brushColorUI = GameObject.Find("BrushColor");
		brushColorUI.GetComponent<Image>().color = brushColor;
		//this covers RGBA format and is good for opacity changing if needed
		studentCanvas = new Texture2D(canvasWidth, canvasHeight, TextureFormat.RGBA32, false);
		for (int x = 0; x < canvasWidth; x++)
		{
			for (int y = 0; y < canvasHeight; y++)
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
		if(clicked == true)
		{
			clicked = handler.SubmissionStatus();
		}
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			if (Input.GetMouseButton(0) == true)
			{
				RaycastHit raycastHit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out raycastHit) == true
				&& raycastHit.transform.GetComponent<PaintOnCanvas>() != null)
				{
					Vector2 uv;
					if ((int)transform.forward.z == -1)
					{
						uv = new Vector2((raycastHit.point.x - (transform.position.x - (transform.localScale.x / 2))) / (canvasWidth / 256),
						(raycastHit.point.y - (transform.position.y - (transform.localScale.y / 2))) / (canvasHeight / 256));
					}
					else if ((int)transform.forward.z == 1)
					{
						uv = new Vector2(1 - (raycastHit.point.x - (transform.position.x - (transform.localScale.x / 2))) / (canvasWidth / 256),
						(raycastHit.point.y - (transform.position.y - (transform.localScale.y / 2))) / (canvasHeight / 256));
					}
					else if ((int)transform.forward.x == -1)
					{
						uv = new Vector2(1 - (raycastHit.point.z - (transform.position.z - (transform.localScale.x / 2))) / (canvasWidth / 256),
						(raycastHit.point.y - (transform.position.y - (transform.localScale.y / 2))) / (canvasHeight / 256));
					}
					else if ((int)transform.forward.x == 1)
					{
						uv = new Vector2((raycastHit.point.z - (transform.position.z - (transform.localScale.x / 2))) / (canvasWidth / 256),
						(raycastHit.point.y - (transform.position.y - (transform.localScale.y / 2))) / (canvasHeight / 256));
					}
					else
					{
						Debug.Log("Bad canvas angle");
						uv = new Vector2(0.5f, 0.5f);
					}
					//converts raycastHit point into a UV coordinate
					//Vector2 uv = new Vector2((raycastHit.point.x - (transform.position.x - (transform.localScale.x / 2))) / (canvasWidth / 256),
					//(raycastHit.point.y - (transform.position.y - (transform.localScale.y / 2))) / (canvasHeight / 256));
					Vector2 pixelCoord = new Vector2((int)(uv.x * (float)(canvasWidth)), (int)(uv.y * (float)(canvasHeight)));
					dirPath = Application.dataPath;

					//If the mouse wasn't down, don't interpolate
					if (!previousMouseDown)
					{
						//draw area of brush size if greater than 1
						if (brushSize > 1)
						{
							if (textMode == false)
							{
								for (int x = (int)(pixelCoord.x - (brushSize / 2)); x < (int)(pixelCoord.x + (brushSize / 2)); x++)
								{
									if (x >= canvasWidth || x < 0)
									{
										continue;
									}
									for (int y = (int)(pixelCoord.y - (brushSize / 2)); y < (int)(pixelCoord.y + (brushSize / 2)); y++)
									{
										if (y >= canvasHeight || y < 0)
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
								//save point for text
								pixelToDraw = pixelCoord;

							}
						}
					}
					//If the mouse was down, interpolate
					else
					{
						if (brushSize > 1)
						{
							for (int i = 0; i < numberOfInterpolations; i++)
							{
								float distanceX = (i * (pixelCoord.x - previousCoord.x) / numberOfInterpolations);
								float distanceY = (i * (pixelCoord.y - previousCoord.y) / numberOfInterpolations);


								if (textMode == false)
								{
									for (int x = (int)(pixelCoord.x - distanceX - (brushSize / 2)); x < (int)(pixelCoord.x - distanceX + (brushSize / 2)); x++)
									{
										if (x >= canvasWidth || x < 0)
										{
											continue;
										}
										for (int y = (int)(pixelCoord.y - distanceY - (brushSize / 2)); y < (int)(pixelCoord.y - distanceY + (brushSize / 2)); y++)
										{
											if (y >= canvasHeight || y < 0)
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
						}
						else
						{
							for (int i = 0; i < numberOfInterpolations; i++)
							{
								float distanceX = (i * (pixelCoord.x - previousCoord.x) / numberOfInterpolations);
								float distanceY = (i * (pixelCoord.y - previousCoord.y) / numberOfInterpolations);
								if (textMode == false && eraseMode == false && textMode == false)
								{
									studentCanvas.SetPixel((int)(pixelCoord.x - distanceX), (int)(pixelCoord.y - distanceY), brushColor);
								}
								else if (eraseMode == true)
								{
									studentCanvas.SetPixel((int)(pixelCoord.x - distanceX), (int)(pixelCoord.y - distanceY), Color.white);
								}
							}
						}
						studentCanvas.Apply();

					}
					previousCoord = pixelCoord;
					previousMouseDown = true;
				}

			}
			else if (Input.GetMouseButtonUp(0) == true)
			{
				//on mouse release write text to image
				if (textOnType.Equals("") == false && textMode == true)
				{
					Vector2 startChar = new Vector2(pixelToDraw.x, pixelToDraw.y);
					for (int i = 0; i < textOnType.Length; i++)
					{
						int spot = DetermineCharacter(textOnType[i]);
						if (spot != -1)
						{
							DrawCharacter(startChar, spot);
							startChar.x += textWidth;
						}
					}
				}
				else if (textMode == true && textOnType.Equals("") == true)
				{
					GameObject.Find("TextPlaceholder").GetComponent<Text>().text = "empty response not allowed";
				}
			}
			else
			{
				previousMouseDown = false;
			}
		}
	}

	public void SaveOrLoadToPNG(string png)
	{
		if (canSave == true)
		{
			//dont add any directories or file extensions
			if (png.Contains("/") == false && png.EndsWith(".png") == false && png.Equals("alphabet") == false)
			{
				GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "Successfully saved";
				GameObject.Find("SaveField").GetComponent<Image>().color = Color.green;
				byte[] bytes = studentCanvas.EncodeToPNG();
				System.IO.File.WriteAllBytes(dirPath + "/" + png + ".png", bytes);
			}
			else if (png.Contains("/") == false && png.EndsWith(".png") == true && png.Equals("alphabet.png") == false)
			{
				GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "Successfully saved";
				byte[] bytes = studentCanvas.EncodeToPNG();
				System.IO.File.WriteAllBytes(dirPath + "/" + png, bytes);
			}
			else
			{
				GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "Do not enter '/'";
				GameObject.Find("SaveField").GetComponent<Image>().color = Color.red;
			}
		}
		else
		{
			//dont add any directories or file extensions
			if (png.Contains("/") == false && png.EndsWith(".png") == false && png.Equals("alphabet") == false)
			{
				GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "File_Name";
				try
				{
					studentCanvas.LoadImage(System.IO.File.ReadAllBytes(dirPath + "/" + png + ".png"));
				}
				catch
				{
					GameObject.Find("SaveField").GetComponent<Image>().color = Color.red;
				}
				studentCanvas.Apply();
			}
			else if (png.Contains("/") == false && png.EndsWith(".png") == true && png.Equals("alphabet.png") == false)
			{
				GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "File_Name";
				try
				{
					studentCanvas.LoadImage(System.IO.File.ReadAllBytes(dirPath + "/" + png + ".png"));
				}
				catch
				{
					GameObject.Find("SaveField").GetComponent<Image>().color = Color.red;
				}
				studentCanvas.Apply();
			}
			else
			{
				GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "Do not enter '/'";
				GameObject.Find("SaveField").GetComponent<Image>().color = Color.red;
			}
		}
	}
	public void ChangeToWhite(string s)
	{
		GameObject.Find("SaveField").GetComponent<Image>().color = Color.white;
	}
	//sets whether save field will display for UI
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
	//sets whether load field will display for UI
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
		if (eraseMode == true)
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
		if (textMode == true)
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
		for (int x = 0; x < canvasWidth; x++)
		{
			for (int y = 0; y < canvasHeight; y++)
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
		spot *= textWidth;
		int currX = (int)currUV.x;
		int currY = (int)currUV.y;
		for (int x = spot; x < (spot + textWidth); x++)
		{
			for (int y = 0; y < textHeight; y++)
			{
				Color pixelColor = alphabetUsed.GetPixel(x, y);
				studentCanvas.SetPixel(currX, currY + y, pixelColor);
			}
			currX += 1;
		}
		studentCanvas.Apply();
	}
	int DetermineCharacter(char c)
	{
		int modifiedVal = -1;
		if (c >= 97 && c < 123)
		{
			modifiedVal = c - 97;
		}
		else if (c >= 48 && c < 58)
		{
			modifiedVal = c - 48 + 26;
		}
		else if (c >= 32 && c < 48)
		{
			modifiedVal = c + 30;
		}
		else if (c >= 65 && c < 91)
		{
			modifiedVal = c - 29;
		}
		return modifiedVal;
	}
	public void ChangeTextSize(int option)
	{
		if(option == 0)
		{
			textWidth = 7;
			textHeight = 14;
			alphabetUsed = alphabet;
		}
		else if(option == 1)
		{
			textWidth = 12;
			textHeight = 28;
			alphabetUsed = alphabet2;
		}
	}
	public void SubmitPainting()
	{
		// Make sure multiple submissions are not allowed
		if(clicked)
		{
			return;
		}

		GameObject.Find("SavePlaceholder").GetComponent<Text>().text = "Successfully saved";
		byte[] bytes = studentCanvas.EncodeToPNG();
		System.IO.File.WriteAllBytes(dirPath + "/" + "StudentCanvas1" + ".png", bytes);

		Texture2D tex = new Texture2D(256, 512, TextureFormat.RGBA32, false);
		tex.LoadImage(System.IO.File.ReadAllBytes(dirPath + "/" + "StudentCanvas1" + ".png"));

		ASL.GameLiftManager manager = GameObject.Find("GameLiftManager").
			GetComponent<ASL.GameLiftManager>();
		int i = 0;

		// Go through all player canvases and check
		foreach (var gameObjs in GameObject.FindGameObjectsWithTag("StuCanvas"))
		{
			GameObject stuCanvas = gameObjs;

			// Check for an empty canvas space
			if (stuCanvas != null && stuCanvas.GetComponent<Renderer>().
				material.mainTexture == null)
			{
				stuCanvas.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
				{
					stuCanvas.GetComponent<ASL.ASLObject>().SendAndSetTexture2D(tex,
						changeTexture, true);
					stuCanvas.GetComponent<GalleryCanvasVariables>().ChangeName(manager.m_Username);
				});

				// Makes sure that students cannot submit multiple drawings.
				handler.AllCanSubmit(0);
				clicked = true;
				break;
			}

			i++;
		}
	}

	public static void changeTexture(GameObject gameObject, Texture2D tex)
	{
		gameObject.GetComponent<Renderer>().material.mainTexture = tex;
	}

	public void ResetSubmission(bool reset)
	{
		canLoad = reset;
	}
}
