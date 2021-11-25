using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAction : MonoBehaviour
{
    public float spinningSpeed = 200;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("dsfvdshifsdihfhsieuf");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * spinningSpeed * Time.deltaTime, Space.World); 
    }



}
