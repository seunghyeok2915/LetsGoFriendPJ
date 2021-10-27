using UnityEngine;
using UnityEngine.UI;

public class UIAssassinateButton : MonoBehaviour
{
    public Image alertImage;
    public Button assassinateButton;
    public PlayerAttack playerAttack;

    public void Start()
    {
        if (playerAttack == null)
        {
            playerAttack = FindObjectOfType<PlayerAttack>();
        }

        if (assassinateButton == null)
        {
            assassinateButton = GetComponent<Button>();
        }

        assassinateButton.onClick.RemoveAllListeners();
        assassinateButton.onClick.AddListener(playerAttack.AssassinateEnemy);
    }

    public void Update()
    {
        if (!GameManager.Instance.IsCaught)
        {
            alertImage.gameObject.SetActive(false);

            if (playerAttack.CanAssassinate())
            {
                assassinateButton.interactable = true;
                assassinateButton.gameObject.SetActive(true);
            }
            else
            {
                assassinateButton.interactable = false;
                
            }
        }
        else
        {
            assassinateButton.gameObject.SetActive(false);
            alertImage.gameObject.SetActive(true);
        }
    }
}
