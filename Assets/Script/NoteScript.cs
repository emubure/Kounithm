using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour {
	//	----tap,hold用----
	public int _noteOffset;
	public float _lineNum;

	private AudioSource tapSound;

	float noteZ = 0;
	bool NoteJudgeLinePassed = false;

	//	----hold用----
	public int _noteOffsetF;
	float noteZf = 0;


	void Start(){
		tapSound = GameObject.Find ("NoteSound").GetComponent<AudioSource>();
	}

	void Update(){

		//終点の位置+ * ((判定ラインにくる時間 - 曲の再生時間) * レーンの長さ * スピード)
		noteZ = 0 + 1 * ((_noteOffset - GameManager.MusicOffset) * 20 * 0.003f);
		this.transform.position = new Vector3 (_lineNum, 0, noteZ);

		//終点オフセットに何か入ってたら(Holdノーツの時だったら)
		/*if (_noteOffsetF != null) {
			Debug.Log ("noteOffset ≠ null");
			createHoldMesh ();
		}*/

		//判定ラインに来たら破壊
		if (NoteJudgeLinePassed==false&&this.transform.position.z <= 0f) {
			NoteJudgeLinePassed = true;
			tapSound.PlayOneShot (tapSound.clip);
		}
		if (this.transform.position.z <= -2f) {
			Destroy (gameObject);
		}
	}

	void createHoldMesh(){
		MeshFilter mf = GetComponent<MeshFilter> ();
		Mesh mesh = mf.mesh = new Mesh ();
	
		mesh.name = "HoldMesh";
	}
}
