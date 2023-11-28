namespace EssayCheek.Api.Helpers.Extensions;

public static class DeepCloneExtension 
{
    public static T DeepClone<T>(this T item)
    {
        if (item is ICloneable cloneable) return (T)cloneable.Clone();

        throw new ArgumentException("The object must be implemented ICloneable interfaces", nameof(item));
    }
}