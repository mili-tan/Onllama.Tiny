using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;
using Newtonsoft.Json.Linq;

namespace Onllama.Tiny
{
    public partial class FormRegistryInfo : Form
    {
        public FormRegistryInfo(string tags)
        {
            InitializeComponent();
            this.Text += @" - " + tags;

            var repo = tags;
            var version = "latest";
            if (tags.Contains(":"))
            {
                repo = tags.Split(':').First();
                version = tags.Split(':').Last();
            }
            if (!repo.Contains("/"))
            {
                repo = "library/" + repo;
            }

            try
            {
                
                using var httpClient = new HttpClient();
                using var request = new HttpRequestMessage(new HttpMethod("GET"),
                    $"https://registry.ollama.ai/v2/{repo}/manifests/{version}");
                request.Headers.TryAddWithoutValidation("Accept", "application/vnd.docker.distribution.manifest.v2+json");

                var response = JsonNode.Parse(httpClient.SendAsync(request).Result.Content.ReadAsStringAsync().Result);

                Parallel.ForEach(response["layers"].AsArray(), layer =>
                {
                    if (layer["mediaType"].ToString() == "application/vnd.ollama.image.model")
                    {
                        badgeSize.Text =
                            (long.Parse(layer["size"].ToString()) / 1024.00 / 1024.00 / 1024.00).ToString("0.0") + "G";
                    }
                    else if (layer["mediaType"].ToString() == "application/vnd.ollama.image.template")
                    {
                        inputTemplate.Text = new HttpClient().GetStringAsync(
                            $"https://registry.ollama.ai/v2/{repo}/blobs/{layer["digest"]}").Result;
                    }
                    else if (layer["mediaType"].ToString() == "application/vnd.ollama.image.license")
                    {
                        inputLicense.Text = new HttpClient().GetStringAsync(
                            $"https://registry.ollama.ai/v2/{repo}/blobs/{layer["digest"]}").Result;
                    }
                    else if (layer["mediaType"].ToString() == "application/vnd.ollama.image.params")
                    {
                        JObject.Parse(new HttpClient()
                                .GetStringAsync($"https://registry.ollama.ai/v2/{repo}/blobs/{layer["digest"]}")
                                .Result)
                            .Properties().ToList().ForEach(p =>
                            {
                                if (p.Value.Type == JTokenType.Array)
                                {
                                    foreach (var i in p.Value.ToArray())
                                    {
                                        inputParameters.Text += $"{p.Name.PadRight(30)}\"{i}\"{Environment.NewLine}";
                                    }
                                }
                                else if (p.Value.Type == JTokenType.String)
                                {
                                    inputParameters.Text += $"{p.Name.PadRight(30)}\"{p.Value}\"{Environment.NewLine}";

                                }
                                else
                                {
                                    inputParameters.Text += $"{p.Name.PadRight(30)}{p.Value}{Environment.NewLine}";
                                }
                            });
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void FormRegistryInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
