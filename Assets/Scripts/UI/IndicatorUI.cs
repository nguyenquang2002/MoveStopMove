using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorUI : MonoBehaviour
{

    [SerializeField] Canvas canvas;
    [SerializeField] List<OffscrenIndicator> offscrenIndicators = new List<OffscrenIndicator>();
    [SerializeField] Camera cam;
    [SerializeField] GameObject indicatorPrefab;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(offscrenIndicators.Count > 0)
        {
            for(int i = 0; i < offscrenIndicators.Count; i++)
            {
                offscrenIndicators[i].UpdateTargetIndicator();
            }
        }
    }

    public void AddTargetIndicator(GameObject target)
    {
        OffscrenIndicator indicator = GameObject.Instantiate(indicatorPrefab, canvas.transform).GetComponent<OffscrenIndicator>();
        indicator.InitialiseTargetIndicator(target, cam, canvas);
        offscrenIndicators.Add(indicator);
    }
}
