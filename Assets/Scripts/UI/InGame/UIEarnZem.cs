using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEarnZem : MonoBehaviour
{
    public Text zemText;
    public void UpdateZem(int zem)
    {
        zemText.text = zem.ToString();
    }
}
