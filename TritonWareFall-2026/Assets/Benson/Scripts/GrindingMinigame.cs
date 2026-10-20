using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GrindingMinigame : MonoBehaviour
{
    [Header("Scene refs")]
    [SerializeField] RectTransform needlePivot;
    [SerializeField] Image zoneMaskImage;      // on ZoneMask (also has a Mask component)
    [SerializeField] RectTransform zoneVisual; // child with the HitZone sprite

    [Header("Input")]
    [SerializeField] KeyCode hitKey = KeyCode.Space;
    [SerializeField] float inputGrace = 0.25f; // stops the interact key from instantly counting

    [Header("Tuning")]
    [SerializeField] float startSpeed = 110f;  // degrees per second
    [SerializeField] float speedUpPerHit = 12f;
    [SerializeField] float startZoneWidth = 36f; // degrees
    [SerializeField] float zoneShrinkPerHit = 3f;
    [SerializeField] float minZoneWidth = 12f;
    [SerializeField] float hitPadding = 2f;    // forgiveness in degrees
    [SerializeField] float minZoneJump = 45f;  // new zone must move at least this far
    [SerializeField] int hitsToWin = 5;
    [SerializeField] int missesToLose = 3;

    public event Action<bool> Finished;

    // Angles: 0 = far left, 90 = straight up, 180 = far right.
    float angle, dir, speed, zoneWidth, zoneStart, armedAt;
    int hits, misses;
    bool running;

    void Awake()
    {
        // Mask needs a sprite; a white one lets the radial fill act as a clean wedge.
        var tex = Texture2D.whiteTexture;
        zoneMaskImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        zoneMaskImage.type = Image.Type.Filled;
        zoneMaskImage.fillMethod = Image.FillMethod.Radial360;
        zoneMaskImage.fillOrigin = (int)Image.Origin360.Left; // wedge starts at 9 o'clock
        zoneMaskImage.fillClockwise = true;
        zoneMaskImage.GetComponent<Mask>().showMaskGraphic = false;
    }

    public void Begin()
    {
        hits = misses = 0;
        angle = 0f; dir = 1f;
        speed = startSpeed;
        zoneWidth = startZoneWidth;
        zoneStart = -999f;
        PlaceZone();
        ApplyNeedle();
        armedAt = Time.time + inputGrace;
        running = true;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!running) return;

        angle += dir * speed * Time.deltaTime;
        if (angle >= 180f) { angle = 180f; dir = -1f; }
        else if (angle <= 0f) { angle = 0f; dir = 1f; }
        ApplyNeedle();

        if (Time.time >= armedAt && Input.GetKeyDown(hitKey)) Resolve();
    }

    void Resolve()
    {
        bool inZone = angle >= zoneStart - hitPadding && angle <= zoneStart + zoneWidth + hitPadding;
        if (inZone)
        {
            hits++;
            speed += speedUpPerHit;
            zoneWidth = Mathf.Max(minZoneWidth, zoneWidth - zoneShrinkPerHit);
            if (hits >= hitsToWin) { Finish(true); return; }
            PlaceZone();
        }
        else
        {
            misses++;
            if (misses >= missesToLose) Finish(false);
        }
    }

    void PlaceZone()
    {
        float max = 180f - zoneWidth;
        float candidate = zoneStart;
        for (int i = 0; i < 12; i++)
        {
            candidate = Random.Range(0f, max);
            if (Mathf.Abs(candidate - zoneStart) >= minZoneJump) break;
        }
        zoneStart = candidate;

        zoneMaskImage.fillAmount = zoneWidth / 360f;
        zoneMaskImage.rectTransform.localRotation = Quaternion.Euler(0, 0, -zoneStart); // rotate wedge clockwise
        zoneVisual.localRotation = Quaternion.Euler(0, 0, zoneStart);                   // keep the sprite upright
    }

    void ApplyNeedle() => needlePivot.localRotation = Quaternion.Euler(0, 0, 90f - angle);

    void Finish(bool won)
    {
        running = false;
        Finished?.Invoke(won);
        gameObject.SetActive(false);
    }
}