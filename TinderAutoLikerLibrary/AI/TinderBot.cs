using System.Net.Http.Headers;

public class TinderBot
{
    string baseAddress = "https://api.gotinder.com/";

    public async Task<string> Get(string apiRoute)
    { 
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(baseAddress);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage responseMessage = await client.GetAsync(apiRoute);
        responseMessage.EnsureSuccessStatusCode();

        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadAsStringAsync();
        }
        else
        {
            return "no response";
        }
    }


}
