using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerLobby : MonoBehaviour
{
    public LobbyManager lobbyManager;
    public Animator animator;

    public void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        lobbyManager = FindObjectOfType<LobbyManager>();
    }

    public void SetPlayerPos()
    {
        transform.position = new Vector3((lobbyManager.playerStage - 1) * 34 + 6.4f, transform.position.y, transform.position.z);
    }

    public void MoveRight()
    {
        if (animator.GetBool("isMoving"))
        {
            return;
        }

        animator.SetBool("isMoving", true);

        transform.eulerAngles = new Vector3(0, 90, 0);

        transform.DOMoveX(transform.position.x + 34, 1)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                animator.SetBool("isMoving", false);
                transform.eulerAngles = new Vector3(0, -180, 0);
            });
    }

    public void MoveLeft()
    {
        if (animator.GetBool("isMoving"))
        {
            return;
        }

        animator.SetBool("isMoving", true);

        transform.eulerAngles = new Vector3(0, -90, 0);

        transform.DOMoveX(transform.position.x - 34, 1)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                animator.SetBool("isMoving", false);
                transform.eulerAngles = new Vector3(0, -180, 0);
            });
    }

    public void MoveBack()
    {

        if (animator.GetBool("isMoving"))
        {
            return;
        }

        StartCoroutine(LoadScene($"Stage_{lobbyManager.nowStage}"));

        animator.SetBool("isMoving", true);

        transform.eulerAngles = new Vector3(0, 0, 0);

        transform.DOMoveZ(transform.position.z + 30, 3)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                animator.SetBool("isMoving", false);
            });
    }

    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOper.isDone)
        {
            yield return null;
        }
    }
}


