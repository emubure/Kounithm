using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static float MusicOffset = 0;
	public Text offsetText;

	void Start () {
		NotePlaceProcess.CSVLoad ("BOKUTO","13");
	}

	void Update () {
		MusicOffset = MusicOffset + (1000 * Time.deltaTime);
		offsetText.text = "Offset=" + MusicOffset.ToString ("f0");

		if (Input.GetKeyDown (KeyCode.A)) {
			Debug.Log (MusicOffset);
		}
	}
}
