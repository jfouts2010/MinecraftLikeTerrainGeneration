using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISeasonScript : MonoBehaviour
{
    TextMeshProUGUI tmp;
    GameEngine ge;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        ge = GameObject.Find("Engine").GetComponent<GameEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = $"m:{ge.gt.minute} h:{ge.gt.hour} d:{ge.gt.day} s:{ge.gt.season}";
    }
}
