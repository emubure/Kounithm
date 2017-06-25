using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour {
	//	----tap,hold用----
	public int _sTiming;//ノーツの始点タイミング
	public float _lineNum;//ノーツの列番号

	private AudioSource tapSound;

	public float noteZ = 0;//レーンの長さ、判定ラインに来る時間などから計算した現在の曲の再生時間のノーツの位置
	bool NoteJudgeLinePassed = false;//ノーツ判定ラインを通ったかどうか

	void Start(){
		tapSound = GameObject.Find ("NoteSound").GetComponent<AudioSource>();
	}

	void Update(){

		//終点の位置+ * ((判定ラインにくる時間 - 曲の再生時間) * レーンの長さ * スピード)
		noteZ = 0 + 1 * ((_sTiming - GameManager.MusicOffset) * 20 * 0.003f);
			updateTap ();
	}

	void updateTap(){
		this.transform.position = new Vector3 (_lineNum, 0.5f, noteZ);

		//判定ラインに来たら破壊
		if (NoteJudgeLinePassed==false&&this.transform.position.z <= 0f) {
			NoteJudgeLinePassed = true;
			tapSound.PlayOneShot (tapSound.clip);
		}
		if (this.transform.position.z <= -2f) {
			Destroy (gameObject);
		}
	}
}
