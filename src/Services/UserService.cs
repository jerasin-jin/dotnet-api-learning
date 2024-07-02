using AuthenticationPlugin;
using RestApiSample.Models;
using RestApiSample.Interfaces;
using System.Text.RegularExpressions;

namespace RestApiSample.Services
{

    public class UserService : IUserService
    {
        private readonly ApiDbContext _dbContext;
        private readonly FormatResponseService _formatResponseService;

        public UserService(ApiDbContext dbContext, FormatResponseService formatResponseService)
        {
            _dbContext = dbContext;
            _formatResponseService = formatResponseService;

        }

        public async Task initUserAdmin(List<User> users)
        {
            foreach (var user in users)
            {
                var getUser = _dbContext.User.FirstOrDefault(u => u.Email == user.Email);

                if (getUser is null)
                {
                    Console.WriteLine("getUser is null = {0}", user.CreatedBy);
                    await createUser(user);

                }
            }


            Console.WriteLine("initUserAdmin is Running...");
        }

        public async Task<IFormatResponseService> createUser(User user)
        {
            bool isEmail = Regex.IsMatch(user.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            // _users.InsertOne(user);

            if (!isEmail)
            {
                _formatResponseService._status = DefaultStatus.BadRequest;
                _formatResponseService._value = null;
                return _formatResponseService;
            }

            string hashed = SecurePasswordHasherHelper.Hash(user.Password);

            user.Password = hashed;

            // var createUser = new User
            // {
            //     Email = user.Email,
            //     Password = hashed,
            //     Role = "user",
            //     Address = "test",
            //     CreatedBy = "admin"
            // };


            _dbContext.Add(user);
            var saveUser = await _dbContext.SaveChangesAsync();
            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = saveUser;
            return _formatResponseService;
        }

        public async Task<IFormatResponseService> createUser(string email, IUser user)
        {
            bool isEmail = Regex.IsMatch(user.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            // _users.InsertOne(user);

            if (!isEmail)
            {
                _formatResponseService._status = DefaultStatus.BadRequest;
                _formatResponseService._value = null;
                return _formatResponseService;
            }

            string hashed = SecurePasswordHasherHelper.Hash(user.Password);

            var createUser = new User
            {
                Email = user.Email,
                Password = hashed,
                Role = Roles.Admin.ToString(),
                Address = user.Address,
                CreatedBy = email
            };


            _dbContext.Add(createUser);
            var saveUser = await _dbContext.SaveChangesAsync();
            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = saveUser;
            return _formatResponseService;
        }

        public async Task<IFormatResponseService> createUser(IRegister user)
        {
            bool isEmail = Regex.IsMatch(user.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            // _users.InsertOne(user);

            if (!isEmail)
            {
                _formatResponseService._status = DefaultStatus.BadRequest;
                _formatResponseService._value = null;
                return _formatResponseService;
            }

            string hashed = SecurePasswordHasherHelper.Hash(user.Password);

            var createUser = new User
            {
                Email = user.Email,
                Password = hashed,
                Role = Roles.User.ToString(),
                Address = user.Address,
                CreatedBy = user.Email
            };


            _dbContext.Add(createUser);
            var saveUser = await _dbContext.SaveChangesAsync();
            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = saveUser;
            return _formatResponseService;
        }

        public IFormatResponseService getUsers()
        {

            var users = _dbContext.User.ToList();
            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = users;
            return _formatResponseService;
        }

        public async Task<IFormatResponseService> getUser(int id)
        {
            var result = await _dbContext.User.FindAsync(id);

            if (result is null)
            {
                _formatResponseService._status = DefaultStatus.NotFound;
                _formatResponseService._value = null;
                return _formatResponseService;
            }

            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = result;
            return _formatResponseService;
        }

        public IFormatResponseService getUserByEmail(string email)
        {
            var result = _dbContext.User.AsQueryable().FirstOrDefault(user => user.Email == email);

            if (result is null)
            {
                _formatResponseService._status = DefaultStatus.NotFound;
                _formatResponseService._value = null;
                return _formatResponseService;
            }

            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = result;
            return _formatResponseService;
        }

        public async Task<IFormatResponseService> updateUser(int id, User user)
        {
            var result = await _dbContext.User.FindAsync(id);

            if (result is null)
            {
                _formatResponseService._status = DefaultStatus.NotFound;
                _formatResponseService._value = null;
                return _formatResponseService;
            }

            result.Email = user.Email;
            result.Address = user.Address;
            result.Active = user.Active;
            result.Role = user.Role;

            var updateUser = await _dbContext.SaveChangesAsync();
            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = updateUser;
            return _formatResponseService;
        }

        public async Task<int?> deleteUser(int id)
        {
            var user = _dbContext.User.FirstOrDefault(user => user.Id == id);

            if (user is null)
            {
                return null;
            }

            _dbContext.Remove(user);
            return await _dbContext.SaveChangesAsync();
        }

    }


}
