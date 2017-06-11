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

	//	----hold用----
	public bool HoldTrue = false;//Holdノーツなのかどうか
	public int _fTiming;//ノーツの終点タイミング
	public float _noteNum;//ノーツの幅
	public float noteZf = 0;//noteZの終点版
	bool NoteFJudgeLinePassed = false;//ノーツの終点が判定ラインを通ったかどうか
	public float _HoldlineNum;//Holdの場合に使われるノーツの列番号

	void Start(){
		tapSound = GameObject.Find ("NoteSound").GetComponent<AudioSource>();
		if (HoldTrue == true) {
			//Hold用のLineNumが何故かずれるので-10して0.3をかけて修正。
			_HoldlineNum = _lineNum - 10f;
			_HoldlineNum = _lineNum * 0.3f;
			//Hold用のnoteNumを使えるように修正。(中点から引いたり足したりするため)
			_noteNum = _noteNum *0.2f;
			_noteNum = _noteNum * 0.5f;
		}
	}

	void Update(){

		//終点の位置+ * ((判定ラインにくる時間 - 曲の再生時間) * レーンの長さ * スピード)
		noteZ = 0 + 1 * ((_sTiming - GameManager.MusicOffset) * 20 * 0.003f);
		//Hold用:終点位置決定
		noteZf = 0 + 1 * ((_fTiming - GameManager.MusicOffset) * 20 * 0.003f);

		if (HoldTrue == false) {
			updateTap ();
		} else if (HoldTrue == true) {
			createHoldMesh ();
		}
	}
	void updateTap(){
		this.transform.position = new Vector3 (_lineNum, 0, noteZ);

		//判定ラインに来たら破壊
		if (NoteJudgeLinePassed==false&&this.transform.position.z <= 0f) {
			NoteJudgeLinePassed = true;
			tapSound.PlayOneShot (tapSound.clip);
		}
		if (this.transform.position.z <= -2f) {
			//Destroy (gameObject);
		}
	}

	//Holdのメッシュを作る
	void createHoldMesh(){
		//何故かメッシュの座標と通常オブジェクトの座標に差異があるので調整
		noteZ = noteZ*3f;
		noteZf = noteZf*3f;

		//終点座標を決める
		var mesh = new Mesh ();
		mesh.vertices = new Vector3[] {
			new Vector3 (_HoldlineNum-_noteNum, 0.1f, noteZ),//左下
			new Vector3 (_HoldlineNum+_noteNum, 0.1f, noteZ),//右下
			new Vector3 (_HoldlineNum+_noteNum, 0.1f, noteZf),//右上
			new Vector3 (_HoldlineNum-_noteNum, 0.1f, noteZf),//左上
		};

			/*mesh.uv = new Vector2[] {
				new Vector2 (0, 0),
				new Vector2 (1, 0),
				new Vector2 (0, 1),
				new Vector2 (1, 1)
			};*/

		mesh.triangles = new int[]{
			0,2,1,
			0,3,2
		};

		mesh.RecalculateNormals();
		mesh.RecalculateBounds ();

		var filter = GetComponent<MeshFilter> ();
		filter.sharedMesh = mesh;

		if (NoteJudgeLinePassed==false&&noteZ <= 0f) {
			NoteJudgeLinePassed = true;
			tapSound.PlayOneShot (tapSound.clip);
		}
		if (NoteFJudgeLinePassed==false&&noteZf <= 0f) {
			NoteFJudgeLinePassed = true;
			tapSound.PlayOneShot (tapSound.clip);
		}

	}

	/*
	void updateHoldMesh(){
		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		Vector3[] vertices = mesh.vertices;

		vertices [0].x = _lineNum - 0.5f;
		vertices [1].x = _lineNum + 0.5f;
		vertices [2].x = _lineNum + 0.5f;
		vertices [3].x = _lineNum - 0.5f;

		vertices [0].y = 0.1f;
		vertices [1].y = 0.1f;
		vertices [2].y = 0.1f;
		vertices [3].y = 0.1f;

		vertices [0].z = noteZ;
		vertices [1].z = noteZ;
		vertices [2].z = noteZf;
		vertices [3].z = noteZf;

		mesh.vertices = vertices;
		mesh.RecalculateBounds();
	}
	*/
}
