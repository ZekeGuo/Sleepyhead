using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CostumeSwitch : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject costume;
    public List<GameObject> possibleCostume;
    public int whichCostume;
    public CinemachineFreeLook cinemachineFreeLook;
    Transform previousPosition;
    public bool isChanging;
    public GameObject closetUI;
    public GameObject normalCostumeUI;
    public GameObject PrincessCostumeUI;
    public GameObject ClownCostumeUI;
    public int coinNumber;

    // Update is called once per frame
    void Start()
    {
        for (int i = 0; i < possibleCostume.Count; i++)
        {
            possibleCostume[i].SetActive(false);
        }
        possibleCostume[0].SetActive(true);
        coinNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isChanging)
        {
            closetUI.SetActive(true);
        }
        else
        {
            closetUI.SetActive(false);
        }

        GameObject.Find("coinNumber").GetComponent<Text>().text = coinNumber.ToString();

        // Press Q to switch to previous costume
        if (Input.GetKeyDown(KeyCode.Q) && Input.GetAxisRaw("Horizontal") <= 0.1f && Input.GetAxisRaw("Vertical") <= 0.1f && isChanging)
        {
            if (whichCostume == 0)
            {
                whichCostume = possibleCostume.Count - 1;
            }
            else
            {
                whichCostume -= 1;
            }

            SwitchCostume();
        }

        previousPosition = possibleCostume[whichCostume].transform;




        SwitchCameraFocus();
    }

    public void SwitchCostume()
    {

        for (int i = 0; i < possibleCostume.Count; i++)
        {
            possibleCostume[i].SetActive(true);
            possibleCostume[i].transform.position = previousPosition.transform.position;
            possibleCostume[i].SetActive(false);
        }

        possibleCostume[whichCostume].SetActive(true);

        // set color of current costume
        normalCostumeUI.GetComponent<Image>().color = Color.black;
        PrincessCostumeUI.GetComponent<Image>().color = Color.black;
        ClownCostumeUI.GetComponent<Image>().color = Color.black;

        if (whichCostume == 0)
        {
            normalCostumeUI.GetComponent<Image>().color = Color.white;
        }
        else if (whichCostume == 1)
        {
            PrincessCostumeUI.GetComponent<Image>().color = Color.white;
        }
        else
        {
            ClownCostumeUI.GetComponent<Image>().color = Color.white;
        }
    }

    // Make sure the camera focus on the new costume.
    public void SwitchCameraFocus()
    {
        cinemachineFreeLook.Follow = possibleCostume[whichCostume].transform;
        cinemachineFreeLook.LookAt = possibleCostume[whichCostume].transform;
    }

}
