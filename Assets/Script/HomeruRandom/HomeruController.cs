using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class HomeruController : MonoBehaviour
{
    //[SerializeField] AbstractHomeru_randomGenerator _homeruGenerator = new HomeruGene_SimpleRandom();
    [SerializeField] HomeruGene_SimpleRandom _homeruGenerator = new HomeruGene_SimpleRandom();
    [SerializeField] Text homeruWord;

    public void SetHomeruWord()
    {
        homeruWord.text = _homeruGenerator.GetRandomWord();
    }


    [ContextMenu("randomTest")]
    public void RandomTest()
    {
        Debug.Log(_homeruGenerator.GetRandomWord());
    }
}
