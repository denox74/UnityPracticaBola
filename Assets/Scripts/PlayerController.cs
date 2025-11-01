using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    public TMP_Text countText;
    public TMP_Text winText;
    public TMP_Text loseText;
    public AudioClip pickupSound;
    public AudioClip endGameSound;
    public bool isGrounded;
    public bool starGame = false;
    public float speed = 10.0f;
    public float jumpForce = 6;
    public GameObject enemy;
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    private bool isWinning = false;

    private bool isResetting = false;
    


    //Start

    void Start()
    {
        count = 0;
        SetCountText();
        rb = GetComponent<Rigidbody>();
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;

    }   

    void OnJump(InputValue jumpValue)
    {
        if (isGrounded && starGame)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        if (starGame){
            Vector3 movement = new Vector3(movementX, 0.0f, movementY);
            rb.AddForce(movement * speed);

            if (transform.position.y < -10 && !isResetting)
            {
                isResetting = true;
                StartCoroutine(ResetGame());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(ResetGame());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            other.gameObject.SetActive(false);

            count++;
            SetCountText();
        }

    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            winText.gameObject.SetActive(true);
            StartCoroutine(NextStage());
        }
    }

    private IEnumerator NextStage()
    {
        isWinning = true;
        yield return new WaitForSeconds(2);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;

        SceneManager.LoadScene(nextSceneIndex);
    }
    
    private IEnumerator ResetGame()
    {
        if (!isWinning){
        loseText.gameObject.SetActive(true);
        rb.linearVelocity = Vector3.zero;     
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        AudioSource.PlayClipAtPoint(endGameSound, transform.position);
        yield return new WaitForSeconds(2);
         rb.constraints = RigidbodyConstraints.None;
         int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    int nextSceneIndex = currentSceneIndex;

    if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        nextSceneIndex = 0;
    SceneManager.LoadScene(nextSceneIndex);
    }
}
}
