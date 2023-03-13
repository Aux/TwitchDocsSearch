namespace TwitchDocsSearch
{
    public class RestPartialReference
    {
        public string Category { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class RestReference : RestPartialReference
    {
        public RestReference() { }
        public RestReference(RestPartialReference part) 
            => (Category, Id, Name, Description) = (part.Category, part.Id, part.Name, part.Description);

        public string Remarks { get; set; }
        public string? Authorization { get; set; }
        public string[]? Scopes { get; set; }
        public string ApiUrl { get; set; }
    }
}
