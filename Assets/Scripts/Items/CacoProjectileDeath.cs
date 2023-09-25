using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacoProjectileDeath : MonoBehaviour
{
    [SerializeField] GameObject Death1;
    [SerializeField] GameObject Death2;
    [SerializeField] GameObject Death3;
    [SerializeField] GameObject Death4;
    [SerializeField] GameObject Death5;
    [SerializeField] GameObject Death6;
    // Start is called before the first frame update
    void Start()
    {
        Death1.SetActive(true);
        Invoke("MDeath2", 0.057142f);
    }

    void MDeath2()
    {
        Death1.SetActive(false);
        Death2.SetActive(true);
        Invoke("MDeath3", 0.057142f);
    }
    void MDeath3()
    {
        Death2.SetActive(false);
        Death3.SetActive(true);
        Invoke("MDeath4", 0.057142f);
    }
    void MDeath4()
    {
        Death3.SetActive(false);
        Death4.SetActive(true);
        Invoke("MDeath5", 0.057142f);
    }
    void MDeath5()
    {
        Death4.SetActive(false);
        Death5.SetActive(true);
        Invoke("MDeath6", 0.057142f);
    }
    void MDeath6()
    {
        Death5.SetActive(false);
        Death6.SetActive(true);
        Invoke("MDeath", 0.057142f);
    }
    void MDeath()
    {
        Destroy(gameObject);
    }

}
