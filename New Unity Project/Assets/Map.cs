using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Map : MonoBehaviour {
    public GameObject MapObject;

    public GameObject MapPut;

    public TextAsset csvFile;

    string str = "";
    string strget = "";

    int gyou = 10;
    int retu = 10;

    int[,] map = new int[100, 100];
    int[] iDat = new int[100];

    int a = 0;
    int b = 0;
    int c = 0;

    int ix = 0;
    int iy = 0;
    int iz = 0;

	// Use this for initialization
	void Start () {

        csvFile = Resources.Load("test") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        while(reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            str = str +","+ line;
        }

        str = str + ",";

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

        ix = 0;
        iy = 0;
        iz = 0;

        a = 0;
        b = 0;
        c = 0;

        for (int c = 0; c < gyou; c++)
        {
            for (int i = 0; i < retu; i++)
            {
                if (map[a, b] == 1)
                {
                    MapPut = Instantiate(MapObject) as GameObject;
                    MapPut.transform.position = new Vector3(ix, iy, iz);
                }

                b++;
                ix = ix + 1;
            }

            a++;
            b = 0;
            ix = 0;
            iy = iy - 1;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
