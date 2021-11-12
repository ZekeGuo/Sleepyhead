using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeSwitch : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject costume;
    public List<GameObject> possibleCostume;
    public int whichCostume;

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

 
    }

    public void SwitchCostume()
    {
        for (int i = 0; i < possibleCostume.Count; i++)
        {
            possibleCostume[i].SetActive(false);
        }
        possibleCostume[whichCostume].SetActive(true);
    }
}
