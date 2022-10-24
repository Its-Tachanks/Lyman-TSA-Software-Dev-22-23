using UnityEngine;

[System.Serializable]
public class Question
{
    [field: SerializeField]
    public string Prompt { get; private set; }

    [field: SerializeField]
    public string Response { get; private set; }

    public Question(string p, string r)
    {
        Prompt = p;
        Response = r;
    }
}
