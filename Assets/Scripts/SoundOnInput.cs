using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SoundOnInput : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI soundON;

    Color inactiveColor = new Color(0, 0, 0, 1f);
    Color activeColor = new Color(94/255f, 156/255f, 81/255f, 1f);

    // Update is called once per frame
    void Update()
    {
        soundON.color = 0 == AudioListener.volume ? inactiveColor : activeColor;
    }

    public void soundOnCallback()
    {
        AudioListener.volume = 1;
    }
}
