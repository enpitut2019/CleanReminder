using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeruController : MonoBehaviour
{
    //[SerializeField] AbstractHomeru_randomGenerator _homeruGenerator = new HomeruGene_SimpleRandom();
    [SerializeField] HomeruGene_SimpleRandom _homeruGenerator = new HomeruGene_SimpleRandom();





    [ContextMenu("randomTest")]
    public void RandomTest()
    {
        Debug.Log(_homeruGenerator.GetRandomWord());
    }
}
