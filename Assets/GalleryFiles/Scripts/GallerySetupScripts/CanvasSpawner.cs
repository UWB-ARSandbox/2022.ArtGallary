using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSpawner : MonoBehaviour
{
    GameObject[] WallArray = new GameObject[4];
    static List<GameObject> allCans = new List<GameObject>();
    int totalStu = 0;

    static float point1 = 4;
    static float point2 = 4.5f;
    static float point3 = 5;

    // Start is called before the first frame update
    void Start()
    {
        int wallNum = 1;
        for(int i = 0; i < 4; i++)
		{
            WallArray[i] = GameObject.Find("Wall" + wallNum.ToString());
            wallNum += 1;
		}
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad0))
		{
            StartGallery();
		}

        if(Input.GetKeyDown(KeyCode.Delete))
		{
            ClearGallery();
		}
    }

    // Start the deletion of objects
    public void ClearGallery()
    {
        for (int i = 0; i < totalStu; i++)
        {
            GameObject delCan = GameObject.Find("StuCanvas" + i.ToString());
            DeleteCanvas(delCan);
        }
        point1 = 4;
        point2 = 4.5f;
        point3 = 5;
    }

    void DeleteCanvas(GameObject canvas)
    {
        canvas.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            canvas.GetComponent<ASL.ASLObject>().DeleteObject();
        });
    }

    // Starts the creation of blank canvases
    public void StartGallery()
	{
        ASL.GameLiftManager manager = GameObject.Find("GameLiftManager").
            GetComponent<ASL.GameLiftManager>();

        int amountStu = manager.m_Players.Count;

        totalStu = amountStu;

        int wallSize = (int)(amountStu / 40) + 1;

        ChangeWallSizePos(wallSize);

        for (int i = 0; i < amountStu; i++)
        {
            SpawnObj();
        }
    }

    void ChangeWallSizePos(int wallSize)
	{
        GameObject ground = GameObject.Find("Ground");
        ground.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            Vector3 size = ground.transform.localScale;
            Vector3 expandSize = new Vector3(wallSize, size.y, wallSize);
            ground.GetComponent<ASL.ASLObject>().SendAndSetLocalScale(expandSize);
        });

        for (int i = 0; i < WallArray.Length; i++)
        {
            SendWallSize(WallArray[i], wallSize);
        }
	}

    void SendWallSize(GameObject wall, int wallSize)
    {
        wall.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            Vector3 size = wall.transform.localScale;
            Vector3 expandSize = new Vector3(10 * wallSize, size.y, size.z);
            wall.GetComponent<ASL.ASLObject>().SendAndSetLocalScale(expandSize);

            float wallX = 0;
            float wallZ = 0;
            if(wall.transform.position.x != 0)
			{
                if(wall.transform.position.x < 0)
				{
                    wallX = -1;
                }
                else
				{
                    wallX = 1;
                }
			}
            if (wall.transform.position.z != 0)
			{
                if (wall.transform.position.z < 0)
                {
                    wallZ = -1;
                }
                else
                {
                    wallZ = 1;
                }
            }

            Vector3 newPos = new Vector3((5 * wallSize + 0.5f) * wallX,
                wall.transform.position.y, (5 * wallSize + 0.5f) * wallZ);
            wall.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(newPos);

            if(wall.gameObject.name.Contains("Wall1"))
			{
                point1 = expandSize.x / 2 - 1;
                point3 = newPos.z - 0.5f;
            }
        });
    }

    // Spawns an object in for all users 
    void SpawnObj()
	{
        ASL.ASLHelper.InstantiateASLObject("StuCanvas",
            new Vector3(0, 2, 5), Quaternion.identity, "", "", RecievedGameObj);
    }

    // The object has been recieved by all users
    public static void RecievedGameObj(GameObject spawnedObject)
    {
        allCans.Insert(0, spawnedObject);
        int i = allCans.Count - 1;
        spawnedObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            // Name is changed at to fit the time it was recieved
            spawnedObject.name = "StuCanvas" + i.ToString();

            // Wall 1
            if (i % 4 == 0)
            {
                Vector3 newSpot = new Vector3(-point1, point2, point3);
                spawnedObject.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(newSpot);

                Quaternion rot = Quaternion.Euler(0, 180, 0);
                spawnedObject.GetComponent<ASL.ASLObject>().SendAndSetWorldRotation(rot);

                Advance(spawnedObject);
            }
            // Wall 2
            else if (i % 4 == 1)
            {
                Vector3 newSpot = new Vector3(point3, point2, point1);
                spawnedObject.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(newSpot);

                Quaternion rot = Quaternion.Euler(0, 270, 0);
                spawnedObject.GetComponent<ASL.ASLObject>().SendAndSetWorldRotation(rot);

                Advance(spawnedObject);
            }
            // Wall 3
            else if (i % 4 == 2)
            {
                Vector3 newSpot = new Vector3(point1, point2, -point3);
                spawnedObject.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(newSpot);

                Quaternion rot = Quaternion.Euler(0, 0, 0);
                spawnedObject.GetComponent<ASL.ASLObject>().SendAndSetWorldRotation(rot);

                Advance(spawnedObject);
            }
            // Wall 4
            else if (i % 4 == 3)
            {
                Vector3 newSpot = new Vector3(-point3, point2, -point1);
                spawnedObject.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(newSpot);

                Quaternion rot = Quaternion.Euler(0, 90, 0);
                spawnedObject.GetComponent<ASL.ASLObject>().SendAndSetWorldRotation(rot);

                Advance(spawnedObject);

                if(point2 == 4.5)
				{
                    point2 = 1.5f;
				}
				else
				{
                    point1 -= 2f;
                    point2 = 4.5f;
				}
            }
        });
    }

    static void Advance(GameObject spawnedObject)
    {
        allCans.Remove(spawnedObject);
    }
}
