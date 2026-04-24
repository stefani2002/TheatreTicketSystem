using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using TheatreTicketSystem.Domain.Entities;
using TheatreTicketSystem.Service.Interfaces;

namespace TheatreTicketSystem.Service.Implementations
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _http;
        private readonly IPlayService _playService;
        private readonly string _baseUrl;
        private readonly string _model;

        public ChatService(HttpClient http, IConfiguration config, IPlayService playService)
        {
            _http = http;
            _playService = playService;

            _baseUrl = (config["OpenAI:BaseUrl"] ?? "https://api.openai.com/v1").Trim().TrimEnd('/');
            _model = config["OpenAI:Model"] ?? "gpt-4o-mini";
        }

        public async Task<string> GetResponseAsync(string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
                return "Ask me about plays, showtimes, repertoire, or buying tickets.";

           
            var plays = _playService.GetAllPlays(); 
            var theatreData = BuildTheatreData(plays);

        
            var instructions = $"""
You are the official AI assistant for the TheatreTicketSystem website.

STRICT RULES (must follow):
1) Reply ONLY in English. Never use any other language.
2) Only answer questions about this website: plays, showtimes, repertoire calendar, buying tickets, payment (test card), login/register, admin.
3) Use ONLY the THEATRE DATA provided below. Do NOT use outside knowledge.
4) If the answer is not in THEATRE DATA, reply exactly: "I don't have that information in my database."
5) Keep answers short and helpful.
6) Never include <think> or hidden reasoning.
7) If user asks anything unrelated, reply exactly: "I can only help with TheatreTicketSystem questions."

THEATRE DATA:
{theatreData}
""";

       
            // POST https://api.openai.com/v1/responses
            var payload = new
            {
                model = _model,
                instructions = instructions,
                input = new object[]
                {
                    new { role = "user", content = userMessage }
                },
                temperature = 0.2
            };

            var json = JsonSerializer.Serialize(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var res = await _http.PostAsync($"{_baseUrl}/responses", content);
            var body = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
                return $"AI is temporarily unavailable (HTTP {(int)res.StatusCode}). Please try again.";

            var reply = ExtractOutputText(body);
            var text = (reply ?? "").Trim();

           
            if (string.IsNullOrWhiteSpace(text))
                return "AI did not return a response. Please try again.";

            if (!LooksEnglish(text))
                return "I can only respond in English. Please try again.";

            return text;
        }

        private static string BuildTheatreData(List<Play> plays)
        {
            if (plays == null || plays.Count == 0)
                return "No plays available.";

            var sb = new StringBuilder();

            foreach (var p in plays.Take(30))
            {
                sb.AppendLine($"PLAY: {p.Title}");
                sb.AppendLine($"GENRE: {p.Genre}");
                sb.AppendLine($"DURATION_MIN: {p.DurationMinutes}");
                sb.AppendLine($"DESCRIPTION: {Shorten(p.Description, 220)}");

                var shows = (p.Shows ?? new List<Show>())
                    .OrderBy(s => s.ShowTime)
                    .Take(30)
                    .ToList();

                if (shows.Count == 0)
                {
                    sb.AppendLine("SHOWS: none");
                }
                else
                {
                    sb.AppendLine("SHOWS:");
                    foreach (var s in shows)
                        sb.AppendLine($"- {s.ShowTime:yyyy-MM-dd HH:mm} | Price: {s.Price} MKD | Seats: {s.AvailableSeats}");
                }

                sb.AppendLine("----");
            }

            return sb.ToString();
        }

        private static string Shorten(string text, int max)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";
            text = text.Trim();
            return text.Length <= max ? text : text.Substring(0, max) + "...";
        }

        private static bool LooksEnglish(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            int cyrLetters = 0;
            int latinLetters = 0;

            foreach (var ch in text)
            {
                if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z'))
                    latinLetters++;

                if (ch >= '\u0400' && ch <= '\u04FF') // Cyrillic
                    cyrLetters++;
            }

           
            if (latinLetters >= 12) return true;

            
            if (cyrLetters > 30 && latinLetters < 6) return false;

            // Otherwise accept (short answers like "No plays available." etc.)
            return latinLetters >= 6;
        }


        private static string ExtractOutputText(string json)
        {
            
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("output_text", out var outputText) && outputText.ValueKind == JsonValueKind.String)
                return outputText.GetString();

            if (!root.TryGetProperty("output", out var outputArr) || outputArr.ValueKind != JsonValueKind.Array)
                return "";

            var sb = new StringBuilder();

            foreach (var item in outputArr.EnumerateArray())
            {
               
                if (!item.TryGetProperty("content", out var contentArr) || contentArr.ValueKind != JsonValueKind.Array)
                    continue;

                foreach (var c in contentArr.EnumerateArray())
                {
                   
                    if (c.TryGetProperty("text", out var t) && t.ValueKind == JsonValueKind.String)
                        sb.AppendLine(t.GetString());
                }
            }

            return sb.ToString().Trim();
        }
    }
}
