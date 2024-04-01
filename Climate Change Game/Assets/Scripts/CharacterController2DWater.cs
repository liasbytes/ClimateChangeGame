using UnityEngine;
using UnityEngine.Events;

public class CharacterController2DWater : MonoBehaviour
{
	[SerializeField] private float drag_Force;    
	[SerializeField] private float horizontalMultiplier;    
	[SerializeField] private float verticalMultiplier;                          // Amount of force added when the player jumps.
	[Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;   // How much to smooth out the movement

	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		/* bool wasGrounded = m_Grounded;
		m_Grounded = true;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapBoxAll(m_GroundCheck.position, k_GroundedSize,0f, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		} */
	}


	public void Move(float move, float verticalMove)
	{
		// Move the character by finding the target velocity
		Vector3 targetVelocity = new Vector2(move * horizontalMultiplier, verticalMove * verticalMultiplier);
		// And then smoothing it out and applying it to the character
		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		// Adds drag to the rigidbody for water
		//m_Rigidbody2D.drag = drag_Force;
		// If the input is moving the player right and the player is facing left...
		if (move > 0 && !m_FacingRight)
		{
		 	// ... flip the player.
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (move < 0 && m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
		// Add a vertical force to the player.
		// m_Rigidbody2D.AddForce(new Vector2(0f, verticalMove));
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}