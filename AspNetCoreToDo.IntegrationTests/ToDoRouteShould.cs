using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreToDo.IntegrationTests{
    public class ToDoRouteShould : IClassFixture<TestFixture>{
        private readonly HttpClient _client;
        public ToDoRouteShould(TestFixture fixture){
            _client = fixture.Client;
        }

        [Fact]
        public async Task ChallengeAnonymousUser(){
            //Arrange
            var request = new HttpRequestMessage( HttpMethod.Get, "/ToDo");

            //Act: request the /ToDo route
            var response = await _client.SendAsync(request);

            //Assert: the user is sent to the login page
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("https://localhost:8080" + 
                "/Identity/Account/Login?ReturnUrl=%2FToDo",
                response.Headers.Location.ToString());
        }
    }
}