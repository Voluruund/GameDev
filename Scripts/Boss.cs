using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public float speed;
    // points per kill
    public int ppk;

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        // Calculate the direction toward the camera and move
        Vector3 direction = (cameraPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Destroy this component logic when hitting an object tagged "Flame"
            // Destroy the gameObject completely after 1s
            DisableCollider();
            PointManager.instance.UpdatePti(ppk);
            Destroy(this);
            Destroy(gameObject, 1f);
            PointManager.instance.pointsText.text = "You Won!!!";
            QuitAfterDelay();
        }
    }

    // disable collider when dead
    void DisableCollider()
    {
        BoxCollider2D boxCollider = this.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
    }

    // wait 3 seconds and then quits the application
    public void QuitAfterDelay()
    {
        StartCoroutine(Quit());
    }

    private IEnumerator Quit()
    {
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
}
