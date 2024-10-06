using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ResponseInt
    {
        [JsonInclude]
        public int ReturnValue;
        [JsonConstructor]
        public ResponseInt(int returnValue) { ReturnValue = returnValue; }
    }
}
