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
        if (costume == null && possibleCostume.Count >= 1)
        {
            costume = possibleCostume[0];
        }

        SwitchCostume();

    }

    // Update is called once per frame
    void Update()
    {
        previousPosition = possibleCostume[whichCostume].transform;
        // Press Q to switch to previous costume
        if (Input.GetKeyDown(KeyCode.Q))
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

        SwitchCameraFocus();
    }

    public void SwitchCostume()
    {
        for (int i = 0; i < possibleCostume.Count; i++)
        {
            possibleCostume[i].SetActive(false);
        }
        possibleCostume[whichCostume].SetActive(true);
        possibleCostume[whichCostume].transform.position = previousPosition.position; 
    }

    // Make sure the camera focus on the new costume.
    public void SwitchCameraFocus()
    {
        cinemachineFreeLook.Follow = possibleCostume[whichCostume].transform;
        cinemachineFreeLook.LookAt = possibleCostume[whichCostume].transform;
    }

}
