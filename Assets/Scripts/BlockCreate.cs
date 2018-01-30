using UnityEngine;
using System.Collections;

/// <summary>
/// Скрипт для создания блоков
/// </summary>
/// 
public class BlockCreate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateBlockCluster();
        }
	}

    public GameObject CreateBlockCluster()
    {
        GameObject cluster = new GameObject();
        cluster.name = "BlockCluster";
        return cluster;
    }

    public void SetBlock()
    {

    }
}
