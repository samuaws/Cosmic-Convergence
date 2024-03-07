using UnityEngine;

public class Abilities : MonoBehaviour
{
    public GameObject effectPrefab;
    public float sphereCastRadius = 0.5f;
    public float sphereCastDistance = 10f;

    private Camera cam;
    private GameObject abilityPlaceholder;
    private bool casting = false;

    private GameObject spellToSpawn;
    public GameObject supernova;
    private Vector3 spellPosition;
    private Vector3 spellSpawnOffset = new Vector3(0,1.2f,0);
    private float spellDuration;
    public RagdollOnOff rag;

    public enum Spells
    {
        supernova,
        blackhole,
        wormhole
    }
    void Start()
    {
        cam = Camera.main;
    }
    public void TogleCasting()
    {
        casting = !casting;
        Destroy(abilityPlaceholder);
    }
    void Update()
    {
        if (casting)
        {

        
            // Perform sphere cast from the camera
            RaycastHit hit;
            if (Physics.SphereCast(cam.transform.position, sphereCastRadius, cam.transform.forward, out hit, sphereCastDistance))
            {
                // Sphere cast hit something
                Debug.Log("Sphere cast hit: " + hit.point);
                // Instantiate prefab at the point of contact
                if (abilityPlaceholder == null)
                {
                    abilityPlaceholder = Instantiate(effectPrefab, hit.point, Quaternion.identity);
                }
                else
                {
                    // Update position of the instantiated prefab
                    abilityPlaceholder.transform.position = hit.point ;
                }
                spellPosition = hit.point;
            }
            else
            {
                // If sphere cast didn't hit anything, shoot ray downwards from the edge of the first raycast
                Vector3 edgePosition = cam.transform.position + cam.transform.forward * sphereCastDistance;
                Ray edgeRay = new Ray(edgePosition, Vector3.down);
                RaycastHit edgeHit;
                if (Physics.Raycast(edgeRay, out edgeHit))
                {
                    // Ray from the edge of sphere cast hit the ground
                    Debug.Log("Fallback ray hit ground at: " + edgeHit.point);
                    // Instantiate prefab at the point of contact
                    if (abilityPlaceholder == null)
                    {
                        abilityPlaceholder = Instantiate(effectPrefab, edgeHit.point, Quaternion.identity);
                    }
                    else
                    {
                        // Update position of the instantiated prefab
                        abilityPlaceholder.transform.position = edgeHit.point;
                    }
                    spellPosition = edgeHit.point;
                }
                else
                {
                    // Fallback ray didn't hit anything
                    Debug.Log("Fallback ray didn't hit anything");
                }
            }
        }
    }

    public void Supernova()
    {
        spellToSpawn = supernova;
        spellDuration = 2;
        TogleCasting();
    }
    public void CastAbility()
    {
        if (casting)
        {
            var spell = Instantiate(spellToSpawn, spellPosition + spellSpawnOffset, Quaternion.identity);
            Destroy(spell , spellDuration);
            TogleCasting();
        }
    }
}
