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
    

    //ブロックの配置
    GameObject MapPut;

    //csvファイル読み込み
    TextAsset csvFile;

    //csv全文字列を保存する
    string str = "";
    //取り出した文字列を保存する
    string strget = "";

    //csvデータの行列
    int gyou = 10;
    int retu = 100;

    //マップ番号を格納するマップ用変数
    int[,] map = new int[100, 100];

    //文字検索用変数
    int[] iDat = new int[100];

    //濫用数値型変数
    int a = 0;
    int b = 0;
    int c = 0;

    //初期位置の座標
    int ix = 0;
    int iy = 0;
    int iz = 0;
    
    // Use this for initialization
    void Start () {

        //csvデータをstrに保存
        csvFile = Resources.Load(GameObject.Find("Nametransprot").GetComponent<Name>().stagename) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        while(reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            str = str +","+ line;
        }

        str = str + ",";

        //csvデータをマップ配列変数mapに保存
        for(int c = 0; c < gyou; c++)
        {
            for(int i = 0; i < retu; i++)
            {
                try
                {
                    iDat[0] = str.IndexOf(",", iDat[0]);
                }
                catch { break; }

                try
                {
                    iDat[1] = str.IndexOf(",", iDat[0] + 1);
                }
                catch { break; }

                iDat[2] = iDat[1] - iDat[0] - 1;

                try
                {
                    strget = str.Substring(iDat[0] + 1, iDat[2]);
                }
                catch { break; }

                try
                {
                    iDat[3] = int.Parse(strget);
                }
                catch { break; }

                map[a, b] = iDat[3];
                b++;
                iDat[0]++;
            }

            a++;
            b = 0;
        }

        //マップの初期位置
        ix = -50;
        iy = 0;
        iz = 0;

        a = 0;
        b = 0;
        c = 0;

        //番号によって指定のブロックを配置
        for (int c = 0; c < gyou; c++)
        {
            for (int i = 0; i < retu; i++)
            {
                if (map[a, b] == 1)
                {
                    MapPut = Instantiate(mapObjects[0]) as GameObject;
                    MapPut.transform.position = new Vector3(ix, iy, iz);
                }
                if (map[a, b] == 2)
                {
                    MapPut = Instantiate(mapObjects[1]) as GameObject;
                    MapPut.transform.position = new Vector3(ix, iy, iz);
                }

                b++;

                //次のブロックの配置位置へ移動
                ix = ix + 1;
            }

            a++;
            b = 0;

            //次の行の配置位置へ移動
            ix = -50;
            iy = iy - 1;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
