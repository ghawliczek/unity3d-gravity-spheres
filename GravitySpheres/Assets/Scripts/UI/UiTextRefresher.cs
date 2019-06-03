using UnityEngine;
using UnityEngine.UI;

public class UiTextRefresher : MonoBehaviour
{
    private Text UiText { get; set; }

    public void Refresh(string text)
    {
        UiText.text = text;
    }

    private void OnEnable()
    {
        UiText = GetComponent<Text>();
    }
}
