using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	bool ishurting;
	public int hp = 5;
	public float hurtTimer = 0.5f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Hurt(Projectile projectile)
	{
		if (ishurting)
		{
			return;
		}
		ishurting = true;
		hp--;

		//UIHealthPanel.instance.SetLives(maxHP, hp);
		if (hp <= 0)
		{
			Die();
		}
		StartCoroutine(HurtRoutine());
	}

	IEnumerator HurtRoutine()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		float startTime = Time.time;
		while (startTime + hurtTimer > Time.time)
		{
			sr.color = Color.red;
			yield return new WaitForSeconds(0.05f);
			sr.color = Color.white;
			yield return new WaitForSeconds(0.05f);
		}
		sr.color = Color.white;
		ishurting = false;
	}

    void Die() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
