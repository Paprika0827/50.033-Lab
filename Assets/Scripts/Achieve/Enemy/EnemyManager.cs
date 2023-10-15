using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{

    public GameObject goombaPrefab;
    public AudioSource audioSource;
    public AudioClip coin;
    public List<GameObject> gameObjects;
    public Vector3[] goombaPosition = {
        new Vector3(5, 0, 0),
        new Vector3(7, 0, 0),
        new Vector3(0, 0, 0)
        };

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.changeLevel.AddListener(ChangeScene);
        //destroy all game objects
        foreach (GameObject go in gameObjects) {
            Destroy(go);
        }

        gameObjects.Clear();
        //generate goombas
        foreach (Vector3 position in goombaPosition) {
            GenerateGoomba(position);
        }
    }
    public void Reset() {
        Start();
    }
    public void ChangeScene(int scene) {
        goombaPosition = Levels.levels[scene].GoombaPosition;
        Reset();
    }

    private void GenerateGoomba(Vector3 position) {
        GameObject instantiatedChild = Instantiate(goombaPrefab, position + transform.position, Quaternion.identity);
        instantiatedChild.GetComponent<EnemyMovement>().TravelTime = Random.Range(1f, 2f) * (Random.Range(0, 2) * 2 - 1);
        instantiatedChild.transform.SetParent(transform);
        gameObjects.Add(instantiatedChild);
    }

    public GameObject spritePrefab;

    public void PlaceSprite(Vector3 position) {
        GameObject newSprite = Instantiate(spritePrefab, position + Vector3.up*(-0.22f), Quaternion.identity);
        gameObjects.Add(newSprite);
        audioSource.PlayOneShot(coin);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
