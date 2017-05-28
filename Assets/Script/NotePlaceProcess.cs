using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NotePlaceProcess : MonoBehaviour {
    private static string musicName; //読み込む譜面の名前
    private static string level; //難易度

    private static TextAsset csvFile; //CSVファイル

    //譜面を読み込んでキューnoteDataに入れる
    public static void CSVLoad(string _musicName, string _level){
        musicName = _musicName;
        level = _level;
        csvFile = Resources.Load("Maps/"+musicName+level) as TextAsset;/*Resources/CSV下のCSV読み込み*/
        StringReader reader = new StringReader(csvFile.text);
		//csv1行ずつ読みこむ
        while(reader.Peek() > -1){
			//csvFileを1行読み込む
            string line = reader.ReadLine();
			if (line.Split (',') [0] == "1") {//赤ノーツの場合
				PlaceTapNotes(int.Parse(line.Split(',')[1]),int.Parse(line.Split(',')[2]),float.Parse(line.Split(',')[3]));
			}
			if (line.Split (',') [0] == "2") {//ホールドノーツの場合
				PlaceHoldNotes(int.Parse(line.Split(',')[1]),int.Parse(line.Split(',')[2]),float.Parse(line.Split(',')[3]),int.Parse(line.Split(',')[4]));
				Debug.Log ("H");
			}
        }
    }

	//Tapノーツを置くメソッド
	static void PlaceTapNotes(int noteNum, int timing, float lineNum){
		Debug.Log ("PTN");

		string filePass = "Notes/Note" + noteNum;

		if (noteNum % 2 == 0) {
			lineNum = lineNum + 0.5f;
		}

		GameObject Tap = Instantiate (Resources.Load(filePass), new Vector3(1,1,1), Quaternion.identity) as GameObject;
		NoteScript n = Tap.GetComponent<NoteScript> ();
		n._noteOffset = timing;
		n._lineNum = lineNum;
	}

	//Holdノーツを置くメソッド
	static void PlaceHoldNotes(int noteNum, int sTiming, float lineNum, int fTiming){
		Debug.Log ("PHN");
		string filePass = "Notes/Note" + noteNum;

		if (noteNum % 2 == 0) {
			lineNum = lineNum + 0.5f;
		}

		GameObject Hold = Instantiate (Resources.Load(filePass), new Vector3(1,1,1), Quaternion.identity) as GameObject;
		NoteScript n = Hold.GetComponent<NoteScript> ();
		n._noteOffset = sTiming;
		n._lineNum = lineNum;
		n._noteOffsetF = fTiming;
	}
}
