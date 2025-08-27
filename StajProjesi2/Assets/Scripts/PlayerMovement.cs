using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Animator animator;


    private Rigidbody rb;
    private Vector3 dragStart;
    private Vector3 dragEnd;


    public int coinsCollected;

    public float launchForce = 10f;
    public float maxDragDistance = 3f;

    private bool isDragging = false;
    private bool hasMoved = false;

    private TurnManager turnManager;

    [Header("Line Renderer")]
    public LineRenderer lineRenderer;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        turnManager = FindObjectOfType<TurnManager>();

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }
    }

    public void AddCoin()
    {
        coinsCollected++;
    }
    void Update()
    {
        float currentSpeed = rb.velocity.magnitude;
        if (Mathf.Abs(animator.GetFloat("Speed") - currentSpeed) > 0.01f)
        {
            animator.SetFloat("Speed", currentSpeed);
        }

        if (!enabled || hasMoved)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            dragStart = GetMouseWorldPosition();
            isDragging = true;
            if (lineRenderer != null)
                lineRenderer.enabled = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 currentPos = GetMouseWorldPosition();
            Vector3 dragVector = dragStart - currentPos;
            dragVector = Vector3.ClampMagnitude(dragVector, maxDragDistance);

            Vector3 startPoint = transform.position;
            Ray ray = new Ray(startPoint, dragVector.normalized);
            RaycastHit hit;

            float remainingLength = dragVector.magnitude;

            if (Physics.Raycast(ray, out hit, remainingLength))
            {
                Vector3 hitPoint = hit.point;

                Vector3 reflectDir = Vector3.Reflect(dragVector.normalized, hit.normal);
                reflectDir.y = 0f;

                float reflectLength = remainingLength - Vector3.Distance(startPoint, hitPoint);


                lineRenderer.positionCount = 3;
                lineRenderer.SetPosition(0, startPoint);
                lineRenderer.SetPosition(1, hitPoint);
                lineRenderer.SetPosition(2, hitPoint + reflectDir * reflectLength);
            }
            else
            {
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, startPoint);
                lineRenderer.SetPosition(1, startPoint + dragVector);
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            if (lineRenderer != null)
                lineRenderer.enabled = false;

            dragEnd = GetMouseWorldPosition();
            Vector3 launchDir = dragStart - dragEnd;
            float dragDistance = Mathf.Clamp(launchDir.magnitude, 0f, maxDragDistance);
            launchDir.Normalize();

            rb.AddForce(launchDir * dragDistance * launchForce, ForceMode.Impulse);

            hasMoved = true;
            Invoke(nameof(NotifyEndTurn), 1f);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        plane.Raycast(ray, out float distance);
        return ray.GetPoint(distance);
    }

    void NotifyEndTurn()
    {
        hasMoved = false;
        turnManager.EndTurn();
    }

    #region LineSkill

    public IEnumerator UseWallSkill(LineSkillItem skill)
    {
        Debug.Log("Skill hedefleme baþladý");

        GameObject lineObj = Instantiate(skill.linePrefab);
        LineRenderer lr = lineObj.GetComponent<LineRenderer>();
        lr.positionCount = 2;

        bool placed = false;
        Vector3 hitPoint = Vector3.zero;
        bool canPlace = false;

        while (!placed)
        {
            Plane plane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float dist))
            {
                Vector3 targetPoint = ray.GetPoint(dist);
                Vector3 dir = (targetPoint - transform.position).normalized;

                if (Physics.Raycast(transform.position, dir, out RaycastHit hit, skill.maxDistance))
                {
                    hitPoint = hit.point;
                    canPlace = true;
                }
                else
                {
                    hitPoint = transform.position + dir * skill.maxDistance;
                    canPlace = false;
                }

                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, hitPoint);
            }

            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                placed = true;

                Vector3 start = transform.position;
                Vector3 end = hitPoint;
                Vector3 direction = (end - start).normalized;
                float length = Vector3.Distance(start, end);

                GameObject wall = Instantiate(skill.wallPrefab, start, Quaternion.LookRotation(direction));

                wall.transform.localScale = new Vector3(
                    lr.startWidth,
                    wall.transform.localScale.y,
                    0f
                );

                StartCoroutine(AnimateWallGrowFromStart(wall, direction, length, 0.5f));
                Destroy(lineObj);
            }

            yield return null;
        }
    }
    private IEnumerator AnimateWallGrowFromStart(GameObject wall, Vector3 direction, float targetLength, float duration)
    {
        Vector3 startScale = wall.transform.localScale;
        Vector3 finalScale = new Vector3(startScale.x, startScale.y, targetLength);

        Vector3 startPosition = wall.transform.position;

        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            float currentLength = Mathf.Lerp(0f, targetLength, t);

            wall.transform.localScale = new Vector3(startScale.x, startScale.y, currentLength);
            wall.transform.position = startPosition + direction * (currentLength / 2f);

            time += Time.deltaTime;
            yield return null;
        }

        wall.transform.localScale = finalScale;
        wall.transform.position = startPosition + direction * (targetLength / 2f);
    }
    #endregion

    #region ScissorSkill

    public IEnumerator UseScissorSkill(ScissorsSkill skill)
    {
        GameObject lineObj = Instantiate(skill.linePrefab);
        LineRenderer lr = lineObj.GetComponent<LineRenderer>();
        lr.positionCount = 2;

        bool placed = false;
        Vector3 hitPoint = Vector3.zero;
        bool canPlace = false;

        while (!placed)
        {
            Plane plane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float dist))
            {
                Vector3 targetPoint = ray.GetPoint(dist);
                Vector3 dir = (targetPoint - transform.position).normalized;

                if (Physics.Raycast(transform.position, dir, out RaycastHit hit, skill.maxDistance))
                {
                    hitPoint = hit.point;
                    canPlace = true;
                    lr.startColor = lr.endColor = Color.green;
                }
                else
                {
                    hitPoint = transform.position + dir * skill.maxDistance;
                    canPlace = false;
                    lr.startColor = lr.endColor = Color.red;
                }

                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, hitPoint);
            }

            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                placed = true;

                GameObject scissor = Instantiate(skill.scissorPrefab, transform.position, Quaternion.identity);
                Rigidbody rb = scissor.GetComponent<Rigidbody>();
                Vector3 throwDir = (hitPoint - transform.position).normalized;

                rb.AddForce(throwDir * skill.throwForce, ForceMode.Impulse);

                Destroy(lineObj);
            }

            yield return null;
        }
    }

    #endregion

    #region SlimeSkill

    public bool IsSlowed { get; private set; } = false;
    private float slowMultiplier = 0.3f;

    public void ApplySlimeEffect(float turnCount, Transform ChildOther)
    {
        StartCoroutine(SlowPlayer(turnCount, ChildOther));
    }

    private IEnumerator SlowPlayer(float durationInTurns, Transform ChildOther)
    {
        IsSlowed = true;
        float originalLaunchForce = launchForce;
        launchForce *= slowMultiplier;

        int turnsWaited = 0;
        while (turnsWaited < 2)
        {
            yield return new WaitUntil(() => !enabled);
            yield return new WaitUntil(() => enabled);
            turnsWaited++;
        }

        launchForce = originalLaunchForce;
        ChildOther.gameObject.SetActive(false);
        IsSlowed = false;
        
    }

    public IEnumerator UseSlimeSkill(SlimeSkillItem skill)
    {
        GameObject lineObj = new GameObject("Slime Line");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lr.endColor = Color.red;

        bool placed = false;
        Vector3 hitPoint = Vector3.zero;
        bool canPlace = false;

        while (!placed)
        {
            Plane plane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float dist))
            {
                Vector3 targetPoint = ray.GetPoint(dist);
                Vector3 dir = (targetPoint - transform.position).normalized;

                if (Physics.Raycast(transform.position, dir, out RaycastHit hit, skill.maxRadius))
                {
                    hitPoint = hit.point;
                    canPlace = true;
                    lr.startColor = lr.endColor = Color.green;
                }
                else
                {
                    hitPoint = transform.position + dir * skill.maxRadius;
                    canPlace = false;
                    lr.startColor = lr.endColor = Color.red;
                }

                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, hitPoint);
            }

            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                placed = true;

                GameObject slime = Instantiate(skill.slimePrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
                Rigidbody rb = slime.GetComponent<Rigidbody>();

                Vector3 throwDir = (hitPoint - transform.position).normalized;
                rb.AddForce(throwDir * skill.throwForce, ForceMode.Impulse);

                SlimeProjectile slimeProj = slime.GetComponent<SlimeProjectile>();
                slimeProj.owner = this;
                slimeProj.lifetime = skill.slimeLifetime;

                Destroy(lineObj);
            }

            yield return null;
        }
    }

    #endregion

    #region CageSkill
    [SerializeField] Transform raycast_point;
    [SerializeField] Transform cageSpawn_point;
    Vector3 targetPoint;
    RaycastHit hitOtherPlayer;
    bool isCage;
    int cageID;
    GameObject miniCage;
    GameObject cage;
    public IEnumerator UseCageSkill(CageSkill skill)
    {
        GameObject lineObj = new GameObject("Slime Line");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lr.endColor = Color.red;
        bool placed = false;
        Vector3 hitPoint = Vector3.zero;
        bool canPlace = false;
        bool cageLaunch = false;

        while (!placed)
        {
            Plane plane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            

            if (plane.Raycast(ray, out float dist))
            {
                Vector3 targetPoint = ray.GetPoint(dist);
                Vector3 dir = (targetPoint - transform.position).normalized;
                if (Physics.Raycast(raycast_point.position , dir, out hitOtherPlayer, skill.maxRadius))
                {

                    hitPoint = hitOtherPlayer.point;
                    canPlace = true;
                    lr.startColor = lr.endColor = Color.green;
                    if (hitOtherPlayer.collider.gameObject.CompareTag("Player"))
                    {
                        
                        cageLaunch = true;
                        hitPoint = hitOtherPlayer.point;
                        

                    }
                }
                else
                {
                    hitPoint = transform.position + dir * skill.maxRadius;
                    canPlace = false;
                    lr.startColor = lr.endColor = Color.red;
                }

                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, hitPoint);
            }

            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                placed = true;

                if(cageLaunch)
                {
                    cageLaunch = false;
                    CageProjectile(hitPoint, skill);
                    isCage = hitOtherPlayer.transform.gameObject.GetComponent<PlayerController>().myPlayer.inCage;
                    //cage ýdsini eþitle //
                    cageID = hitOtherPlayer.transform.gameObject.GetComponent<PlayerController>().myPlayer.playerID;

                    
                    


                }
                Destroy(lineObj);





            }

            yield return null;
        }
    }
    IEnumerator CageSpawnObject(CageSkill skill, Vector3 hitPoint)
    {
        yield return new WaitForSeconds(1f);
        cage = Instantiate(skill.cageObject, hitPoint, Quaternion.identity);


        

    }

    public void CageProjectile(Vector3 hitPoint, CageSkill skill)
    {
        targetPoint = hitPoint;
        miniCage = Instantiate(skill.miniCageObject, cageSpawn_point.position , Quaternion.identity);
        miniCage.transform.LookAt(targetPoint);
        //nesneyi hedef yönünde fýrlatmamýz kaldý 

        InvokeRepeating(nameof(MoveObject), 0f, 0.01f);
        StartCoroutine(CageSpawnObject(skill, targetPoint));
    }

    void MoveObject()
    {
        float cageSpeed = 7f;
        if(miniCage != null)
        {
            miniCage.transform.position = Vector3.MoveTowards(miniCage.transform.position,targetPoint, cageSpeed * Time.deltaTime);
            if (Vector3.Distance(miniCage.transform.position, targetPoint) < 0.1f)
            {
            miniCage.transform.position = targetPoint;
            Destroy(miniCage);
            CancelInvoke(nameof(MoveObject));   

                // Ýstersen burada objeyi yok et veya baþka bir iþlem yap
                // Destroy(miniCage);
            }

        }  
    }
    public void CageDestroy(bool isCages, int ID)
    {
        if (cageID == ID  && cage != null)
        {
            {
                Debug.Log("giremedi");
                if (isCages)
                {
                    Debug.Log("GÝRDÝ");
                    Destroy(cage);
                    isCages = false;
                }
                isCages = true;

            }
        }
    }
    #endregion
}
