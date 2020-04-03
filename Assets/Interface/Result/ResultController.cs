using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ResultController : MonoBehaviour
{
    public void Initialize(string message)
    {
        switch (message)
        {
            case UIManager.Next:
                AudioManager.Instance.Play("Ready");
                AudioManager.Instance.Play("Win");
                break;
            case UIManager.Win:
                AudioManager.Instance.Play("Win");
                break;
            default:
                AudioManager.Instance.Play("Lose " + Random.Range(1, 4));
                break;
        }
        GetComponent<Text>().text = message;
        Destroy(gameObject, 1);
    }
}