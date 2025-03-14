using System;

namespace API.Contracts.Authorization
{
    [Serializable]
    public class UserModel
    {
        public string login;
        public string password;
    }
}