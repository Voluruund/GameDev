using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy, enemy1, enemy2, enemy3, boss;
    private int types = 4;
    private int[] rates = { 1, 6, 8, 12 };
    private bool isBossfight = false;
    private float timer = 0f;
    private float delay = 0f;
    private bool isWaiting = false;
    private int selectedType = -1;
    public GameObject player;
    private PointManager PointManager;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        Spawner(0);
        StartCoroutine("BossFight");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
        }
        if (isWaiting)
        {
            timer += Time.deltaTime;

            // Check if we reached the random delay time between 0.8 and 2 seconds
            if (timer >= delay)
            {
                // Perform the selection after the delay
                selectedType = SelectBasedOnRates(rates);
                Debug.Log($"Selected number: {selectedType}");

                // Reset the timer and waiting state
                isWaiting = false;
                timer = 0f;
                Spawner(selectedType);
            }
        }
        else
        {
            // Trigger a new selection when the waiting process is done
            // Generate a random delay between 0.8 and 2 seconds
            delay = Random.Range(0.8f, 2f);
            isWaiting = true;
            Debug.Log($"Waiting for {delay} seconds...");
        }
    }

    private int SelectBasedOnRates(int[] rates)
    {
        // Calculate the total sum of rates
        int total = 0;
        foreach (int rate in rates)
        {
            total += rate;
        }

        // Generate a random number between 1 and total
        int randomValue = Random.Range(1, total + 1);

        // Determine the selected number based on rates
        int cumulative = 0;
        for (int i = 0; i < rates.Length; i++)
        {
            cumulative += rates[i];
            if (randomValue <= cumulative)
            {
                return i;
            }
        }

        return -1; // This should never be reached
    }

    private void Spawner(int type)
    {
        // if currently fighting the boss, spawner stops
        if (isBossfight || PointManager.instance.getPti() > 20000) { return; }
        float lowerOffsetY = transform.position.y - 10;
        float lowerOffsetX = transform.position.x - 10;
        float upperOffsetY = transform.position.y + 10;
        float upperOffsetX = transform.position.x + 10;
        GameObject enemy = null;
        if (type == 0)
        {
            enemy = Instantiate(enemy1, new Vector3(Random.Range(lowerOffsetX, upperOffsetX), Random.Range(lowerOffsetY, upperOffsetY), 0), transform.rotation);
            enemy.GetComponent<Enemy1>().speed = 10;
            enemy.GetComponent<Enemy1>().ppk = 50;
        }
        else if (type == 1)
        {
            enemy = Instantiate(enemy2, new Vector3(Random.Range(lowerOffsetX, upperOffsetX), Random.Range(lowerOffsetY, upperOffsetY), 0), transform.rotation);
            enemy.GetComponent<Enemy1>().speed = 7;
            enemy.GetComponent<Enemy1>().ppk = 200;
        }
        else if (type == 2)
        {
            enemy = Instantiate(enemy3, new Vector3(Random.Range(lowerOffsetX, upperOffsetX), Random.Range(lowerOffsetY, upperOffsetY), 0), transform.rotation);
            enemy.GetComponent<Enemy1>().speed = 4;
            enemy.GetComponent<Enemy1>().ppk = 500;
        }
        // horde logic, spawns 2 of each
        else if (type == 3)
        {
            enemy = Instantiate(enemy1, new Vector3(Random.Range(lowerOffsetX, upperOffsetX), Random.Range(lowerOffsetY, upperOffsetY), 0), transform.rotation);
            enemy.GetComponent<Enemy1>().speed = 10;
            enemy.GetComponent<Enemy1>().ppk = 50;
            enemy = Instantiate(enemy2, new Vector3(Random.Range(lowerOffsetX, upperOffsetX), Random.Range(lowerOffsetY, upperOffsetY), 0), transform.rotation);
            enemy.GetComponent<Enemy1>().speed = 7;
            enemy.GetComponent<Enemy1>().ppk = 200;
            enemy = Instantiate(enemy3, new Vector3(Random.Range(lowerOffsetX, upperOffsetX), Random.Range(lowerOffsetY, upperOffsetY), 0), transform.rotation);
            enemy.GetComponent<Enemy1>().speed = 4;
            enemy.GetComponent<Enemy1>().ppk = 500;
            enemy = Instantiate(enemy1, new Vector3(Random.Range(lowerOffsetX, upperOffsetX), Random.Range(lowerOffsetY, upperOffsetY), 0), transform.rotation);
            enemy.GetComponent<Enemy1>().speed = 10;
            enemy.GetComponent<Enemy1>().ppk = 50;
            enemy = Instantiate(enemy2, new Vector3(Random.Range(lowerOffsetX, upperOffsetX), Random.Range(lowerOffsetY, upperOffsetY), 0), transform.rotation);
            enemy.GetComponent<Enemy1>().speed = 7;
            enemy.GetComponent<Enemy1>().ppk = 200;
            enemy = Instantiate(enemy3, new Vector3(Random.Range(lowerOffsetX, upperOffsetX), Random.Range(lowerOffsetY, upperOffsetY), 0), transform.rotation);
            enemy.GetComponent<Enemy1>().speed = 4;
            enemy.GetComponent<Enemy1>().ppk = 500;
        }
        else
        {
            return;    
        }
    }

    // todo
    private IEnumerator BossFight()
    {
        yield return new WaitUntil(()=> PointManager.instance.getPti() > 20000);
        isBossfight = true;
        enemy = Instantiate(boss, new Vector3(transform.position.x, transform.position.y + 20, 0), transform.rotation);
        enemy.GetComponent<Boss>().speed = 4;
        enemy.GetComponent<Boss>().ppk = 10000;
    }
}
