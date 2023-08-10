using System;
using System.Collections;
using System.Web.Services.Protocols;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Metadata;
using Microsoft.Crm.Sdk.Query;
using Microsoft.Crm.SdkTypeProxy;
using Microsoft.Crm.SdkTypeProxy.Metadata;

public static class CRM4Extended
    {

        #region property
        /// <summary>
        /// Returns the entity attribute
        /// </summary>
        /// <param name="dynamicEntityInstance">Current instance</param>
        /// <param name="entityPropertyLogicalName">Property/Attribute Name</param>
        /// <returns>Attribute (Property)</returns>
        public static Property GetProperty(this DynamicEntity dynamicEntityInstance, string entityPropertyLogicalName)
        {
            if (dynamicEntityInstance.Properties.Contains(entityPropertyLogicalName))
            {
                return (Property)dynamicEntityInstance[entityPropertyLogicalName];
            }
            else
            {
                return null;
            }
        }

        // Field Value
        /// <summary>
        /// Returns the string property value
        /// </summary>
        /// <param name="dynamicEntityInstance">Entity</param>
        /// <param name="entityPropertyLogicalName">Attribute name</param>
        /// <returns>Attribute Value(String)</returns>
        public static string GetPropertyValue(this DynamicEntity dynamicEntityInstance, string entityPropertyLogicalName)
        {
            string returnValue = "";
            string propertyType = null;

            try
            {
                propertyType = dynamicEntityInstance.Properties[entityPropertyLogicalName].GetType().ToString();
            }
            catch (Exception e)
            {
                propertyType = e.Message;
                returnValue = e.Message;
                return null;
            }

            if (propertyType == "Microsoft.Crm.Sdk.Owner")
            {
                Owner ownerType = (Owner)dynamicEntityInstance[entityPropertyLogicalName];
                return ownerType.name;
            }
            else if (propertyType == "Microsoft.Crm.Sdk.Customer")
            {
                Customer customerType = (Customer)dynamicEntityInstance[entityPropertyLogicalName];
                return customerType.name;
            }
            else if (propertyType == "Microsoft.Crm.Sdk.CrmDateTime")
            {
                CrmDateTimeProperty crmDataTimeType = new CrmDateTimeProperty(entityPropertyLogicalName, (CrmDateTime)dynamicEntityInstance.Properties[entityPropertyLogicalName]);
                return crmDataTimeType.Value.date;
            }
            else if (propertyType == "Microsoft.Crm.Sdk.CrmBoolean")
            {
                CrmBooleanProperty crmBooleanProperty = new CrmBooleanProperty(entityPropertyLogicalName, (CrmBoolean)dynamicEntityInstance.Properties[entityPropertyLogicalName]);
                returnValue = crmBooleanProperty.Value.Value.ToString();
            }
            else if (propertyType == "System.String")
            {
                return dynamicEntityInstance.Properties[entityPropertyLogicalName].ToString();
            }
            else if (propertyType == "Microsoft.Crm.Sdk.Lookup")
            {
                Lookup lookupType = (Lookup)dynamicEntityInstance[entityPropertyLogicalName];
                return lookupType.name;
            }
            else if (propertyType == "Microsoft.Crm.Sdk.Picklist")
            {
                Picklist picklistType = (Picklist)dynamicEntityInstance[entityPropertyLogicalName];
                return picklistType.name;
            }
            return returnValue;
        }

        //Field Value Object
        /// <summary>
        /// Returns the entity attribute
        /// </summary>
        /// <param name="dynamicEntityInstance">Entity</param>
        /// <param name="entityPropertyLogicalName">Attribute name<</param>
        /// <returns>Attribute value(Object)</returns>
        public static Object GetPropertyObj(this DynamicEntity dynamicEntityInstance, string entityPropertyLogicalName)
        {
            if (dynamicEntityInstance.Properties.Contains(entityPropertyLogicalName))
            {
                string propertyType = null;
                Object returnObject;
                try
                {
                    //"CrmTypes"
                    propertyType = dynamicEntityInstance.Properties[entityPropertyLogicalName].GetType().ToString();
                }
                catch (Exception e)
                {
                    propertyType = e.Message;
                    returnObject = e.Message;
                    return null;
                }

                if (propertyType == "Microsoft.Crm.Sdk.Owner")
                {
                    Owner ownerObject = (dynamicEntityInstance[entityPropertyLogicalName]) as Owner;
                    returnObject = new { Value = ownerObject, Type = propertyType };
                    return returnObject;
                }
                if (propertyType == "Microsoft.Crm.Sdk.Customer")
                {
                    Customer customerObject = (dynamicEntityInstance[entityPropertyLogicalName]) as Customer;
                    returnObject = new { Value = customerObject, Type = propertyType };
                    return returnObject;
                }
                if (propertyType == "Microsoft.Crm.Sdk.CrmDateTime")
                {
                    CrmDateTimeProperty datetimeObject = new CrmDateTimeProperty(entityPropertyLogicalName, (CrmDateTime)dynamicEntityInstance.Properties[entityPropertyLogicalName]);
                    returnObject = new { Value = datetimeObject, Type = propertyType };
                    return returnObject;
                }
                if (propertyType == "Microsoft.Crm.Sdk.CrmBoolean")
                {
                    CrmBooleanProperty booleanObject = new CrmBooleanProperty(entityPropertyLogicalName, (CrmBoolean)dynamicEntityInstance.Properties[entityPropertyLogicalName]);
                    returnObject = new { Value = booleanObject, Type = propertyType };
                    return returnObject;
                }
                if (propertyType == "System.String")
                {
                    returnObject = new { Value = (dynamicEntityInstance.Properties[entityPropertyLogicalName]), Type = propertyType };
                    return returnObject;
                }
                if (propertyType == "Microsoft.Crm.Sdk.Lookup")
                {
                    Lookup lookupObject = dynamicEntityInstance[entityPropertyLogicalName] as Lookup;
                    returnObject = new { Value = lookupObject, Type = propertyType };
                    return returnObject;
                }
                if (propertyType == "Microsoft.Crm.Sdk.Picklist")
                {
                    Picklist piclkistObject = dynamicEntityInstance[entityPropertyLogicalName] as Picklist;
                    returnObject = new { Value = piclkistObject, Type = propertyType };
                    return returnObject;
                }
            }
            else
            {
                object nullObject = new { value = "Null", type = "Null" };
                return nullObject;
            }
            object nullreturn = new { value = "null", type = "null" };
            return nullreturn;
        }
        
        /// <summary>
        /// Set field value
        /// </summary>
        /// <param name="DynamicEntityObject">DynamicEntity</param>
        /// <param name="service">MetadataService</param>
        /// <param name="AttributeType">Type of attribute</param>
        /// <param name="AttributeName">Attribute name</param>
        /// <param name="AttributeValue1">Attribute first value</param>
        /// <param name="AttributeValue2">Attribute first value</param>
        /// <returns></returns>
        public static bool SetFieldValue(this DynamicEntity DynamicEntityObject, MetadataService service, String AttributeType, string AttributeName, String AttributeValue1, String AttributeValue2)
        {
            //bool ret = false;
            string TypeOfAttribute;// = null;
            try
            {
                //$ when the 
                if (DynamicEntityObject.Properties.Contains(AttributeName))
                {
                    TypeOfAttribute = DynamicEntityObject.Properties[AttributeName].GetType().Name;
                }
                else
                {
                    //TODO: replace this function with meta data service
                    TypeOfAttribute = RetrieveAttributeMetadata(service, DynamicEntityObject.Name, AttributeName);
                }


                if (TypeOfAttribute == null || TypeOfAttribute == "PrimaryKey")
                {
                    return false;
                }
                else if (TypeOfAttribute == "Microsoft.Crm.Sdk.Owner" || TypeOfAttribute == "Owner")
                {
                    DynamicEntityObject[AttributeName] = new Lookup(AttributeValue1, new Guid(AttributeValue2));
                    return true;
                }
                else if (TypeOfAttribute == "Microsoft.Crm.Sdk.Customer" || TypeOfAttribute == "Customer")
                {
                    DynamicEntityObject[AttributeName] = new Customer(AttributeValue1, new Guid(AttributeValue2));
                    return true;
                }
                else if (TypeOfAttribute == "Microsoft.Crm.Sdk.CrmDateTime" || TypeOfAttribute == "CrmDateTime" || TypeOfAttribute == "Microsoft.Crm.Sdk.Metadata.DateTimeAttributeMetadata")
                {
                    DynamicEntityObject[AttributeName] = new CrmDateTime(AttributeValue1);
                    return true;
                }
                else if (TypeOfAttribute == "Microsoft.Crm.Sdk.CrmBoolean" || TypeOfAttribute == "CrmBoolean" || TypeOfAttribute == "Microsoft.Crm.Sdk.Metadata.BooleanAttributeMetadata")
                {
                    DynamicEntityObject.Properties[AttributeName] = new CrmBoolean(bool.Parse(AttributeValue1));
                    return true;
                }
                else if (TypeOfAttribute == "Microsoft.Crm.Sdk.String" || TypeOfAttribute == "String" || TypeOfAttribute == "Microsoft.Crm.Sdk.Metadata.StringAttributeMetadata")
                {
                    DynamicEntityObject[AttributeName] = (string)AttributeValue1;
                    return true;
                }
                else if (TypeOfAttribute == "Microsoft.Crm.Sdk.Lookup" || TypeOfAttribute == "Lookup" || TypeOfAttribute == "Microsoft.Crm.Sdk.Metadata.LookupAttributeMetadata")
                {
                    DynamicEntityObject[AttributeName] = new Lookup(AttributeValue1, new Guid(AttributeValue2));
                    return true;
                }
                else if (TypeOfAttribute == "Microsoft.Crm.Sdk.Picklist" || TypeOfAttribute == "Picklist" || TypeOfAttribute == "Microsoft.Crm.Sdk.Metadata.PicklistAttributeMetadata")
                {
                    DynamicEntityObject[AttributeName] = new Picklist(int.Parse(AttributeValue1));
                    return true;
                }
                else if (TypeOfAttribute == "Microsoft.Crm.Sdk.CrmNumber" || TypeOfAttribute == "CrmNumber" || TypeOfAttribute == "Integer" || TypeOfAttribute == "Microsoft.Crm.Sdk.Metadata.IntegerAttributeMetadata")
                {
                    DynamicEntityObject[AttributeName] = new CrmNumber(int.Parse(AttributeValue1));
                    return true;
                }

            }
            catch (SoapException sopEx)
            {
                throw;
            }

            return false;

        }
        
        /// <summary>
        /// Returns the default string value
        /// </summary>
        /// <param name="crmEntity">Entity</param>
        /// <param name="propertyName">Attribute Name</param>
        /// <returns>Attribute value(String)</returns>
        public static string GetDefaultFieldValue(this DynamicEntity crmEntity, string propertyName)
        {
            if (crmEntity.Properties.Contains(propertyName))
            {
                string ret = string.Empty;
                string type = null;


                try
                {
                    //TODO: Google "CrmTypes"
                    type = crmEntity.Properties[propertyName].GetType().ToString();
                }
                catch (Exception exception)
                {
                    type = exception.Message;
                    ret = exception.Message;
                    return null;
                }

                if (type == "Microsoft.Crm.Sdk.Owner")
                {
                    Owner owner = (Owner)crmEntity[propertyName];
                    return owner.name;
                }
                if (type == "Microsoft.Crm.Sdk.Customer")
                {
                    Customer customer = (Customer)crmEntity[propertyName];
                    return customer.name;
                }
                if (type == "Microsoft.Crm.Sdk.CrmDateTime")
                {
                    CrmDateTimeProperty crmDataTime = new CrmDateTimeProperty(propertyName, (CrmDateTime)crmEntity.Properties[propertyName]);
                    return crmDataTime.Value.date;
                }
                if (type == "Microsoft.Crm.Sdk.CrmBoolean")
                {
                    CrmBooleanProperty crmBooleanProperty = new CrmBooleanProperty(propertyName, (CrmBoolean)crmEntity.Properties[propertyName]);
                    ret = crmBooleanProperty.Value.Value.ToString();
                    return ret;
                }
                if (type == "System.String")
                {
                    return crmEntity.Properties[propertyName].ToString();
                }
                if (type == "Microsoft.Crm.Sdk.Lookup")
                {
                    Lookup crmLookup = (Lookup)crmEntity[propertyName];
                    return crmLookup.name;
                }
                if (type == "Microsoft.Crm.Sdk.Picklist")
                {
                    Picklist crmPicklist = (Picklist)crmEntity[propertyName];
                    return crmPicklist.name;
                }
                if (type == "Microsoft.Crm.Sdk.CrmNumber")
                {
                    CrmNumber num = (CrmNumber)crmEntity[propertyName];
                    return num.Value.ToString();
                }
                return ret;
            }
            else
            {
                return "The Entity doesn't have this property defined";
            }
        }
        
        /// <summary>
        /// Serialize Dynamic Entity
        /// </summary>
        /// <param name="dynamicEntity">Entity</param>
        /// <returns>Serialized (Xml)</returns>
        public static string SerializeEntity(this DynamicEntity dynamicEntity)
        {
            System.IO.StringWriter stringwriter = new System.IO.StringWriter();
            System.Xml.Serialization.XmlSerializer serilizer = new System.Xml.Serialization.XmlSerializer(dynamicEntity.GetType());
            serilizer.Serialize(stringwriter, dynamicEntity);
            return stringwriter.ToString();
        }

        /// <summary>
        /// Serialize object to xml
        /// </summary>
        /// <param name="Entity">Object</param>
        /// <returns>Serialized Xml</returns>
        public static string SerializeObject(this Object Entity)
        {
            System.IO.StringWriter stringwriter = new System.IO.StringWriter();
            System.Xml.Serialization.XmlSerializer serilizer = new System.Xml.Serialization.XmlSerializer(Entity.GetType());
            serilizer.Serialize(stringwriter, Entity);
            return stringwriter.ToString();
        }

        /// <summary>
        /// De-serialize Dynamic Entity
        /// </summary>
        /// <param name="serializeEntity">Entity</param>
        /// <returns>DynamicEntity (Object)</returns>
        public static DynamicEntity DeserializeAsEntity(this string serializeEntity)
        {
            var stringReader = new System.IO.StringReader(serializeEntity);
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(DynamicEntity));
            return serializer.Deserialize(stringReader) as DynamicEntity;
        }

        /// <summary>
        /// DeSerialize object from xml 
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="serializedObject"></param>
        /// <returns>De-serialized object</returns>
        public static Object DeserializeObject<T>(this string serializedObject)
        {
            var stringReader = new System.IO.StringReader(serializedObject);
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            return serializer.Deserialize(stringReader) as DynamicEntity;
        }
        
        //ops
        /// <summary>
        /// Retrieve Entity from ICrmservice
        /// </summary>
        /// <param name="iservice">ICrmService</param>
        /// <param name="entityName">Entity Name</param>
        /// <param name="entityGuid">Entity Guid(String)</param>
        /// <returns></returns>
        public static DynamicEntity RetriveDynamicEntity(this ICrmService iservice, String entityName, String entityGuid)
        {
            // Create/Set the Target object.
            TargetRetrieveDynamic targetRetrieve = new TargetRetrieveDynamic
            {
                EntityName = entityName,
                EntityId = new Guid(entityGuid)
            };

            // Create/Set the Retrieve object.
            RetrieveRequest retrieve = new RetrieveRequest
            {
                Target = targetRetrieve,
                //ColumnSet = new AllColumns(),
                ReturnDynamicEntities = true      // Indicate that the BusinessEntity should be retrieved as a DynamicEntity.
            };

            // Execute the request.
            RetrieveResponse retrieved = (RetrieveResponse)iservice.Execute(retrieve);
            // Extract the DynamicEntity from the request.
            DynamicEntity retriveEntity = (DynamicEntity)retrieved.BusinessEntity;
            return retriveEntity;
        }

        /// <summary>
        /// Retrieve Entity from ICrmservice
        /// </summary>
        /// <param name="iservice">ICrmService</param>
        /// <param name="entityName">Entity Name</param>
        /// <param name="entityGuid">Entity Guid(String)</param>
        /// <param name="retrieveAllColumns"> True to retrieve all columns</param>
        /// <returns></returns>
        public static DynamicEntity RetriveDynamicEntity(this ICrmService iservice, String entityName, String entityGuid, Boolean retrieveAllColumns)
        {
            // Create/Set the Target object.
            TargetRetrieveDynamic targetRetrieve = new TargetRetrieveDynamic
            {
                EntityName = entityName,
                EntityId = new Guid(entityGuid)
            };

            // Create/Set the Retrieve object.
            RetrieveRequest retrieve = new RetrieveRequest();
            retrieve.Target = targetRetrieve;
            if (retrieveAllColumns == true) { retrieve.ColumnSet = new AllColumns(); } //// Indicate to retrieve all columns
            retrieve.ReturnDynamicEntities = true;      // Indicate that the BusinessEntity should be retrieved as a DynamicEntity.

            // Execute the request.
            RetrieveResponse retrieved = (RetrieveResponse)iservice.Execute(retrieve);
            // Extract the DynamicEntity from the request.
            DynamicEntity retriveEntity = (DynamicEntity)retrieved.BusinessEntity;
            return retriveEntity;
        }

        /// <summary>
        /// Retrieve Entity from ICrmservice
        /// </summary>
        /// <param name="iservice">ICrmService</param>
        /// <param name="entityName">Entity Name</param>
        /// <param name="entityGuid">Entity Guid(Guid)</param>
        /// <returns></returns>
        public static DynamicEntity RetriveDynamicEntity(this ICrmService iservice, String entityName, Guid entityGuid)
        {
            // Create/Set the Target object.
            TargetRetrieveDynamic targetRetrieve = new TargetRetrieveDynamic
            {
                EntityName = entityName,
                EntityId = (entityGuid)
            };

            // Create/Set the Retrieve object.
            RetrieveRequest retrieve = new RetrieveRequest
            {
                Target = targetRetrieve,
                //ColumnSet = new AllColumns(),
                ReturnDynamicEntities = true    // Indicate that the BusinessEntity should be retrieved as a DynamicEntity.
            };

            // Execute the request.
            RetrieveResponse retrieved = (RetrieveResponse)iservice.Execute(retrieve);
            // Extract the DynamicEntity from the request.
            DynamicEntity retriveEntity = (DynamicEntity)retrieved.BusinessEntity;
            return retriveEntity;
        }

        /// <summary>
        /// Retrieve Entity from ICrmservice
        /// </summary>
        /// <param name="iservice">ICrmService</param>
        /// <param name="entityName">Entity Name</param>
        /// <param name="entityGuid">Entity Guid(Guid)</param>
        /// <param name="retrieveAllColumns">True to retrieve all columns </param>
        /// <returns></returns>
        public static DynamicEntity RetriveDynamicEntity(this ICrmService iservice, String entityName, Guid entityGuid, Boolean retrieveAllColumns)
        {
            // Create/Set the Target object.
            TargetRetrieveDynamic targetRetrieve = new TargetRetrieveDynamic
            {
                EntityName = entityName,
                EntityId = (entityGuid)
            };

            // Create/Set the Retrieve object.
            RetrieveRequest retrieve = new RetrieveRequest();
            retrieve.Target = targetRetrieve;
            if (retrieveAllColumns == true) { retrieve.ColumnSet = new AllColumns(); } //// Indicate to retrieve all columns
            retrieve.ReturnDynamicEntities = true; // Indicate that the BusinessEntity should be retrieved as a DynamicEntity.

            // Execute the request.
            RetrieveResponse retrieved = (RetrieveResponse)iservice.Execute(retrieve);
            // Extract the DynamicEntity from the request.
            DynamicEntity retriveEntity = (DynamicEntity)retrieved.BusinessEntity;
            return retriveEntity;
        }

        // crm service
        /// <summary>
        ///  Retrieve Entity from Crm service proxy
        /// </summary>
        /// <param name="cservice">Crm Service proxy</param>
        /// <param name="entityName">Entity Name</param>
        /// <param name="entityGuid">Entity Guid (String)</param>
        /// <returns></returns>
        public static DynamicEntity RetriveDynamicEntity(this CrmService cservice, String entityName, String entityGuid)
        {
            // Create/Set the Target object.
            TargetRetrieveDynamic targetRetrieve = new TargetRetrieveDynamic
            {
                EntityName = entityName,
                EntityId = new Guid(entityGuid)
            };

            // Create/Set the Retrieve object.
            RetrieveRequest retrieve = new RetrieveRequest
            {
                Target = targetRetrieve,
                //ColumnSet = new AllColumns(),
                ReturnDynamicEntities = true    // Indicate that the BusinessEntity should be retrieved as a DynamicEntity.
            };


            // Execute the request.
            RetrieveResponse retrieved = (RetrieveResponse)cservice.Execute(retrieve);
            // Extract the DynamicEntity from the request.
            DynamicEntity retriveEntity = (DynamicEntity)retrieved.BusinessEntity;
            return retriveEntity;
        }

        // crm service
        /// <summary>
        ///  Retrieve Entity from Crm service proxy
        /// </summary>
        /// <param name="cservice">Crm Service proxy</param>
        /// <param name="entityName">Entity Name</param>
        /// <param name="entityGuid">Entity Guid (String)</param>
        /// <returns></returns>
        public static DynamicEntity RetriveDynamicEntity(this CrmService cservice, String entityName, String entityGuid, Boolean retrieveAllColumns)
        {
            // Create/Set the Target object.
            TargetRetrieveDynamic targetRetrieve = new TargetRetrieveDynamic
            {
                EntityName = entityName,
                EntityId = new Guid(entityGuid)
            };

            // Create/Set the Retrieve object.
            RetrieveRequest retrieve = new RetrieveRequest();
            retrieve.Target = targetRetrieve;
            if (retrieveAllColumns == true) { retrieve.ColumnSet = new AllColumns(); } //// Indicate to retrieve all columns
            retrieve.ReturnDynamicEntities = true;    // Indicate that the BusinessEntity should be retrieved as a DynamicEntity.

            // Execute the request.
            RetrieveResponse retrieved = (RetrieveResponse)cservice.Execute(retrieve);
            // Extract the DynamicEntity from the request.
            DynamicEntity retriveEntity = (DynamicEntity)retrieved.BusinessEntity;
            return retriveEntity;
        }


        // crm service
        /// <summary>
        ///  Retrieve Entity from Crm service proxy
        /// </summary>
        /// <param name="cservice">Crm Service proxy</param>
        /// <param name="entityName">Entity Name</param>
        /// <param name="entityGuid">Entity Guid (Guid)</param>
        /// <returns></returns>
        public static DynamicEntity RetriveDynamicEntity(this CrmService cservice, string entityName, Guid entityGuid)
        {
            // Create/Set the Target object.
            TargetRetrieveDynamic targetRetrieve = new TargetRetrieveDynamic
            {
                EntityName = entityName,
                EntityId = (entityGuid)
            };

            // Create/Set the Retrieve object.
            RetrieveRequest retrieve = new RetrieveRequest
            {
                Target = targetRetrieve,
                //ColumnSet = new AllColumns(),
                ReturnDynamicEntities = true
            };

            // Indicate that the BusinessEntity should be retrieved as a DynamicEntity.
            // Execute the request.
            RetrieveResponse retrieved = (RetrieveResponse)cservice.Execute(retrieve);
            // Extract the DynamicEntity from the request.
            DynamicEntity retriveEntity = (DynamicEntity)retrieved.BusinessEntity;
            return retriveEntity;
        }


        // crm service
        /// <summary>
        ///  Retrieve Entity from Crm service proxy
        /// </summary>
        /// <param name="cservice">Crm Service proxy</param>
        /// <param name="entityName">Entity Name</param>
        /// <param name="entityGuid">Entity Guid (Guid)</param>
        /// <returns></returns>
        public static DynamicEntity RetriveDynamicEntity(this CrmService cservice, String entityName, Guid entityGuid, Boolean retrieveAllColumns)
        {
            // Create/Set the Target object.
            TargetRetrieveDynamic targetRetrieve = new TargetRetrieveDynamic
            {
                EntityName = entityName,
                EntityId = (entityGuid)
            };

            // Create/Set the Retrieve object.
            RetrieveRequest retrieve = new RetrieveRequest();
            retrieve.Target = targetRetrieve;
            if (retrieveAllColumns == true) { retrieve.ColumnSet = new AllColumns(); } //// Indicate to retrieve all columns
            retrieve.ReturnDynamicEntities = true;

            // Indicate that the BusinessEntity should be retrieved as a DynamicEntity.
            // Execute the request.
            RetrieveResponse retrieved = (RetrieveResponse)cservice.Execute(retrieve);
            // Extract the DynamicEntity from the request.
            DynamicEntity retriveEntity = (DynamicEntity)retrieved.BusinessEntity;
            return retriveEntity;
        }


        /// <summary>
        /// Make a record inactive
        /// </summary>
        /// <param name="dynamicEntityObject">Entity</param>
        /// <param name="service">ICrmService</param>
        public static void SetInactive(this DynamicEntity dynamicEntityObject, ICrmService service)
        {
            try
            {
                SetStateDynamicEntityRequest setInactiveRequest = new SetStateDynamicEntityRequest();

                Key recordGuid = (Key)dynamicEntityObject[dynamicEntityObject.Name + "id"];
                Moniker entityMoniker = new Moniker(dynamicEntityObject.Name, recordGuid.Value);
                //HACK: Might not work always since the status are different for quite a few entities 
                setInactiveRequest.Entity = entityMoniker;
                setInactiveRequest.Status = -1;
                setInactiveRequest.State = "Inactive";

                SetStateDynamicEntityResponse setInactiveResponse =
                    (SetStateDynamicEntityResponse)service.Execute(setInactiveRequest);
            }
            catch (System.Web.Services.Protocols.SoapException)
            {
                throw;
            }
        }

        //
        /// <summary>
        /// Make a record inactive 
        /// </summary>
        /// <param name="dynamicEntityObject">Entity</param>
        /// <param name="service">CrmService Proxy</param>
        public static void SetInactive(this DynamicEntity dynamicEntityObject, CrmService crmServiceProxy)
        {
            try
            {
                SetStateDynamicEntityRequest setInactiveRequest = new SetStateDynamicEntityRequest();
                Key recordGuid = (Key)dynamicEntityObject[dynamicEntityObject.Name + "id"];
                Moniker entityMoniker = new Moniker(dynamicEntityObject.Name, recordGuid.Value);
                //HACK: Might not work always since the status are different for quite a few entities 
                setInactiveRequest.Entity = entityMoniker;
                setInactiveRequest.Status = -1;
                setInactiveRequest.State = "Inactive";

                SetStateDynamicEntityResponse setInactiveResponse =
                    (SetStateDynamicEntityResponse)crmServiceProxy.Execute(setInactiveRequest);
            }
            catch (System.Web.Services.Protocols.SoapException)
            {
                throw;
            }
        }

        //Set inactive ICrmService
        /// <summary>
        /// Set a record inactive
        /// </summary>
        /// <param name="dynamicEntityObject">Entity</param>
        /// <param name="service">CrmService</param>
        public static void SetActive(this DynamicEntity dynamicEntityObject, ICrmService service)
        {
            try
            {
                SetStateDynamicEntityRequest setInactiveRequest = new SetStateDynamicEntityRequest();

                Key recordGuid = (Key)dynamicEntityObject[dynamicEntityObject.Name + "id"];
                Moniker entityMoniker = new Moniker(dynamicEntityObject.Name, recordGuid.Value);
                //HACK: Might not work always since the status are different for quite a few entities 
                setInactiveRequest.Entity = entityMoniker;
                setInactiveRequest.Status = 1;
                setInactiveRequest.State = "Active";

                SetStateDynamicEntityResponse setInactiveResponse =
                    (SetStateDynamicEntityResponse)service.Execute(setInactiveRequest);
            }
            catch (System.Web.Services.Protocols.SoapException)
            {
                throw;
            }
        }

        /// <summary>
        ///  Set a record active
        /// </summary>
        /// <param name="dynamicEntityObject">Entity</param>
        /// <param name="service">CrmService Proxy</param>
        public static void SetActive(this DynamicEntity dynamicEntityObject, CrmService crmServiceProxy)
        {
            try
            {
                SetStateDynamicEntityRequest setInactiveRequest = new SetStateDynamicEntityRequest();

                Key recordGuid = (Key)dynamicEntityObject[dynamicEntityObject.Name + "id"];
                Moniker entityMoniker = new Moniker(dynamicEntityObject.Name, recordGuid.Value);
                //HACK: Might not work always since the status are different for quite a few entities 
                setInactiveRequest.Entity = entityMoniker;
                setInactiveRequest.Status = 1;
                setInactiveRequest.State = "Active";

                SetStateDynamicEntityResponse setInactiveResponse =
                    (SetStateDynamicEntityResponse)crmServiceProxy.Execute(setInactiveRequest);
            }
            catch (System.Web.Services.Protocols.SoapException)
            {
                throw;
            }
        }


        /// <summary>
        /// Sets the state for the record
        /// </summary>
        /// <param name="dynamicEntityObject"></param>
        /// <param name="service">CrmService</param>
        /// <param name="status">status code</param>
        /// <param name="state">Status text</param>
        public static void SetState(this DynamicEntity dynamicEntityObject, ICrmService service, int status, String state)
        {
            try
            {
                SetStateDynamicEntityRequest setInactiveRequest = new SetStateDynamicEntityRequest();

                Key recordGuid = (Key)dynamicEntityObject[dynamicEntityObject.Name + "id"];
                Moniker entityMoniker = new Moniker(dynamicEntityObject.Name, recordGuid.Value);
                setInactiveRequest.Entity = entityMoniker;
                setInactiveRequest.Status = status;
                setInactiveRequest.State = state;

                SetStateDynamicEntityResponse setInactiveResponse =
                    (SetStateDynamicEntityResponse)service.Execute(setInactiveRequest);
            }
            catch (System.Web.Services.Protocols.SoapException)
            {
                throw;
            }
        }
        
        //
         /// <summary>
        /// Enumeration for the Target type
        /// </summary>
        public enum TargetType
        {
            TargetPreEntityImage = 1,
            TargetInputParameters = 2,
            TargetPostEntityImages = 3,
            TargetOutputParameters = 4
        }

        /// <summary>
        /// Gets the target from plugin context
        /// </summary>
        /// <param name="context">IPluginExecutionContext</param>
        /// <param name="entityType">TargetType</param>
        /// <returns></returns>
        public static DynamicEntity GetTargetEntity(this IPluginExecutionContext context, TargetType entityType)
        {
            DynamicEntity _returnEntity = null;
            if (context.PreEntityImages.Contains("Target") && entityType == TargetType.TargetPreEntityImage)
                _returnEntity = (context.PreEntityImages.Properties["Target"]) as DynamicEntity;
            else if (context.InputParameters.Contains("Target") && entityType == TargetType.TargetInputParameters)
                _returnEntity = (context.InputParameters.Properties["Target"]) as DynamicEntity;
            else if (context.PostEntityImages.Contains("Target") && entityType == TargetType.TargetPostEntityImages)
                _returnEntity = (context.PostEntityImages.Properties["Target"]) as DynamicEntity;
            else if (context.OutputParameters.Contains("Target") && entityType == TargetType.TargetOutputParameters)
                _returnEntity = (context.OutputParameters.Properties["Target"]) as DynamicEntity;

            return _returnEntity;
        }
        
        //
        /// <summary>
        /// Re-Create Crm Service with calling user
        /// </summary>
        /// <param name="pluginContext"></param>
        /// <returns></returns>
        public static ICrmService RefreshService(this IPluginExecutionContext pluginContext)
        {
            ICrmService iservice = pluginContext.CreateCrmService(true);
            return iservice;
        }

        /// <summary>
        /// Re-Create Crm Service with the current user id
        /// </summary>
        /// <param name="pluginContext"></param>
        /// <param name="userGuid">User Guid to use </param>
        /// <returns></returns>
        public static ICrmService RefreshService(this IPluginExecutionContext pluginContext, Guid userGuid)
        {
            ICrmService iservice = pluginContext.CreateCrmService(userGuid);
            return iservice;
        }

        /// <summary>
        /// Re-Create Crm Service by using the current user
        /// </summary>
        /// <param name="CrmService">Crm Service</param>
        /// <param name="pluginContext">Plugin Context</param>
        /// <param name="UseCurrentUser">True/False</param>
        public static void RefreshService(this ICrmService CrmService, IPluginExecutionContext pluginContext, bool UseCurrentUser)
        {
            CrmService = pluginContext.CreateCrmService(UseCurrentUser);
        }

        ///// <summary>
        /// Re-Create Crm Service by using the current user
        /// </summary>
        /// <param name="CrmService">CrmService</param>
        /// <param name="pluginContext">IPluginExecutionContext</param>
        /// <param name="UserGuid">Guid of the system user</param>
        public static void RefreshService(this ICrmService CrmService, IPluginExecutionContext pluginContext, Guid UserGuid)
        {
            CrmService = pluginContext.CreateCrmService(UserGuid);
        }

        /// <summary>
        /// Get the depth of the plugin
        /// </summary>
        /// <param name="context">IPluginExecutionContext</param>
        /// <param name="depthValue">Value of plugin depth</param>
        /// <returns></returns>
        public static string DepthIs(this IPluginExecutionContext pluginContext, int depthValue)
        {
            if (pluginContext.Depth == depthValue)
            {
                return string.Format("Context depth equals to {0}", pluginContext.Depth.ToString());
            }
            else if (pluginContext.Depth > depthValue)
            {
                return string.Format("Context depth is greater then the one provided {0}", depthValue);
            }
            else if (pluginContext.Depth < depthValue)
            {
                return string.Format("Context depth is smaller then the one provided {0}", depthValue);
            }
            else
            {
                return "Aww.. Something went wrong while getting value";
            }
        }
        
        //
        public static Guid RetrieveEntityId(this DynamicEntity entity)
        {
            try
            {
                string name = entity.Name + "id";
                Key recordGuid = entity[name] as Key;
                return recordGuid.Value;
            }
            catch (SoapException guidIsNull)
            {
                return Guid.Empty;
            }
        }
        
        
        
        
    }
