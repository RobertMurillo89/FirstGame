using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacoTestingmeshswop : MonoBehaviour
{
    [SerializeField] GameObject Idel;
    [SerializeField] GameObject Attack1;
    [SerializeField] GameObject Attack2;
    [SerializeField] GameObject Attack3;
    [SerializeField] GameObject Pain1;
    [SerializeField] GameObject Pain1f;
    [SerializeField] GameObject Pain2;
    [SerializeField] GameObject Pain2f;

    // Start is called before the first frame update
    void Start()
    {
        Attack1.SetActive(false);
        Attack2.SetActive(false);
        Attack3.SetActive(false);
        Pain1.SetActive(false);
        Pain2.SetActive(false);
        Pain1f.SetActive(false);
        Pain2f.SetActive(false);
        Idel.SetActive(true);
        MIdel();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void rollcall()
    {
        while (true)
        {
            Invoke("MIdel", 1);
            Invoke("MAtt1", 1);
            Invoke("MAtt2", 1);
            Invoke("MAtt3", 1);
            Invoke("MIdel", 1);
            Invoke("MPain1", 1);
            Invoke("MPain1f", 1);
            Invoke("MPain2", 1);
            Invoke("MPain2f", 1);
            Invoke("MPain1", 1);
            //Invoke("MIdel", 0.285714f);
            //Invoke("MAtt1", 0.142857f);
            //Invoke("MAtt2", 0.142857f);
            //Invoke("MAtt3", 0.142857f);
            //Invoke("MIdel", 0.285714f);
            //Invoke("MPain1", 0.028571f);
            //Invoke("MPain1f", 0.028571f);
            //Invoke("MPain2", 0.028571f);
            //Invoke("MPain2f", 0.028571f);
            //Invoke("MPain1", 0.057142f);
        }
    }
    void MIdel()
    {
        Attack3.SetActive(false);
        Pain1.SetActive(false);
        Pain2f.SetActive(false);
        Idel.SetActive(true); 
        Invoke("MAtt1", 0.142857f);
    }

    void MAtt1()
    {
        Pain1.SetActive(false);
        Idel.SetActive(false);
        Attack1.SetActive(true);
        Invoke("MAtt2", 0.142857f);
    }

    void MAtt2()
    {
        Attack1.SetActive(false);
        Attack2.SetActive(true);
        Invoke("MAtt3", 0.142857f);
    }
    void MAtt3()
    {
        Attack2.SetActive(false);
        Attack3.SetActive(true);
        Invoke("MPain1", 0.028571f);
    }

    void MPain1()
    {
        Attack1.SetActive(false);
        Attack2.SetActive(false);
        Attack3.SetActive(false);
        Pain1.SetActive(false);
        Pain2.SetActive(false);
        Pain1f.SetActive(false);
        Pain2f.SetActive(false);
        Pain1.SetActive(true);
        Invoke("MPain1f", 0.028571f);
    }

    void MPain1f()
    {
        Pain2f.SetActive(false);
        Pain1.SetActive(false);
        Pain1f.SetActive(true);
        Invoke("MPain2", 0.028571f);
    }

    void MPain2()
    {
        Pain1f.SetActive(false);
        Pain2.SetActive(true);
        Invoke("MPain2f", 0.028571f);
    }
    void MPain2f()
    {
        Pain2.SetActive(false);
        Pain2f.SetActive(true);
        Invoke("MIdel", 0.285714f);
    }

}
