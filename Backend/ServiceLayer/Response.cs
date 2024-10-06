using System;
using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    [Serializable]
    public class Response<T>
    {
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorMessage { get; set; }
        [JsonInclude]
        [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? ReturnValue { get; set; }

        [JsonConstructor]
        public Response(string errorMessage, T? returnValue)
        {
            ErrorMessage = errorMessage;
            ReturnValue = returnValue;
        }
    }
}