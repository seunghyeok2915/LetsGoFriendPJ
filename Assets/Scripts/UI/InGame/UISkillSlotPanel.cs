using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillSlotPanel : MonoBehaviour
{
    public GameObject[] slotSkillGO;
    public Button[] slot;

    public Sprite[] skillSprite;

    [System.Serializable]
    public class DisplayItemSlot
    {
        public List<Image> slotSprite = new List<Image>();
    }
    public DisplayItemSlot[] DisplayItemSlots;

    public Image displayResultImage;

    public List<int> startList = new List<int>();
    public List<int> resultIndexList = new List<int>();
    int itemCount = 3;

    private void Start()
    {
        for (int i = 0; i < itemCount * slot.Length; i++)
        {
            startList.Add(i);
        }

        for (int i = 0; i < slot.Length; i++)
        {
            for (int j = 0; j < itemCount; j++)
            {
                slot[i].interactable = false;
                int randomIndex = Random.Range(0, startList.Count);
                if (i == 0 && j == 1 || i == 1 && j == 0 || i == 2 && j == 2)
                {
                    resultIndexList.Add(startList[randomIndex]);
                }
                DisplayItemSlots[i].slotSprite[j].sprite = skillSprite[startList[randomIndex]];

                if (j == 0)
                {
                    DisplayItemSlots[i].slotSprite[itemCount].sprite = skillSprite[startList[randomIndex]];
                }
                startList.Remove(randomIndex);
            }
        }

        for (int i = 0; i < slot.Length; i++)
        {
            StartCoroutine(StartSlot(i));
        }
    }

    int[] answer = { 2, 3, 1 };
    private IEnumerator StartSlot(int slotIndex)
    {
        for (int i = 0; i < (itemCount * (6 + slotIndex * 4) + answer[slotIndex]) * 2; i++)
        {
            slotSkillGO[slotIndex].transform.localPosition -= new Vector3(0, 50f, 0);

            if (slotSkillGO[slotIndex].transform.localPosition.y < 0)
            {
                slotSkillGO[slotIndex].transform.localPosition += new Vector3(0, 300f, 0);
            }
            yield return new WaitForSeconds(0.03f);
        }

        for (int i = 0; i < itemCount; i++)
        {
            slot[i].interactable = true;
        }
    }
}
