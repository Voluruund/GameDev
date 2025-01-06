using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class BossLevel : MonoBehaviour
{
    public int sceneIndex; // Index of the new scene
    private bool isLoadingScene = false;
    private static GameLevel instance;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isLoadingScene)
        {
            isLoadingScene = true;
            Debug.Log("Switching scene");
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to sceneLoaded event
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single); // Load the new scene
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the spawn point in the new scene
        GameObject spawnPoint = GameObject.FindWithTag("Respawn");
        if (spawnPoint != null)
        {
            // Move the player to the spawn point
            GameObject player = GameObject.FindWithTag("Player");
            // Teleport the player to spawn point and make sure it’s at the exact position
            if (player != null)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                Debug.Log("Player teleported to spawn point.");
                playerController.Teleport(spawnPoint.transform.position);
                Debug.Log("Diocane: " + spawnPoint.transform.position);
            }
        }
        else
        {
            Debug.LogWarning("No spawn point found in the new scene.");
        }

        // Unsubscribe from the event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
        isLoadingScene = false;
    }
}
