using System.Text.Json;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class JsonConverter
    {
        private readonly static JsonSerializerOptions properties = new() { WriteIndented = true };

        /// <summary>
        /// This method serializes an element to json format.
        /// </summary>
        /// <param name="element">The object to serialize</param>
        /// <returns>The element serialized to json format</returns>
        public string ToSerialize<T>(T element)
        {
            return JsonSerializer.Serialize<T>(element, properties);
        }

        /// <summary>
        /// This method deserializes an element from json format.
        /// </summary>
        /// <param name="json">The json string to deserialize</param>
        /// <returns>The object deserialized from json format</returns>
        public T ToDeSerialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, properties);
        }
    }
}
