using System.Text.Json;
using TraineeAccounting.Client.Models;
using TraineeAccounting.Client.Requests;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using TraineeAccounting.Client.Response;

namespace TraineeAccounting.Client.Services;

public class ApiService
{
    private readonly HttpClient _client;

    public ApiService(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<PagedResult<T>> LoadDataAsync<T>(string endpoint, SearchAndSortRequest request)
    {
        try
        {
            var queryString = $"?PageIndex={request.PageIndex}" +
                              $"&PageSize={request.PageSize}" +
                              $"&Search={Uri.EscapeDataString(request.Search)}" +
                              $"&Sort={request.Sort}" +
                              $"&SortDirection={request.SortDirection}";

            return await _client.GetFromJsonAsync<PagedResult<T>>(endpoint + queryString);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка подключения: {ex.Message}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Ошибка данных: {ex.Message}");
        }

        return default;
    }
    
    private async Task<PagedResult<T>> LoadProjectsAsync<T>(string endpoint, SearchAndSortRequest request)
    {
        try
        {
            var queryString = $"?PageIndex={request.PageIndex}" +
                              $"&PageSize={request.PageSize}" +
                              $"&Search={Uri.EscapeDataString(request.Search)}" +
                              $"&Sort={request.Sort}" +
                              $"&SortDirection={request.SortDirection}";
            return await _client.GetFromJsonAsync<PagedResult<T>>(endpoint + queryString);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка подключения: {ex.Message}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Ошибка данных: {ex.Message}");
        }
        return default;
    }
    
    public async Task<(bool Success, string Message, List<string> Errors)> AddNewItemAsync<T>(string endpoint, T newItem)
    {
        try
        {
            var response = await _client.PostAsJsonAsync(endpoint, newItem);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Элемент успешно создан!", null);
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<List<FormattedMessageError>>();
                var errors = errorResponse?.Select(e => e.ErrorMessage).ToList() ?? new List<string>();
                return (false, "Произошла ошибка при создании элемента.", errors);
            }
        }
        catch (HttpRequestException ex)
        {
            return (false, "Ошибка подключения к серверу.", null);
        }
        catch (JsonException ex)
        {
            return (false, "Ошибка при чтении данных с сервера.", null);
        }
    }
    
    public async Task<List<TraineeDto>> GetTraineeExist(int id, bool isTraineeship)
    {
        try
        {
            var url = isTraineeship
                ? $"api/Traineeship/{id}/trainees"
                : $"api/Project/{id}/trainees";

            return await _client.GetFromJsonAsync<List<TraineeDto>>(url) ?? new List<TraineeDto>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка подключения: {ex.Message}");
            return new List<TraineeDto>();
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Ошибка данных: {ex.Message}");
            return new List<TraineeDto>();
        }
    }
    
    public async Task SaveChangesAsync(int id, bool isTraineeship, List<int> selectedIds)
    {
        try
        {
            var url = isTraineeship
                ? $"api/Traineeship/{id}/trainees"
                : $"api/Project/{id}/trainees";

            await _client.PutAsJsonAsync(url, selectedIds);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка при отправке PUT-запроса: {ex.Message}");
        }
    }
    
    public async Task<(bool Success, string ErrorMessage)> DeleteItemAsync(int id, bool isTraineeship)
    {
        try
        {
            var url = isTraineeship
                ? $"api/Traineeship/{id}"
                : $"api/Project/{id}";

            var response = await _client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return (true, null); 
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return (false, "Невозможно удалить элемент, так как у него есть зависимости.");
            }
            else
            {
                return (false, "Произошла ошибка при удалении элемента.");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка при удалении: {ex.Message}");
            return (false, "Ошибка подключения к серверу.");
        }
    }
}