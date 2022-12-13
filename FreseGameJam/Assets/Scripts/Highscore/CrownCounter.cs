using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrownCounter : MonoBehaviour
{
    //Script shows Stats in the Transition
    [SerializeField] TextMeshProUGUI CrownCounters;
    [SerializeField] TextMeshProUGUI TimeCounter;
    [SerializeField] TextMeshProUGUI DeathCounters;
    [SerializeField] int level = 0;
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
            float fullTime = PlayerPrefs.GetFloat("timer" + level, 0) + PlayerPrefs.GetFloat("lastTimer" + level, 0);
            TimeCounter.text = fullTime.ToString();
        }

        if (DeathCounters != null)
        {
            DeathCounters.text = PlayerPrefs.GetInt("deaths" + level, 1).ToString();
        }
    }

    
}
