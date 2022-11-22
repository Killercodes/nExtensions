using Microsoft.Xrm.Sdk;

namespace Sdk.Crm.Extensions
{
    public static class CrmExtensionMethods
    {
        /// <summary>
        /// Trace & Dump Plugincontext
        /// </summary>
        public static void DumpContext(this IPluginExecutionContext ctx, ITracingService tracingService)
        {
            try
            {
                string msg = " ";
                msg += $"PrimaryEntity={ctx.PrimaryEntityName}\n PrimaryEntityId={ctx.PrimaryEntityId}\n Message={ctx.MessageName}\n";
                msg += $"BusinessUnitId={ctx.BusinessUnitId}\n CorrelationId={ctx.CorrelationId}\n Depth{ctx.Depth}";
                msg += $"InitiatingUserId{ctx.InitiatingUserId}\n Mode={ctx.Mode}";
                msg += $"OrganizationId={ctx.OrganizationId}\n OrganizationName={ctx.OrganizationName}\n";


                var target = ctx.InputParameters["Target"] as Entity;
                if (target != null)
                {
                    msg += $"Target Attributes";
                    foreach (var item in target.Attributes)
                    {
                        msg += $"{item.Key}={item.Value},\n";
                    }
                }


                if (ctx.PreEntityImages != null && ctx.PreEntityImages.Contains("preimage"))
                {
                    Entity preImage = (Entity)ctx.PreEntityImages["preimage"];
                    if (preImage != null)
                    {
                        msg += $"preimage Attributes";
                        foreach (var item in preImage.Attributes)
                        {
                            msg += $"{item.Key}={item.Value},\n";
                        }
                    }
                }

                if (ctx.PostEntityImages != null && ctx.PostEntityImages.Contains("postimage"))
                {
                    Entity postImage = (Entity)ctx.PreEntityImages["postimage"];
                    if (postImage != null)
                    {
                        msg += $"postimage Attributes";
                        foreach (var item in postImage.Attributes)
                        {
                            msg += $"{item.Key}={item.Value},\n";
                        }
                    }
                }


                tracingService.Trace(msg);
            }
            catch (Exception ext)
            {
                tracingService.Trace(ext.Message);
            }
        }

        //convert to Jsonsring 
        public static string ToJson<T>(this T t)
        {
            var stream1 = new MemoryStream();
            var ser = new DataContractJsonSerializer(typeof(T));
            ser.WriteObject(stream1, t);
            stream1.Position = 0;
            var sr = new StreamReader(stream1);
            return (sr.ReadToEnd());
        }

        //convert from json string
        public static T FromJson<T>(this string jsonString)
        {
            T deserializedEntity = default(T);
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            var ser = new DataContractJsonSerializer(typeof(T));
            deserializedEntity = (T)ser.ReadObject(ms);// as T;
            ms.Close();
            return deserializedEntity;
        }

        //failsafe way to read attribute
        public static T TryGet<T>(this Entity ent, string key)
        {
            if (ent.Contains(key))
                return (T)ent[key];
            else
                return default(T);
        }

        //set the attribute with value
        public static void TrySet<T>(this Entity ent, string key, T value)
        {
            ent[key] = value;
        }

        //get attribute value of related record
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
