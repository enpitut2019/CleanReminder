using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AbstractHomeru_randomGenerator
{
    [SerializeField] protected List<string> _wordList = new List<string>();
    
    public abstract string GetRandomWord();
    
    public void AddWord(string _word)
    {
        _wordList.Add(_word);
    }

    public void RemoveWord(string word)
    {
        _wordList.Remove(word);
    }
}
