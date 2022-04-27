using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrownCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CrownCounters;
    [SerializeField] TextMeshProUGUI TimeCounter;
    [SerializeField] int level;
    int crowns = 0;
    

    // Start is called before the first frame update
    void Update()
    {
        if (CrownCounters != null)
        {
            crowns = PlayerPrefs.GetInt("crowns" + level, 1);
            CrownCounters.text = crowns.ToString();
        }
        

        if (TimeCounter != null)
        {
            TimeCounter.text = PlayerPrefs.GetFloat("timer" + level, 0).ToString();
        }
    }

    
}
