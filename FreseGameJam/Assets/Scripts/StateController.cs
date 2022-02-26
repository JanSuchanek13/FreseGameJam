using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public bool ball;
    public bool human = true;
    public bool frog;
    public bool crane;
    private KeyCode[] keyCodes = new KeyCode[] { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < keyCodes.Length; ++i)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                Debug.Log(i * 3);

                switch (i)
                {
                    case 1:
                        ball = false;
                        human = true;
                        frog = false;
                        crane = false;
                        break;

                    case 2:
                        ball = false;
                        human = false;
                        frog = true;
                        crane = false;
                        break;

                    case 3:
                        ball = false;
                        human = false;
                        frog = false;
                        crane = true;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
