//
//10月12日
//吉川陽詩
//csvの読み込み、ブロックの配置
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Map : MonoBehaviour {
    //ブロックのPrefabを入れる箱をGameObject型変数で宣言
    public List<GameObject> mapObjects ;

    //csvのデータシートを格納するリスト
    public List<string[]> map = new List<string[]>();

    //ブロックの大きさ
    public int blocksize;

    //ブロックの配置
    GameObject MapPut;

    //csvファイル読み込み
    TextAsset csvFile;

    //csv全文字列を保存する
    string str = "";

    //初期位置の座標
    int ix = 0;
    int iy = 0;
    int iz = 0;

    //画像ずらし用変数
    int _x = 100;
    int _y = 100;
    
    // Use this for initialization
    void Start () {

        //csvデータをstrに保存
        csvFile = Resources.Load(GameObject.Find("Nametransprot").GetComponent<Name>().stagename) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        while(reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            map.Add(line.Split(','));
        }

        //番号によって指定のブロックを配置
        for (int g = 0; g < map.Count; g++)
        {
            for (int r = 0; r < map[0].Length; r++)
            {

                int a = 1;
                string block = a.ToString();
                if (map[g][r] == block)
                {
                    MapPut = Instantiate(mapObjects[0]) as GameObject;
                    //MapPut.transform.position = new Vector3(blocksize * r, 0, blocksize * g);
                }

                int b = 2;
                string player = b.ToString();

                if (map[g][r] == player)
                {
                    MapPut = Instantiate(mapObjects[1]) as GameObject;
                    MapPut.transform.position = new Vector3(blocksize * r, 2, blocksize * g);
                }

                int c = 3;
                string cameras = c.ToString();
                if(map[g][r] == cameras)
                {
                    MapPut = Instantiate(mapObjects[2]) as GameObject;
                    MapPut.transform.position = new Vector3(blocksize * r, 0, blocksize * g);
                }

                int d = 4;
                string waveManager = d.ToString();
                if (map[g][r] == waveManager)
                {
                    MapPut = Instantiate(mapObjects[3]) as GameObject;
                    MapPut.transform.position = new Vector3(blocksize * r, 2, blocksize * g);
                }
                int e = 5;
                string moveEnemySpawn = e.ToString();
                if (map[g][r] == moveEnemySpawn)
                {
                    MapPut = Instantiate(mapObjects[4]) as GameObject;
                    MapPut.transform.position = new Vector3(blocksize * r, 2, blocksize * g);
                }
                int f = 5;
                string PaulEnemySpawn = f.ToString();
                if (map[g][r] == PaulEnemySpawn)
                {
                    MapPut = Instantiate(mapObjects[5]) as GameObject;
                    MapPut.transform.position = new Vector3(blocksize * r, 2, blocksize * g);
                }
                int s = 5;
                string StickEnemySpawn = s.ToString();
                if (map[g][r] == StickEnemySpawn)
                {
                    MapPut = Instantiate(mapObjects[6]) as GameObject;
                    MapPut.transform.position = new Vector3(blocksize * r, 2, blocksize * g);
                }

                ix = ix + blocksize * r;
            }
            iz = iz + blocksize * g;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
