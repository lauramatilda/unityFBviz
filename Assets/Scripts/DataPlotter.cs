using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class DataPlotter : MonoBehaviour
{
	public string inputfile;
	private List<Dictionary<string, object>> pointList;

	// Columns to be assigned
	public int columnX = 0;
	public int columnY = 1;
	public int columnZ = 2;
	public int columnE = 3;
    public int columnG = 4;

	public string xName;
	public string yName;
	public string zName;
    public string colName;
    public string gName;

	public float plotScale = 10; //this gets overriden anyway, nevermind

	// The prefab
	public GameObject PointPrefab;

	// Objects to hold different groups of people
	public GameObject PointHolder;
    public GameObject PointHolder2;
    public GameObject PointHolder3;
    public GameObject PointHolder4;
    public GameObject PointHolder5;
    public GameObject PointHolder6;
    public GameObject PointHolder7;

    public List<GameObject> allPoints = new List<GameObject>(); //array for calling individuals

	

    // INITIALIZE ALL-----------------------------------------------------------------------------------------------------
	void Start()
	{

		// Set pointlist to results of function Reader with argument inputfile
		pointList = CSVReader.Read(inputfile);

		// Declare list of strings, fill with keys (column names)
		List<string> columnList = new List<string>(pointList[1].Keys);

		foreach (string key in columnList)

		// Assign column name from columnList to Name variables
		xName = columnList[columnX];
		yName = columnList[columnY];
		zName = columnList[columnZ];
        colName = columnList[columnE];
        gName = columnList[columnG];
		
		// Get maxes and mins of each axis
		float xMax = FindMaxValue(xName);
		float yMax = FindMaxValue(yName);
		float zMax = FindMaxValue(zName);

		float xMin = FindMinValue(xName);
		float yMin = FindMinValue(yName);
		float zMin = FindMinValue(zName);

		

        //LOOP FOR ALL DATAPOINTS------------------------------------------------------------------------------------------
		for (var i = 0; i < pointList.Count; i++)
		{
			float x =
				(System.Convert.ToSingle(pointList[i][xName]) - xMin)
				/ (xMax - xMin);

			float y =
				(System.Convert.ToSingle(pointList[i][yName]) - yMin)
				/ (yMax - yMin);

			float z =
				(System.Convert.ToSingle(pointList[i][zName]) - zMin)
				/ (zMax - zMin);

            float group = System.Convert.ToSingle(pointList[i][gName]);

			GameObject dataPoint = Instantiate( // Instantiate as gameobject variable so that it can be manipulated within loop
					PointPrefab,
                    new Vector3(x,y,z) * (z*5), //*Plotscale originally used to size here
					Quaternion.identity);

            dataPoint.transform.RotateAround(Vector3.zero, Vector3.up, x*500); //find a place for it, degrees

            allPoints.Add(dataPoint); //add each GameObject to an array for calling later

            // Assigns values to dataPointName
            string dataPointName =
                pointList[i][colName] + ", friends since " + (2017 - Convert.ToInt32(pointList[i][yName]));

			switch (Convert.ToInt32(group)) // Add each to some group for rotation variance
			{
				case 7:
					dataPoint.transform.parent = PointHolder7.transform;
					break;
				case 6:
					dataPoint.transform.parent = PointHolder6.transform;
					break;
				case 5:
					dataPoint.transform.parent = PointHolder5.transform;
					break;
				case 4:
					dataPoint.transform.parent = PointHolder4.transform;
					break;
				case 3:
					dataPoint.transform.parent = PointHolder3.transform;
					break;
				case 2:
					dataPoint.transform.parent = PointHolder2.transform;
					break;
				default: 
					dataPoint.transform.parent = PointHolder.transform;
                    break;
			}

			// Assigns name and color to the datapoint
			dataPoint.transform.name = dataPointName;
			dataPoint.GetComponent<Renderer>().material.color =
				new Color(z*z, y*z, x, 1.0f);
		}

	}


    void Update() //ROTATE POINTS AROUND THE PERSON----------------------------------------------------------------
	{
        //rotate groups first
        PointHolder.transform.Rotate(0, 1, 0);
        PointHolder2.transform.Rotate(0, 10 * Time.deltaTime, 0);
		PointHolder3.transform.Rotate(0, 5 * Time.deltaTime, 0);
		PointHolder4.transform.Rotate(0, 10 * Time.deltaTime, 0);
        PointHolder5.transform.Rotate(0, 8 * Time.deltaTime, 0);
        PointHolder6.transform.Rotate(0, 1, 0);
        PointHolder7.transform.Rotate(0, 3 * Time.deltaTime, 0);

        //a little extra action for a few favorite people
        allPoints[1].transform.RotateAround(Vector3.zero, Vector3.up, 4); // Martti
		allPoints[188].transform.RotateAround(Vector3.zero, Vector3.up, 3); // Stephanie
        allPoints[107].transform.RotateAround(Vector3.zero, Vector3.up, 3); // Julia
		allPoints[5].transform.RotateAround(Vector3.zero, Vector3.up, 2); //Pol
        allPoints[11].transform.RotateAround(Vector3.zero, Vector3.up, 2); //Samar

		//allPoints[Random.Range(10, 600)].transform.Translate(0, 0, 3 * Time.deltaTime); //Atrophy simulation, nevermind


		//ALL INTERACTIVE & MOUSE FUNCTIONS HERE---------------------------------------------------------------------

		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
			if (hit)
			{
					Debug.Log("HIT" + hitInfo.transform.gameObject.name);
                    hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                    GameObject.Find("x_Title").GetComponent<TextMesh>().text = hitInfo.transform.gameObject.name;

			}
			else
			{
				//do nothing for now
			}
		}

	}

    //MAX-MIN CALCULATIONS-------------------------------------------------------------------------------------------------

	private float FindMaxValue(string columnName)
	{
		//set initial value to first value
		float maxValue = Convert.ToSingle(pointList[0][columnName]);

		//Loop through Dictionary, overwrite existing maxValue if new value is larger
		for (var i = 0; i < pointList.Count; i++)
		{
			if (maxValue < Convert.ToSingle(pointList[i][columnName]))
				maxValue = Convert.ToSingle(pointList[i][columnName]);
		}

		return maxValue;
	}

	private float FindMinValue(string columnName)
	{

		float minValue = Convert.ToSingle(pointList[0][columnName]);

		//Loop through Dictionary, overwrite existing minValue if new value is smaller
		for (var i = 0; i < pointList.Count; i++)
		{
			if (Convert.ToSingle(pointList[i][columnName]) < minValue)
				minValue = Convert.ToSingle(pointList[i][columnName]);
		}
		return minValue;
	}

}
