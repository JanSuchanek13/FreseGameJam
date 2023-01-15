using UnityEngine;

public class HoverSeeThroughWalls : MonoBehaviour
{
    [SerializeField] Material seeThroughMaterial;
    Material myMaterial;
    private void Awake()
    {
        myMaterial = gameObject.GetComponent<MeshRenderer>().material;
    }
    private void OnMouseOver()
    {
        //gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().material = seeThroughMaterial;

        gameObject.GetComponent<BoxCollider>().enabled = false;
        Invoke("RestartCollider", 1f);
    }
    // has to be restarted like this or it will jump on/off unstoppable
    void RestartCollider()
    {
        //gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().material = myMaterial;

        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}



//***************************************


/*
   Debug.Log("mouseIsOver");
   Vector2 cutoutPos = mainCamera.WorldToViewportPoint(myPos);
   Debug.Log("step 1 reached");

   cutoutPos.y /= (Screen.width / Screen.height);
   Debug.Log("step 2 reached");

   Vector3 offset = myPos - cameraObject.transform.position;
   Debug.Log("step 3 reached");

   RaycastHit[] hitObjects = Physics.RaycastAll(cameraObject.transform.position, offset, offset.magnitude, wallMask);
   Debug.Log("step 4 reached");

   for (int i = 0; i < hitObjects.Length; ++i)
   {
       Debug.Log("step 5 reached");

       Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;
       Debug.Log("step 6 reached");

       for (int m = 0; m < materials.Length; ++m)
       {
           Debug.Log("step 7 reached");

           materials[m].SetVector("_CutoutPos", cutoutPos);
           materials[m].SetFloat("_CutoutSize", 0.1f);
           materials[m].SetFloat("_FalloffSize", 0.05f);
           Debug.Log("step 8 reached");
       }
       Debug.Log("step 9 reached");
   }
   Debug.Log("step 10 reached");*/


/*private void OnMouseExit()
{
    gameObject.GetComponent<Renderer>().enabled = true;
    //gameObject.GetComponent<BoxCollider>().enabled = true;

    //gameObject.SetActive(true);
}*/

/*private LayerMask wallMask;
    private GameObject cameraObject;
    private Camera mainCamera;
    private Vector3 myPos;

    private void Awake()
    {
        //cameraObject = GameObject.Find("Main Camera");
        //mainCamera = cameraObject.GetComponent<Camera>();
        myPos = gameObject.transform.position;
    }*/