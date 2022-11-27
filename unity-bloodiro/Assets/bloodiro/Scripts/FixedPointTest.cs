using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics.FixedPoint;
using TMPro;

public class FixedPointTest : MonoBehaviour
{
    [SerializeField] GameObject fixedContainer;
    [SerializeField] GameObject floatContainer;
    [SerializeField] GameObject textGO;
    // Start is called before the first frame update
    void Start()
    {
        fp fp1 = (fp)Mathf.PI;
        float test = Mathf.PI;
        for (int ii = 0; ii < 100; ii++)
        {
            fp1 += (fp)Mathf.PI;
            test += Mathf.PI;

            GameObject newFixed = Instantiate(textGO, fixedContainer.transform);
            newFixed.transform.localScale = new Vector3(1, 1, 1);
            TextMeshProUGUI newFixedText = newFixed.GetComponent<TextMeshProUGUI>();
            newFixedText.color = new Color(1, 0.5f, 0);
            newFixedText.text = fp1.ToString();

            GameObject newFloat = Instantiate(textGO, floatContainer.transform);
            newFloat.transform.localScale = new Vector3(1, 1, 1);
            TextMeshProUGUI newFloatText = newFloat.GetComponent<TextMeshProUGUI>();
            newFloatText.color = Color.cyan;
            newFloatText.text = test.ToString();

            Debug.Log($"<color=orange>Fixed</color>: {fp1}, <color=cyan>Floating</color>: {test}");
        }
    }
}
