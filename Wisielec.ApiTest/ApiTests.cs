using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Wisieilec.API.Data;
using Wisieilec.Data.Entities;
using Wisieilec.Dtos;
using Xunit;

namespace Wisielec.ApiTest
{
    public class LobbyTests
    {
        private readonly string _baseUrl = "http://localhost:5000/api/lobbies";
        private readonly HttpClient _client = new HttpClient();

        public LobbyTests()
        {
        }

        [Fact]
        public async Task GetLobbies()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(_baseUrl);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetLobby()
        {
            // Arrange
            Lobby lobby = CreateLobbyInDatabase();

            // Act
            var response = await _client.GetAsync(_baseUrl + "/" + lobby.Id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateLobby()
        {
            // Arrange
            var lobby = new LobbyDto { LobbySize = 5, NumberOfLives = 15 };
            var content = new StringContent(JsonSerializer.Serialize(lobby), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_baseUrl, content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        private static Lobby CreateLobbyInDatabase()
        {
            Lobby lobby;
            var options = new DbContextOptions<DataContext>();
            using (var context = new DataContext(options))
            {
                lobby = new Lobby()
                {
                    LobbySize = 1,
                    NumberOfLives = 1
                };

                context.Lobbies.Add(lobby);
                context.SaveChanges();
            }

            return lobby;
        }

        private static User CreateUserInDatabase()
        {
            User user;
            var options = new DbContextOptions<DataContext>();
            using (var context = new DataContext(options))
            {
                user = new User()
                {
                    Username = "test"
                };

                context.Users.Add(user);
                context.SaveChanges();
            }

            return user;
        }
    }
}