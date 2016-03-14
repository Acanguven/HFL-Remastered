namespace LoLLauncher.RiotObjects
{
    using LoLLauncher;
    using LoLLauncher.RiotObjects.Platform.Game;
    using LoLLauncher.RiotObjects.Platform.Reroll.Pojo;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public abstract class RiotGamesObject
    {
        protected RiotGamesObject()
        {
        }

        public virtual void DoCallback(TypedObject result)
        {
        }

        public TypedObject GetBaseTypedObject()
        {
            TypedObject obj2 = new TypedObject(this.TypeName);
            Type type = base.GetType();
            foreach (PropertyInfo info in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                InternalNameAttribute attribute = info.GetCustomAttributes(typeof(InternalNameAttribute), false).FirstOrDefault<object>() as InternalNameAttribute;
                if (attribute != null)
                {
                    object item = null;
                    Type propertyType = info.PropertyType;
                    string name = propertyType.Name;
                    if (propertyType == typeof(int[]))
                    {
                        int[] source = info.GetValue(this) as int[];
                        if (source != null)
                        {
                            item = source.Cast<object>().ToArray<object>();
                        }
                    }
                    else if (propertyType == typeof(double[]))
                    {
                        double[] numArray2 = info.GetValue(this) as double[];
                        if (numArray2 != null)
                        {
                            item = numArray2.Cast<object>().ToArray<object>();
                        }
                    }
                    else if (propertyType == typeof(string[]))
                    {
                        string[] strArray = info.GetValue(this) as string[];
                        if (strArray != null)
                        {
                            item = strArray.Cast<object>().ToArray<object>();
                        }
                    }
                    else if (propertyType.IsGenericType && (propertyType.GetGenericTypeDefinition() == typeof(List<>)))
                    {
                        IList list = info.GetValue(this) as IList;
                        if (list != null)
                        {
                            object[] array = new object[list.Count];
                            list.CopyTo(array, 0);
                            List<object> list2 = new List<object>();
                            foreach (object obj4 in array)
                            {
                                Type c = obj4.GetType();
                                if (typeof(RiotGamesObject).IsAssignableFrom(c))
                                {
                                    item = (obj4 as RiotGamesObject).GetBaseTypedObject();
                                }
                                else
                                {
                                    item = obj4;
                                }
                                list2.Add(item);
                            }
                            item = TypedObject.MakeArrayCollection(list2.ToArray());
                        }
                    }
                    else if (typeof(RiotGamesObject).IsAssignableFrom(propertyType))
                    {
                        RiotGamesObject obj5 = info.GetValue(this) as RiotGamesObject;
                        if (obj5 != null)
                        {
                            item = obj5.GetBaseTypedObject();
                        }
                    }
                    else
                    {
                        item = info.GetValue(this);
                    }
                    obj2.Add(attribute.Name, item);
                }
            }
            Type baseType = type.BaseType;
            if (baseType != null)
            {
                foreach (PropertyInfo info2 in baseType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    InternalNameAttribute attribute2 = info2.GetCustomAttributes(typeof(InternalNameAttribute), false).FirstOrDefault<object>() as InternalNameAttribute;
                    if ((attribute2 != null) && !obj2.ContainsKey(attribute2.Name))
                    {
                        obj2.Add(attribute2.Name, info2.GetValue(this));
                    }
                }
            }
            return obj2;
        }

        public void SetFields<T>(T obj, TypedObject result)
        {
            if (result != null)
            {
                this.TypeName = result.type;
                foreach (PropertyInfo info in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    InternalNameAttribute attribute = info.GetCustomAttributes(typeof(InternalNameAttribute), false).FirstOrDefault<object>() as InternalNameAttribute;
                    if (attribute != null)
                    {
                        object obj2;
                        Type propertyType = info.PropertyType;
                        if (result.TryGetValue(attribute.Name, out obj2))
                        {
                            try
                            {
                                if (result[attribute.Name] == null)
                                {
                                    obj2 = null;
                                }
                                else if (propertyType == typeof(string))
                                {
                                    obj2 = Convert.ToString(result[attribute.Name]);
                                }
                                else if (propertyType == typeof(int))
                                {
                                    obj2 = Convert.ToInt32(result[attribute.Name]);
                                }
                                else if (propertyType == typeof(long))
                                {
                                    obj2 = Convert.ToInt64(result[attribute.Name]);
                                }
                                else if (propertyType == typeof(double))
                                {
                                    obj2 = Convert.ToInt64(result[attribute.Name]);
                                }
                                else if (propertyType == typeof(bool))
                                {
                                    obj2 = Convert.ToBoolean(result[attribute.Name]);
                                }
                                else if (propertyType == typeof(DateTime))
                                {
                                    obj2 = result[attribute.Name];
                                }
                                else if (propertyType == typeof(TypedObject))
                                {
                                    obj2 = (TypedObject) result[attribute.Name];
                                }
                                else if (propertyType.IsGenericType && (propertyType.GetGenericTypeDefinition() == typeof(List<>)))
                                {
                                    object[] array = result.GetArray(attribute.Name);
                                    Type type = propertyType.GetGenericArguments()[0];
                                    Type[] typeArguments = new Type[] { type };
                                    IList list = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(typeArguments));
                                    foreach (object obj3 in array)
                                    {
                                        if (obj3 == null)
                                        {
                                            list.Add(null);
                                        }
                                        if (type == typeof(string))
                                        {
                                            list.Add((string) obj3);
                                        }
                                        else if (type == typeof(Participant))
                                        {
                                            TypedObject obj4 = (TypedObject) obj3;
                                            if (obj4.type == "com.riotgames.platform.game.BotParticipant")
                                            {
                                                list.Add(new BotParticipant(obj4));
                                            }
                                            else if (obj4.type == "com.riotgames.platform.game.ObfruscatedParticipant")
                                            {
                                                list.Add(new ObfruscatedParticipant(obj4));
                                            }
                                            else if (obj4.type == "com.riotgames.platform.game.PlayerParticipant")
                                            {
                                                list.Add(new PlayerParticipant(obj4));
                                            }
                                            else if (obj4.type == "com.riotgames.platform.reroll.pojo.AramPlayerParticipant")
                                            {
                                                list.Add(new AramPlayerParticipant(obj4));
                                            }
                                        }
                                        else if (type == typeof(int))
                                        {
                                            list.Add((int) obj3);
                                        }
                                        else
                                        {
                                            object[] args = new object[] { obj3 };
                                            list.Add(Activator.CreateInstance(type, args));
                                        }
                                    }
                                    obj2 = list;
                                }
                                else if (propertyType == typeof(Dictionary<string, object>))
                                {
                                    obj2 = (Dictionary<string, object>) result[attribute.Name];
                                }
                                else if (propertyType.IsGenericType && (propertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>)))
                                {
                                    obj2 = (Dictionary<string, object>) result[attribute.Name];
                                }
                                else if (propertyType == typeof(int[]))
                                {
                                    obj2 = result.GetArray(attribute.Name).Cast<int>().ToArray<int>();
                                }
                                else if (propertyType == typeof(string[]))
                                {
                                    obj2 = result.GetArray(attribute.Name).Cast<string>().ToArray<string>();
                                }
                                else if (propertyType == typeof(object[]))
                                {
                                    obj2 = result.GetArray(attribute.Name);
                                }
                                else if (propertyType == typeof(object))
                                {
                                    obj2 = result[attribute.Name];
                                }
                                else
                                {
                                    try
                                    {
                                        object[] objArray3 = new object[] { result[attribute.Name] };
                                        obj2 = Activator.CreateInstance(propertyType, objArray3);
                                    }
                                    catch (Exception exception)
                                    {
                                        throw new NotSupportedException(string.Format("Type {0} not supported by flash serializer", propertyType.FullName), exception);
                                    }
                                }
                                info.SetValue(obj, obj2, null);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }

        [InternalName("dataVersion")]
        public int DataVersion { get; set; }

        [InternalName("futureData")]
        public int FutureData { get; set; }

        public string TypeName { virtual get; private set; }
    }
}

