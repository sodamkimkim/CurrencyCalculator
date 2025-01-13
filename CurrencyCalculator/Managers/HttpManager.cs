using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using CurrencyCalculator.Managers;
using Newtonsoft.Json;


namespace CurrencyCalculator.Managers
{
    public class Root
    {
        public int Pkid { get; set; }
        public int Count { get; set; }
        public List<Country> Country { get; set; }
        public string CalculatorMessage { get; set; }
    }

    public class Country
    {
        public string Value { get; set; }
        public string SubValue { get; set; }
        public string CurrencyUnit { get; set; }
    }

}
internal class HttpManager
{
    private readonly HttpClient client = new HttpClient();
    private Root? _root;

    public async Task<string> GetData()
    {
        try
        {
            var url = "https://m.search.naver.com/p/csearch/content/qapirender.nhn?key=calculator&pkid=141&q=%ED%99%98%EC%9C%A8&where=m&u1=keb&u6=standardUnit&u7=0&u3=USD&u4=KRW&u8=down&u2=1";


            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            try
            {
                string cleanJson = json.Replace(@"\", "");
                _root = JsonConvert.DeserializeObject<Root>(cleanJson);

                // 결과 출력

                if (_root != null && _root.Country.Count > 1)
                {
                    Debug.WriteLine(_root.ToString());
                    return _root.Country[1].Value;
                }
                else return string.Empty;
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"JSON Parsing Error: {ex.Message}");
                return string.Empty;
            }
        }
        catch (JsonException jEx)
        {
            Debug.WriteLine($"JSON Parsing Error: {jEx.Message}");
            return string.Empty;
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"HTTP Request Error: {ex.Message}");
            return string.Empty;
        }

    }
}
