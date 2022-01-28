using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoundOffInput : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI soundOFF;

    Color inactiveColor = new Color(0, 0, 0, 1f);
    Color activeColor = new Color(237/255f, 108/255f, 97/255f, 1f);

    // Update is called once per frame
    void Update()
    {
          soundOFF.color = 0 != AudioListener.volume ? inactiveColor : activeColor;
    }

    public void soundOffCallback()
    {
        AudioListener.volume = 0;
    }
}
