using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;

namespace Onllama.Tiny
{
    public partial class FormRegistryInfo : Form
    {
        public FormRegistryInfo(string tags)
        {
            InitializeComponent();
            this.Text += @" - " + tags;

            var model = tags;
            var version = "latest";
            if (tags.Contains(":"))
            {
                model = tags.Split(':').First();
                version = tags.Split(':').Last();
            }

            try
            {
                using var httpClient = new HttpClient();
                using var request = new HttpRequestMessage(new HttpMethod("GET"),
                    $"https://registry.ollama.ai/v2/library/{model}/manifests/{version}");
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
                            $"https://registry.ollama.ai/v2/library/{model}/blobs/{layer["digest"]}").Result;
                    }
                    else if (layer["mediaType"].ToString() == "application/vnd.ollama.image.license")
                    {
                        inputLicense.Text = new HttpClient().GetStringAsync(
                            $"https://registry.ollama.ai/v2/library/{model}/blobs/{layer["digest"]}").Result;
                    }
                    else if (layer["mediaType"].ToString() == "application/vnd.ollama.image.params")
                    {
                        inputParameters.Text += new HttpClient()
                            .GetStringAsync($"https://registry.ollama.ai/v2/library/{model}/blobs/{layer["digest"]}")
                            .Result;
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
