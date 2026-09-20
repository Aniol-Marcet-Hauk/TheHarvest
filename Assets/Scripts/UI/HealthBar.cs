using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

	[SerializeField] private Slider m_Slider;
	[SerializeField] private Gradient m_Gradient;
	[SerializeField] private Image m_Fill;

	public void SetMaxHealth(float health)
	{
		m_Slider.maxValue = health;
		//slider.value = health;

		m_Fill.color = m_Gradient.Evaluate(1f);
	}

    public void SetHealth(float health)
	{
		m_Slider.value = health;

		m_Fill.color = m_Gradient.Evaluate(m_Slider.normalizedValue);
	}

}
