using UnityEngine;
using UnityEngine.UI;

public class UIAssassinateButton : MonoBehaviour
{
    public Button assassinateButton;
    public PlayerAttack playerAttack;
    public PlayerInput playerInput;

    public void Start()
    {
        if (playerAttack == null)
        {
            playerAttack = FindObjectOfType<PlayerAttack>();
        }

        if (playerInput == null)
        {
            playerInput = FindObjectOfType<PlayerInput>();
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
        if (GameManager.Instance.IsCaught) //�����ٸ�
        {
            assassinateButton.gameObject.SetActive(false);
            assassinateButton.interactable = false;
            return;
        }

        if (playerInput.canMove) //�����ϼ����� ���� (�ϻ����� ����)���
        {
            if (playerAttack.CanAssassinate())
            {
                assassinateButton.interactable = true;
                //assassinateButton.gameObject.SetActive(true);
            }
            else
            {
                assassinateButton.interactable = false;
            }
        }
        else
        {
            assassinateButton.interactable = false;
        }
    }
}
