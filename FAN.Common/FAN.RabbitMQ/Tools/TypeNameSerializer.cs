using System;

namespace FAN.RabbitMQ
{
    public static class TypeNameSerializer
    {
        public static Type DeSerialize(string typeName)
        {
            var nameParts = typeName.Split(':');
            if (nameParts.Length != 2)
            {
                throw new Exception(string.Format("type name {0}, is not a valid RabbitMQ type name. Expected Type:Assembly", typeName));
            }
            var type = Type.GetType(nameParts[0] + ", " + nameParts[1]);
            if (type == null)
            {
                throw new Exception(
                    string.Format("Cannot find type {0}",
                    typeName));
            }
            return type;
        }

        public static string Serialize(Type type)
        {
            Preconditions.CheckNotNull(type, "type");
            var typeName = type.FullName + ":" + type.Assembly.GetName().Name;
            if (typeName.Length > 255)
            {
                throw new Exception(string.Format("The serialized name of type '{0}' exceeds the AMQP" + "maximum short string lengh of 255 characters.", type.Name));
            }
            return typeName;
        }
    }
}
