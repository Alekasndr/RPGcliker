using UnityEngine;

public static class ObjectCreator 
{
    #region Fields

//    static PoolManager poolManagerInstance;

    #endregion

	
	
    #region Properties

//    static PoolManager PoolManagerInstance => poolManagerInstance ?? (poolManagerInstance = PoolManager.Instance);
    
    #endregion
	
	
	
    #region Public methods
	
    public static GameObject CreateObject(GameObject prefab, Transform anchor = null, bool shouldUsePool = false, bool shouldReset = true)
    {	
        GameObject result = null;
        
//        if (shouldUsePool)
//        {
//            ObjectPool objPool = PoolManagerInstance.PoolForObject(prefab);
//            result = objPool.Pop((obj) =>
//            {
//                obj.transform.SetParent(anchor); 
//            });
//        }
//        else
        {
            result = GameObject.Instantiate(prefab, anchor);
        }
        
        if (result != null && shouldReset)
        {
            Transform objTransform = result.transform;
            
            objTransform.localScale = Vector3.one;
            objTransform.localPosition = Vector3.zero;
            objTransform.localRotation = Quaternion.identity;
        }
        
        return result;
    }
    
    
    public static T Create<T>(Object original) where T : Object
    {
        return (T)Object.Instantiate(original);
    }
    
    
    public static T CreateObject<T>(GameObject prefab, Transform anchor = null, bool shouldUsePool = false, bool shouldReset = true) where  T : Object
    {
        GameObject obj = CreateObject(prefab, anchor, shouldUsePool, shouldReset);
        
        return obj != null ? obj.GetComponent<T>() : null;
    }
	
    #endregion
}