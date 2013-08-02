using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Communication.Sockets.Core.Message
{
    public class MessageComposer
    {
        public static byte[] Serialize(int messageKind, MessageBase msg)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf1 = new BinaryFormatter();

            bf1.Serialize(ms, messageKind);
            bf1.Serialize(ms, msg);

            return ms.ToArray();
        }

        public static void Deserialize(byte[] buffer, 
            out int messageKind, out MessageBase msg)
        {
            MemoryStream ms = new MemoryStream(buffer);
            BinaryFormatter formatter = new BinaryFormatter();
            messageKind = (int)formatter.Deserialize(ms);
            msg = (MessageBase)formatter.Deserialize(ms);
        }
    }
}
