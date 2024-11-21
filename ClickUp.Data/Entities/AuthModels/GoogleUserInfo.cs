using System.Text.Json.Serialization;

public class GoogleUserInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("given_name")]
    public string GivenName { get; set; }

    [JsonPropertyName("family_name")]
    public string FamilyName { get; set; }

    [JsonPropertyName("picture")]
    public string Picture { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }
}
