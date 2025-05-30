// PlayerPowerUp.cs
using UnityEngine;
using System.Collections;

public class PlayerAnimationPowerUp : MonoBehaviour
{
    [Header("Animation Controllers")]
    public RuntimeAnimatorController normalAnimatorController;
    public RuntimeAnimatorController powerUpAnimatorController;
    
    [Header("Character Sprites")]
    public Sprite normalCharacterSprite;
    public Sprite powerUpCharacterSprite;
    
    [Header("Visual Settings")]
    public Color normalColor = Color.white;
    public Color powerUpColor = Color.yellow;
    public float scaleMultiplier = 1.2f;
    
    [Header("Effects")]
    public bool enableGlow = true;
    public float glowSpeed = 2f;
    public ParticleSystem powerUpParticles;
    
    [Header("Attack Effects")]
    public bool enableAttackFlash = true;
    public Color attackFlashColor = Color.red;
    public float attackFlashDuration = 0.1f;
    public bool enableCameraShake = true;
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.2f;
    
    [Header("Debug Settings")]
    public bool autoAddAnimator = true;
    
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;
    private Vector3 originalScale;
    private bool isPowerUpActive = false;
    private Sprite originalSprite;
    private RuntimeAnimatorController originalController;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            enabled = false;
            return;
        }
        
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            enabled = false;
            return;
        }
        
        SetupAnimator();
        originalScale = transform.localScale;
        originalSprite = spriteRenderer.sprite;
        
        if (normalCharacterSprite == null)
            normalCharacterSprite = originalSprite;
        
        StartCoroutine(DelayedSubscription());
    }
    
    IEnumerator DelayedSubscription()
    {
        yield return new WaitForEndOfFrame();
        
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.OnPowerUpStateChanged += OnPowerUpStateChanged;
        }
    }
    
    void Update()
    {
        AnimationController();
        
        if (isPowerUpActive && enableGlow && spriteRenderer != null)
        {
            float alpha = Mathf.PingPong(Time.time * glowSpeed, 1f);
            Color currentColor = Color.Lerp(powerUpColor, Color.white, alpha);
            spriteRenderer.color = currentColor;
        }
    }
    
    void SetupAnimator()
    {
        animator = GetComponent<Animator>();
        
        if (animator == null && autoAddAnimator)
        {
            animator = gameObject.AddComponent<Animator>();
            if (normalAnimatorController != null)
            {
                animator.runtimeAnimatorController = normalAnimatorController;
            }
        }
        
        if (animator != null)
        {
            originalController = animator.runtimeAnimatorController;
            if (normalAnimatorController == null)
                normalAnimatorController = animator.runtimeAnimatorController;
        }
    }
    
    void AnimationController()
    {
        if (animator == null || playerController == null || animator.runtimeAnimatorController == null) 
            return;
        
        float verticalVelocity = playerController.VerticalVelocity;
        bool isGrounded = playerController.IsGrounded;
        bool isSliding = playerController.IsSliding;
        
        SafeSetBool("isRunning", false);
        SafeSetBool("isJumping", false);
        SafeSetBool("isFalling", false);
        SafeSetBool("isSliding", false);

        if (isGrounded && GameManager.Instance != null && GameManager.Instance.IsPlaying)
        {
            SafeSetBool(isSliding ? "isSliding" : "isRunning", true);
        }
        else
        {
            SafeSetBool(verticalVelocity > 0.1f ? "isJumping" : "isFalling", true);
        }
    }
    
    void OnPowerUpStateChanged(bool isActive)
    {
        isPowerUpActive = isActive;
        
        if (isActive)
            ActivateCharacterPowerUp();
        else
            DeactivateCharacterPowerUp();
    }
    
    void ActivateCharacterPowerUp()
    {
        if (powerUpCharacterSprite != null && spriteRenderer != null)
            spriteRenderer.sprite = powerUpCharacterSprite;
            
        if (powerUpAnimatorController != null && animator != null)
            animator.runtimeAnimatorController = powerUpAnimatorController;
            
        if (spriteRenderer != null)
            spriteRenderer.color = powerUpColor;
            
        transform.localScale = originalScale * scaleMultiplier;
        
        if (powerUpParticles != null)
            powerUpParticles.Play();
    }
    
    void DeactivateCharacterPowerUp()
    {
        if (normalCharacterSprite != null && spriteRenderer != null)
            spriteRenderer.sprite = normalCharacterSprite;
            
        if (normalAnimatorController != null && animator != null)
            animator.runtimeAnimatorController = normalAnimatorController;
            
        if (spriteRenderer != null)
            spriteRenderer.color = normalColor;
            
        transform.localScale = originalScale;
        
        if (powerUpParticles != null)
            powerUpParticles.Stop();
    }
    
    public void TriggerAttackAnimation()
    {
        if (animator == null || !isPowerUpActive) return;
        
        SafeSetTrigger("isAttacking");
        StartCoroutine(AttackEffectCoroutine());
    }
    
    private IEnumerator AttackEffectCoroutine()
    {
        if (enableAttackFlash && spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = attackFlashColor;
            
            yield return new WaitForSeconds(attackFlashDuration);
            
            spriteRenderer.color = isPowerUpActive ? powerUpColor : originalColor;
        }
        
        if (enableCameraShake && Camera.main != null)
        {
            StartCoroutine(CameraShake(shakeDuration, shakeIntensity));
        }
    }
    
    private IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            
            Camera.main.transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        Camera.main.transform.position = originalPos;
    }
    
    void SafeSetBool(string parameterName, bool value)
    {
        if (animator == null || animator.runtimeAnimatorController == null) return;
        
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == parameterName && param.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(parameterName, value);
                return;
            }
        }
    }
    
    void SafeSetTrigger(string parameterName)
    {
        if (animator == null || animator.runtimeAnimatorController == null) return;
        
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == parameterName && param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.SetTrigger(parameterName);
                return;
            }
        }
    }
    
    public bool IsPowerUpActive() => isPowerUpActive;
    
    public void SetCharacterSprite(Sprite newSprite)
    {
        if (spriteRenderer != null && newSprite != null)
            spriteRenderer.sprite = newSprite;
    }
    
    public Sprite GetCurrentSprite() => spriteRenderer?.sprite;
    
    void OnDestroy()
    {
        if (PowerUpManager.Instance != null)
            PowerUpManager.Instance.OnPowerUpStateChanged -= OnPowerUpStateChanged;
    }
}