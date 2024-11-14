using Jump.Attributes.Components;

namespace Jump;

public static class Utility
{
    public static bool InheritsFromComponent(Type attributeType)
    {
        while (attributeType != null && attributeType != typeof(object))
        {
            if (attributeType == typeof(Component))
                return true;

            if (attributeType.BaseType != null) attributeType = attributeType.BaseType;
        }
        return false;
    }
}