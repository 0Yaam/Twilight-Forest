using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private GameObject grapeProjectileShadow;
    [SerializeField] private GameObject splatterPrefab;

    private void Start()
    {
        GameObject grapeShadow =
        Instantiate(grapeProjectileShadow, transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);

        Vector3 playerPos = GetPlayerFootPosition();
        Vector3 grapeShadowStartPosition = grapeShadow.transform.position;

        StartCoroutine(ProjectileCurveRoutine(transform.position, playerPos));
        StartCoroutine(MoveGrapeShadowRoutine(grapeShadow, grapeShadowStartPosition, playerPos));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0f, height);

            yield return null;
        }
        Instantiate(splatterPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator MoveGrapeShadowRoutine(GameObject grapeShadow, Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            grapeShadow.transform.position = Vector2.Lerp(startPosition, endPosition, linearT);
            yield return null;
        }

        Destroy(grapeShadow);
    }

    private Vector3 GetPlayerFootPosition()
    {
        if (PlayerController.Instance == null)
        {
            return transform.position;
        }

        Transform playerTransform = PlayerController.Instance.transform;
        Collider2D playerCollider = PlayerController.Instance.GetComponent<Collider2D>();

        if (playerCollider == null)
        {
            Debug.LogWarning("GrapeProjectile could not find Player Collider2D. Falling back to player transform position.");
            return playerTransform.position;
        }

        Bounds bounds = playerCollider.bounds;
        return new Vector3(bounds.center.x, bounds.min.y, playerTransform.position.z);
    }
}
