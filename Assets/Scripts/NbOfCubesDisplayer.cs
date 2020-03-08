using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NbOfCubesDisplayer : MonoBehaviour
{
    private int m_totalNumberOfCubes;
    [SerializeField]
    private TMP_Text m_text;

    // Start is called before the first frame update
    void Start()
    {
        m_text.SetText("Total: "+m_totalNumberOfCubes);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToCount(int _number)
    {
        m_totalNumberOfCubes += _number;
        m_text.SetText("Total: "+m_totalNumberOfCubes);
    }
}
