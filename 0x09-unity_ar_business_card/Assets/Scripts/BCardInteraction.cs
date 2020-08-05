using UnityEngine;

public class BCardInteraction : MonoBehaviour, IIconAction

{
    // Start is called before the first frame update

    [SerializeField] private string url;

    public string Url
    {
        get => url;
        set => url = value;
    }

    /// <summary>
    ///     primary action goto url
    /// </summary>
    public void InvokeAction()
    {
        Application.OpenURL(Url);
    }
}

public interface IIconAction
{
    string Url { get; set; }

    /// <summary>
    ///     perform icon primary action
    /// </summary>
    void InvokeAction();
}