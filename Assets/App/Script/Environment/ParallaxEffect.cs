using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Timeline;

public class ParallaxEffect : MonoBehaviour
{
    [Header("The layers can be automatically obtained from the child objects of the GameObject")]
    public Transform[] layers;
    [Header("The clones automatically clone the layers from the child objects")]
    public Transform[] clones;
    [Header("The speed of the background parallax")]
    public float parallaxSpeed = 0.2f;
    private bool isForestPhase = true;
    private bool otherPhase;
    private float layerWidth;
    private float startPos;
    private float startPosClone;
    private bool needInit = false;


    private void Awake()
    {
        int childCount = transform.childCount;
        if (childCount <= 0)
        {
            Debug.Log("No child layers found for ParallaxEffect");
            return;
        }

        if (transform.Find("FieldPhase") != null && transform.Find("ForestPhase") != null)
        {
            SetUpLandPhase(isForestPhase);
            otherPhase = !isForestPhase;
        }
        else
        {
            otherPhase = true;
            isForestPhase = !otherPhase;
            OtherSetUpPhase(otherPhase);
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying) return;

        if (needInit)
        {
            SetUpLandPhase(isForestPhase);
            needInit = false;
            return;   // stop one frame so that new data is used in the next loop
        }

        Vector3 move = parallaxSpeed * Time.deltaTime * Vector3.left;
        for (int i = 0; i < layers.Length; i++)
        {
            // Move both the original layer and its clone
            layers[i].position += move * (i == 0 ? parallaxSpeed : i);
            clones[i].position += move * (i == 0 ? parallaxSpeed : i);

            // return to the starting point when it is at its limit
            if (layers[i].position.x <= startPos - layerWidth)
            {
                layers[i].position = new Vector2(startPos, layers[i].position.y);
            }
            if (clones[i].position.x <= startPosClone - layerWidth)
            {
                clones[i].position = new Vector2(startPosClone, clones[i].position.y);
            }
        }
    }

    public void TrueForForestAndFalseForField(bool value) // This function for change phase to forest or field, true = forest, false = field
    {
        if (value == isForestPhase) return;
        isForestPhase = value;

        if (clones != null)
        {
            foreach (var c in clones)
            {
                if (c != null)
                    Destroy(c.gameObject);
            }
        }

        needInit = true;
    }

    private void OtherSetUpPhase(bool OtherPhase)
    {
        // This function for air parallax and water parallax, so just change or add||reduce the layer in gameObject AirParallax or WaterParallax
        if (OtherPhase)
        {
            int count = transform.childCount;
            layers = new Transform[count];
            clones = new Transform[count];

            for (int i = 0; i < count; i++)
            {
                layers[i] = transform.GetChild(i);

                // Calculate width based on first child's SpriteRenderer
                if (layers[0].TryGetComponent<SpriteRenderer>(out var sr))
                {
                    layerWidth = sr.bounds.size.x;
                    startPos = layers[0].transform.position.x;
                }
                else
                {
                    Debug.Log("SpriteRenderer not found on layers[0]: " + transform.name);
                }

                // Initialize clones layer
                clones[i] = Instantiate(
                    layers[i],
                    layers[i].position + Vector3.right * layerWidth,
                    layers[i].transform.rotation,
                    transform
                );

                startPosClone = clones[0].transform.position.x;
            }
        }
    }

    private void SetUpLandPhase(bool IsForestPhase)
    {
        // for change the phase on land phase (have field parallax and forest parallax)
        Transform fieldContainer = transform.Find("FieldPhase");
        Transform forestContainer = transform.Find("ForestPhase");

        forestContainer.gameObject.SetActive(IsForestPhase);
        fieldContainer.gameObject.SetActive(!IsForestPhase);

        Transform phase = IsForestPhase ? forestContainer : fieldContainer;
        int count = phase.childCount;

        layers = new Transform[count];
        clones = new Transform[count];

        for (int i = 0; i < count; i++)
        {
            layers[i] = phase.GetChild(i);

            // Calculate width based on first child's SpriteRenderer
            if (layers[0].TryGetComponent<SpriteRenderer>(out var sr))
            {
                layerWidth = sr.bounds.size.x;
                startPos = layers[0].transform.position.x;
            }
            else
            {
                Debug.Log("SpriteRenderer not found on layers[0]: " + phase.name);
            }

            // Initialize clones layer
            clones[i] = Instantiate(
                layers[i],
                layers[i].position + Vector3.right * layerWidth,
                layers[i].transform.rotation,
                phase
            );

            startPosClone = clones[0].transform.position.x;
        }
    }
}

