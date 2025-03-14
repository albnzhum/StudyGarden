using System;

namespace API.Contracts.Authorization
{
    [Serializable]
    public class LoginResponse
    {
        public string token;
        public int id;
    }
}