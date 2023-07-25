using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class AuthService
{
  public async Task<Boolean> AuthenticateAsync(string matricula, string password)
  {
    try
    {
      var httpClient = new HttpClient();

      var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{matricula}:{password}"));
      httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authValue);

      var response = await httpClient.PostAsync("https://gc.moura.com.br/auth", null);

      if (response.IsSuccessStatusCode)
      {
        var result = await response.Content.ReadAsStringAsync();
        return true;
      }
      else
      {
        Console.WriteLine("Falha ao autenticar. Status code: " + response.StatusCode);
        return false;
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine("Error: " + ex.Message);
      return false;
    }
  }
}
