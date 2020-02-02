using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public int index = 0;

    public Material[] materials;
    public Vector2 randX = Vector2.zero;
    public Vector2 randZ = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        if(Input.GetKeyDown(KeyCode.A) && index == 0)
        {
            var block = Instantiate(prefab, this.transform.position, this.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.S) && index == 1)
        {
            Instantiate(prefab, this.transform.position, this.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.D) && index == 2)
        {
            Instantiate(prefab, this.transform.position, this.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.F) && index == 3)
        {

        }
    }

    public void Spawn(int type, int followerCount)
    {
        var block =  
        Instantiate(prefab, new Vector3(this.transform.position.x + Random.Range(randX.x, randX.y), this.transform.position.y, this.transform.position.z + Random.Range(randZ.x, randZ.y)), this.transform.rotation);
        block.GetComponent<MeshRenderer>().material = materials[type];
       
        float moreMass = 0f;

        if(followerCount > 200)
        {
            moreMass = 0.075f;
        }else if(followerCount > 100)
        {
            moreMass = 0.025f;
        }
        Debug.Log("Mass to add " + moreMass);
        block.GetComponent<Rigidbody>().mass += moreMass;
    }

}


