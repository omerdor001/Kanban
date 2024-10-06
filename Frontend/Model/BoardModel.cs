using System.Text.Json.Serialization;

namespace Frontend.Model
{
    public class BoardModel
    {
        public string Name { get; set; }
        private string owner;

        [JsonConstructor]
        public BoardModel(string Name, string Owner)
        {
            this.Name = Name;
            owner = Owner;
        }

        /// <summary>
        /// Board owner getter and setter.
        /// </summary>
        public string Owner { get { return owner; } set { owner = value; } }
    }
}
