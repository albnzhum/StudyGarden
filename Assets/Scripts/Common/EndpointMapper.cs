public static class EndpointMapper
{
    private static readonly string BaseUrl = "http://147.45.110.199";

    //Users Endpoint
    public static readonly string Login = $"{BaseUrl}/Login";
    public static readonly string Register = $"{BaseUrl}/Register";
    public static readonly string CheckToken = $"{BaseUrl}/CheckToken/";
    public static readonly string Logout = $"{BaseUrl}/Logout";

    //Achievement Endpoint
    public static readonly string CreateAchievement = $"{BaseUrl}/CreateAchievement";
    public static readonly string UpdateAchievement = $"{BaseUrl}/UpdateAchievement";
    public static readonly string DeleteAchievement = $"{BaseUrl}/DeleteAchievement/";
    public static readonly string GetAllAchievement = $"{BaseUrl}/GetAllAchievement";
    public static readonly string GetAchievement = $"{BaseUrl}/GetAchievement/";
    public static readonly string GetAchievementPlant = $"{BaseUrl}/GetAchievementPlant/";

    //User Friends Endpoint
    public static readonly string CreateFriend = $"{BaseUrl}/CreateFriend";
    public static readonly string UpdateFriend = $"{BaseUrl}/UpdateFriend";
    public static readonly string DeleteFriend = $"{BaseUrl}/DeleteFriend/";
    public static readonly string GetFriend = $"{BaseUrl}/GetFriend/";
    public static readonly string GetAllFriends = $"{BaseUrl}/GetAllFriends";
    
    //Garden Endpoint
    public static readonly string CreateGarden = $"{BaseUrl}/CreateGarden";
    public static readonly string UpdateGarden = $"{BaseUrl}/UpdateGarden";
    public static readonly string DeleteGarden = $"{BaseUrl}/DeleteGarden/";
    public static readonly string GetGarden = $"{BaseUrl}/GetGarden/";
    public static readonly string GetAllGardens = $"{BaseUrl}/GetAllGardens/";
    public static readonly string GetGardenByUserID = $"{BaseUrl}/GetByUserID/";
    
    public static readonly string CreatePlantUrl = $"{BaseUrl}/CreatePlant";
    public static readonly string UpdatePlantUrl = $"{BaseUrl}/UpdatePlant";
    public static readonly string DeletePlantUrl = $"{BaseUrl}/DeletePlant/";
    public static readonly string GetPlantUrl = $"{BaseUrl}/GetPlant";
    public static readonly string GetAllPlantsUrl = $"{BaseUrl}/GetAllPlants";
    public static readonly string GetPlantsByPlantTypeIDUrl = $"{BaseUrl}/GetPlantsByPlantTypeID/";

    public static readonly string CreateTask = $"{BaseUrl}/CreateTask";
    public static readonly string UpdateTask = $"{BaseUrl}/UpdateTask";
    public static readonly string DeleteTask = $"{BaseUrl}/DeleteTask/";
    public static readonly string GetAllTasks = $"{BaseUrl}/GetAllTasks/";
    public static readonly string GetTask  = $"{BaseUrl}/GetTask/";
}

