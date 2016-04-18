﻿using UnityEngine;
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
	public bool useRender;
	string tempStrokes = "";
	public List<string> gestures = new List<string>();
	public List<string> gestureNames = new List<string>();
	private List<GameObject> tipList = new List<GameObject> ();
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
	}

	// Update is called once per frame
	void Update()
	{
		checkSpawningAndDeleting ();
		if (playerController.mouseHoldTimer > 0 && !useRender) {
			return;
		}
		if (Input.GetMouseButtonDown(0))
		{
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (useRender) {
				penTipTrailRender.SetActive (true);
				penTipTrailRender.transform.position = new Vector3(transform.position.x, transform.position.y, -15f);
				Debug.Log ("ADAD");
//				Debug.Break ();
			}
		}
		if (Input.GetMouseButton(0))
		{
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (!useRender) {
				GameObject tip = (GameObject)Instantiate (penTip, transform.position, Quaternion.identity);
				tipList.Add (tip);
				tip.GetComponent<penTipControl> ().startTrail ();
			} 
			transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
			points.Add(new Vector2(transform.position.x, transform.position.y));
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (useRender) {
				penTipTrailRender.SetActive (false);
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
			tempPoints.Add(points[points.Count-1]);

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
					//Debug.DrawLine(tempPoints[i], new Vector3(tempPoints[i].x + stroke.x, tempPoints[i].y + stroke.y, 0), Color.red, 50);
					strokes.Add(findStroke(tempPoints[i], tempPoints[i + 1]));
				}
			}

			for(int i = 0; i < strokes.Count; i++)
			{
				tempStrokes += strokes[i].ToString();
			}
			Debug.Log(tempStrokes);

			points.Clear();
			tempPoints.Clear();
			strokes.Clear();
		}
	}

	private void checkSpawningAndDeleting(){
		string gesture = spawn ();
		if (gestureNames.Count > 0) {
			if (gesture.Equals (gestureNames [0]) || gesture.Equals (gestureNames [1])) {
				playerController.spawnElevCat ();
			} else if (gesture.Equals (gestureNames [2])) {
				playerController.spawnAttackCat (playerController.attackCatRight);
			} else if (gesture.Equals (gestureNames [3])) {
				playerController.spawnAttackCat (playerController.attackCatLeft);
			} else if (gesture.Equals (gestureNames [6])) {
				playerController.spawnYarnCat (playerController.yarnCatLeft);
			} else if (gesture.Equals (gestureNames [5])) {
				playerController.spawnYarnCat (playerController.yarnCatRight);
			} else if (gesture.Equals(gestureNames[4])){
				playerController.deleteCats ();
			}
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
		if(p1.x < p2.x)
		{
			stroke = new Vector2(1, stroke.y);
		}
		else
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
		else
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


