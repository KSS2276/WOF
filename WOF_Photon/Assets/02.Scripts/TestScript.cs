using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    public Transform Cube1;
    public Transform Cube2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Mathf.Atan2(Cube2.position.x - Cube1.position.x, Cube2.position.y - Cube1.position.y) * Mathf.Rad2Deg);
        Debug.Log(Mathf.Atan2((Cube1.GetChild(0).position.x - Cube1.GetChild(1).position.x), (Cube2.GetChild(0).position.y - Cube2.GetChild(1).position.y)) * Mathf.Rad2Deg);
    }
}
