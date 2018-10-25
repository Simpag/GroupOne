using UnityEngine.UI;
using UnityEngine;

public class Popup : MonoBehaviour {

    public Text text;

	public void Setup(string _text)
    {
        CancelInvoke();
        text.text = _text;
        Invoke("Close", 40);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
