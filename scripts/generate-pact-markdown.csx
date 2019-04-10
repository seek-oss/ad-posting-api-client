#!/usr/bin/env dotnet-script
#r "nuget: Newtonsoft.Json, 10.0.3"
using System.Text.RegularExpressions;
using Newtonsoft.Json;

public class PactInteraction
{
    public string Description { get; set; }

    [JsonProperty(PropertyName="providerState")]
    public string ProviderState { get; set; }

    public Newtonsoft.Json.Linq.JObject Request { get; set; }
    public Newtonsoft.Json.Linq.JObject Response { get; set; }

    public string RequestJson => JsonConvert.SerializeObject(Request, Formatting.Indented);
    public string ResponseJson => JsonConvert.SerializeObject(Response, Formatting.Indented);
}

public class PactConsumer
{
    public string Name { get; set; }
}

public class PactProvider
{
    public string Name { get; set; }
}

public class PactFile
{
    public static PactFile FromFile(string filePath)
    {
        return JsonConvert.DeserializeObject<PactFile>(File.ReadAllText(filePath));
    }

    public PactConsumer Consumer { get; set; }
    public PactConsumer Provider { get; set; }
    public PactInteraction[] Interactions { get; set; }
}

public class PactMarkdownGenerator
{
    private readonly PactFile _pact;
    private static readonly Dictionary<string, string> EscapeCharacters = new Dictionary<string, string>
    {
        {"*", @"\*"},
        {"#", @"\#"},
        {"(", @"\("},
        {")", @"\)"},
        {"[", @"\["},
        {"]", @"\]"},
        {"_", @"\_"},
        {@"\", @"\\"},
        {"+", @"\+"},
        {"-", @"\-"},
        {"`", @"\`"},
        {"<", "&lt;"},
        {">", "&gt;"},
        {"&", "&amp;"}
    };

    public PactMarkdownGenerator(PactFile pact)
    {
        this._pact = pact;
    }

    private static string Anchor(string input) => Regex.Replace(input, @"[^A-Za-z0-9_]", "_");
    private static string Anchor(PactInteraction interaction)
    {
        string providerState = string.IsNullOrEmpty(interaction.ProviderState) ? "" : $" given {Escape(interaction.ProviderState)}";
        return Anchor($"{interaction.Description}{providerState}");
    }
    private static string Escape(string input) => Regex.Replace(input, @"[\*\()\[\]\+\-\\_\`\#\<\>\&]", match => EscapeCharacters[match.ToString()]);
    private static string Upperise(string input) => !string.IsNullOrEmpty(input) ? (char.ToUpper(input[0]) + input.Substring(1)) : input;
    private static string Lowerise(string input) => !string.IsNullOrEmpty(input) ? (char.ToLower(input[0]) + input.Substring(1)) : input;

    public void WriteFile(string filePath)
    {
        var markdown = new StringBuilder();
        markdown.AppendLine($"# A pact between {Escape(_pact.Consumer.Name)} and {Escape(_pact.Provider.Name)}");
        markdown.AppendLine();

        markdown.AppendLine($"## Requests from {Escape(_pact.Consumer.Name)} to {Escape(_pact.Provider.Name)}");
        markdown.AppendLine();

        PactInteraction[] orderedInteractions = _pact.Interactions.OrderBy(i => i.Description).ToArray();

        foreach(PactInteraction interaction in orderedInteractions)
        {
            string providerState = string.IsNullOrEmpty(interaction.ProviderState) ? "" : $" given {Escape(interaction.ProviderState)}";
            markdown.AppendLine($"* [{Escape(Upperise(interaction.Description))}](#{Anchor(interaction)}){providerState}");
            markdown.AppendLine();
        }

        markdown.AppendLine($"## Interactions from {Escape(_pact.Consumer.Name)} to {Escape(_pact.Provider.Name)}");
        markdown.AppendLine();
        foreach(PactInteraction interaction in orderedInteractions)
        {
            markdown.AppendLine($"<a name=\"{Anchor(interaction)}\"></a>");
            string providerState = string.IsNullOrEmpty(interaction.ProviderState) ? "" : $"Given **{Escape(Lowerise(interaction.ProviderState))}**, ";
            markdown.AppendLine(Upperise($"{providerState}upon receiving **{Escape(interaction.Description)}** from {Escape(_pact.Consumer.Name)}, with"));
            markdown.AppendLine("```json");
            markdown.AppendLine(interaction.RequestJson);
            markdown.AppendLine("```");
            markdown.AppendLine($"{Escape(_pact.Provider.Name)} will respond with:");
            markdown.AppendLine("```json");
            markdown.AppendLine(interaction.ResponseJson);
            markdown.AppendLine("```");
            markdown.AppendLine();
        }
        File.WriteAllText(filePath, markdown.ToString());
    }
}

// PactNet does not have support for generating Markdown from PACT files
// Doing it manually avoids a build dependency on Ruby
void GenerateMarkdown(string pactJsonPath, string destinationMarkdownPath)
{
    PactFile pactFile = PactFile.FromFile(pactJsonPath);
    var generator = new PactMarkdownGenerator(pactFile);
    generator.WriteFile(destinationMarkdownPath);
}

const string PactDir = "./pact";

GenerateMarkdown($"{PactDir}/ad_posting_api_client-ad_posting_api.json", $"{PactDir}/Ad Posting API Client - Ad Posting API.md");
GenerateMarkdown($"{PactDir}/ad_posting_api_client-ad_posting_template_api.json", $"{PactDir}/Ad Posting API Client - Ad Posting Template API.md");
