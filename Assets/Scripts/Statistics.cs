using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    [SerializeField] public Text fishProducedText;
    [SerializeField] public Text warriorsProducedText;
    [SerializeField] public Text fisherProducedText;
    [SerializeField] public Text repelledRaidsText;
    void Update()
    {
        fishProducedText.text = $"{SaveTheVillage.Instance.fishProduced}";
        warriorsProducedText.text = $"{SaveTheVillage.Instance.warriorsProduced}";
        fisherProducedText.text = $"{SaveTheVillage.Instance.fishersProduced}";
        repelledRaidsText.text = $"{SaveTheVillage.Instance.repelledRaids}";
    }
}
