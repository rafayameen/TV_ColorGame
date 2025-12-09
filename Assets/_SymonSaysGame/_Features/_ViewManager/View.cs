using UnityEngine;

public abstract class View : MonoBehaviour
{
	[SerializeField] private Views _viewType;

	public Views ViewType => _viewType;

	public abstract void Initialize();

	public virtual void Hide() => gameObject.SetActive(false);

	public virtual void Show(ViewContext context = null)
	{
		gameObject.SetActive(true);
		EventManager.DoFireSetCurrentView(ViewType.ToString());
		EventManager.DoFireLogAnalyticsEvent($"V_{ViewType}_Shn");
	}

	// Override this in derived views to handle custom logic
	public virtual void ProcessContext(ViewContext context) { }
}