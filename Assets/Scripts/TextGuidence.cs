using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextGuidence : MonoBehaviour
{
    public Button nextGuideButton;
    // current guidence number
    int guidenceNum;
    [SerializeField] private GameObject Move;
    [SerializeField] private GameObject Sprint;
    [SerializeField] private GameObject Jump;
    [SerializeField] private GameObject Crouch;
    [SerializeField] private GameObject Costume;




    // Start is called before the first frame update
    void Start()
    {
        Move.SetActive(true);
        guidenceNum = 0;
        nextGuideButton.onClick.AddListener(ChangeGuide);
    }

    // Update is called once per frame
    void Update()
    {
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
            Destroy(gameObject);
        }
    }
}
