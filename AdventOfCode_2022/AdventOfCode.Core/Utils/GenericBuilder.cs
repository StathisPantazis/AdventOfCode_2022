using AdventOfCode.Core.Models.Bases;
using System.Reflection;

namespace AdventOfCode.Core.Utils;

internal static class GenericBuilder
{
    public static T GetDefault<T>(T emptyValue = default)
    {
        if (typeof(T).GetConstructor(Type.EmptyTypes) is ConstructorInfo constructor)
        {
            var instance = (T)constructor.Invoke(null);

            if (instance is Emptyable emptyableInstance)
            {
                var emptyValueProperty = instance.GetType().BaseType.GetProperty(nameof(GridInput<object>.EmptyValue));

                var propertyToFill = instance.GetType().GetProperty(emptyableInstance.ValueToFillWithEmptyName);
                propertyToFill.SetValue(instance, emptyValueProperty.GetValue(instance));
            }

            return instance;
        }

        return emptyValue;
    }
}
