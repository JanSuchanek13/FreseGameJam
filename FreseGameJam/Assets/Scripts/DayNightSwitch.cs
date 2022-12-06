using UnityEngine;

public class DayNightSwitch : MonoBehaviour
{
    // this is not ready nor working as the PP's are not set up!
    // this sits inactive on the GameManager game object

    #region variables:
    [Header("Day and Night Post-Processing Settings:")]
    [SerializeField] bool useDayPostProcessing = true;
    [SerializeField] bool useNightPostProcessing = false;
    [SerializeField] GameObject dayPP;
    [SerializeField] GameObject nightPP;
    #endregion

    private void Start()
    {
        if (useDayPostProcessing)
        {
            useNightPostProcessing = false;
            nightPP.SetActive(false);
            dayPP.SetActive(true);
        } else if(useNightPostProcessing)
        {
            dayPP.SetActive(false);
            nightPP.SetActive(true);
        }
    }
}
