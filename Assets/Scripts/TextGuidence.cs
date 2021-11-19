using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextGuidence : MonoBehaviour
{
    public RawImage bg;
    // current guidence number
    int guidenceNum;
    [SerializeField] private GameObject Move;
    [SerializeField] private GameObject Sprint;
    [SerializeField] private GameObject Jump;
    [SerializeField] private GameObject Crouch;
    [SerializeField] private GameObject Costume;
    [SerializeField] private GameObject Princess;




    // Start is called before the first frame update
    void Start()
    {
        Move.SetActive(true);
        guidenceNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeGuide();
        }

        if (Input.GetKeyDown(KeyCode.Q) && guidenceNum >= 4)
        {
            Costume.SetActive(false);
            Move.SetActive(false);
            Sprint.SetActive(false);
            Jump.SetActive(false);
            Crouch.SetActive(false);
            Princess.SetActive(false);
            bg.gameObject.SetActive(false);

            if (GameObject.Find("Parent_Player").GetComponent<CostumeSwitch>().whichCostume == 1)
            {
                Princess.SetActive(true);
                bg.gameObject.SetActive(true);
            } 
        }
    }

    public void ChangeGuide()
    {
        guidenceNum ++;
        Debug.Log(guidenceNum);
        if (guidenceNum == 1)
        {
            Move.SetActive(false);
            Sprint.SetActive(true);
        }
        if (guidenceNum == 2)
        {
            Sprint.SetActive(false);
            Jump.SetActive(true);
        }
        if (guidenceNum == 3)
        {
            Jump.SetActive(false);
            Crouch.SetActive(true);
        }
        if (guidenceNum == 4)
        {
            Crouch.SetActive(false);
            Costume.SetActive(true);
        }
        if (guidenceNum >= 5)
        {
            bg.gameObject.SetActive(false);
            Costume.SetActive(false);
            Princess.SetActive(false);
        }
    }
}
