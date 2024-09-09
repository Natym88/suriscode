using Newtonsoft.Json;
using Suriscode_API.Models;
using System.Text.Json.Nodes;

namespace Suriscode_API.Helper
{
    public static class JsonFileHelper
    {
        /// <summary>
        /// Lista los elementos de un atributo pasado por parámetro, existente en el archivo del path también dado por parámetro. Mappeando al tipo correspondiente.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="atributo"></param>
        /// <returns>Lista de elementos mappeados</returns>
        /// <exception cref="Exception"></exception>
        public static List<T> ReadFromJsonFile<T>(string path, string atributo)
        {
            using StreamReader file = File.OpenText(path);
            JsonSerializer serializer = new JsonSerializer();
            var jsonObject = serializer.Deserialize<Dictionary<string, object>>(new JsonTextReader(file));
            if (jsonObject != null && jsonObject.ContainsKey(atributo))
            {
                var jsonArray = jsonObject[atributo].ToString();

                List<T> result = JsonConvert.DeserializeObject<List<T>>(jsonArray);

                return result;
            }
            else
            {
                throw new Exception($"El atributo '{atributo}' no fue encontrado en el JSON.");
            }
        }
    }
}
