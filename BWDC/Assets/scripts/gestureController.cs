using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class gestureController : MonoBehaviour
{
	List<Vector2> points = new List<Vector2>();
	List<Vector2> tempPoints = new List<Vector2>();
	List<Vector2> strokes = new List<Vector2>();
	public GameObject penTip;
	public GameObject penTipTrailRender;
	public GameObject penTipLineRender;
	private LineRenderer myLine;
	public bool useRender;
	string tempStrokes = "";
	public List<string> gestures = new List<string>();
	public List<string> gestureNames = new List<string>();
	private List<GameObject> tipList = new List<GameObject> ();
	private float zPos = -5f;
	private float trailTime;
	//public GameObject testDot;


	public GameObject player;
	private movment playerController;


	public bool tablet;

	// Use this for initialization
	void Start()
	{
		playerController = player.GetComponent<movment> ();
		if (tablet) {
			Cursor.visible = false;
		}
		penTipTrailRender.GetComponent<TrailRenderer> ().sortingLayerName = "trailLayer";
		penTipTrailRender.GetComponent<TrailRenderer> ().sortingOrder = 2;
		trailTime = penTipTrailRender.GetComponent<TrailRenderer> ().time;
//		myLine = penTipLineRender.GetComponent<LineRenderer>();
//		myLine.sortingLayerName = "trailLayer";
//		myLine.sortingOrder = 2;
	}

	// Update is called once per frame
	void Update()
	{
		checkSpawningAndDeleting ();
		if (playerController.mouseHoldTimer > 0){// && !useRender) {
//			myLine.enabled = false;
//			return;
			Invoke("resetTrail", 0.01f);
		}
		if (Input.GetMouseButton(0))
		{
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = new Vector3(transform.position.x, transform.position.y, zPos);

			if (!useRender) {
				GameObject tip = (GameObject)Instantiate (penTip, transform.position, Quaternion.identity);
				tipList.Add (tip);
				tip.GetComponent<penTipControl> ().startTrail ();
				if (points.Count > 0) {
//					Debug.Break ();
					Vector2 endPoint = points [points.Count - 1];
					Vector2 midPoint = new Vector2 ((transform.position.x + endPoint.x) / 2f,
						                   (transform.position.y + endPoint.y) / 2f);
					tip = (GameObject)Instantiate (penTip, midPoint, Quaternion.identity);
					tipList.Add (tip);
					tip.GetComponent<penTipControl> ().startTrail ();
				}
			} else {
//				myLine.enabled = true;
//				Invoke("resetTrail", 0.01f);
//				penTipTrailRender.SetActive (true);
				if (playerController.mouseHoldTimer <= 0f) {
					penTipTrailRender.transform.position = new Vector3 (transform.position.x, transform.position.y, zPos);
					penTipTrailRender.GetComponent<TrailRenderer> ().time = trailTime;
//					penTipTrailRender.GetComponent<TrailRenderer> ().startWidth = 1f;
				}
			}
			points.Add(new Vector2(transform.position.x, transform.position.y));
			if (useRender) {
//				float width = 0.5f;
//				myLine.SetWidth (width, width);
//				myLine.SetColors (Color.black, Color.black);
//				myLine.SetVertexCount (points.Count);
//				Vector3[] linePoints = new Vector3[points.Count];
//				for (int i = 0; i < points.Count; i++) {
//					linePoints [i] = new Vector3 (points [i].x, points [i].y, zPos);
//				}
//				myLine.SetPositions (linePoints);
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (useRender) {
				Invoke ("resetTrail", 0.01f);
//				penTipTrailRender.transform.position = new Vector3 (penTipTrailRender.transform.position.x, 
//					penTipTrailRender.transform.position.y, zPos * 5);
//				penTipTrailRender.GetComponent<TrailRenderer> ().time = -1f;
//				penTipTrailRender.SetActive (false);

//				myLine.SetPositions(new Vector3[0]);
//				myLine.SetVertexCount (0);
//				myLine.enabled = false;
			} else {
				foreach (GameObject tip in tipList) {
					Destroy (tip);
				}
				tipList = new List<GameObject> ();
			}
			float angleSum = 0;
			int connerSize = 5;
			for (int i = 2; i < points.Count - connerSize; i++)
			{
				for (int j = 0; j < connerSize; j++) {
					float a1 = Vector2.Angle(points[i + j - 2], points[i + j - 1]);
					float a2 = Vector2.Angle(points[i + j - 1], points[i + j]) + 90;
					float angle = (a1 - a2);
					angleSum += angle;
				}
				if(Mathf.Abs(angleSum)/connerSize > 110)
				{
					if (tempPoints.Count > 0)
					{
						if (Vector2.Distance(points[i - 1], tempPoints[tempPoints.Count - 1]) > 0.5f)
						{
							tempPoints.Add(points[i - 1]);
							i += connerSize;
						}
					}
				}
				else
				{
					tempPoints.Add(points[i - 1]);
					i += connerSize;
				}
			}
			if (points.Count > 0) {
				tempPoints.Add (points [points.Count - 1]);
			}

			if (tempPoints.Count >= 2)
			{
				strokes.Add(findStroke(tempPoints[0], tempPoints[1]));
			}
			for (int i = 1; i < tempPoints.Count-1; i++)
			{
				if(findStroke(tempPoints[i], tempPoints[i + 1]) != strokes[strokes.Count-1])
				{
					Vector2 stroke = findStroke(tempPoints[i], tempPoints[i + 1]);
					//Instantiate(testDot, tempPoints[i], Quaternion.identity);
//					Debug.DrawLine(tempPoints[i], new Vector3(tempPoints[i].x + stroke.x, tempPoints[i].y + stroke.y, 0), Color.red, 50);
					strokes.Add(findStroke(tempPoints[i], tempPoints[i + 1]));
				}
			}

			for(int i = 0; i < strokes.Count; i++)
			{
				tempStrokes += strokes[i].ToString();
			}
//			Debug.Log(tempStrokes);

			points.Clear();
			tempPoints.Clear();
			strokes.Clear();
		}
	}

	private void resetTrail(){
//		penTipTrailRender.SetActive (true);
//		penTipTrailRender.transform.position = new Vector3(transform.position.x, transform.position.y, zPos);
//		penTipTrailRender.GetComponent<TrailRenderer> ().time = trailTime;

		penTipTrailRender.transform.position = new Vector3 (penTipTrailRender.transform.position.x, 
			penTipTrailRender.transform.position.y, zPos * 5);
		penTipTrailRender.GetComponent<TrailRenderer> ().time = -1f;
//		penTipTrailRender.GetComponent<TrailRenderer> ().startWidth = 0f;
//		penTipTrailRender.SetActive (false);
	}

	private void checkSpawningAndDeleting(){
		string gesture = spawn ();
		if (gestureNames.Count > 0) {
            //elevator cat
			if (gesture.Equals (gestureNames [0]) || gesture.Equals (gestureNames [1]) || 
				gesture.Equals(gestureNames[13]) || gesture.Equals(gestureNames[14])) {
				playerController.spawnElevCat ();
            //attack cat
			} else if (gesture.Equals (gestureNames [2])) {
				playerController.spawnAttackCat (playerController.attackCatRight);
			} else if (gesture.Equals (gestureNames [3])) {
				playerController.spawnAttackCat (playerController.attackCatLeft);
            //yarn cat
			} else if (gesture.Equals (gestureNames [9]) || gesture.Equals(gestureNames[10]) || 
						gesture.Equals(gestureNames[11]) || gesture.Equals(gestureNames[12])) {
				playerController.spawnYarnCat (playerController.yarnCatLeft);
			} else if (gesture.Equals (gestureNames [5]) || gesture.Equals(gestureNames[6]) || 
						gesture.Equals(gestureNames[7]) || gesture.Equals(gestureNames[8])) {
				playerController.spawnYarnCat (playerController.yarnCatRight);
            //delete cat
			} 
//			else if (gesture.Equals(gestureNames[4])){
//				playerController.deleteCats ();
//			}
		}
	}

	string spawn()
	{
		int save = -1;
		for (int i = 0; i < gestures.Count; i++)
		{
//			if (tempStrokes.Equals(gestures[i]))
			if (tempStrokes.Contains(gestures[i]))
			{
//				tempStrokes = "";
//				return gestureNames[i];
//				Debug.Log("save: " + save + " len: " + save.Length);
//				Debug.Log ("check: " + gestures [i] + " len: " + gestures[i].Length);
				if (save == -1 || gestures[save].Length < gestures[i].Length){
//					Debug.Break ();
					save = i;
				}
			}
		}
		tempStrokes = "";
		if (save >= 0) {
			Debug.Log ("returning: " + gestureNames[save]);
			return gestureNames [save];
		}
		return "none";
	}

	Vector2 findStroke(Vector2 p1, Vector2 p2)
	{
		Vector2 stroke = new Vector2(0,0);
		float angle = Vector2.Angle(p1, p2);
        if (p1.x < p2.x)
        {
            stroke = new Vector2(1, stroke.y);
        }
        else if (p1.x > p2.x)
        {
            stroke = new Vector2(-1, stroke.y);
        }
		//if (Mathf.Abs(p1.x - p2.x) < Mathf.Abs(p1.y - p2.y))
		//{
		//    stroke = new Vector2(stroke.x, 0);
		//}

		if(p1.y > p2.y)
		{
			stroke = new Vector2(stroke.x, -1);
		}
		else if(p1.y < p2.y)
		{
			stroke = new Vector2(stroke.x, 1);
		}
		//if (Mathf.Abs(p1.x - p2.x) > Mathf.Abs(p1.y - p2.y))
		//{
		//    stroke = new Vector2(0, stroke.y);
		//}
		return stroke;
	}
}


