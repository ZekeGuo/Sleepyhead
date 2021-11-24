using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAction : MonoBehaviour
{
    public float spinningSpeed = 200; 
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * spinningSpeed * Time.deltaTime, Space.World); 
    }


}
