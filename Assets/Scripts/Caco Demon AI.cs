using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caco_Demon_AI : MonoBehaviour, IDamage
{

    [SerializeField] GameObject Idel;
    [SerializeField] GameObject Attack1;
    [SerializeField] GameObject Attack2;
    [SerializeField] GameObject Attack3;
    [SerializeField] GameObject Pain1;
    [SerializeField] GameObject Pain1f;
    [SerializeField] GameObject Pain2;
    [SerializeField] GameObject Pain2f;
    [SerializeField] int HP;
    [SerializeField] int Speed;

    [SerializeField] Renderer model;

    // Start is called before the first frame update
    void Start()
    {
        //report to the game manager that you exist.
        GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.player.transform.position, Time.deltaTime * Speed);
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            GameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
}
