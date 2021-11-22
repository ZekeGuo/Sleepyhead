using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CostumeSwitch : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject costume;
    public List<GameObject> possibleCostume;
    public int whichCostume;
    public CinemachineFreeLook cinemachineFreeLook;
    Transform previousPosition;

    // Update is called once per frame
    void Start()
    {
        for (int i = 0; i < possibleCostume.Count; i++)
        {
            possibleCostume[i].SetActive(false);
        }
        possibleCostume[0].SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
        // Press Q to switch to previous costume
        if (Input.GetKeyDown(KeyCode.Q) && Input.GetAxisRaw("Horizontal") <= 0.1f && Input.GetAxisRaw("Vertical") <= 0.1f)
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
    }

    // Make sure the camera focus on the new costume.
    public void SwitchCameraFocus()
    {
        cinemachineFreeLook.Follow = possibleCostume[whichCostume].transform;
        cinemachineFreeLook.LookAt = possibleCostume[whichCostume].transform;
    }

}
