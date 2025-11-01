using UnityEngine;

public class StartBotton: MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public void StartLevel()
    {
        
        var pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in enemies)
        {
                var em = e.GetComponent<EnemyMovement>();
                if (em != null)
                {
                    em.canMove = true;
                }
        }
            pc.starGame = true;
            gameObject.SetActive(false);
        }

     
        
    }
}
