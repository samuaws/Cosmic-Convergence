using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Abilities : MonoBehaviour
{
    public float sphereCastRadius = 0.5f;
    public float sphereCastDistance = 10f;

    private Camera cam;
    private GameObject abilityPlaceholder;
    private bool casting = false;

    public float supernovaCooldown = 2f;
    public float blackholeCooldown = 5f;

    public GameObject supernova;
    public GameObject supernovaPlaceholder;
    private GameObject placeholderPrefab;
    public GameObject blackhole;
    public GameObject blackholePlaceholder;

    private GameObject spellToSpawn;
    private Spells spellToCast;
    private Vector3 spellPosition;
    private Vector3 spellSpawnOffset = new Vector3(0, 1.2f, 0);
    private float spellDuration;
    private float[] abilityCooldowns;

    public TextMeshProUGUI supernovaCooldownText;
    public TextMeshProUGUI blackholeCooldownText;
    // Add more UI Text variables for other spells if needed

    public enum Spells
    {
        supernova,
        blackhole,
        nothing
        // Add more spells here if needed
    }

    void Start()
    {
        cam = Camera.main;
        InitializeCooldowns();
    }

    void InitializeCooldowns()
    {
        abilityCooldowns = new float[System.Enum.GetNames(typeof(Spells)).Length];
    }

    public void ToggleCasting()
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
                // Instantiate prefab at the point of contact
                if (abilityPlaceholder == null)
                {
                    abilityPlaceholder = Instantiate(placeholderPrefab, hit.point, Quaternion.identity);
                }
                else
                {
                    // Update position of the instantiated prefab
                    abilityPlaceholder.transform.position = hit.point;
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
                        abilityPlaceholder = Instantiate(placeholderPrefab, edgeHit.point, Quaternion.identity);
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
        UpdateCooldownUI();
    }

    void UpdateCooldownUI()
    {
        UpdateSpellCooldownUI(Spells.supernova, supernovaCooldownText);
        UpdateSpellCooldownUI(Spells.blackhole, blackholeCooldownText);
        // Update other spell cooldown UI if needed
    }

    void UpdateSpellCooldownUI(Spells spell, TextMeshProUGUI cooldownText)
    {
        int spellIndex = (int)spell;
        if (abilityCooldowns[spellIndex] > Time.time)
        {
            float remainingTime = abilityCooldowns[spellIndex] - Time.time;
            cooldownText.text = "Cooldown: " + Mathf.CeilToInt(remainingTime).ToString();
        }
        else
        {
            cooldownText.text = "Ready";
        }
    }

    public void Supernova()
    {
        spellToCast = Spells.supernova;
        if (!IsOnCooldown(spellToCast))
        {
            spellToSpawn = supernova;
            placeholderPrefab = supernovaPlaceholder;
            spellDuration = 2;
            ToggleCasting();
        }
    }

    public void Blackhole()
    {
        spellToCast = Spells.blackhole;
        if (!IsOnCooldown(spellToCast))
        {
            spellToSpawn = blackhole;
            placeholderPrefab = blackholePlaceholder;
            spellSpawnOffset = new Vector3(0, 1.5f, 0);
            spellDuration = 5;
            ToggleCasting();
        }
    }
    public void CancelCasting()
    {
        spellToCast = Spells.nothing;
        spellToSpawn = null;
        casting = false;
    }

    bool IsOnCooldown(Spells spell)
    {
        int spellIndex = (int)spell;
        return abilityCooldowns[spellIndex] > Time.time;
    }

    void StartCooldown(Spells spell)
    {
        int spellIndex = (int)spell;
        abilityCooldowns[spellIndex] = Time.time + GetCooldownTime(spell);
    }

    float GetCooldownTime(Spells spell)
    {
        switch (spell)
        {
            case Spells.supernova:
                return supernovaCooldown; // Cooldown time for supernova
            case Spells.blackhole:
                return blackholeCooldown; // Cooldown time for blackhole
            default:
                return 0f;
        }
    }

    public void CastAbility()
    {
        if (casting)
        {
            if (!IsOnCooldown(spellToCast))
            {
                var spell = Instantiate(spellToSpawn, spellPosition + spellSpawnOffset, Quaternion.identity);
                Destroy(spell, spellDuration);
                StartCooldown(spellToCast);
                ToggleCasting();
            }
        }
    }
}
