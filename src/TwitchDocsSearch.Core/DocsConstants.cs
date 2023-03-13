namespace TwitchDocsSearch
{
    public static class DocsConstants
    {
        public const string DocsBaseUrl = "https://dev.twitch.tv/";

        public const string RestUrl = DocsBaseUrl + "docs/api/";
        public const string RestReferenceUrl = RestUrl + "reference/";

        public const string ChatUrl = DocsBaseUrl + "docs/irc/";
        public const string ChatCommandsUrl = ChatUrl + "commands/";
        public const string ChatTagsUrl = ChatUrl + "tags/";

        public const string EventSubUrl = DocsBaseUrl + "docs/eventsub/";
        public const string EventSubTopicsUrl = EventSubUrl + "eventsub-subscription-types/";
        public const string EventSubReferenceUrl = EventSubUrl + "eventsub-reference/";

        public const string PubSubUrl = DocsBaseUrl + "docs/pubsub/";
        public const string PubSubTopicsUrl = PubSubUrl + "#topics";
    }
}
