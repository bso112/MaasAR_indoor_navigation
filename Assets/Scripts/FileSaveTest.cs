using UnityEngine;

using System.Collections;



using System.Runtime.Serialization.Formatters.Binary; //Formatters 쓸려고..

using System.IO; //File 쓰기 위해

using System.Collections.Generic; //List쓰려고



public class FileStreamTest : MonoBehaviour
{



    public class ScoreEntry

    {

        public string name;  //플레이어 이름

        public int score;   // 스코어

    }




    public List<ScoreEntry> highScores = new List<ScoreEntry>();   //하이스코어 테이블



    void SaveScores()

    {

        var b = new BinaryFormatter(); //BinartFormatter를 받아옴

        var f = File.Create(Application.persistentDataPath + "/highscores.dat"); //파일을 생성.



        b.Serialize(f, highScores); // 스코어를 저장.

        f.Close();

    }



    void Start()

    {

        if (File.Exists(Application.persistentDataPath + "/highscores.dat")) //비어있지 않으면 로드!

        {

            var b = new BinaryFormatter(); //바이너리 포맷터

            var f = File.Open(Application.persistentDataPath + "/highscores.dat", FileMode.Open); // 파일 열기.

            highScores = (List<ScoreEntry>)b.Deserialize(f); //스코어를 로드. 디 시리얼라이즈.

            f.Close(); //파일 닫기.

        }

    }



}

