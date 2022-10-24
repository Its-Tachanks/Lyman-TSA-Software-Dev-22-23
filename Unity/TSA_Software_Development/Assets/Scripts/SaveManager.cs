using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    [SerializeField, ReadOnly] private List<Question> questions;
    [Tooltip("Leave blank for Application.dataPath")]
    [SerializeField] private string path;
    [SerializeField] private string fileName;

    public static SaveManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        questions = new List<Question>();
        Load();
    }

    private void Start()
    {
        AddQuestion(new Question(Random.ColorHSV().ToString(), Random.ColorHSV().ToString()));
    }

    [ContextMenu("Save")]
    public void Save()
    {
        string json = JsonHelper.ToJson<Question>(questions.ToArray(), true);
        if (path.Equals(""))
        {
            File.WriteAllText(Application.dataPath + "/" + fileName + ".json", json);
        }
        else
        {
            File.WriteAllText(path + "/" + fileName + ".json", json);
        }
    }

    public void Load()
    {
        try
        {
            string json = "";
            if (path.Equals(""))
            {
                json = File.ReadAllText(Application.dataPath + "/" + fileName + ".json");
            }
            else
            {
                json = File.ReadAllText(path + "/" + fileName + ".json");
            }
            questions = new List<Question>(JsonHelper.FromJson<Question>(json));
        }
        catch
        {
            Save();
        }
    }

    public Question GetQuestion(int index)
    {
        return questions[index];
    }

    public Question GetQuestion(string prompt)
    {
        foreach (Question q in questions)
        {
            if (q.Prompt.Equals(prompt))
            {
                return q;
            }
        }
        return null;
    }

    public int GetIndex(string prompt)
    {
        for (int i = 0; i < questions.Count; i++)
        {
            if (questions[i].Prompt.Equals(prompt))
            {
                return i;
            }
        }
        return -1;
    }

    public void SetQuestion(int index, Question question)
    {
        questions[index] = question;
        Save();
    }

    public void ChangePrompt(int index, string prompt)
    {
        Question q = questions[index];
        questions[index] = new Question(prompt, q.Response);
        Save();
    }

    public void ChangeResponse(int index, string response)
    {
        Question q = questions[index];
        questions[index] = new Question(q.Prompt, response);
        Save();
    }

    public void AddQuestion(Question question)
    {
        questions.Add(question);
        Save();
    }

    public void InsertQuestion(int index, Question question)
    {
        questions.Insert(index, question);
        Save();
    }

    public void RemoveQuestion(Question question)
    {
        questions.Remove(question);
        Save();
    }

    public void RemoveQuestion(int index)
    {
        questions.RemoveAt(index);
        Save();
    }
}
