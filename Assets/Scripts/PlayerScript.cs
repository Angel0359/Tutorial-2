using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public float jumpForce;

    public Text score;
    public Text winText;
    public Text livesText;
    private int scoreValue = 0;
    private int lives;

    public AudioSource musicSource;
    public AudioClip musicTheme;
    public AudioClip musicVictory;

    private bool isOnGround;
    private bool levelTwo;
    private bool themeMusic;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    Animator anim;
    private bool facingRight = true;
    private bool inTheAir = false;
    private bool hitGround = false;



    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        winText.text = "";
        lives = 3;
        SetLivesText();
        levelTwo = false;
        musicSource.clip = musicTheme;
        musicSource.Play();
        themeMusic = true;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKey("escape"))
            {
            Application.Quit();
            }
    }
    
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement > 0)
        {
           Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (inTheAir == true && vertMovement > 0)
        {
            anim.SetInteger("State", 2);
        }
        
        if (inTheAir == false && hitGround == true)
        {
            anim.SetInteger("State", 0);
            hitGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.tag == "Enemy")
        {
            lives = lives -1;
            SetLivesText();
            Destroy(collision.collider.gameObject);
        }
        if (scoreValue == 4 && levelTwo == false)
        {
            transform.position = new Vector2 (-5.1f, 53.5f);
            levelTwo = true;
            lives = 3;
            SetLivesText();
        }

        if (scoreValue == 8 && themeMusic == true)
        {
            winText.text = "Congratulations You Won!" + " " + "Game created by Angel Rodriguez.";
            musicSource.clip = musicVictory;
            musicSource.Play();
            themeMusic = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            inTheAir = false;

            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                inTheAir = true;
                hitGround = true;
            }
        }

    }

    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives <= 0)
        {
            Destroy(gameObject);
            winText.text = "You Lost!";
        }
    }

    void Flip()
   {
    facingRight = !facingRight;
    Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
    transform.localScale = Scaler;
   }
}
