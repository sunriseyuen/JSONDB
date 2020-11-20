using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Volte.Data.Dapper
{
    public class ObjectPropertyMaps
    {
        private static Dictionary<string, ObjectProperty> _Propertys = new Dictionary<string, ObjectProperty>();

        public ObjectPropertyMaps()
        {

        }
        internal static ObjectProperty Build<T>()
        {
            var type = typeof(T);
            ObjectProperty cacheValue;
            _Propertys.TryGetValue(type.FullName, out cacheValue);

            if (cacheValue == null)
            {
                ObjectProperty model = new ObjectProperty();
                string tableName = type.Name;
                Attribute[] attibutes = null;

                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (Attribute attr in type.GetCustomAttributes(true))
                {
                    if (attr.GetType() == typeof(ObjectAttribute))
                    {
                        ObjectAttribute _attribute = (ObjectAttribute)attr;
                        tableName = _attribute.TableName;
                        break;
                    }
                }
                List<ObjectAttribute> Property = new List<ObjectAttribute>();
                foreach (PropertyInfo p in properties)
                {
                    if (p.CanWrite && p.CanRead)
                    {
                        attibutes = Attribute.GetCustomAttributes(p);
                        ObjectAttribute _AttributeMapping=null;

                        foreach (Attribute attribute in attibutes)
                        {
                            Console.WriteLine(attribute.ToString());

                            //检怀是倀设瀀了AttributeMapping倀性
                            if (attribute.GetType() == typeof(ObjectAttribute))
                            {
                                string key = ("att_" + type.FullName + "_" + p.Name).ToLower();

                                _AttributeMapping = (ObjectAttribute)attribute;

                                if (string.IsNullOrEmpty(_AttributeMapping.TableName) && !string.IsNullOrEmpty(tableName))
                                {
                                    _AttributeMapping.TableName = tableName;
                                }

                                if (string.IsNullOrEmpty(_AttributeMapping.ColumnName))
                                {
                                    _AttributeMapping.ColumnName = p.Name;
                                }
                            }
                        }


                        if (_AttributeMapping == null)
                        {
                            _AttributeMapping = new ObjectAttribute();
                            _AttributeMapping.TableName = tableName;
                            _AttributeMapping.ColumnName = p.Name;
                        }

                        _AttributeMapping.Nullable = Nullable.GetUnderlyingType(p.PropertyType) != null;

                        if (_AttributeMapping.Type == DbType.Object)
                        {

                            if (Type.GetTypeCode(p.PropertyType) == TypeCode.Boolean)
                            {
                                _AttributeMapping.Type = DbType.Boolean;
                            }
                            else if (Type.GetTypeCode(p.PropertyType) == TypeCode.Byte)
                            {
                                _AttributeMapping.Type = DbType.Byte;
                            }
                            else if (Type.GetTypeCode(p.PropertyType) == TypeCode.Char)
                            {
                                _AttributeMapping.Type = DbType.String;
                            }
                            else if (Type.GetTypeCode(p.PropertyType) == TypeCode.DateTime)
                            {
                                _AttributeMapping.Type = DbType.DateTime;
                            }
                            else if (Type.GetTypeCode(p.PropertyType) == TypeCode.Decimal)
                            {
                                _AttributeMapping.Type = DbType.Decimal;
                            }
                            else if (Type.GetTypeCode(p.PropertyType) == TypeCode.Double)
                            {
                                _AttributeMapping.Type = DbType.Double;
                            }
                            else if (Type.GetTypeCode(p.PropertyType) == TypeCode.Int16)
                            {
                                _AttributeMapping.Type = DbType.Int16;
                            }
                            else if (Type.GetTypeCode(p.PropertyType) == TypeCode.Int32)
                            {
                                _AttributeMapping.Type = DbType.Int32;
                            }
                            else if (Type.GetTypeCode(p.PropertyType) == TypeCode.Int64)
                            {
                                _AttributeMapping.Type = DbType.Int64;
                            }
                            else if (Type.GetTypeCode(p.PropertyType) == TypeCode.SByte)
                            {
                                _AttributeMapping.Type = DbType.SByte;
                            }
                            else if (Type.GetTypeCode(p.PropertyType) == TypeCode.Single)
                            {
                                _AttributeMapping.Type = DbType.Single;
                            }
                            else if (Type.GetTypeCode(p.PropertyType) == TypeCode.String)
                            {
                                _AttributeMapping.Type = DbType.String;
                            }
                            else if (Type.GetTypeCode(p.PropertyType) == TypeCode.UInt16)
                            {
                                _AttributeMapping.Type = DbType.UInt16;
                            }
                            else if (Type.GetTypeCode(p.PropertyType) == TypeCode.UInt32)
                            {
                                _AttributeMapping.Type = DbType.UInt32;
                            }
                            else if (Type.GetTypeCode(p.PropertyType) == TypeCode.UInt64)
                            {
                                _AttributeMapping.Type = DbType.UInt64;
                            }
                            else
                            {
                                if (_AttributeMapping.Nullable)
                                {
                                    //ZZLogger.Debug(ZFILE_NAME, p.PropertyType.Name);

                                    if (p.PropertyType.Name.Contains("DateTime"))
                                    {
                                        _AttributeMapping.Type = DbType.DateTime;
                                    }
                                    else
                                    {
                                        _AttributeMapping.Type = DbType.Object;
                                    }
                                }
                                else
                                {
                                    _AttributeMapping.Type = DbType.Object;
                                }
                            }

                            //ZZLogger.Debug(ZFILE_NAME, Type.GetTypeCode(p.PropertyType));
                            //ZZLogger.Debug(ZFILE_NAME, p.Name);
                        }
                        Property.Add(_AttributeMapping);
                    }
                }
                model.Property = Property;
                model.TableName = tableName;
                _Propertys.TryAdd(type.FullName, model);
                cacheValue = model;
            }
            return cacheValue;
        }
    }
}
