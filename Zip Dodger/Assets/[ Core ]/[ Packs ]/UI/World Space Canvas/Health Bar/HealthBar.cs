using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBar : MonoBehaviour
{
    private Player player;
    public Player Player { get { return player == null ? player = GetComponentInParent<Player>() : player; } }

    private AI ai;
    public AI AI { get { return ai == null ? ai = GetComponentInParent<AI>() : ai; } }

    private float currentHp;
    private float maxHp;
    
    [Header("-- SETUP --")]
    [SerializeField, Tooltip("Custom gradient if you're not using images for filling. Leave it white when using images!")] private Gradient gradient;
    [SerializeField, Tooltip("Seconds that will take to update the progress bar.")] private float updateSpeedSeconds = 0.5f;
    private Slider slider;
    private Image fill;

    // Events
    public event Action OnChange, OnDamage;

    private void OnEnable()
    {
        slider = GetComponent<Slider>();
        fill = transform.GetChild(1).GetComponent<Image>();
        InitializeHealthBar();
        UpdateHealth();

        OnDamage += UpdateHealth;
        OnChange += HealthChanged;
    }

    private void OnDisable()
    {
        OnDamage -= UpdateHealth;
        OnChange -= HealthChanged;
    }

    private void InitializeHealthBar()
    {
        fill.color = gradient.Evaluate(1f);
        slider.maxValue = maxHp;
        slider.value = currentHp;
    }

    private void UpdateHealth()
    {
        // Do something when hp changes.
        StartCoroutine(SmoothUpdateHealthBar());
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    private void HealthChanged()
    {
        InitializeHealthBar();
        UpdateHealth();
    }

    private IEnumerator SmoothUpdateHealthBar()
    {
        float preChange = slider.value;
        float elapsed = 0f;
        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            Mathf.Lerp(preChange, currentHp, 0);

            slider.value = Mathf.Lerp(preChange, currentHp, elapsed / updateSpeedSeconds);
            yield return null;
        }

        slider.value = currentHp;
    }

    public void ChangedTrigger() => OnChange?.Invoke();
    public void DamageTrigger() => OnDamage?.Invoke();
}
