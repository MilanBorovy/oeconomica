using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class EnumExtensions
{
    /// <summary>
    /// Gets custom attribute of enumerator
    /// </summary>
    /// <typeparam name="T">Type of attribute</typeparam>
    /// <param name="value">Enumerator</param>
    /// <returns></returns>
    public static T GetAttribute<T>(this Enum value)
        where T : Attribute
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        return type.GetField(name).GetCustomAttributes(false).OfType<T>().SingleOrDefault();
    }
}