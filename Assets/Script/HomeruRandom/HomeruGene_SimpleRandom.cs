using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ただの乱数で選択
[System.Serializable]
public class HomeruGene_SimpleRandom : AbstractHomeru_randomGenerator
{
    public override string GetRandomWord()
    {
        float rand = Random.Range(0, 100f);
        float unit = 100f / _wordList.Count;

        for(int unitCount = 0; unitCount < _wordList.Count; unitCount++)
        {
            if (rand < unit * (unitCount+1))
            {
                return _wordList[unitCount];
            }
        }
        return "";
    }
}
