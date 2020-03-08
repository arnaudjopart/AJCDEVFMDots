using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private float m_timeBetweenRefresh;
    private float m_refreshRate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_refreshRate -= Time.deltaTime;
        if (m_refreshRate < 0)
        {
            m_refreshRate = m_timeBetweenRefresh;
            
            var timeBetweenTwoFrames = Time.deltaTime;
            var nbFramePerSecond = (1 / timeBetweenTwoFrames);
            m_text.SetText("FPS: "+nbFramePerSecond.ToString("F2"));
        }
    }
}
