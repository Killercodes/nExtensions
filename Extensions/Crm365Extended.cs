using Microsoft.Xrm.Sdk;

namespace Sdk.Crm.Extensions
{
    public static class CrmExtensionMethods
    {
        public static T GetAliasedAttributeValue<T>(this Entity entity, string attributeName)
        {
            if (entity == null)
                return default(T);

            AliasedValue fieldAliasValue = entity.GetAttributeValue<AliasedValue>(attributeName);

            if (fieldAliasValue == null)
                return default(T);

            if (fieldAliasValue.Value != null && fieldAliasValue.Value.GetType() == typeof(T))
            {
                return (T)fieldAliasValue.Value;
            }

            return default(T);
        }
    }
}
