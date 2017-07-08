using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer),typeof(MeshFilter))]
public class SlideScript : MonoBehaviour {
	//	----tap,hold用----
	public int _sTiming;//ノーツの始点タイミング
	public int _fTiming;//ノーツの終点タイミング

	public float _lineNum;//ノーツの列番号
	public float _flineNum;
	public float _noteNum;//ノーツの幅
	public float _fnoteNum;

	public float noteZ = 0;//レーンの長さ、判定ラインに来る時間などから計算した現在の曲の再生時間のノーツの位置
	public float noteZf = 0;//noteZの終点版

	bool NoteJudgeLinePassed = false;//ノーツ判定ラインを通ったかどうか
	bool NoteFJudgeLinePassed = false;//ノーツの終点が判定ラインを通ったかどうか

	//public float _SlidelineNum;//Holdの場合に使われるノーツの列番号

	private AudioSource tapSound;

	void Start(){
		tapSound = GameObject.Find ("NoteSound").GetComponent<AudioSource>();
		//HoldのX座標がちょっとズレるので修正
		_lineNum -= 0.5f;
		_flineNum -= 0.5f;
		//Hold用のnoteNumを使えるように修正。(中点から引いたり足したりするため)
		_noteNum = _noteNum * 0.5f - 0.2f;
		_fnoteNum = _fnoteNum * 0.5f - 0.2f;

		Debug.Log ("_lineNum: "+_lineNum);
		Debug.Log ("_flineNum: "+_flineNum);
		Debug.Log ("_noteNum: "+_noteNum);
		Debug.Log ("_fnoteNum: "+_fnoteNum);
	}


	void Update () {
		noteZ = 0 + 1 * ((_sTiming - GameManager.MusicOffset) * 20 * 0.003f);
		noteZf = 0 + 1 * ((_fTiming - GameManager.MusicOffset) * 20 * 0.003f);
		createSlideMesh ();
	}

	//Slideのメッシュを作る
	void createSlideMesh(){
		var mesh = new Mesh ();
		mesh.vertices = new Vector3[] {
			new Vector3 (_lineNum-_noteNum, -0.2f, noteZ),//左下
			new Vector3 (_lineNum+_noteNum, -0.2f, noteZ),//右下
			new Vector3 (_flineNum+_fnoteNum, -0.2f, noteZf),//右上
			new Vector3 (_flineNum-_fnoteNum, -0.2f, noteZf),//左上
		};

		mesh.uv = new Vector2[] {
			new Vector2 (0, 0),
			new Vector2 (1, 0),
			new Vector2 (0, 1),
			new Vector2 (1, 1)
		};

		mesh.triangles = new int[]{
			0,2,1,
			0,3,2
		};

		mesh.colors = new Color[] {
			Color.yellow,
			Color.yellow,
			Color.yellow,
			Color.yellow
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
			//Destroy (gameObject);
		}

	}

}
