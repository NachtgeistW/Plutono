//using Godot;

//public class Singleton<T> : Node where T : Singleton<T>
//{
//	private static T _instance;
//	public static T Instance
//	{
//		get
//		{
//			if (_instance == null)
//			{
//				_instance = (T)FindObjectOfType(typeof(T));
//				if (_instance == null)
//				{
//					SetupInstance();
//				}
//			}
//			return _instance;
//		}
//	}

//	protected virtual void Awake()
//	{
//		if (_instance != null)
//			QueueFree();
//		else
//		{
//			_instance = this as T;
//		}
//	}

//	private static void SetupInstance()
//	{
//		_instance = (T)FindObjectOfType(typeof(T));
//		if (_instance == null)
//		{
//			var obj = new Node
//			{
//				Name = typeof(T).Name
//			};
//			_instance = obj.AddComponent<T>();
//		}
//	}
//}