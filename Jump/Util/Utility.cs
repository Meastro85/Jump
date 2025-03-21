﻿namespace Jump.Util;

internal static class Utility
{
    internal static bool InheritsFromAttribute(Type attributeType, Type inheritedAttributeType)
    {
        while (attributeType != null && attributeType != typeof(object))
        {
            if (attributeType == inheritedAttributeType)
                return true;

            if (attributeType.BaseType != null) attributeType = attributeType.BaseType;
        }

        return false;
    }
}