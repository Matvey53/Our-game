using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Heatlh : MonoBehaviour
{
    public Image bar;
    public float fill;
    public GameObject Dead;
    public GameObject Chouse;
    public Animator anim;
    public GameObject Close;
    // Start is called before the first frame update
    void Start()
    {
        fill = 1f;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        fill -= Time.deltaTime * 1/8;
        bar.fillAmount = fill;
        if (fill == 0)
        {
            SceneManager.LoadScene("MAIN");
        }
    }
    void GameOver()
    {
        
    }

}

