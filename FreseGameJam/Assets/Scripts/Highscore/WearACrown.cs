using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WearACrown : MonoBehaviour
{
    [Header("save Character Crowns")]
    [Tooltip("binary int of all activated Crowns 1= get 1000Crowns; 10= WasFastestHardcoreRunner; 100= WasFastestHardcoreCrownRunner")]
    private int activatedCrowns = 000;

    [Header("REFERENCES")]
    [Tooltip("Reference of the #1 Crown")]
    [SerializeField] GameObject Crown1;
    [Tooltip("Reference of the #2 Crown")]
    [SerializeField] GameObject Crown2;
    [Tooltip("Reference of the #3 Crown")]
    [SerializeField] GameObject Crown3;


     //only importend if you want to store the activatedCrowns
    void Start()
    {
        if(PlayerPrefs.GetInt("ActiveCrown") != 0)
        {
            activatedCrowns = PlayerPrefs.GetInt("ActiveCrown");
            PutOnCrown(activatedCrowns);
        }
    }

    /// <summary>
    /// 1= get 1000Crowns; 10= WasFastestHardcoreRunner; 100= WasFastestHardcoreCrownRunner
    /// </summary>
    /// <param name="crown"></param>
    public void UnlockCharacterCrown(int crown)
    {
        if((activatedCrowns/crown) %10 == 0)
        {
            activatedCrowns += crown;
        }
        Debug.Log(activatedCrowns);
        PlayerPrefs.SetInt("ActiveCrown", activatedCrowns);
    }
    

    public void PutOnCrown(int _crown)
    {
        switch (_crown)
        {
            case 0:
                Crown1.SetActive(false);
                Crown2.SetActive(false);
                Crown3.SetActive(false);
                break;

            case >=100:
                Crown1.SetActive(false);
                Crown2.SetActive(false);
                Crown3.SetActive(true);
                break;

            case >=10:
                Crown1.SetActive(false);
                Crown2.SetActive(true);
                Crown3.SetActive(false);
                break;

            case 1:
                Crown1.SetActive(true);
                Crown2.SetActive(false);
                Crown3.SetActive(false);
                break;

            

            default:
                
                break;
        }
    }
}
