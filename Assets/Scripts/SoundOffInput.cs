using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoundOffInput : MonoBehaviour
{

    public void soundOffCallback()
    {
        AudioListener.volume = 0;
    }
}
