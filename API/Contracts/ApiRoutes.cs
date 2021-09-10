namespace API.Contracts
{
    public static class ApiRoutes
    { 
        public static class Diffs
        {
            public const string GetDiff="v1/diff/{id}";
            public const string AddLeft="v1/diff/{id}/left";
            public const string AddRight="v1/diff/{id}/right";
        }   
    }
}