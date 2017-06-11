using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour {
	//	----tap,hold用----
	public int _noteOffset;
	public float _lineNum;

	private AudioSource tapSound;

	public float noteZ = 0;
	bool NoteJudgeLinePassed = false;

	//	----hold用----
	public bool HoldTrue = false;
	public int _noteOffsetf = 0;
	public float noteZf = 0;


	void Start(){
		tapSound = GameObject.Find ("NoteSound").GetComponent<AudioSource>();
		if (HoldTrue == true) {
			createHoldMesh ();
		}
	}

	void Update(){

		//終点の位置+ * ((判定ラインにくる時間 - 曲の再生時間) * レーンの長さ * スピード)
		noteZ = 0 + 1 * ((_noteOffset - GameManager.MusicOffset) * 20 * 0.003f);
		//Hold用:終点位置決定
		noteZf = 0 + 1 * ((_noteOffsetf - GameManager.MusicOffset) * 20 * 0.003f);

		if (HoldTrue == false) {
			updateTap ();
		} else if (HoldTrue == true) {
			updateHoldMesh ();
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
	//改善点：実行してからScene画面で見渡せば分かるけど、変な場所にメッシュが生成されてる。
	void createHoldMesh(){
			//終点座標を決める

			var mesh = new Mesh ();
			mesh.vertices = new Vector3[] {
			new Vector3 (_lineNum-0.5f, 0.1f, noteZ),//左下
			new Vector3 (_lineNum+0.5f, 0.1f, noteZ),//右下
			new Vector3 (_lineNum+0.5f, 0.1f, noteZf),//右上
			new Vector3 (_lineNum-0.5f, 0.1f, noteZf),//左上
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
	}
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
}
