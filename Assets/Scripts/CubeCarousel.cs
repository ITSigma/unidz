using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CubeCarousel : MonoBehaviour
{
    [SerializeField] private float rightSpeed;
    [SerializeField][Range(0, 8)] private float radius;
    [SerializeField][Min(1)] private int cubeCount;
    [SerializeField] private Cube prefab;

    private readonly List<Cube> cubes = new();

    private const float FullCircleAngle = 360f;

    void Awake()
    {
        for (var i = 0; i < cubeCount; i++)
        {
            var cube = Instantiate(prefab, transform);
            cubes.Add(cube);
        }

        UpdateCarouselView();
    }

    void FixedUpdate()
    {
        var angle = rightSpeed / FullCircleAngle;
        transform.Rotate(0, 0, angle);
    }

    private void OnValidate()
    {
        UpdateCarouselView();
    }

    private void UpdateCarouselView()
    {
        if (cubes.Count == 0)
            return;

        var angle = FullCircleAngle / cubeCount;

        for (var i = 0; i < cubeCount; i++)
        {
            var cubeAngle = angle * i;
            cubes[i].transform.localRotation = Quaternion.Euler(0, 0, cubeAngle);
            cubes[i].transform.localPosition = Vector3.up * math.sin(math.radians(cubeAngle)) * radius
                + Vector3.right * math.cos(math.radians(cubeAngle)) * radius;
        }
    }
}