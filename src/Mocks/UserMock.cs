using RestApiSample.Models;

namespace RestApiSample.Mocks
{
    public class UserMock : BaseMock<List<User>>
    {

        public UserMock()
        {
            _value = new List<User>();
            _value.Add(new User
            {
                Email = "admin@gmail.com",
                Password = "123456",
                Address = "admin location",
                Role = Roles.Admin.ToString(),
                CreatedBy = "admin"
            });

            _value.Add(new User
            {
                Email = "test1@gmail.com",
                Password = "123456",
                Address = "test1 location",
                Role = Roles.User.ToString(),
                CreatedBy = "admin"
            });
        }
    }
}