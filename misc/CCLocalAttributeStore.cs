using VRC.SDKBase;

namespace catclientv3.misc;

public static class CCLocalAttributeStore
{
    
}

public class CCLocalAttribute<T>
{
    public T value;
    public bool ignoreNextChange;
    public CCLocalAttribute()
    {
        
    }
}