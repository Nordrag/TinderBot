using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TinderAutoLikerLibrary.Helpers
{
    public class Serializer
    {
        string path => Environment.CurrentDirectory + @"\Save\bot.save";

        public void Serialize<T>(T obj)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.OpenOrCreate);
            formatter.Serialize(stream, obj);
            stream.Close();
        }

        public T Deserialize<T>()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Open);
            var res = formatter.Deserialize(stream);
            stream.Close();
            return (T)res;
        }
    }
}
