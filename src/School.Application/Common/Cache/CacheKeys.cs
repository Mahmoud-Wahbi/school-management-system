namespace School.Application.Common.Cache;

public static class CacheKeys
{
    public static string StudentById(Guid id) => $"students:id:{id}";

    public static string StudentsList(
        string userId,
        int page,
        int pageSize,
        string? name,
        bool? isActive,
        string? gender,
        string? sortBy,
        bool desc)
        => $"students:list:user={userId}:page={page}:pageSize={pageSize}:name={name}:isActive={isActive}:gender={gender}:sortBy={sortBy}:desc={desc}";

    public const string StudentsListPrefix = "students:list:";
}